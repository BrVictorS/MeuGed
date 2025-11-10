using Microsoft.AspNetCore.Mvc;
using SGD.Dtos.Index;
using SGD.Models.ViewModels;
using SGD.Services.API;
using SGD.Services.Arquivo;
using SGD.Services.Fluxo;
using SGD.Services.Index;
using System.Security.Claims;

namespace SGD.Controllers
{
    public class IndexController : BaseController
    {

        private readonly IApiInterface _api;
        private readonly IArquivoInterface _arquivo;
        private readonly IFluxoInterface _fluxo;
        private readonly IIndexInterface _index;
        public IndexController(IApiInterface api, IArquivoInterface arquivo, IFluxoInterface fluxo, IIndexInterface index)
        {
            _api = api;
            _arquivo = arquivo;
            _fluxo = fluxo;
            _index = index;
        }
        public async Task<IActionResult> Index(int id)
        {
            var lote = await _api.GetLote(id);
            if (!lote.Status)
            {
                TempData["erro"] = lote.Mensagem;
                return RedirectToAction("Index", "SelecionarLote");
            }
            var idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var Fluxo = await _fluxo.InsereFluxo(idUsuario, int.Parse(HttpContext.Session.GetString("idProjeto")), id, 6);
            if (!Fluxo.Status)
            {
                TempData["erro"] = Fluxo.Mensagem;
                return RedirectToAction("Index", "SelecionarLote", new { fila = 3 });
            }
            else
            {
                HttpContext.Session.SetInt32("idFluxo", Fluxo.Dados);

                IndexacaoViewModel model = new IndexacaoViewModel();
                model.IdLote = lote.Dados.idLote;
                model.NumLote = lote.Dados.numLote;
                model.Documentos = lote.Dados.imagens.Where(x => !string.IsNullOrEmpty(x.documentoId)).Select(x => x.documentoId).ToList();
                model.TiposDocumento = _index.GetTiposDocumentoLote(lote.Dados.idLote).Result.Dados;

                return View("Index", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsereIndexacaoDocumento([FromBody] IndexacaoDocumentoDto valores)
        {
            if (valores.metadados.Where(s=>string.IsNullOrEmpty(s.valor)).Count() == valores.metadados.Count())
            {  
                return Erro("Indexação do documento não pode ser em branco!");
            };

            var insere = await _index.SalvarIndexaoDocumento(valores);


            return Resposta(insere);
        }

        [HttpPost]
        public async Task<IActionResult> GetIndexacaoDocumento([FromBody] string documento)
        {
            if (!string.IsNullOrEmpty(documento))
            {
                var dados = await _index.GetIndexacaoDocumento(documento);
                return Json(dados.Dados);
            }
            return Erro("Erro ao retornar indexação para o documento");
        }

        public async Task<IActionResult> FinalizaLote()
        {
            var response = await _fluxo.FinalizaFluxo(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), HttpContext.Session.GetInt32("idFluxo") ?? 0, "");
            if (response.Status)
            {
                HttpContext.Session.Remove("idFluxo");
                TempData["ok"] = response.Mensagem.ToString();
                return RedirectToAction("Index", "SelecionarLote", new { fila = 5 });
            }
            else
            {
                TempData["erro"] = response.Mensagem.ToString();
                return PartialView("_Alerts");
            }
        }
    }
}
