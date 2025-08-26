using System.ComponentModel.DataAnnotations;

namespace SGD.Dtos.Login
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Digite o Email!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Digite a senha!")]
        public string Senha { get; set; }
    }
}
