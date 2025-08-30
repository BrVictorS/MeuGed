using System.Text.Json.Serialization;

namespace SGD.Models
{
    public class EspecieTipoDoc
    {
        public int Id { get; set; }
        public int EspecieId { get; set; }
        [JsonInclude]
        public EspecieModel Especie { get; set; }
        public int TipoDocumentalId { get; set; }
        [JsonIgnore]
        public TipoDocumentalModel TipoDocumental { get; set; }
    }
}