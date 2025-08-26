using SGD.Models;
using System.ComponentModel.DataAnnotations;

namespace SGD.Dtos.Projeto
{
    public class ProjetoEditarDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }        
        public ICollection<UsuarioProjetoModel>? UsuariosProjetos { get; set; }
    }
}
