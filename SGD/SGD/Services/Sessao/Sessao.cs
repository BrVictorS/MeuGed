using SGD.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Security.Claims;

namespace SGD.Services.Sessao
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public Sessao(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }
        public UsuarioModel BuscarSessao()
        {
            string sessaoUsuario = _contextAccessor.HttpContext.Session.GetString("SessaoUsuario");
            if (string.IsNullOrEmpty(sessaoUsuario))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);
        }

        public async Task CriarSessaoAsync(UsuarioModel usuario)
        {
            string usuarioJson = JsonConvert.SerializeObject(usuario);
            _contextAccessor.HttpContext.Session.SetString("SessaoUsuario", usuarioJson);

            var claims = new List<Claim>
                                                {
                                                    new Claim(ClaimTypes.Name, usuario.Nome),
                                                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
                                                };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public async Task RemoverSessaoAsync()
        {
            _contextAccessor.HttpContext.Session.Clear();
            await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            _contextAccessor.HttpContext.Session.Remove("SessaoUsuario");
        }
    }
}
