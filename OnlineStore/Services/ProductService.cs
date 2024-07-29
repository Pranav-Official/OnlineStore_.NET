using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IProductService
    {
       IEnumerable<Products> GetProducts();
       Products GetProductById(string id);
       void AddProducts(Products product);
       string BuyProduct(string ProductId, int Quantity);
       string DeleteProduct(string id);
    }
}
