using System.ComponentModel.DataAnnotations;

namespace SGD.Dtos.Usuarios
{
    public class UsuarioRegisterDto
    {

        [Required(ErrorMessage = "Digite o Nome!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o e-mail!")]
        [EmailAddress(ErrorMessage = "E-mail inválido!")]
        public string Email { get; set; }
        public string Senha { get; set; } = String.Empty;
        [Required(ErrorMessage = "Digite a confirmação da senha!")]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem!")]
        public string ConfirmarSenha { get; set; } = String.Empty;
    }
}
