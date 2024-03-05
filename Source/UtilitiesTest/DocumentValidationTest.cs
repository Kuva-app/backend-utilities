using Xunit;
using Utilities.General.Validations;

namespace UtilitiesTest
{
    public class DocumentValidationTest
    {
        [Theory]
        [InlineData("", false)]
        [InlineData("1", false)]
        [InlineData("ac", false)]
        [InlineData("00000000000000", false)]
        [InlineData("11111111111111", false)]
        [InlineData("22222222222222", false)]
        [InlineData("33333333333333", false)]
        [InlineData("44444444444444", false)]
        [InlineData("55555555555555", false)]
        [InlineData("66666666666666", false)]
        [InlineData("77777777777777", false)]
        [InlineData("88888888888888", false)]
        [InlineData("99999999999999", false)]
        [InlineData("00000000000001", false)]
        [InlineData("885094000123", false)]
        [InlineData("00885094000123", true)]
        [InlineData("00.885.094/0001-23", true)]
        [InlineData("29.415.175/0001-41", true)]
        [InlineData("29415175000141", true)]
        public void IsCnpjTest(string document, bool expected)
        {
            var actual = DocumentValidation.IsCnpj(document);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("1", false)]
        [InlineData("ac", false)]
        [InlineData("00000000000000", false)]
        [InlineData("00000000000", false)]
        [InlineData("11111111111", false)]
        [InlineData("22222222222", false)]
        [InlineData("33333333333", false)]
        [InlineData("44444444444", false)]
        [InlineData("55555555555", false)]
        [InlineData("66666666666", false)]
        [InlineData("77777777777", false)]
        [InlineData("88888888888", false)]
        [InlineData("99999999999", false)]
        [InlineData("00000000001", false)]
        [InlineData("9920262021", false)]
        [InlineData("09920262021", true)]
        [InlineData("532.272.930-56", true)]
        [InlineData("53227293056", true)]
        public void IsCpfTest(string document, bool expected)
        {
            var actual = DocumentValidation.IsCpf(document);
            Assert.Equal(expected, actual);
        }
    }
}