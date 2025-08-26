using SGD.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SGD.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DataDbContext _context;
        public DashboardController(DataDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("nomeProjeto", _context.Projeto.Where(p => p.Id == int.Parse(HttpContext.Session.GetString("idProjeto"))).Select(pj => pj.Nome).FirstOrDefault());
            return View();
        }
    }
}
