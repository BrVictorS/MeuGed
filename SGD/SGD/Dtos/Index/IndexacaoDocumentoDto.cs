using System.ComponentModel.DataAnnotations;

namespace SGD.Dtos.Index
{
    public class IndexacaoDocumentoDto
    {
        [Required]
        public string idDocumento { get; set; }
        public string idTipoDoc { get; set; }
        public string idLote { get; set; }
        public List<MetadadoDto> metadados { get; set; } = new List<MetadadoDto>();
    }
    public class MetadadoDto
    {
        public string id { get; set; }
        public string valor { get; set; }
    }
}
