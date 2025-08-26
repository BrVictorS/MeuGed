using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SGD.Services.Autenticacao
{
    public class AuthorizeProjetoAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly int _projetoId;
        private readonly string _permissaoNecessaria;

        public AuthorizeProjetoAttribute(int projetoId, string permissao)
        {
            _projetoId = projetoId;
            _permissaoNecessaria = permissao;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claims = context.HttpContext.User.Claims;
            var temPermissao = claims.Any(c =>
                c.Type == "PermissaoProjeto" &&
                c.Value == $"{_projetoId}:{_permissaoNecessaria}");

            if (!temPermissao)
            {
                context.Result = new ForbidResult(); // ou Redirect
            }
        }
    }
}
