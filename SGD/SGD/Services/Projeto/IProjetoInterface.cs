using SGD.Dtos.Projeto;
using SGD.Dtos.Usuarios;
using SGD.Models;

namespace SGD.Services.Projeto
{
    public interface IProjetoInterface
    {
        public Task<ProjetoModel> Cadastrar(ProjetoModel projeto);
        public Task<List<ProjetoModel>> BuscarProjetos();
        //public Task<List<ProjetoModel>> BuscarProjetos(ProjetoModel projetoModel);
        public Task<List<UsuarioModel>> BuscarUsuarios(string? term);

        public Task<List<UsuarioDto>> ListarUsuarios(int? id);
        public Task Editar(ProjetoEditarDto projetoEditarDto);
    }
}
