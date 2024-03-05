using UtilitiesTest.Domain;
using UtilitiesTest.VOs;
using Xunit;

namespace UtilitiesTest
{
    public class JsonTest
    {
        private static readonly string FileMockPath = $"Mock/{VOs.General.FileNameLeandroUserMockJson}";

        [Fact]
        public void SerializeJsonTest()
        {
            var expected = VOs.General.LoadFileContentFrom(FileMockPath);
            var actual = Utilities.General.Json.Utils.Serialize(Users.Leandro);
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeserializeJsonTest()
        {
            var expected = Users.Leandro;
            var json = VOs.General.LoadFileContentFrom(FileMockPath);
            Assert.NotNull(json);
            if (string.IsNullOrEmpty(json)) return;
            Assert.NotEmpty(json);
            var actual = Utilities.General.Json.Utils.Deserialize<UserDomain>(json);
            Assert.Equal(expected.Email, actual?.Email);
            Assert.Equal(expected.Name, actual?.Name);
            Assert.Equal(expected.CreateAt, actual?.CreateAt);
        }
    }
}