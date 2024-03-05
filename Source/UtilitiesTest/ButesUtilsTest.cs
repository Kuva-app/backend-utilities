using Utilities.Security.Utils;
using UtilitiesTest.Domain;
using Xunit;

namespace UtilitiesTest
{
    public class ButesUtilsTest
    {
        [Fact]
        public void SerializeTest()
        {
            var actual = BytesUtils.GetBytesFrom(VOs.Users.Tiago);
            Assert.NotNull(actual);
        }

        [Fact]
        public void Deserialize()
        {
            var currentUser = VOs.Users.Tiago;
            var serialized = BytesUtils.GetBytesFrom(currentUser);
            Assert.NotNull(serialized);
            var actual = BytesUtils.GetObjectFromBytes<UserDomain>(serialized);
            Assert.NotNull(actual);
            Assert.Equal(currentUser.Email, actual.Email);
            Assert.Equal(currentUser.Name, actual.Name);
            Assert.Equal(currentUser.CreateAt, actual.CreateAt);
        }
    }
}