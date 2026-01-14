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
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + $"\\{Environment.MachineName}LogsError";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filepath = AppDomain.CurrentDomain.BaseDirectory + $"\\{Environment.MachineName}LogsError\\Hata_" + tarihStatick.Date.ToShortDateString().Replace('/', '_') + ".txt";
                
                // Използваме FileShare.ReadWrite за да избегнем file locking проблеми
                // и retry логика ако файлът е заключен
                int maxRetries = 3;
                int retryDelay = 100; // milliseconds
                
                for (int attempt = 0; attempt < maxRetries; attempt++)
                {
                    try
                    {
                        if (!File.Exists(filepath))
                        {
                            // Create a file to write to with FileShare.ReadWrite
                            using (var fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                            using (StreamWriter sw = new StreamWriter(fs))
                            {
                                sw.WriteLine(Message + " " + tarihStatick);
                            }
                        }
                        else
                        {
                            // Append to existing file with FileShare.ReadWrite
                            using (var fs = new FileStream(filepath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                            using (StreamWriter sw = new StreamWriter(fs))
                            {
                                sw.WriteLine(Message + " " + tarihStatick);
                            }
                        }
                        // Успешно записано, излизаме от retry loop
                        return;
                    }
                    catch (IOException) when (attempt < maxRetries - 1)
                    {
                        // Файлът е заключен, чакаме и опитваме отново
                        System.Threading.Thread.Sleep(retryDelay);
                        retryDelay *= 2; // Exponential backoff
                    }
                }
            }
            catch
            {
                // Игнорираме грешки при логване - не искаме да прекъсваме основния процес
            }
        }
    }
}
