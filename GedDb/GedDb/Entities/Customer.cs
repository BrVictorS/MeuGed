using MongoDB.Bson.Serialization.Attributes;

namespace GedDb.Entities
{
    public class Customer
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? id { get; set; }
        [BsonElement("customer_name"), BsonRepresentation(MongoDB.Bson.BsonType.String)]

        public string? CustomerName { get; set; }
        [BsonElement("email"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string? Email { get; set; }
    }
}
