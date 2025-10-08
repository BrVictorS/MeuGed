using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SGD.Dtos.Processolote;
using SGD.Dtos.Response;
using SGD.Services.API;
using SGD.Services.SelecionarLote;

namespace SGD.Controllers
{
    public class SelecionarLoteController : Controller
    {
        private readonly ISelecionalote _selecionalote;        

        public SelecionarLoteController(ISelecionalote selecionalote)
        {
            _selecionalote = selecionalote;            
        }

        public IActionResult Index(int fila)
        {
            ServiceResponse<List<SelecionarLoteDto>> lotes = new ServiceResponse<List<SelecionarLoteDto>>();
            if (fila == 3)
            {
               lotes =  _selecionalote.GetLotesFila(5).Result;
            }
            else
            {
                lotes = _selecionalote.GetLotesFila(6).Result;
            }

            

            return View(lotes);
            
        }
        

        public  async Task<IActionResult> SelectVerify(int id)
        {
            TempData["LoteSelecionado"] = true;
            return RedirectToAction("Index","Verify",new {id = id});
        }

        public async Task<IActionResult> SelectIndex(int id)
        {
            TempData["LoteSelecionado"] = true;
            return RedirectToAction("Index", "Index", new { id = id });
        }

    }
}
