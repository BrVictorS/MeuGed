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

            var fluxolote = _context.FluxoLote.AsNoTracking().AsEnumerable();


            var teste = from fluxo in fluxolote
                        group fluxo by fluxo.LoteId into g
                        select g;

          
            // último FluxoLote por Lote (pela maior DtInicio)
            var ultimosFluxos =
                    from fl in fluxolote
                    group fl by fl.LoteId into g

                    let maxIni = g.Max(x => x.DtInicio)

                    from fl in g.Where(x => x.DtInicio == maxIni).Take(1)
                    select fl;

            var lotes = _context.Lote.AsNoTracking().AsEnumerable();
           

                var query =
                    from l in lotes
                    join f in ultimosFluxos on l.Id equals f.LoteId
                    where (f.SituacaoId == id && f.DtFim == null)
                       || (f.SituacaoId == filaAnterior && f.DtFim != null)
                    select new SelecionarLoteDto
                    {
                        Id = l.Id,
                        NumLote = l.NumLote,
                        IdSituacao = (f.SituacaoId == filaAnterior && f.DtFim != null) ? id : f.SituacaoId
                    };

                resp.Dados =  query.ToList();
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
