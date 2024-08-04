using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public class MongoDBService
    {
        public readonly IMongoCollection<Users> _Users;

        public MongoDBService(IOptions<MongoDbSetting> mongoDbSetting)
        {
            MongoClient client = new MongoClient(mongoDbSetting.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDbSetting.Value.DatabaseName);
            _Users = database.GetCollection<Users>("users");

        }

        public async Task CreateUserAsync(Users users)
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }
            else
            {
                await _Users.InsertOneAsync(users);
                return;
            }
        }

        public async Task<bool> IfUserExistsAsync(Users users)
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

                var filter = Builders<Users>.Filter.Eq(u => u.UserName, users.UserName);
                var user = await _Users.FindAsync(filter);
                return user != null;
        }

        public async Task<Users> GetUser(Users users)
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            var filter = Builders<Users>.Filter.And(
                 Builders<Users>.Filter.Eq(u => u.UserName, users.UserName),
                 Builders<Users>.Filter.Eq(u => u.Password, users.Password)
                 );
            var cursor = await _Users.FindAsync(filter);
            var userList = await cursor.ToListAsync();

            return userList.FirstOrDefault();
        }
    }
}
