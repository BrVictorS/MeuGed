using SGD.Enums;
using SGD.Models;

namespace SGD.Dtos.Fluxo
{
    public class FluxoDto
    {
        public int SituacaoId { get; set; }        
        public int UsuarioId { get; set; }
        public DateTime DtInicio { get; set; } = DateTime.Now;
        public DateTime DtFim { get; set; }
        public int LoteId { get; set; } 
    }
}
