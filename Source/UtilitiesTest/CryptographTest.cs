#nullable enable
using System;
using Xunit;
using Utilities.Security.Utils;
using Utilities.Security.Cryptography;

namespace UtilitiesTest
{
    public class CryptographTest
    {
        [Fact]
        public void Md5EncryptStringTest()
        {
            var actual = CryptographyUtil.Shared.Md5Encrypt(VOs.General.CertThumbPrint);
            Assert.Equal(VOs.General.CertThumbPrintMd5, actual);
        }

        [Fact]
        public void Md5EncryptDataTest()
        {
            var current = VOs.Users.Tiago;
            var data = BytesUtils.GetBytesFrom(current);
            var actual = CryptographyUtil.Shared.Md5Encrypt(data);
            const string? expected = "9d6e8b3aaa9c44a372b357277979a66b";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RijndaelTest()
        {
            const string? plainText = VOs.General.CertThumbPrint;
            var encrypted = CryptographyUtil.Shared.EncryptRijndael(plainText, VOs.General.SecurityKey);
            var decrypted = CryptographyUtil.Shared.DecryptRijndael(encrypted, VOs.General.SecurityKey);
            Assert.Equal(plainText, decrypted);
        }

        [Fact]
        public void EncryptSha256Test()
        {
            var actual = CryptographyUtil.Shared.EncryptSha256(VOs.General.CertThumbPrint);
            Assert.Equal(VOs.General.CertThumbPrintSha256, actual);
        }

        [Fact]
        public void Base64Test()
        {
            var base64 = CryptographyUtil.Shared.Base64Encode(VOs.General.CertThumbPrint);
            Assert.Equal(VOs.General.CertThumbPrintB64, base64);
            var actual = CryptographyUtil.Shared.Base64Decode(base64);
            Assert.Equal(VOs.General.CertThumbPrint, actual);
        }

        [Fact]
        public void HashApiKeyTest()
        {
            var date = DateTime.Now.AddHours(16);
            var encrypted = CryptographyUtil.Shared.EncryptHashApiKey(VOs.Users.Tiago.Email,
                VOs.General.ApiKey, date, VOs.General.CertThumbPrint);
            var (documentNumber, apiKey, validateTime) = CryptographyUtil.Shared.DecryptHashApiKey(encrypted,
                VOs.General.CertThumbPrint);
            Assert.Equal(VOs.Users.Tiago.Email, documentNumber);
            Assert.Equal(VOs.General.ApiKey, apiKey);
            Assert.Equal($"{date:yyyy-MM-dd HH:mm:ss}", $"{validateTime:yyyy-MM-dd HH:mm:ss}");
        }

        [Fact(Skip = "TOBASE32 NOT USED")]
        public void ToBase32Test()
        {
            var base32Bytes = CryptographyUtil.Shared.ToBase32Bytes(VOs.General.CertThumbPrint);
            var base32String = CryptographyUtil.Shared.ToBase32String(base32Bytes);
            Assert.Equal(VOs.General.CertThumbPrint, base32String);
        }
    }
}