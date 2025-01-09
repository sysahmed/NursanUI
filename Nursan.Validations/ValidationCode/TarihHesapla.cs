using Microsoft.Data.SqlClient;
using Nursan.XMLTools;

namespace Nursan.Validations.ValidationCode
{
    public class TarihHesaplama
    {
        // private string sqlConn = "Server=200.2.10.5;Database=UretimOtomasyon;User Id=sa;Password=wrjkd34mk22;";
        //private static string sqlConnn = "Server=200.2.10.5;Database=UretimOtomasyon;User Id=sa;Password=wrjkd34mk22;";
        //private string sqlConn = $"Server={XMLSeverIp.XmlServerIP()};Database=UretimOtomasyon;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate = True;";
        private static string sqlConnn = $"Server={XMLSeverIp.XmlServerIP()};Database=UretimOtomasyon;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate = True;";
        //private string sqlConn = "Server=localhost;Database=UretimOtomasyon;User Id=sa;Password=Eda.801120;";
        //private static string sqlConnn = "Server=localhost;Database=UretimOtomasyon;User Id=sa;Password=Eda.801120;";
        public static DateTime GetSystemDate()
        {
            using (var connection = new SqlConnection(sqlConnn))
            {
                try
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "select getdate()";
                    return (DateTime)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    return new DateTime();
                }
            }
        }
        public static TarihHIM TarihHesab()
        {
            // Вземаме текущото време
            DateTime now = GetSystemDate();

            // Извличаме часа и минутата
            int hour = now.Hour;
            int minute = now.Minute;

            TarihHIM tarih = new TarihHIM();

            // Ако часът е 6 и минутата е 1
            if (hour == 6 && minute == 1)
            {
                DateTime dateTime = DateTime.Today.AddHours(5);
                tarih.tarih1 = dateTime.AddMinutes(58); // 5:58 AM
                dateTime = DateTime.Today.AddDays(1).AddHours(5);
                tarih.tarih2 = dateTime.AddMinutes(58); // 5:58 AM на следващия ден
            }
            // Ако часът е преди 6
            else if (hour < 6)
            {
                DateTime dateTime = DateTime.Today.AddDays(-1).AddHours(6);
                tarih.tarih1 = dateTime; // 6:00 AM вчера
                dateTime = DateTime.Today.AddHours(6);
                tarih.tarih2 = dateTime; // 6:00 AM днес
            }
            // Ако часът е 6 или след 6
            else
            {
                DateTime dateTime = DateTime.Today.AddHours(6);
                tarih.tarih1 = dateTime; // 6:00 AM днес
                dateTime = DateTime.Today.AddDays(1).AddHours(6);
                tarih.tarih2 = dateTime; // 6:00 AM утре
            }

            return tarih;
        }


    }
}
