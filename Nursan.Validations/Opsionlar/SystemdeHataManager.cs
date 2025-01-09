using Nursan.Business.Manager;
using Nursan.Domain.Entity;
using System.Text.RegularExpressions;

namespace Nursan.Validations.Opsionlar
{
    public class SystemdeHataManager
    {
        private ErTestHatalari testerError;
        //private ErErrorCode codeError;
        //private int isleme = 0;
        //private int idcik = 0;
        //private int idikici = 0;
        public void Gelenveri(string line, string sicil)
        {
            string[] gelenAD;
            gelenAD = line.Split(';');
            try
            {
                testerError.Referans = gelenAD[0];
                testerError.IdDonanim = int.Parse(Regex.Replace(gelenAD[1], "[^0-9]", ""));
                testerError.CreateDate = DateTime.Parse($"{gelenAD[2]} {gelenAD[3]}");
                testerError.Vardiya = gelenAD[4];
                testerError.Bolge1 = gelenAD[5];
                testerError.Goz1 = gelenAD[6];
                testerError.HataCodu = int.Parse(gelenAD[8]);
                testerError.Onarma = gelenAD[7];
                testerError.Operator = sicil;
            }
            catch (ErrorExceptionHandller)
            {
            }
            finally
            {
            }
            if (testerError.IdDonanim != 0 && testerError.CreateDate != null)
            {
                Degerlendir(testerError);
            }

        }
        private void Degerlendir(ErTestHatalari testError)
        {
            if (testError.Id == 0 || string.IsNullOrEmpty(testError.Vardiya) || string.IsNullOrEmpty(testError.Bolge1) || testError.HataCodu == 0 || string.IsNullOrEmpty(testError.Onarma))
            {
                List<string> lines = new List<string>
        {
            "KK2T 14401 UAED;1997646;04:12:2020;18:42:20;T05-2;C2R114-B;40;Onar;67;",
            "KK2T 14401 UAED;1997646;04:12:2020;18:42:20;T05-2;C2R114-A;41;Onar;67;"
            // Добавете още редове при необходимост
        };

                Dictionary<string, Dictionary<string, string>> combinedData = new Dictionary<string, Dictionary<string, string>>();

                foreach (var line in lines)
                {
                    string[] values = line.Split(';');

                    string id = values[1];
                    string key = values[5];

                    if (!combinedData.ContainsKey(id))
                    {
                        combinedData[id] = new Dictionary<string, string>();
                    }

                    for (int i = 6; i < values.Length - 1; i += 2)
                    {
                        string columnName = values[i];
                        string columnValue = values[i + 1];

                        combinedData[id][columnName] = columnValue;
                    }
                }

                foreach (var entry in combinedData)
                {
                    string id = entry.Key;
                    Dictionary<string, string> columns = entry.Value;

                    Console.Write($"{id};");

                    foreach (var column in columns)
                    {
                        Console.Write($"{column.Key}={column.Value};");
                    }

                    Console.WriteLine();
                }
            }
        }

    }
}
