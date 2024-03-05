using System;
using System.Linq;

namespace Utilities.Security.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// Generates the random code.
        /// </summary>
        /// <param name="lenght">The lenght.</param>
        /// <param name="chars">The chars</param>
        /// <returns></returns>
        public static string GenerateRandomCode(int lenght, string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, lenght)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray());
            return result;
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ComputeHash<T>(T value)
        {
            var resultBytes = BytesUtils.GetBytesFrom(value);
            return Cryptography.CryptographyUtil.Shared.Md5Encrypt(resultBytes);
        }
    }
}