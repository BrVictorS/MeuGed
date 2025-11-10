using SGD.Dtos.Lote;
using SGD.Dtos.Response;
using SGD.Enums;
using SGD.Models;

namespace SGD.Services.Fluxo
{
    public interface IFluxoInterface
    {        
        Task<ServiceResponse<int>> InsereFluxo(int idUsuario, int idProjeto, int idLote, int idSituacao);
        Task<ServiceResponse<string>> FinalizaFluxo(int idUsuario,int idFluxo, string observacao);
        Task<ServiceResponse<bool>> SalvarFluxo(int idUsuario, int idProjeto, string numeroLote, int idSituacao, string observacao);

        public Task<int?> GetLoteByNum(string numLote,string idProjeto);

    }
}
