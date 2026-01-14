using System;
using System.IO;

namespace Nursan.Business.Logging
{
    /// <summary>
    /// Предоставя помощни методи за маскиране на чувствителни стойности.
    /// </summary>
    public static class SensitiveDataMasker
    {
        /// <summary>
        /// Маскира текст, запазвайки първите и последните два символа.
        /// </summary>
        /// <param name="input">Стойност за маскиране.</param>
        /// <returns>Маскиран резултат.</returns>
        public static string MaskValue(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            if (input.Length <= 4)
            {
                return new string('*', input.Length);
            }

            string prefix = input.Substring(0, 2);
            string suffix = input.Substring(input.Length - 2, 2);
            return string.Concat(prefix, new string('*', input.Length - 4), suffix);
        }

        /// <summary>
        /// Маскира път към файл като връща само името на файла.
        /// </summary>
        /// <param name="path">Пълният път.</param>
        /// <returns>Име на файл.</returns>
        public static string MaskPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            return Path.GetFileName(path);
        }

        /// <summary>
        /// Маскира IP адрес чрез скриване на последния октет.
        /// </summary>
        /// <param name="ipAddress">IP адрес.</param>
        /// <returns>Маскиран IP.</returns>
        public static string MaskIp(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                return string.Empty;
            }

            string[] segments = ipAddress.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length == 4)
            {
                segments[3] = "xxx";
                return string.Join(".", segments);
            }

            return MaskValue(ipAddress);
        }
    }
}

