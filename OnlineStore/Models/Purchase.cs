namespace OnlineStore.Models
{
    public class Purchase
    {
        public string ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

    }
}
