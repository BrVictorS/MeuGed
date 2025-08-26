using SGD.Data;
using SGD.Models;
using SGD.Services.Fluxo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SGD.Services.Lote
{
    public class LoteService : ILoteInterface
    {
        private readonly DataDbContext _context;
        private readonly IFluxoInterface _fluxo;

        public LoteService(DataDbContext context, IFluxoInterface fluxo)
        {
            _context = context;
            _fluxo = fluxo;
        }

        public List<LoteModel> BuscarLotes()
        {
            var lotes = _context.Lote.Include(u => u.Projeto).ToList();

            if (lotes == null)
            {
                return new List<LoteModel>();
            }
            else
            {
                return lotes;
            }
        }

        public async Task<string> GetUltimoLoteProjeto(int id)
        {
            string idlote = await _context.Lote.Where(p => p.ProjetoId == id).OrderBy(o => o.Id).Select(ps => ps.NumLote).LastOrDefaultAsync();

            if (idlote == null)
            {
                return "000001";
            }
            else
            {
                string novoid = (int.Parse(idlote)+1).ToString().PadLeft(6,'0');
                return novoid;
            }            
        }

        public async Task SalvarNovoLote(LoteModel lote, int idProjeto, int idUsuario)
        {
            lote.ProjetoId = idProjeto;
            FluxoModel fluxo = new FluxoModel()
            {
                SituacaoId = 1,
                DtFim = DateTime.Now,
                LoteId = lote.Id,
                UsuarioId = idUsuario,
                Usuario = await _context.Usuarios.Where(u => u.Id == idUsuario).FirstOrDefaultAsync(),
                Lote = new LoteModel()
            };
            lote.Fluxos = new List<FluxoModel>() { fluxo };            
            _context.Lote.Add(lote);
            await _context.SaveChangesAsync();
        }
    }
}
