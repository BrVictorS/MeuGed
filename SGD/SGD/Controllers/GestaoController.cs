using SGD.Data;
using SGD.Models;
using SGD.Models.ViewModels;
using SGD.Services.Lote;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SGD.Controllers
{
    public class GestaoController : Controller
    {
        private readonly ILoteInterface _loteInterface;
        private readonly DataDbContext _context;

        public GestaoController(ILoteInterface loteInterface, DataDbContext context)
        {
            _context = context;
            _loteInterface = loteInterface;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ReceberLote()
        {
            var lotes = _loteInterface.BuscarLotes() ?? new List<LoteModel>();
            return View(lotes);           
        }

        public IActionResult NovoLote()
        {
            var lote = new NovoLoteViewModel()
            {
                novoLote = new LoteModel()
                {
                    
                },
                projetos = _context.Projeto.ToList()
            };

            return PartialView("_EditarLoteModal", lote);
        }

        public async Task<string> GetUltimoLoteProjeto(int id)
        {
            string idlote = await _loteInterface.GetUltimoLoteProjeto(id);

            return idlote;
        }

        [HttpPost]
        public async Task<IActionResult> SalvarNovoLote(LoteModel lt, [FromForm] int idProjeto)
        {            
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _loteInterface.SalvarNovoLote(lt, idProjeto,idUsuario);
            return RedirectToAction("ReceberLote");
        }

    }
}
