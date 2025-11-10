using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SGD.Models
{
    public class ProtocoloModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public long Etiqueta { get; set; }
        public DocumentoModel Documento { get; set; }
        public int LoteId { get; set; }
        [JsonIgnore]
        public LoteModel Lote { get; set; }
    }
}
