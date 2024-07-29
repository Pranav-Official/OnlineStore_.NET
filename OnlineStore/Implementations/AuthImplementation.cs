using OnlineStore.Services;
using OnlineStore.Models;
using System.Data;

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
