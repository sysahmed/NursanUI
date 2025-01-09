using Microsoft.Data.SqlClient;
using Nursan.Domain.SystemClass;

namespace Nursan.Business.Services
{
    public static class OtherTools
    {
        private readonly static string sqlConnectionStringServer = Domain.SystemClass.XMLSeverIp.XmlServerIP();
        // static DateTime derto;
        public static DateTime GetValuesDatetime()
        {
            DateTime derto; // Декларация на променлива за връщане на резултата
            string sqlConnectionString = $"Data Source={sqlConnectionStringServer};Initial Catalog=UretimOtomasyon;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate=True;";// Задайте вашия connection string

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open(); // Отваряне на връзката
                using (var cmd = new SqlCommand("SELECT GETDATE()", connection))
                {
                    // Изпълнение на заявката и получаване на текущата дата и час
                    derto = (DateTime)cmd.ExecuteScalar();
                }
            }

            return derto; // Връщане на резултата
        }

        public static Tarih TarihHesab()
        {
            Tarih tr = new Tarih();
            DateTime now = GetValuesDatetime();
            int hour = now.Hour;
            int minute = now.Minute;

            // Ако часът е 6 и минутата е 1, не правим нищо
            if (hour == 6 && minute == 1)
            {
                // Може да добавиш логика тук, ако е необходимо
            }
            else
            {
                // Ако часът е по-малък или равен на 6, то дата1 е вчерашния ден 6 часа, а дата2 е днешния ден 6 часа
                if (hour < 6)
                {
                    tr.tarih1 = DateTime.Today.AddDays(-1).AddHours(6); // Вчера, 6:00
                    tr.tarih2 = DateTime.Today.AddHours(6); // Днес, 6:00
                }
                // Ако часът е по-голям от 6, то дата1 е днешния ден 6 часа, а дата2 е утрешния ден 6 часа
                else
                {
                    tr.tarih1 = DateTime.Today.AddHours(6); // Днес, 6:00
                    tr.tarih2 = DateTime.Today.AddDays(1).AddHours(6); // Утре, 6:00
                }
            }

            return tr;
        }
        public static Tarih TarihHesapla(Tarih tr)
        {
            DateTime now = GetValuesDatetime(); // Получаваме текущото време
            int hour = now.Hour;  // Извличаме час
            int minute = now.Minute;  // Извличаме минута

            // Ако часът е 6 и минутата е 1, не правим нищо
            if (hour == 6 && minute == 1)
            {
                // Може да добавиш логика тук, ако е необходимо
            }
            else
            {
                // Ако часът е по-малък или равен на 6, тогава започваме от вчерашния ден в 6 часа
                if (hour < 6)
                {
                    tr.tarih1 = DateTime.Today.AddDays(-1).AddHours(6); // Вчера, 6:00
                    tr.tarih2 = DateTime.Today.AddHours(6); // Днес, 6:00
                }
                // Ако часът е по-голям или равен на 6, тогава започваме от днешния ден в 6 часа
                else
                {
                    tr.tarih1 = DateTime.Today.AddHours(6); // Днес, 6:00
                    tr.tarih2 = DateTime.Today.AddDays(1).AddHours(6); // Утре, 6:00
                }
            }

            return tr;  // Връщаме обновения обект
        }


    }
}
