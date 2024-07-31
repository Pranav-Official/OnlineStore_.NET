using OnlineStore.Services;
using OnlineStore.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineStore.Implementations
{
    public class AuthImplementation : IAuthenticationService
    {
        private readonly string _userFilePath = "D:\\Work\\.NET training\\OnlineStore\\OnlineStore\\users.txt";

        public void Register(Users newUser)
        {
            var userList = RetrieveUsers().ToList();

            if (userList.Any(user => user.UserName == newUser.UserName))
            {
                throw new Exception("This username is already taken.");
            }

            var userToAdd = new Users
            {
                UserName = newUser.UserName,
                Password = newUser.Password,
                Role = newUser.Role
            };

            using (var fileWriter = new StreamWriter(_userFilePath, true))
            {
                fileWriter.WriteLine($"{userToAdd.UserName},{userToAdd.Password},{userToAdd.Role}");
            }
        }

        public bool Login(string userName, string password, out string userRole)
        {
            userRole = null;
            var users = RetrieveUsers();
            var authenticatedUser = users.FirstOrDefault(user => user.UserName == userName && user.Password == password);

            if (authenticatedUser != null)
            {
                userRole = authenticatedUser.Role;
                return true;
            }
            return false;
        }

        public string GenerateJwt(string userName, string password, string role)
        {
            var key = "thisisasecretkey1234567890987654321";
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                 {
                 new Claim(ClaimTypes.NameIdentifier, userName)
                 }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                                      new SymmetricSecurityKey(keyBytes),
                                      SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private IEnumerable<Users> RetrieveUsers()
        {
            var userList = new List<Users>();

            using (var fileReader = new StreamReader(_userFilePath))
            {
                bool skipFirstLine = true;
                while (!fileReader.EndOfStream)
                {
                    var currentLine = fileReader.ReadLine();
                    if (skipFirstLine)
                    {
                        skipFirstLine = false;
                        continue;
                    }

                    var userFields = currentLine?.Split(',');

                    if (userFields?.Length != 3)
                    {
                        continue;
                    }

                    userList.Add(new Users
                    {
                        UserName = userFields[0],
                        Password = userFields[1],
                        Role = userFields[2]
                    });
                }
            }
            return userList;
        }

    }
}
