using System.ComponentModel.DataAnnotations;

namespace SGD.Models
{
    public class SituacaoModel
    {
        [Key]
        public int IdSituacao { get; set; }

        [Required]
        public string Nome { get; set; }
        public string? Caminho { get; set; }
        public int? IdSituacaoAnterior { get; set; }
    }
}
