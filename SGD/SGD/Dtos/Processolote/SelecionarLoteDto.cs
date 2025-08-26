using SGD.Models;

namespace SGD.Dtos.Processolote
{
    public class SelecionarLoteDto
    {
        public int Id { get; set; }
        public string NumLote { get; set; }
        public int IdSituacao { get; set; }
    }
}
