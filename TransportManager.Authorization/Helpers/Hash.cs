using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace TransportManager.Authorization.Helpers
{
    /// <summary>
    ///     Класс Hash используется для хеширования строк
    /// </summary>
    public static class Hash
    {
        /// <summary>
        ///     метод HashString хеширует строку
        /// </summary>
        /// <param name="str"></param>
        /// <param name="salt">передаём, если необходимо хешировать строку конкретной солью</param>
        /// <param name="needsOnlyHash">передаём true, если необходимо получить только хешированную строку</param>
        /// <returns></returns>
        public static string HashString(string str, byte[] salt = null, bool needsOnlyHash = false)
        {
            if (salt == null || salt.Length != 16)
            {
                // генерируем соль
                salt = new byte[128 / 8];
                using var rngCsp = new RNGCryptoServiceProvider();
                rngCsp.GetNonZeroBytes(salt);
            }

            // хешируем строку
            var hashedString = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: str,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (needsOnlyHash) return hashedString;

            // совмещаем хешированную строку и соль через ':'
            var hashedStringWithSalt = $"{hashedString}:{Convert.ToBase64String(salt)}";

            return hashedStringWithSalt;
        }

        /// <summary>
        ///     метод VerifyHashedString проверяет равенство ранее хешированной строки и текущей
        /// </summary>
        /// <remarks>
        ///     для сравнения текущая строка хешируется солью, полученной из ранее хешированной строки
        /// </remarks>
        /// <param name="hashedStringWithSalt">хешированная строка с солью</param>
        /// <param name="stringToCheck">строка для проверки равенства</param>
        /// <returns></returns>
        public static bool VerifyHashedString(string hashedStringWithSalt, string stringToCheck)
        {
            if (hashedStringWithSalt == null) throw new ArgumentNullException(nameof(hashedStringWithSalt));
            if (stringToCheck == null) throw new ArgumentNullException(nameof(stringToCheck));

            // получаем массив со хешированной строкой и солью из 'hashedStringWithSalt'
            var hashedStringAndSalt = hashedStringWithSalt.Split(':');

            if (hashedStringAndSalt.Length != 2) return false;

            var salt = Convert.FromBase64String(hashedStringAndSalt[1]);

            // хешируем stringToCheck с использованием полученной соли
            var hashedStringToCheck = HashString(stringToCheck, salt, true);

            // возвращаем результат сравнения хешированных строк
            return hashedStringAndSalt[0] == hashedStringToCheck;
        }
    }
}
