using SGD.Services.Arquivo;
using SGD.Services.Fluxo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Security.Claims;
using SGD.Dtos.Response;
using SGD.Services.API;


namespace SGD.Controllers
{
    public class ScanController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IArquivoInterface _arquivo;
        private readonly IFluxoInterface _fluxo;
        private readonly IApiInterface _API;

        public ScanController(IWebHostEnvironment env, IArquivoInterface arquivoInterface, IFluxoInterface fluxoInterface, IApiInterface api)
        {
            _env = env;
            _arquivo = arquivoInterface;
            _fluxo = fluxoInterface;
            _API = api;
        }

        public IActionResult Index()
        {
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Salvar(int idUsuario, string idLote)
        {
            var idProjeto = int.Parse(HttpContext.Session.GetString("idProjeto"));

            var loteApi = await _API.GetLote(int.Parse(idLote));

            if (loteApi.Status!)
            {
                TempData["erro"] = "Lote já foi digitalizado!";
                return View("Index");
            }

            var response =  await _fluxo.InsereFluxo(idUsuario, idProjeto, int.Parse(idLote), 3);

            if (response.Status)
            {                
                var copiado = await _arquivo.EnviaLote(Request.Form.Files, idLote.ToString());
                if (copiado.Status)
                {
                    var finalizado = await _fluxo.FinalizaFluxo(idUsuario, response.Dados, Request.Form.Files.Count().ToString());
                    TempData["ok"] = $"Scan realizado com sucesso em {finalizado.Dados}";

                }
                else
                {
                    TempData["erro"] = copiado.Mensagem;
                    return View("Index");
                }
            }
            else
            {
                TempData["erro"] = response.Mensagem;
                return View("Index");
            }
                                     

            
                        
            return View("Index");
        }

        public async Task<ActionResult> GetLote([FromBody]string numLote)
        {
            var lote = await _fluxo.GetLoteByNum(numLote, HttpContext.Session.GetString("idProjeto"));



            return Json(new { numlote = lote });
        }

    }

}
