using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SGD.Models
{
    public class MetadadosModel
    {
        [Key]
        public int Id { get; set; }
        [Required]       
        public string Nome { get; set; }
        public List<MetadadosTipoDocModel> MetadadosTipoDoc { get; set; }
    }
}