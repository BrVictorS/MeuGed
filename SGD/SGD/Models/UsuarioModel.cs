using SGD.Enums;
using System.ComponentModel.DataAnnotations;

namespace SGD.Models
{
    public class UsuarioModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool Situação { get; set; } = true;
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime DataAlteracao { get; set; } = DateTime.Now;

        public ICollection<UsuarioProjetoModel> UsuariosProjetos { get; set; } // Relacionamento muitos-para-muitos
        public ICollection<UsuarioPermissaoModel> UsuarioPermissoes { get; set; } // Relacionamento com permissões
    }
}