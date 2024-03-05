#nullable enable
using Xunit;
using Utilities.General.Extensions;
using Utilities.Security.Utils;

namespace UtilitiesTest
{
    public class StringExtensionTest
    {
        const int Lenght = 5;
        const string? Text = nameof(VOs.General.CertThumbPrint);

        [Fact]
        public void GenerateUnicodeStringTest()
        {
            var expected = VOs.General.CertThumbPrint;
            var actual = expected.GetUnicodeString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StringLeftTest()
        {
            var actual = Text.Left(Lenght);
            Assert.Equal("CertT", actual);
        }

        [Fact]
        public void StringRightTest()
        {
            var actual = Text.Right(Lenght);
            Assert.Equal("Print", actual);
        }

        [Theory]
        [InlineData("isTrueLowerChar", true)]
        // ReSharper disable once StringLiteralTypo
        [InlineData("ISFALSELOWERCHAR", false)]
        public void HasLowerCharTest(string value, bool expected)
        {
            var actual = value.HasLowerChar();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("isTrueUpperChar", true)]
        // ReSharper disable once StringLiteralTypo
        [InlineData("isfalseupperchar", false)]
        public void HasUpperCharTest(string value, bool expected)
        {
            var actual = value.HasUpperChar();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("1", true)]
        [InlineData("number1", true)]
        [InlineData("         1", true)]
        [InlineData("notHasNumber", false)]
        public void HasNumberTest(string value, bool expected)
        {
            var actual = value.HasNumber();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("hasSymbol@", true)]
        [InlineData("1234@", true)]
        [InlineData("      $", true)]
        [InlineData("hasSymbol", false)]
        [InlineData("", false)]
        public void HasSymbolTest(string value, bool expected)
        {
            var actual = value.HasSymbols();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GenerateRandomCodeTest()
        {
            var size = 10;
            var actual = StringUtils.GenerateRandomCode(size);
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
            Assert.True(actual.Length == size);
        }

    }
}