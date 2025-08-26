using SGD.Dtos.Usuarios;
using SGD.Models;
using Microsoft.AspNetCore.Mvc;

namespace SGD.Services.Usuario
{
    public interface IUsuarioInterface
    {
        Task<UsuarioRegisterDto> Cadastrar(UsuarioRegisterDto usuarioRegisterDto);
        Task<List<UsuarioModel>> BuscarUsuarios(int? id);
        Task<UsuarioModel> BuscarUsuarioPorId(int? idUsuario);
        Task<UsuarioModel> MudarSituacaoUsuario(int idUsuario);
        Task<UsuarioEditarDto> Editar(UsuarioEditarDto usuarioEditado);
        Task<bool> VerificaSeExisteUsuarioEEmail(UsuarioRegisterDto usuarioRegisterDto);
        Task<bool> RedefinirSenha(int idUsuario, string senha);

    }
}
