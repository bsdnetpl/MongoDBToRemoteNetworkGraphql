using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDBToRemoteNetwork.Properties.Data
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string OrderName { get; set; } = null!;

        public decimal Price { get; set; }

        public string BookName { get; set; } = null!;

    }
}
