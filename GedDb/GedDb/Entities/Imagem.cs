using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GedDb.Entities
{
    public class Imagem
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.String)]
        public string? Id { get; set; }

        [BsonElement("caminho")]
        public string Caminho { get; set; }

        [BsonElement("situacao")]
        [BsonRepresentation(BsonType.String)]
        public string Situacao { get; set; }

        [BsonElement("documento")]
        public string? DocumentoId { get; set; }
    }
}
