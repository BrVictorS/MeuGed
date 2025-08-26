using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SGD.Models
{
    public class DocumentoModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TipoDocId {  get; set; }
        [JsonIgnore]
        public TipoDocumentalModel TipoDoc { get; set; }
        public int ProtocoloId { get; set; } // Chave estrangeira (FK)
        [JsonIgnore]
        public ProtocoloModel Protocolo { get; set; } // Propriedade de navegação
    }
}
