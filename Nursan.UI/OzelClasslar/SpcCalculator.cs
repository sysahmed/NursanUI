﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nursan.UI.OzelClasslar
{

    public class SpcCalculator
    {
        private readonly string connectionString;
        private readonly string tableName;
        private readonly ListView listView;
        //private const double DefaultUsl = 10.0;
        //private const double DefaultLsl = 10.0;
        private string makine =  Environment.MachineName;
        List<double> cpks;

        public SpcCalculator(string connectionString, string tableName, ListView listView)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            this.tableName = !string.IsNullOrWhiteSpace(tableName) ? tableName : throw new ArgumentNullException(nameof(tableName));
            this.listView = listView;//?? throw new ArgumentNullException(nameof(listBox));
      
            cpks = new List<double>();

        }

        public void CalculateCpK()
        {
            listView.Items.Clear();
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                var torkColumns = GetTorkColumns(connection).ToList();

                Console.WriteLine("\nCPK стойности за последните 100 реда:");
                Console.WriteLine("=====================================");
                ProcessLastRows(connection, torkColumns);
                Console.WriteLine("=====================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Грешка при изчисляване на CpK: {ex.Message}");
            }
        }

        private IEnumerable<string> GetTorkColumns(SqlConnection connection)
        {
            //MAKINE='{makine}' AND

            var query = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = @TableName  AND COLUMN_NAME LIKE 'TORK[_][0-9]'  -- За TORK_1 до TORK_9  OR (COLUMN_NAME = 'TORK_10')         -- За TORK_10 ORDER BY  CASE    WHEN COLUMN_NAME = 'TORK_10' THEN 10    ELSE CAST(SUBSTRING(COLUMN_NAME, 6, 1) AS INT)  END";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TableName", tableName);

            using var reader = command.ExecuteReader();
            var columns = new List<string>();
            while (reader.Read())
            {
                var columnName = reader["COLUMN_NAME"].ToString();
                if (columnName != null)
                {
                    columns.Add(columnName);
                }
            }
            return columns;
        }

        private int GetTotalRows(SqlConnection connection)
        {
            var query = $"SELECT COUNT(*) FROM {tableName}";
            using var command = new SqlCommand(query, connection);
            return (int)command.ExecuteScalar();
        }

        private void AddToListBox(string text)
        {
            if (listView.InvokeRequired)
            {
                listView.Invoke(new Action(() => listView.Items.Add(text)));
            }
            else
            {
                listView.Items.Add(text);
            }
        }

        private double CalculateCpK(List<double> values)
        {
            var (DefaultUsl, DefaultLsl) = CalculateUSL_LSL(values);
            if (values.Count == 0) return 0;

            var mean = values.Average();
            var sigma = Math.Sqrt(values.Select(x => Math.Pow(x - mean, 2)).Average());

            if (sigma == 0) return 0;

            var cpu = (DefaultUsl - mean) / (3 * sigma);
            var cpl = (mean - DefaultLsl) / (3 * sigma);

            return Math.Min(cpu, cpl);
        }

        private (double usl, double lsl) CalculateUSL_LSL(List<double> values, double margin = 3)
        {
            if (values == null || values.Count == 0)
                throw new ArgumentException("Списъкът със стойности не може да бъде празен.");

            // Намираме минималната и максималната стойност в списъка
            double minValue = values.Min();
            double maxValue = values.Max();

            // Можеш да използваш минималната и максималната стойност плюс/минус стандартното отклонение
            // като USL и LSL, или да приложиш разширения марж към тях (например 3 пъти стандартното отклонение).

            // Може да се замени margin с вашите изисквания, ако има такъв
            var mean = values.Average();
            var sigma = Math.Sqrt(values.Select(x => Math.Pow(x - mean, 2)).Average());

            // Изчисляване на USL и LSL
            double usl = maxValue + (margin * sigma);
            double lsl = minValue - (margin * sigma);

            return (usl, lsl);
        }
        private void ProcessLastRows(SqlConnection connection, List<string> columns)
        {

            var query = $@"SELECT TOP 100 Id, NR, {string.Join(", ", columns)} 
                   FROM {tableName} WHERE MAKINE = @makine
                   ORDER BY Id DESC";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@makine", makine); // Използване на параметри за защита от SQL инжекции
            using var reader = command.ExecuteReader();
            var dataTable = new DataTable();
            dataTable.Load(reader);

            if (dataTable.Rows.Count == 0)
            {
                AddToListBox("Няма данни за обработка.");
                return;
            }
            foreach (var columnName in columns)
            {
                var values = dataTable.AsEnumerable()
                    .Select(row =>
                    {
                        var valueStr = row[columnName]?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(valueStr))
                        {
                            // Премахване на нежелани символи и заместване на запетая с точка
                            valueStr = valueStr.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace(",", ".");

                            // Опит за парсване на стойността в double
                            if (double.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedValue))
                            {
                                return Math.Round(parsedValue, 2);
                            }
                            else
                            {
                                Debug.WriteLine($"Грешка при парсване: {valueStr}");
                            }
                        }
                        return (double?)null; // Връщане на null, ако не можем да парснем
                    })
                    .Where(x => x.HasValue) // Премахване на всички стойности, които не са валидни (null)
                    .Select(x => x.Value) // Преобразуване от Nullable<double> към double
                    .ToList();

                if (values.Count == 0)
                {
                    AddToListBox($"{columnName}: Няма валидни данни за изчисление.");
                    continue;
                }

                var mean = values.Average();
                var sigma = Math.Sqrt(values.Select(x => Math.Pow(x - mean, 2)).Average());
                var cpk = CalculateCpK(values);

                if (cpk is not 0)
                {
                    AddToListBox($"{columnName} - CPK: {cpk:F3}");
                    cpks.Add(cpk);
                }
            }
            bool color = cpks.Any(x => x < 1.33);
            if (color)
            {
                listView.BackColor = Color.Red;
                listView.ForeColor = Color.White;
            }
            else
            {
                listView.BackColor = Color.Lime;
                listView.ForeColor = Color.White;
            }
        }
    }
}
