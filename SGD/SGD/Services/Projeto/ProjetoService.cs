using AutoMapper;
using LojaLivros.Services.Projeto;
using SGD.Data;
using SGD.Dtos.Projeto;
using SGD.Dtos.Usuarios;
using SGD.Models;
using Microsoft.EntityFrameworkCore;

namespace SGD.Services.Projeto
{
    public class ProjetoService : IProjetoInterface
    {
        private readonly DataDbContext _context;
        private readonly IMapper _mapper;

        public ProjetoService(DataDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjetoModel> Cadastrar(ProjetoModel projeto)
        {
            _context.Projeto.Add(projeto);
            _context.SaveChangesAsync();
            return projeto;
        }
        public async Task<List<ProjetoModel>> BuscarProjetos()
        {

            try
            {
                var registros = await _context.Projeto.ToListAsync();
                if (registros == null)
                {
                    return new List<ProjetoModel>();
                }
                

                return registros;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UsuarioModel>> BuscarUsuarios(string? term)
        {
            var usuarios = new List<UsuarioModel>();
            usuarios = await _context.Usuarios.Where(u => u.Nome.StartsWith(term)).ToListAsync();
            return usuarios;
        }

        public async Task<List<UsuarioDto>> ListarUsuarios(int? id)
        {

            List<UsuarioDto> users = new List<UsuarioDto>();
            var usuariosprojetos = await _context.UsuarioProjetos.Where(u => u.ProjetoId == id).Select(s => s.Usuario).ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();
            foreach (var u in usuarios)
            {
                if (usuariosprojetos.Contains(u))
                {
                    users.Add(new UsuarioDto { Id = u.Id, Nome = u.Nome, AlocadoProjeto = true });
                }
                else
                {
                    users.Add(new UsuarioDto { Id = u.Id, Nome = u.Nome, AlocadoProjeto = false });
                }


            }
            return users;
        }

        public async Task Editar(ProjetoEditarDto projetoEditarDto)
        {
            try
            {
                var projetoExistente = await _context.Projeto
                                            .Include(p => p.UsuariosProjetos) // ou outro relacionamento
                                            .FirstOrDefaultAsync(p => p.Id == projetoEditarDto.Id);

                if (projetoExistente != null)
                {
                    int projetoid = projetoEditarDto.Id;

                    projetoExistente.Nome = projetoEditarDto.Nome;

                    int[] uadicionar = projetoEditarDto.UsuariosProjetos.Select(u => u.UsuarioId).Where(uid => !projetoExistente.UsuariosProjetos.Any(a => a.UsuarioId == uid)).ToArray();

                    List<UsuarioProjetoModel> uremover = projetoExistente.UsuariosProjetos.Where(uid => !projetoEditarDto.UsuariosProjetos.Any(a => a.UsuarioId == uid.UsuarioId)).ToList();                
                    _context.UsuarioProjetos.RemoveRange(uremover);

                    projetoExistente = null;

                    foreach (int i in uadicionar)
                    {
                        _context.UsuarioProjetos.Add(new UsuarioProjetoModel()
                        {
                            ProjetoId = projetoid,
                            UsuarioId = i
                        });                        
                    }                                   
                    
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Projeto não encontrado");
                }                                    
            }
            catch (Exception)
            {
                throw;
            }
        }        
    }
}
