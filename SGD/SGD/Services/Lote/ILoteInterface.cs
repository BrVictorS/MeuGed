using SGD.Models;
using Microsoft.AspNetCore.Mvc;

namespace SGD.Services.Lote
{
    public interface ILoteInterface
    {
        public List<LoteModel> BuscarLotes();
        public Task<string> GetUltimoLoteProjeto(int id);
        public Task SalvarNovoLote(LoteModel lote,int idProjeto,int idUsuario);

  
    }
}
