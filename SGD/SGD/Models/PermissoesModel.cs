using System.ComponentModel.DataAnnotations;

namespace SGD.Models
{
    public class PermissoesModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<UsuarioPermissaoModel> UsuariosPermissoes { get; set; } // Relacionamento com usuários
        public ICollection<ProjetoPermissaoModel> ProjetoPermissoes { get; set; } // Relacionamento com projetos
    }
}