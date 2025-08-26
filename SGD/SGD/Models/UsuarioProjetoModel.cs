using System.ComponentModel.DataAnnotations;

namespace SGD.Models
{
    public class UsuarioProjetoModel
    {
        [Key]
        public int UsuarioId { get; set; }
        public UsuarioModel Usuario { get; set; }

        [Key]
        public int ProjetoId { get; set; }
        public ProjetoModel Projeto { get; set; }
    }
}
