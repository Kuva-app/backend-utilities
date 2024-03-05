using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utilities.Security.Cryptography
{
    /// <summary>
    /// CryptographyUtil
    /// </summary>
    public class CryptographyUtil
    {
        private const string FormatHex = "x2";

        /// <summary>
        /// The instance
        /// </summary>
        private static CryptographyUtil _instance;

        /// <summary>
        /// Gets the shared, Prevents a default instance of the <see cref="CryptographyUtil"/> class from being created.
        /// </summary>
        /// <value>
        /// The shared.
        /// </value>
        public static CryptographyUtil Shared => _instance ??= new CryptographyUtil();

        /// <summary>
        /// Prevents a default instance of the <see cref="CryptographyUtil"/> class from being created.
        /// </summary>
        private CryptographyUtil()
        {
        }

        #region Private Methods

        /// <summary>
        /// Gets the byte key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        private byte[] GetByteKey(string key, byte[] salt)
        {
            using var algorithm = new TripleDESCryptoServiceProvider();
            if (algorithm.LegalKeySizes.Length > 0)
            {
                var keySize = key.Length * 8;
                var minSize = algorithm.LegalKeySizes[0].MinSize;
                var maxSize = algorithm.LegalKeySizes[0].MaxSize;
                var skipSize = algorithm.LegalKeySizes[0].SkipSize;
                if (keySize > maxSize)
                {
                    key = key.Substring(0, maxSize / 8);
                }
                else if (keySize < maxSize)
                {
                    var validSize = keySize <= minSize ? minSize : keySize - keySize % skipSize + skipSize;

                    if (keySize < validSize)
                        key = key.PadRight(validSize / 8, '*');
                }
            }

            using var returns = new Rfc2898DeriveBytes(key, salt);
            byte[] bytes = returns.GetBytes(key.Length);
            return bytes;
        }

        /// <summary>
        /// Base32s the character to value.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Character is not a Base32 character. - c</exception>
        private int Base32CharToValue(char c)
        {
            int value = c;

            //65-90 == uppercase letters
            if (value < 91 && value > 64)
            {
                return value - 65;
            }

            //50-55 == numbers 2-7
            if (value < 56 && value > 49)
            {
                return value - 24;
            }

            //97-122 == lowercase letters
            if (value < 123 && value > 96)
            {
                return value - 97;
            }

            throw new ArgumentException("Character is not a Base32 character.", nameof(c));
        }

        /// <summary>
        /// Base32s the value to character.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Byte is not a value Base32 value. - b</exception>
        private char Base32ValueToChar(byte b)
        {
            if (b < 26)
            {
                return (char)(b + 65);
            }

            if (b < 32)
            {
                return (char)(b + 24);
            }

            throw new ArgumentException("Byte is not a value Base32 value.", nameof(b));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        public string Encrypt(string data, string key, byte[] salt)
        {
            using var des = new TripleDESCryptoServiceProvider
            {
                Key = GetByteKey(key, salt),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform desEnc = des.CreateEncryptor();
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(desEnc.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="key">The key.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        public string Decrypt(string data, string key, byte[] salt)
        {
            using var des = new TripleDESCryptoServiceProvider
            {
                Key = GetByteKey(key, salt),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform desEnc = des.CreateDecryptor();
            byte[] buffer = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(desEnc.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        /// <summary>
        /// ms the d5 encrypt.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public string Md5Encrypt(string input)
        {
            var text = new StringBuilder();
            using var md5Hash = MD5.Create();
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (var t in data)
            {
                text.Append(t.ToString(FormatHex));
            }

            return text.ToString();
        }

        /// <summary>
        /// ms the d5 encrypt.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public string Md5Encrypt(byte[] bytes)
        {
            StringBuilder text = new StringBuilder();

            using MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(bytes);

            foreach (var t in data)
            {
                text.Append(t.ToString(FormatHex));
            }

            return text.ToString();
        }

        /// <summary>
        /// Encrypts the rijndael.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="securityKey">The security key.</param>
        /// <returns></returns>
        public string EncryptRijndael(string plainText, string securityKey)
        {
            var buffer = Encoding.Unicode.GetBytes(plainText);
            var sb = new StringBuilder();
            sb.Append(securityKey);
            var sbSalt = new StringBuilder();
            for (var i = 0; i < 8; i++)
            {
                sbSalt.Append("," + sb.Length);
            }

            var salt = Encoding.ASCII.GetBytes(sbSalt.ToString());
            byte[] cipherText2;
            using (var rijndaelManaged = new RijndaelManaged { BlockSize = 128 })
            {
                using (var pwdGen = new Rfc2898DeriveBytes(sb.ToString(), salt, 10000))
                {
                    var key = pwdGen.GetBytes(rijndaelManaged.KeySize / 8);
                    var iv = pwdGen.GetBytes(rijndaelManaged.BlockSize / 8);
                    rijndaelManaged.Key = key;
                    rijndaelManaged.IV = iv;
                }

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(buffer, 0, buffer.Length);
                    }

                    cipherText2 = ms.ToArray();
                }
            }

            return Convert.ToBase64String(cipherText2);
        }

        /// <summary>
        /// Decrypts the rijndael.
        /// </summary>
        /// <param name="cypherText">The cypher text.</param>
        /// <param name="securityKey">The security key.</param>
        /// <returns></returns>
        public string DecryptRijndael(string cypherText, string securityKey)
        {
            var cypherTextBytes = Convert.FromBase64String(cypherText);
            var sb = new StringBuilder();
            sb.Append(securityKey);
            var sbSalt = new StringBuilder();
            for (var i = 0; i < 8; i++)
            {
                sbSalt.Append("," + sb.Length.ToString());
            }

            byte[] plainText2;
            var salt = Encoding.ASCII.GetBytes(sbSalt.ToString());
            using (var rijndaelManaged = new RijndaelManaged { BlockSize = 128 })
            {
                using (var pwdGen = new Rfc2898DeriveBytes(sb.ToString(), salt, 10000))
                {
                    var key = pwdGen.GetBytes(rijndaelManaged.KeySize / 8);
                    var iv = pwdGen.GetBytes(rijndaelManaged.BlockSize / 8);
                    rijndaelManaged.Key = key;
                    rijndaelManaged.IV = iv;
                }

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cypherTextBytes, 0, cypherTextBytes.Length);
                    }

                    plainText2 = ms.ToArray();
                }
            }

            return Encoding.Unicode.GetString(plainText2);
        }

        /// <summary>
        /// Converts to base32bytes.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">input</exception>
        public byte[] ToBase32Bytes(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            input = input.TrimEnd('=');
            var byteCount = input.Length * 5 / 8;
            byte[] returnArray = new byte[byteCount];

            byte curByte = 0, bitsRemaining = 8;
            var arrayIndex = 0;
            foreach (char c in input)
            {
                var cValue = Base32CharToValue(c);
                int mask;
                if (bitsRemaining > 5)
                {
                    mask = cValue << bitsRemaining - 5;
                    curByte = (byte)(curByte | mask);
                    bitsRemaining -= 5;
                }
                else
                {
                    mask = cValue >> 5 - bitsRemaining;
                    curByte = (byte)(curByte | mask);
                    returnArray[arrayIndex++] = curByte;
                    curByte = (byte)(cValue << 3 + bitsRemaining);
                    bitsRemaining += 3;
                }
            }

            if (arrayIndex != byteCount)
            {
                returnArray[arrayIndex] = curByte;
            }

            return returnArray;
        }

        /// <summary>
        /// Converts to base32string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">input</exception>
        public string ToBase32String(byte[] input)
        {
            if (input == null || input.Length == 0)
            {
                throw new ArgumentNullException(nameof(input));
            }

            int charCount = (int)Math.Ceiling(input.Length / 5d) * 8;
            char[] returnArray = new char[charCount];

            byte nextChar = 0, bitsRemaining = 5;
            int arrayIndex = 0;

            foreach (byte b in input)
            {
                nextChar = (byte)(nextChar | b >> 8 - bitsRemaining);
                returnArray[arrayIndex++] = Base32ValueToChar(nextChar);

                if (bitsRemaining < 4)
                {
                    nextChar = (byte)(b >> 3 - bitsRemaining & 31);
                    returnArray[arrayIndex++] = Base32ValueToChar(nextChar);
                    bitsRemaining += 5;
                }

                bitsRemaining -= 3;
                nextChar = (byte)(b << bitsRemaining & 31);
            }

            //if we didn't end with a full char
            if (arrayIndex != charCount)
            {
                returnArray[arrayIndex++] = Base32ValueToChar(nextChar);
                while (arrayIndex != charCount) returnArray[arrayIndex++] = '='; //padding
            }

            return new string(returnArray);
        }

        /// <summary>
        /// Encrypts the sh a256.
        /// </summary>
        /// <param name="plain">The plain.</param>
        /// <returns></returns>
        public string EncryptSha256(string plain)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plain));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Base64s the encode.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns></returns>
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Base64s the decode.
        /// </summary>
        /// <param name="base64EncodedData">The base64 encoded data.</param>
        /// <returns></returns>
        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Encrypts the hash API key.
        /// </summary>
        /// <param name="documentNumber">The document number.</param>
        /// <param name="key">The key.</param>
        /// <param name="validateTime">The validate time.</param>
        /// <param name="securityPhase">The security phase.</param>
        /// <returns></returns>
        public string EncryptHashApiKey(string documentNumber, Guid key, DateTime validateTime, string securityPhase)
        {
            string plainText = $"{documentNumber}|{key:N}|{validateTime:yyyy-MM-dd HH:mm:ss}";
            return Base64Encode(EncryptRijndael(plainText, securityPhase));
        }

        /// <summary>
        /// Decrypts the hash API key.
        /// </summary>
        /// <param name="cypherText">The cypher text.</param>
        /// <param name="securityPhase">The security phase.</param>
        /// <returns></returns>
        public (string documentNumber, Guid apiKey, DateTime validateTime) DecryptHashApiKey(string cypherText,
            string securityPhase)
        {
            string plainText = DecryptRijndael(Base64Decode(cypherText), securityPhase);
            var (documentNumber, apiKey, validateTime) = (string.Empty, default(Guid), default(DateTime));
            string[] vector = plainText.Split('|');
            if (vector.Length > 0)
                documentNumber = vector[0];
            if (vector.Length >= 1)
                Guid.TryParse(vector[1], out apiKey);
            if (vector.Length >= 2)
                DateTime.TryParse(vector[2], out validateTime);
            return (documentNumber, apiKey, validateTime);
        }

        #endregion
    }
}