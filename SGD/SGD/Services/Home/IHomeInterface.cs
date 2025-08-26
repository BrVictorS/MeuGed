using SGD.Dtos.Login;
using SGD.Dtos.Response;
using SGD.Models;

namespace SGD.Services.Home
{
    public interface IHomeInterface
    {        
        Task<ServiceResponse<UsuarioModel>> RealizarLogin(LoginDto loginDto);
    }
}
