using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nursan.UI.OzelClasslar
{
    public class _SpcCalculator
    {
        private readonly string connectionString;
        private readonly string tableName;
        private const double DefaultUsl = 10.0;
        private const double DefaultLsl = 0.0;

        public _SpcCalculator(string connectionString, string tableName)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            this.tableName = !string.IsNullOrWhiteSpace(tableName) ? tableName : throw new ArgumentNullException(nameof(tableName));
        }

        public void CalculateCpK(ListBox list)
        {
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
            var query = @"SELECT COLUMN_NAME 
                     FROM INFORMATION_SCHEMA.COLUMNS 
                     WHERE TABLE_NAME = @TableName 
                     AND COLUMN_NAME LIKE 'TORK[_][0-9]'  -- За TORK_1 до TORK_9
                     OR (COLUMN_NAME = 'TORK_10')         -- За TORK_10
                     ORDER BY 
                        CASE 
                            WHEN COLUMN_NAME = 'TORK_10' THEN 10 
                            ELSE CAST(SUBSTRING(COLUMN_NAME, 6, 1) AS INT) 
                        END";

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

        private void ProcessLastRows(SqlConnection connection, List<string> columns)
        {
            // Вземаме последните 100 реда
            var query = $@"SELECT TOP 100 Id, {string.Join(", ", columns)} 
                      FROM {tableName}
                      ORDER BY Id DESC";

            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            var dataTable = new DataTable();
            dataTable.Load(reader);

            var firstId = dataTable.Rows[0]["Id"];
            var lastId = dataTable.Rows[dataTable.Rows.Count - 1]["Id"];
            Console.WriteLine($"Анализ на редове от ID {lastId} до ID {firstId}");
            Console.WriteLine();

            foreach (var columnName in columns)
            {
                var values = dataTable.AsEnumerable()
                    .Select(row => Convert.ToDouble(row[columnName]))
                    .Where(x => x != 0)
                    .ToList();

                if (values.Count == 0) continue;

                var mean = values.Average();
                var sigma = Math.Sqrt(values.Select(x => Math.Pow(x - mean, 2)).Average());
                var cpk = CalculateCpK(values);

                Console.WriteLine($"{columnName}:");
                Console.WriteLine($"  Средна стойност: {mean:F3}");
                Console.WriteLine($"  Стандартно отклонение: {sigma:F3}");
                Console.WriteLine($"  CPK: {cpk:F3}");
                Console.WriteLine($"  Брой измервания: {values.Count}");
                Console.WriteLine($"  Min: {values.Min():F3}");
                Console.WriteLine($"  Max: {values.Max():F3}");
                Console.WriteLine();
            }
        }

        private double CalculateCpK(List<double> values)
        {
            if (values.Count == 0) return 0;

            var mean = values.Average();
            var sigma = Math.Sqrt(values.Select(x => Math.Pow(x - mean, 2)).Average());

            if (sigma == 0) return 0;

            var cpu = (DefaultUsl - mean) / (3 * sigma);
            var cpl = (mean - DefaultLsl) / (3 * sigma);

            return Math.Min(cpu, cpl);
        }
    }

}
