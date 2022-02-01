using Catel.Data;

using Equality.Models.Interfaces;

namespace Equality.Models
{
    public class User : ModelBase, IUser
    {
        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}