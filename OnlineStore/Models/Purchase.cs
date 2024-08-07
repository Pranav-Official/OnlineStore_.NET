using MongoDB.Bson.Serialization.Attributes;

namespace OnlineStore.Models
{
    public class Purchase
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

    }
}
