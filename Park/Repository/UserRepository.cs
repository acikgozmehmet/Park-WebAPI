using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Park.Data;
using Park.Models;
using Park.Repository.IRepository;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Park.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;


        public UserRepository(ApplicationDbContext db, IOptions<AppSettings> appSettings)
        {
            _db = db;
            _appSettings = appSettings.Value; 
        }
        public User Authenticate(string username, string password)
        {

            var user = _db.Users.SingleOrDefault(x => x.Username.ToLower().Equals(username.ToLower()));

            // if user not found
            if (user == null)
                return null;
            //else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            //{
            //    return null;
            //}

            //if user found genarate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimValueTypes.DnsName, user.Id.ToString()),

                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            //user.Password = "";

            return user;
        }

        public bool IsUniqueUser(string username)
        {

            //var user = _db.Users.SingleOrDefault(x => x.Username.ToLower() == username.ToLower());

            //if (user == null)
            //return false;


            if (_db.Users.Any(u => u.Username.ToLower() == username.ToLower()))
                return true;
            else
                return false;
        }

        public User Register(string username, string password, string role)
        {

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var userObj = new User()
            {
                Username = username,
                //Password = password, 
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = role
            };

            _db.Users.Add(userObj);
            _db.SaveChanges();
            //userObj.Password = "";
            return userObj;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
