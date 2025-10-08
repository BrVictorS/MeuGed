using SGD.Dtos.Index;
using SGD.Dtos.Response;
using SGD.Models;

namespace SGD.Services.Index
{
    public interface IIndexInterface
    {
        public Task<ServiceResponse<object>> GetDocumentosLote(int idLote);
        public Task<ServiceResponse<object>> GetMetadadosTipoDoc(int tipoDoc);
        public Task<ServiceResponse<object>> GetIndexacaoDocumento(string idDocumento);
        public Task<ServiceResponse<object>> SalvarIndexaoDocumento(IndexacaoDocumentoDto indexacaoDocumentoDto);
        public Task<ServiceResponse<List<IndexTipoDocDto>>> GetTiposDocumentoLote(string idLote);
    }
}
