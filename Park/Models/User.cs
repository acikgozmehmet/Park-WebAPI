using System.ComponentModel.DataAnnotations.Schema;

namespace Park.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        //public string Password { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }

        [NotMapped]
        public string Token { get; set; }


    }
}
