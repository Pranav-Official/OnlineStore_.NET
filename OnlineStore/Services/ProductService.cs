using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IProductService
    {
       Task<List<Products>> GetProductsAsync();
        Task<Products> GetProductByIdAsync(string id);
        void AddProducts(Products product);
        Task BuyProductAsync(string ProductId, int Quantity);
        Task DeleteProductAsync(string id);
    }
}
