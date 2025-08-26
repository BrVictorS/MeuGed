using System.ComponentModel.DataAnnotations;

namespace SGD.Models
{
    public class TipoDocumentalModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<MetadadosTipoDocModel> Metadados { get; set;}        
    }
}