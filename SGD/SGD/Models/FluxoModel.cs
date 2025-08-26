using SGD.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SGD.Models
{
    public class FluxoModel
    {
        [Key]
        public int Id { get; set; }   
        public int SituacaoId { get; set; }
        [JsonIgnore]
        public SituacaoModel Situacao { get; set; }
        public int UsuarioId { get; set; }
        [JsonIgnore]
        public UsuarioModel Usuario { get; set; }
        public DateTime DtInicio { get; set; } = DateTime.Now;
        public DateTime? DtFim { get; set; }
        public int LoteId { get; set; } // Chave estrangeira (FK)
        [JsonIgnore]
        public LoteModel Lote { get; set; } // Propriedade de navegação        

        public string? Observacao { get; set; }
    }
}
