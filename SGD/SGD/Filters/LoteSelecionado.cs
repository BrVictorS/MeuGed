using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SGD.Filters
{
    public class LoteSelecionado : ActionFilterAttribute
    {
        private readonly ITempDataDictionaryFactory _tempDataFactory;
        private readonly ITempDataProvider _tempDataProvider;

        public LoteSelecionado(
            ITempDataDictionaryFactory tempDataFactory,
            ITempDataProvider tempDataProvider)
        {
            _tempDataFactory = tempDataFactory;
            _tempDataProvider = tempDataProvider;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var tempData = _tempDataFactory.GetTempData(context.HttpContext);

            if (tempData["LoteSelecionado"] == null)
            {
                tempData["erro"] = "Selecione o lote";

                // Salva manualmente o TempData
                _tempDataProvider.SaveTempData(context.HttpContext, tempData);

                context.Result = new RedirectToActionResult("Index", "SelecionarLote", null);
                return;
            }
        }
    }
}