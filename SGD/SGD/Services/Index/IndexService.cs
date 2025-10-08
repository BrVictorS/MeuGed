using Microsoft.EntityFrameworkCore;
using SGD.Data;
using SGD.Dtos.Index;
using SGD.Dtos.Response;

namespace SGD.Services.Index
{
    public class IndexService : IIndexInterface
    {
        private readonly DataDbContext _context;
        public IndexService(DataDbContext context)
        {
            _context = context;
        }
        public Task<ServiceResponse<object>> GetDocumentosLote(int idLote)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<object>> GetIndexacaoDocumento(string idDocumento)
        {
            throw new NotImplementedException();

        }

        public Task<ServiceResponse<object>> GetMetadadosTipoDoc(int tipoDoc)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<IndexTipoDocDto>>> GetTiposDocumentoLote(string idLote)
        {
            var response = new ServiceResponse<List<IndexTipoDocDto>>();
            var docs = _context.TipoDocumental.AsNoTracking().Select(x => 
                    new IndexTipoDocDto() {
                            Id = x.Id,
                            Name = x.Name,
                            Metadados = x.Metadados.Select(m => new IndexMetadadoDto() {
                                Id = m.Id,
                                Nome = m.Metadado.Nome
                            }).ToList()

                    }).ToList();
            response.Dados = docs;
            return response;
        }

        public Task<ServiceResponse<object>> SalvarIndexaoDocumento(IndexacaoDocumentoDto indexacaoDocumentoDto)
        {
            throw new NotImplementedException();
        }
    }
}
