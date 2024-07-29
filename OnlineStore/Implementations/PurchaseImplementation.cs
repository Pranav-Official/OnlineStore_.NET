using OnlineStore.Services;
using OnlineStore.Models;

namespace OnlineStore.Implementations
{
    public class PurchaseImplementation : IPurchaseService
    {
        private readonly string _salesFilePath = "D:\\Work\\.NET training\\OnlineStore\\OnlineStore\\purchaseReport.csv";

        public IEnumerable<Purchase> GetPurchaseRecord()
        {
            var sales = new List<Purchase>();
            using (var reader = new StreamReader(_salesFilePath))
            {
                bool isFirstLine = true;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    var values = line?.Split(',');

                    if (values?.Length != 5)
                    {
                        Console.WriteLine($"Invalid line format: {line}");
                        continue;
                    }

                    try
                    {
                        string productID = values[0];
                        int quantity = int.Parse(values[1]);
                        decimal rate = decimal.Parse(values[2]);
                        decimal price = decimal.Parse(values[3]);
                        DateTime date = DateTime.Parse(values[4]).Date;

                        sales.Add(new Purchase
                        {
                            ProductID = productID,
                            Quantity = quantity,
                            Rate = rate,
                            Price = price,
                            Date = date
                        });
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Format error in line: {line}. Error: {ex.Message}");
                    }
                }
            }
            return sales;
        }
    }
}