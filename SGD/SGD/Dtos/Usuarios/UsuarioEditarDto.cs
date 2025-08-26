using SGD.Enums;
using SGD.Models;
using System.ComponentModel.DataAnnotations;

namespace SGD.Dtos.Usuarios
{
    public class UsuarioEditarDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Digite o Nome!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o e-mail!")]

        [EmailAddress(ErrorMessage = "E-mail inválido!")]
        public string Email { get; set; }
        public ICollection<PermissoesModel> UsuarioPermissoes { get; set; }
    }

}

