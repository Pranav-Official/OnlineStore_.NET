using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OnlineStore.Exceptions;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public class MongoDBService
    {
        public readonly IMongoCollection<Users> _Users;
        private IMongoCollection<Products> _Products;
        private IMongoCollection<Purchase> _Purchases;


        public MongoDBService(IOptions<MongoDbSetting> mongoDbSetting)
        {
            MongoClient client = new MongoClient(mongoDbSetting.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDbSetting.Value.DatabaseName);
            _Users = database.GetCollection<Users>("users");
            _Products = database.GetCollection<Products>("products");
            _Purchases = database.GetCollection<Purchase>("purchases");

        }

        public async Task RegisterAsync(Users newUser)
        {
            if (newUser == null)
            {
                throw new ArgumentNullException(nameof(newUser));
            }

            if (await UserExistsAsync(newUser.UserName))
            {
                throw new UserAlreadyExistsException("User already exists.");
            }

            var userToAdd = new Users
            {
                UserName = newUser.UserName,
                Password = newUser.Password,
                Role = newUser.Role
            };

            await CreateUserAsync(userToAdd);
        }

        private async Task<bool> UserExistsAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var filter = Builders<Users>.Filter.Eq(u => u.UserName, userName);
            var count = await _Users.CountDocumentsAsync(filter);
            return count > 0;
        }

        private async Task CreateUserAsync(Users user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _Users.InsertOneAsync(user);
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

        public async Task AddProducts(Products products)
        {
            if (products == null)
            {
                throw new ArgumentNullException(nameof(products));
            }
            else
            {
                await _Products.InsertOneAsync(products);
                return;
            }
        }

        public async Task<List<Products>> getAllProducts()
        {
                var allProducts = await _Products.Find(_ => true).ToListAsync(); ;
                return allProducts;
        }

        public async Task DeleteProductAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentNullException(nameof(productId));
            }

            var filter = Builders<Products>.Filter.Eq(p => p.ProductId, productId);
            var result = await _Products.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
            {
                throw new KeyNotFoundException($"No product found with ID: {productId}");
            }
        }

        public async Task UpdateProductStockAsync(string productId, int stock)
        {
            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentNullException(nameof(productId));
            }

            var filter = Builders<Products>.Filter.Eq(p => p.ProductId, productId);
            var update = Builders<Products>.Update.Set(p => p.Stock, stock);

            var result = await _Products.UpdateOneAsync(filter, update);

            if (result.MatchedCount == 0)
            {
                throw new KeyNotFoundException($"No product found with ID: {productId}");
            }

            if (result.ModifiedCount == 0)
            {
                throw new InvalidOperationException("Product stock update failed.");
            }
        }

        public async Task AddPurchaseAsync(Purchase purchase)
        {
            if (purchase == null)
            {
                throw new ArgumentNullException(nameof(purchase));
            }

            await _Purchases.InsertOneAsync(purchase);
        }

        public async Task<List<Purchase>> GetPurchaseListAsync()
        {
            var purchases = await _Purchases.Find(_ => true).ToListAsync();
            return purchases;
        }

    }
}
