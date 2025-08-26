using System.ComponentModel.DataAnnotations;

namespace SGD.Dtos.Usuarios
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Digite o Nome!")]
        public string Nome { get; set; }
        public bool AlocadoProjeto { get; set; }
    }
}
