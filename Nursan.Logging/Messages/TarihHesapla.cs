using Microsoft.Data.SqlClient;
using Nursan.XMLTools;

namespace Nursan.Validations.ValidationCode
{
    public class TarihHesapla
    {
        //private string sqlConn = "Server=localhost;Database=UretimOtomasyon;User Id=sa;Password=Eda.801120;";
        //private static string sqlConnn = "Server=localhost;Database=UretimOtomasyon;User Id=sa;Password=Eda.801120;";
        // private string sqlConn = "Server=200.2.10.5;Database=UretimOtomasyon;User Id=sa;Password=wrjkd34mk22;";
        //private static string sqlConnn = "Server=200.2.10.5;Database=UretimOtomasyon;User Id=sa;Password=wrjkd34mk22;";
        private string sqlConn = $"Server={XMLSeverIp.XmlServerIP()};Database=UretimOtomasyon;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate = True;";
        private static string sqlConnn = $"Server={XMLSeverIp.XmlServerIP()};Database=UretimOtomasyon;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate = True;";
        public TarihHesapla()
        {
        }

        public static TarihHIM TarihHesab()
        {
            TarihHIM tarih = new TarihHIM();
            DateTime gelenTarih = GetSystemDate();
            string str = gelenTarih.ToString("HH:mm:ss");
            int hour = gelenTarih.Hour;
            int minute = gelenTarih.Minute;

            if (hour < 6 || (hour == 6 && minute < 1))
            {
                // Ако времето е преди 6 часа, връщаме дата1 предния ден от 6 часа до момента
                tarih.tarih1 = gelenTarih.Date.AddHours(6).AddDays(-1);
                tarih.tarih2 = gelenTarih.Date.AddHours(6);
            }
            else
            {
                // Ако времето е след 6 часа, връщаме дата1 от 6 часа днес до утре 6 часа
                tarih.tarih1 = gelenTarih.Date.AddHours(6);
                tarih.tarih2 = gelenTarih.Date.AddHours(6).AddDays(1);
            }

            return tarih;
        }

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
        public DateTime GetSystemDateNEw()
        {
            using (var connection = new SqlConnection(sqlConn))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "select getdate()";
                return (DateTime)command.ExecuteScalar();
            }
        }
    }
}
