using SGD.Dtos.Index;
using SGD.Dtos.TipoDocumento;

namespace SGD.Models.ViewModels
{
    public class IndexacaoViewModel
    {
        public string IdLote { get; set; }
        public string NumLote { get; set; }
        public List<string> Documentos { get; set; }
        public List<IndexTipoDocDto> TiposDocumento { get; set; }
    }
}
