using Microsoft.AspNetCore.Mvc;

namespace SGD.Controllers
{
    public abstract class BaseController : Controller
    {
        // Este método é o que você precisa.
        // Ele pode ser chamado de qualquer Action em qualquer controller que herde de BaseController.
        protected IActionResult Sucesso(string mensagem)
        {
            TempData["ok"] = mensagem;
            return PartialView("_Alerts");
        }

        protected IActionResult Erro(string mensagem)
        {
            TempData["erro"] = mensagem;
            return PartialView("_Alerts");
        }
    }
}
