using Nursan.Validations.ValidationCode;

namespace Nursan.Logging.Messages
{
    public class Messaglama
    {
        DateTime tarih = TarihHesapla.GetSystemDate();
        static DateTime tarihStatick = TarihHesapla.GetSystemDate();

        public bool messanger(string Message, string id)

        {
            string path = AppDomain.CurrentDomain.BaseDirectory + $"\\{Environment.MachineName}Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + $"\\{Environment.MachineName}Logs\\Durum_" + tarih.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(id + "-" + Message + " " + tarih);
                    return true;
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(id + "-" + Message + " " + tarih); return true;
                }
            }
        }
        public bool messanger(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + $"\\{Environment.MachineName}Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + $"\\{Environment.MachineName}Logs\\Durum_" + tarih.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message + " " + tarih);
                    return true;
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message + " " + tarih); return true;
                }
            }
        }
        public static void MessagYaz(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + $"\\{Environment.MachineName}Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + $"\\{Environment.MachineName}Logs\\Durum_" + tarihStatick.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message + " " + tarihStatick);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message + " " + tarihStatick);
                }
            }
        }
        public static void MessagException(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + $"\\{Environment.MachineName}LogsError";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + $"\\{Environment.MachineName}LogsError\\Hata_" + tarihStatick.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message + " " + tarihStatick);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message + " " + tarihStatick);
                }
            }
        }
    }
}
