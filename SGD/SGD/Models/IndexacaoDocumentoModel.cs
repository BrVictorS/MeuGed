using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SGD.Models
{
    public class IndexacaoDocumentoModel
    {
        [Required]
        public int MetadadoTipoDocId { get; set; }
        [JsonIgnore]
        public MetadadosTipoDocModel MetadadoTipoDoc { get; set; }

        [Required]
        public int DocumentoId { get; set; }
        [JsonIgnore]
        public DocumentoModel Documento { get; set; }

        [Required]
        public int LoteId { get; set; }
        [JsonIgnore]
        public LoteModel Lote { get; set; }

        [Required]
        public string Valor { get; set; }
    }
}
