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
        public ICollection<DocumentoModel> Documentos { get; set; }
        public int LoteId { get; set; } // Chave estrangeira (FK)
        [JsonIgnore]
        public LoteModel Lote { get; set; } // Propriedade de navegação
    }
}
