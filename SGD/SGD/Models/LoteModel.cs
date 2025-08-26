using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SGD.Models
{
    public class LoteModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NumLote { get; set; }
        [Required]     
        public string Observacao { get; set; }
        [Required]
        public int ProjetoId { get; set; }
        [JsonIgnore]
        public ProjetoModel Projeto { get; set; }

        public ICollection<ProtocoloModel> Protocolos { get; set; }
        public ICollection<FluxoModel> Fluxos { get; set; }
    }
}
