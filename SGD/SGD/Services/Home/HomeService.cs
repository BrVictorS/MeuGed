using SGD.Data;
using SGD.Dtos.Login;
using SGD.Dtos.Response;
using SGD.Models;
using SGD.Services.Autenticacao;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.EntityFrameworkCore;

namespace SGD.Services.Home
{
    public class HomeService:IHomeInterface
    {
        private readonly DataDbContext _context;
        private readonly IAutenticacaoInterface _autenticacaoInterface;

        public HomeService(DataDbContext context, IAutenticacaoInterface autenticacaoInterface)
        {
            _context = context;
            _autenticacaoInterface = autenticacaoInterface;
        }

        public async Task<ServiceResponse<UsuarioModel>> RealizarLogin(LoginDto loginDto)
        {

            ServiceResponse<UsuarioModel> serviceResponse = new ServiceResponse<UsuarioModel>();
            try
            {
                var cliente = _context.Usuarios.FirstOrDefault(usuario => usuario.Email == loginDto.Email);

                if (cliente == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Credenciais Inválidas!";
                    serviceResponse.Status = false;
                    return serviceResponse;
                }

                if (!_autenticacaoInterface.VerificaLogin(loginDto.Senha, cliente.PasswordHash, cliente.PasswordSalt))
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Credenciais Inválidas!";
                    serviceResponse.Status = false;
                    return serviceResponse;
                }


                var token = _autenticacaoInterface.CreateRandomToken(cliente);


                serviceResponse.Dados = cliente;
                serviceResponse.Mensagem = "Login Efetuado com sucesso!";

            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Status = false;
            }
            return serviceResponse;
        }
    }
}
