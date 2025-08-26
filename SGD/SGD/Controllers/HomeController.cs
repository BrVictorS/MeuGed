using LojaLivros.Services.Projeto;
using SGD.Dtos.Login;
using SGD.Models;
using SGD.Services.Home;
using SGD.Services.Sessao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace SGD.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeInterface _homeInterface;
        private readonly IProjetoInterface _projetoInterface;
        private readonly ISessao _sessao;

        private readonly ILogger<HomeController> _logger;

        public HomeController(IHomeInterface homeInterface, ISessao sessao, IProjetoInterface projeto)
        {
            _homeInterface = homeInterface;
            _sessao = sessao;
            _projetoInterface = projeto;
        }

        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SessaoUsuario")))
            {
                return RedirectToAction("Index", "Dashboard");
            }


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var login = await _homeInterface.RealizarLogin(loginDto);
                if (login != null)
                {
                    if (login.Status == false)
                    {
                        TempData["MensagemErro"] = "Credenciais inválidas!";
                        return RedirectToAction("Index");
                    }

                    if (login.Dados.Situação == false)
                    {

                        TempData["MensagemErro"] = "Procure o suporte para verificar o status de sua conta!";
                        return RedirectToAction("Login");
                    }

                    await _sessao.CriarSessaoAsync(login.Dados);                    
                    return RedirectToAction("SelecionaProjeto");
                }
            }
            else
            {
                return View("Index", loginDto);
            }
            return Ok();
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoAsync();
            ViewBag.DeslogadoMsg = "Usuário deslogado com sucesso!";
            return View("Index");
        }

        public async Task<IActionResult> SelecionaProjetoAsync() 
        {
            List<ProjetoModel> projetos = await _projetoInterface.BuscarProjetos();
            return View("SelecionaProjeto", projetos);
        }

        [HttpPost]
        public IActionResult DefineProjeto([FromBody] int idProjeto)
        {
            HttpContext.Session.SetString("idProjeto", idProjeto.ToString());            
            return RedirectToAction("Index","Dashboard");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}