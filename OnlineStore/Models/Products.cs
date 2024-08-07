using MongoDB.Bson.Serialization.Attributes;

namespace OnlineStore.Models
{
    public class Products
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public string ProductId { get; set; }
        public string Category { get; set; }
            public string ProductName { get; set; }
            public string Desription { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
    }
}
