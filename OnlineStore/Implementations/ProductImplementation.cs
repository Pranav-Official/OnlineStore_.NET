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

        public IEnumerable<Products> GetProducts()
        {
            var productList = new List<Products>();

            using (var reader = new StreamReader(_productFilePath))
            {
                bool skipHeader = true;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (skipHeader)
                    {
                        skipHeader = false;
                        continue;
                    }

                    var values = line?.Split(',');

                    if (values?.Length != 6)
                    {
                        Console.WriteLine($"Malformed line: {line}");
                        continue;
                    }

                    try
                    {
                        var product = new Products
                        {
                            ID = values[0],
                            Category = values[1],
                            ProductName = values[2],
                            Desription = values[3],
                            Price = decimal.Parse(values[4]),
                            Stock = int.Parse(values[5])
                        };
                        productList.Add(product);
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Error parsing line: {line}. Details: {ex.Message}");
                    }
                }
            }

            return productList;
        }

        public Products GetProductById(string id)
        {
            var products = GetProducts();
            return products.FirstOrDefault(p => p.ID.Equals(id));
        }

        public void AddProducts(Products product)
        {
            var csvLine = $"{product.ID},{product.Category},{product.ProductName},{product.Desription},{product.Price},{product.Stock}";

            using (var writer = new StreamWriter(_productFilePath, true))
            {
                writer.WriteLine(csvLine);
            }
        }

        public string BuyProduct(string productId, int quantity)
        {
            var productList = GetProducts().ToList();
            var selectedProduct = productList.FirstOrDefault(p => p.ID.Equals(productId));

            if (selectedProduct == null)
            {
                return "Product not available";
            }

            if (selectedProduct.Stock < quantity)
            {
                return "Not enough stock";
            }

            selectedProduct.Stock -= quantity;

            using (var writer = new StreamWriter(_productFilePath, false))
            {
                writer.WriteLine("ID,Category,ProductName,Details,Price,Stock");
                foreach (var prod in productList)
                {
                    writer.WriteLine($"{prod.ID},{prod.Category},{prod.ProductName},{prod.Desription},{prod.Price},{prod.Stock}");
                }
            }

            var sale = new Purchase
            {
                ProductID = selectedProduct.ID,
                Quantity = quantity,
                Rate = selectedProduct.Price,
                Price = selectedProduct.Price * quantity,
                Date = DateTime.Now
            };

            var saleCsvLine = $"{sale.ProductID},{sale.Quantity},{sale.Rate},{sale.Price},{sale.Date}";
            using (var writer = new StreamWriter(_salesFilePath, true))
            {
                writer.WriteLine(saleCsvLine);
            }

            return "Product purchased successfully";
        }

        public string DeleteProduct(string productId)
        {
            var productList = GetProducts().ToList();
            var productToRemove = productList.FirstOrDefault(p => p.ID.Equals(productId));

            Console.WriteLine(productToRemove);

            if (productToRemove == null)
            {
                return "Product does not exist";
            }

            productList.Remove(productToRemove);

            using (var writer = new StreamWriter(_productFilePath))
            {
                writer.WriteLine("ID,Category,ProductName,Details,Price,Stock");
                foreach (var prod in productList)
                {
                    writer.WriteLine($"{prod.ID},{prod.Category},{prod.ProductName},{prod.Desription},{prod.Price},{prod.Stock}");
                }
            }

            return $"{productToRemove.ProductName} has been successfully deleted";
        }
    }
}
