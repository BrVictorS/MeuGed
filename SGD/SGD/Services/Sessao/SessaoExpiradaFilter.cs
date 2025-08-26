using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SGD.Controllers;

namespace SGD.Services.Sessao
{
    public class SessaoExpiradaFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var session = context.HttpContext.Session;
            var path = context.HttpContext.Request.Path.Value?.ToLower();

            if (session.GetString("SessaoUsuario") == null &&
                 context.Controller is not HomeController)
            {
                context.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/_401.cshtml"
                };                
                return;
            }

            await next();
        }
    }
}
