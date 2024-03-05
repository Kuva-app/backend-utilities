using System;
using UtilitiesTest.Domain;

namespace UtilitiesTest.VOs
{
    [Serializable]
    public class Users
    {
        public static UserDomain Tiago => new UserDomain
        {
            CreateAt = Convert.ToDateTime("2020-12-20 03:36:00"),
            Email = "tsabian@hotmail.com",
            Name = "Tiago Oliveira"
        };

        public static UserDomain Leandro => new UserDomain
        {
            CreateAt = Convert.ToDateTime("2020-11-25 03:36:00"),
            Email = "leandro@kuva.com.br",
            Name = "Leandro Oliveira"
        };
    }
}
