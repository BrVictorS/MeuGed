using Microsoft.AspNetCore.Mvc;
using SGD.Dtos.Lote;
using SGD.Dtos.Response;
using System.Net;

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

        protected IActionResult Resposta( ServiceResponse<object> serviceResponse)
        {
            if (serviceResponse.Status)
            {
                TempData["ok"] = serviceResponse.Mensagem;
            }else
            {
                TempData["erro"] = serviceResponse.Mensagem;
            }

            return PartialView("_Alerts",Json(serviceResponse.Dados));
        }

        protected IActionResult Resposta(ApiResponseDto serviceResponse)
        {
            if (serviceResponse.Status)
            {
                TempData["ok"] = serviceResponse.Mensagem;
            }
            else
            {
                TempData["erro"] = serviceResponse.Mensagem;
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return PartialView("_Alerts");
        }

        public async Task<IActionResult> RespostaJson(ApiResponseDto apiResponse)
        {
            if (apiResponse.Status)
            {
                TempData["ok"] = apiResponse.Mensagem;
            }
            else
            {
                TempData["erro"] = apiResponse.Mensagem;
                Response.StatusCode = (int)HttpStatusCode.BadRequest;               
            }

            var ret = Json(apiResponse);
            return ret;
        }

        public async Task<IActionResult> RespostaJson(ServiceResponse<string> apiResponse)
        {
            if (apiResponse.Status)
            {
                TempData["ok"] = apiResponse.Mensagem;
            }
            else
            {
                TempData["erro"] = apiResponse.Mensagem;
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            var ret = Json(apiResponse);
            return ret;
        }
        public async Task<IActionResult> RespostaJson(ServiceResponse<int> apiResponse)
        {
            if (apiResponse.Status)
            {
                TempData["ok"] = apiResponse.Mensagem;
            }
            else
            {
                TempData["erro"] = apiResponse.Mensagem;
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            var ret = Json(apiResponse);
            return ret;
        }

    }
}
