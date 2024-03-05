using Xunit;
using Utilities.Security.Extensions;
using Utilities.Security;

namespace UtilitiesTest
{
    public class WebSecurityExtensionTest
    {
        [Theory]
        [InlineData("", PasswordComplexityErrorType.PasswordEmpty, false)]
        [InlineData("1234567", PasswordComplexityErrorType.PasswordMinimumCharsRequest, false)]
        [InlineData("12345678", PasswordComplexityErrorType.PasswordLowerCharRequest, false)]
        [InlineData("12345678a", PasswordComplexityErrorType.PasswordUpperCharRequest, false)]
        [InlineData("aaaaaaaaA", PasswordComplexityErrorType.PasswordNumericCharRequest, false)]
        [InlineData("aaaaaA123", PasswordComplexityErrorType.PasswordSpecialCharRequest, false)]
        [InlineData("aaaaA12_!3", PasswordComplexityErrorType.Nothing, true)]
        public void PasswordComplexityTest(string value, PasswordComplexityErrorType errorType, bool success)
        {
            var actual = value.ValidatePasswordComplexity(out var error);
            Assert.Equal(errorType, error);
            Assert.Equal(success, actual);
        }

        [Fact]
        public void EncodeBase64Test()
        {
            var current = VOs.General.CertThumbPrint;
            var expected = "YWYyOTg3MzJiNjc5ZDZiZTJjMmJiMDE0NGZhY2YwNWIwNmFlYmUwNA==";
            var actual = current.Base64Encode();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeBase64Test()
        {
            var current = "YWYyOTg3MzJiNjc5ZDZiZTJjMmJiMDE0NGZhY2YwNWIwNmFlYmUwNA==";
            var actual = current.Base64Decode();
            Assert.Equal(VOs.General.CertThumbPrint, actual);
        }
    }
}