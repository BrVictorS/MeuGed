using MongoDB.Bson.Serialization.Attributes;

namespace GedDb.Entities
{
    public class Lote
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? id { get; set; }

        [BsonElement("id_lote"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string idLote { get; set; }

        [BsonElement("num_lote"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string NumLote { get; set; }

        [BsonElement("imagens")]
        public List<Imagem> Imagens { get; set; }

    }
}
