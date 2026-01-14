using System;
using System.Security.Cryptography;
using System.Text;

namespace Nursan.Domain.Security
{
    /// <summary>
    /// Помощен клас за генериране и проверка на защитен ключ за станция (UrIstasyon).
    /// </summary>
    public static class StationSecureKeyGenerator
    {
        private const int KeySizeBytes = 32; // 256 bit
        private const int SaltSizeBytes = 16; // 128 bit

        /// <summary>
        /// Генерира нов защитен ключ. Връща оригиналния ключ, както и стойностите за запис в БД.
        /// </summary>
        public static StationSecureKey Generate()
        {
            string rawKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(KeySizeBytes));
            string salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(SaltSizeBytes));
            string hash = ComputeHash(rawKey, salt);
            return new StationSecureKey(rawKey, hash, salt);
        }

        /// <summary>
        /// Проверява дали подаденият ключ съответства на записаните hash + salt.
        /// </summary>
        public static bool Verify(string rawKey, string storedHash, string storedSalt)
        {
            if (string.IsNullOrWhiteSpace(rawKey) || string.IsNullOrWhiteSpace(storedHash) || string.IsNullOrWhiteSpace(storedSalt))
            {
                return false;
            }

            string computed = ComputeHash(rawKey, storedSalt);
            return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(computed), Encoding.UTF8.GetBytes(storedHash));
        }

        private static string ComputeHash(string rawKey, string salt)
        {
            using var hmac = new HMACSHA512(Convert.FromBase64String(salt));
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawKey));
            return Convert.ToBase64String(hashBytes);
        }
    }

    public readonly record struct StationSecureKey(string RawKey, string Hash, string Salt);
}


