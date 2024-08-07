using OnlineStore.Services;
using OnlineStore.Models;

namespace OnlineStore.Implementations
{
    public class PurchaseImplementation : IPurchaseService
    {
        private readonly string _salesFilePath = "D:\\Work\\.NET training\\OnlineStore\\OnlineStore\\purchaseReport.csv";

        private readonly MongoDBService _mongoDbService;

        public PurchaseImplementation(MongoDBService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<List<Purchase>> GetPurchaseRecord()
        {
            var Sales = await _mongoDbService.GetPurchaseListAsync();
            return Sales;
        }
    }
}