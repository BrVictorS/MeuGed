using SGD.Models;
using Microsoft.AspNetCore.Mvc;
using SGD.Dtos.Lote;
using SGD.Dtos.Response;

namespace SGD.Services.Lote
{
    public interface ILoteInterface
    {
        public List<LoteModel> BuscarLotes();
        public Task<ServiceResponse<object>> GetLoteById(int idLote);
        public Task<string> GetUltimoLoteProjeto(int id);
        public Task SalvarNovoLote(LoteModel lote,int idProjeto,int idUsuario);

  
    }
}
