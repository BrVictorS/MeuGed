using System.ComponentModel.DataAnnotations;

namespace SGD.Dtos.Index
{
    public class IndexacaoDocumentoDto
    {
        [Required]
        public int IdDocumento { get; set; }
        public Dictionary<int,string> Metadados { get; set; } = new Dictionary<int, string>();
    }
}
