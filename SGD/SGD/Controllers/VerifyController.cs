using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGD.Dtos.Arquivo;
using SGD.Dtos.Lote;
using SGD.Dtos.Response;
using SGD.Dtos.Verify;
using SGD.Filters;
using SGD.Services.API;
using SGD.Services.Arquivo;
using SGD.Services.Fluxo;
using System.Security.Claims;

namespace SGD.Controllers
{
    public class VerifyController : BaseController
    {
        private readonly IApiInterface _api;
        private readonly IArquivoInterface _arquivo;
        private readonly IFluxoInterface _fluxo;        

        public VerifyController(IApiInterface api, IArquivoInterface arquivo, IFluxoInterface fluxo)
        {
            _api = api;
            _arquivo = arquivo;
            _fluxo = fluxo;
        }


        /*[ServiceFilter(typeof(LoteSelecionado))]*/
        public async Task<IActionResult> Index(int id)
        {
            
            var lote = await _api.GetLote(id);
            if (!lote.Status)
            {
                TempData["erro"] = lote.Mensagem;
                return RedirectToAction("Index", "SelecionarLote");
            }
            var idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var Fluxo = await _fluxo.InsereFluxo(idUsuario, int.Parse(HttpContext.Session.GetString("idProjeto")), id, 5);
            if (!Fluxo.Status)
            {
                TempData["erro"] = Fluxo.Mensagem;
                return RedirectToAction("Index", "SelecionarLote", new { fila = 3});
            }
            else
            {
                HttpContext.Session.SetInt32("idFluxo", Fluxo.Dados);
                return View("Index", lote.Dados);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> MostrarImagem([FromBody] string imagem)
        {
            var img = await _arquivo.GetImagem(imagem);

            return File(img, "image/png");
        }

        [HttpPost]
        public async Task<IActionResult> MostrarImagemIndex([FromBody] string documento)
        {
            var img = await _arquivo.GetImagemIndex(documento);

            return File(img, "image/png");
        }

        [HttpPost]
        public async Task<IActionResult> AtualizaImagem([FromBody] AtualizaImagemDto imagem)
        {
            var img = await _api.AtualizaImagem(imagem);
            if (img.Status)
            {
                return Ok();
            }
            else
            {
                TempData["erro"] = img.Mensagem.ToString();
                return BadRequest( img.Mensagem);
            }
        }

        [HttpPost]
        public async Task<IActionResult> MoveImagem([FromBody] MoveImagemDto imagem)
        {
            var img = await _api.MoveImagem(imagem);
            if (img.Status)
            {
                return Ok();
            }
            else
            {
                TempData["erro"] = img.Mensagem.ToString();
                return BadRequest(img.Mensagem);
            }
        }

        public async Task<IActionResult> InsereDocumento([FromBody] InsereDocumentoDto documentoDto)
        {
            var insere = await _api.InsereDocumento(documentoDto);
            return Resposta(insere);
        }

        public async Task<IActionResult> ExcluiDocumento([FromBody] InsereDocumentoDto documentoDto)
        {
            var insere = await _api.InsereDocumento(documentoDto);
            return Resposta(insere);
        }


        public async Task<IActionResult> InsereImagem([FromForm]InsereImagemDto insereImagemDto)
        {
            var envio = await _arquivo.InsereImagem(insereImagemDto);
            if (envio.Status)
            {
                TempData["ok"] = envio.Mensagem.ToString();
                return PartialView("_Alerts");
            }
            else
            {
                TempData["erro"] = envio.Mensagem.ToString();
                return PartialView("_Alerts");
            }
        }

        public async Task<IActionResult> FinalizaLote()
        {
            var response = await _fluxo.FinalizaFluxo(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), HttpContext.Session.GetInt32("idFluxo") ?? 0, "");
            if (response.Status)
            {
                HttpContext.Session.Remove("idFluxo");
                TempData["ok"] = response.Mensagem.ToString();
                return RedirectToAction("Index","SelecionarLote",new { fila = 3});
            }
            else
            {
                TempData["erro"] = response.Mensagem.ToString();
                return PartialView("_Alerts");
            }
        }
    }
}
