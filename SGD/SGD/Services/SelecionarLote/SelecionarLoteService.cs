using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SGD.Data;
using SGD.Dtos.Processolote;
using SGD.Dtos.Response;

namespace SGD.Services.SelecionarLote
{
    public class SelecionarLoteService: ISelecionalote
    {
        private readonly DataDbContext _context;

        public SelecionarLoteService(DataDbContext context)
        {
            _context = context;
        }

        public ServiceResponse<List<SelecionarLoteDto>> GetLotesProxFila(int id)
        {

            ServiceResponse<List<SelecionarLoteDto>> lotes = new ServiceResponse<List<SelecionarLoteDto>>();
           // var fluxos = _context.Lote.Include(l => l.Fluxos).Where(l => l.Fluxos.OrderByDescending(f => f.Id).FirstOrDefault().SituacaoId == id);

            var lotess =  _context.Lote
                                            .Include(x => x.Fluxos)
                                            .Where(l => 
                                            (l.Fluxos.OrderByDescending(f => f.Id).FirstOrDefault().Situacao.IdSituacaoAnterior == id && l.Fluxos.OrderByDescending(f => f.Id).FirstOrDefault().DtFim != null) ||
                                            (l.Fluxos.OrderByDescending(f => f.Id).FirstOrDefault().Situacao.IdSituacao == id && l.Fluxos.OrderByDescending(f => f.Id).FirstOrDefault().DtFim == null))
                                            
                                            .Select(l => new SelecionarLoteDto
                                            {
                                                Id = l.Id,
                                                NumLote = l.NumLote,
                                                IdSituacao = l.Fluxos
                                                    .OrderByDescending(f => f.Id)
                                                    .Select(f => f.SituacaoId)
                                                    .FirstOrDefault()
                                            })
                                            .ToList();            
            lotes.Dados = lotess;

            return lotes;
        }

    }
}
