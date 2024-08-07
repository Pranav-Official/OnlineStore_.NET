using OnlineStore.Exceptions;
using OnlineStore.Models;
using OnlineStore.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OnlineStore.Implementations
{
    public class ProductImplementation : IProductService
    {
        private readonly string _productFilePath = "D:\\Work\\.NET training\\OnlineStore\\OnlineStore\\prducts.csv";
        private readonly string _salesFilePath = "D:\\Work\\.NET training\\OnlineStore\\OnlineStore\\purchaseReport.csv";

        private readonly MongoDBService _mongoDbService;

        public ProductImplementation(MongoDBService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<List<Products>> GetProductsAsync()
        {
            var productList = await _mongoDbService.getAllProducts();

            return productList;
        }

        public async Task<Products> GetProductByIdAsync(string id)
        {
            var products = await GetProductsAsync();
            return products.FirstOrDefault(p => p.ProductId.Equals(id));
        }

        public async void AddProducts(Products product)
        {
            await _mongoDbService.AddProducts(product);
        }

        public async Task BuyProductAsync(string productId, int quantity)
        {
            var productList = (await GetProductsAsync()).ToList();
            var selectedProduct = productList.FirstOrDefault(p => p.ProductId.Equals(productId));

            if (selectedProduct == null)
            {
                throw new KeyNotFoundException($"No product found with ID: {productId}");
            }

            if (selectedProduct.Stock < quantity)
            {
                throw new NotEnoughStock("Not enough stock to proceed with order");
            }

            var newStock = selectedProduct.Stock - quantity;

            await _mongoDbService.UpdateProductStockAsync(productId, newStock);

            var sale = new Purchase
            {
                ProductID = selectedProduct.ProductId,
                Quantity = quantity,
                Rate = selectedProduct.Price,
                Price = selectedProduct.Price * quantity,
                Date = DateTime.Now
            };

            await _mongoDbService.AddPurchaseAsync(sale);
        }

        public async Task DeleteProductAsync(string productId)
        { 
                await _mongoDbService.DeleteProductAsync(productId);
        }
    }
}
