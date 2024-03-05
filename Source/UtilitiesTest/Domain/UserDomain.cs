using System;

namespace UtilitiesTest.Domain
{
    [Serializable]
    public class UserDomain
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
