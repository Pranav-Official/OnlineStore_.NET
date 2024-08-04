using OnlineStore.Services;
using OnlineStore.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OnlineStore.Exceptions;

namespace OnlineStore.Implementations
{
    public class AuthImplementation : IAuthenticationService
    {
        private readonly string _userFilePath = "D:\\Work\\.NET training\\OnlineStore\\OnlineStore\\users.txt";
        private readonly MongoDBService _mongoDbService;

        public AuthImplementation(MongoDBService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }
        public async Task Register(Users newUser)
        {

            var userToAdd = new Users
            {
                UserName = newUser.UserName,
                Password = newUser.Password,
                Role = newUser.Role
            };

            if (await _mongoDbService.IfUserExistsAsync(userToAdd))
            {
                throw new UserAlreadyExistsException("User with this username already exists.");
            }
            else
            {
                await _mongoDbService.CreateUserAsync(userToAdd);
            }

        }

        public async Task<Users> LoginAsync(string userName, string password)
        {
            var userToAdd = new Users
            {
                UserName = userName,
                Password = password,
                Role = " "
            };

            var authUser = await _mongoDbService.GetUser(userToAdd);
            if (authUser != null)
            {
                return authUser;
            }

            return new Users
            {
                UserName = "",
                Password = "",
                Role = " "
            };
        }

    }
}
