using System.ComponentModel.DataAnnotations;

namespace SGD.Dtos.Lote
{
    public class LoteApiDto
    {
        public string idLote { get; set; }
        public string numLote { get; set; }
        public List<ImagemLoteDto> imagens { get; set; }
    }
}
