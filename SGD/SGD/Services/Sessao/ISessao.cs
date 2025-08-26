using SGD.Models;

namespace SGD.Services.Sessao
{
    public interface ISessao
    {
        UsuarioModel BuscarSessao();
        Task CriarSessaoAsync(UsuarioModel usuario);  // Método assíncrono

        Task RemoverSessaoAsync();
    }
}
