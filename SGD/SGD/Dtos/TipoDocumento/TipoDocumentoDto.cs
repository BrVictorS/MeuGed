using SGD.Models;
using System.ComponentModel.DataAnnotations;

namespace SGD.Dtos.TipoDocumento
{
    public class TipoDocumentoDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Digite o título!")]
        public string Name { get; set; }
        public List<int> MetadadosSelecionados { get; set; }

        public List<MetadadosDocumentoDto> metadadosDocumentos { get; set; } = new List<MetadadosDocumentoDto>();
    }
}
