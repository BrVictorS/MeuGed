using SGD.Dtos.Processolote;
using SGD.Dtos.Response;

namespace SGD.Services.SelecionarLote
{
    public interface ISelecionalote
    {
        public ServiceResponse<List<SelecionarLoteDto>> GetLotesProxFila(int id);
    }
}
