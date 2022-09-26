using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDBToRemoteNetwork.Properties.Data
{
    [BsonIgnoreExtraElements]
    public class Users
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Password { get; set; }

        public string RePassword { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string? DataCreated { get; set; }
        public string? Role { get; set; }
    }
}
