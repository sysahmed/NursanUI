namespace Nursan.Business.Manager
{
    public class ErrorExceptionHandller : Exception
    {
        private readonly string logFilePath;

        // Конструктор по подразбиране
        public ErrorExceptionHandller() : this("ErrorLog.txt") { }

        // Конструктор с персонализиран път
        public ErrorExceptionHandller(string logFilePath)
        {
            this.logFilePath = logFilePath;
        }

        // Презаписваме Message и автоматично записваме в лог
        public override string Message
        {
            get
            {
                string message = base.Message;
                LogMessage(message, this.StackTrace);
                return message;
            }
        }

        // Метод за логване
        private void LogMessage(string message, string stackTrace)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine("*********************");
                    writer.WriteLine($"Date: {DateTime.Now}");
                    writer.WriteLine($"Message: {message}");
                    writer.WriteLine($"Stack Trace: {stackTrace}");
                    writer.WriteLine("*********************");
                    writer.WriteLine();
                }
            }
            catch
            {
                // Може да се добави алтернативно поведение, ако логването не успее
            }
        }
    }

}