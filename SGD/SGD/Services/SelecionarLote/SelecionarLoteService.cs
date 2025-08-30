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

       public async Task<ServiceResponse<List<SelecionarLoteDto>>> GetLotesFila(int id)
{
                var resp = new ServiceResponse<List<SelecionarLoteDto>>();
                var filaAnterior = GetFilaAnterior(id);

                // último FluxoLote por Lote (pela maior DtInicio)
                var ultimosFluxos =
                    from fl in _context.FluxoLote.AsNoTracking()
                    group fl by fl.LoteId into g
                    let maxIni = g.Max(x => x.DtInicio)
                    from fl in g.Where(x => x.DtInicio == maxIni).Take(1)
                    select fl;

                var query =
                    from l in _context.Lote.AsNoTracking()
                    join f in ultimosFluxos on l.Id equals f.LoteId
                    where (f.SituacaoId == id && f.DtFim == null)
                       || (f.SituacaoId == filaAnterior && f.DtFim != null)
                    select new SelecionarLoteDto
                    {
                        Id = l.Id,
                        NumLote = l.NumLote,
                        IdSituacao = (f.SituacaoId == filaAnterior && f.DtFim != null) ? id : f.SituacaoId
                    };

                resp.Dados = await query.ToListAsync();
                return resp;
            }

        public int GetFilaAnterior(int id)
        {
            var idSituacao = _context.Situacoes.ToList();
            if (id == 5)
            {
                id = 4;
            }

            int x = idSituacao.IndexOf(idSituacao.FirstOrDefault(i => i.IdSituacao == id))-1;

            int nx = idSituacao[x].IdSituacao;

            return nx;
        }

    }
}
