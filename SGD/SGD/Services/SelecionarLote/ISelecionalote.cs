using SGD.Dtos.Processolote;
using SGD.Dtos.Response;

namespace SGD.Services.SelecionarLote
{
    public interface ISelecionalote
    {
        public Task<ServiceResponse<List<SelecionarLoteDto>>> GetLotesFila(int id);
    }
}
