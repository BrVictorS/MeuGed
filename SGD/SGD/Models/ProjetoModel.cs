using System.ComponentModel.DataAnnotations;

namespace SGD.Models
{
    public class ProjetoModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }

        public ICollection<UsuarioProjetoModel>? UsuariosProjetos { get; set; } // Relacionamento muitos-para-muitos        
        public ICollection<LoteModel>? Lote { get; set; }
        public ICollection<ProjetoPermissaoModel>? Permissoes { get; set;}

    }
}