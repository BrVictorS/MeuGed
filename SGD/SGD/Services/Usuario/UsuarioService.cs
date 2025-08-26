using AutoMapper;
using SGD.Data;
using SGD.Dtos.Usuarios;
using SGD.Models;
using SGD.Services.Autenticacao;
using Microsoft.EntityFrameworkCore;

namespace SGD.Services.Usuario
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly DataDbContext _context;
        private readonly IAutenticacaoInterface _autenticacaoInterface;
        private readonly IMapper _mapper;

        public UsuarioService(DataDbContext context, IAutenticacaoInterface autenticacaoInterface, IMapper mapper)
        {
            _context = context;
            _autenticacaoInterface = autenticacaoInterface;
            _mapper = mapper;
        }


        public async Task<UsuarioRegisterDto> Cadastrar(UsuarioRegisterDto usuarioRegisterDto)
        {

            try
            {

                _autenticacaoInterface.CriarPasswordHash(usuarioRegisterDto.Senha, out byte[] passwordHash, out byte[] passwordSalt);

                var Usuario = new UsuarioModel
                {
                    Nome = usuarioRegisterDto.Nome,                    
                    Email = usuarioRegisterDto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                //Criar Cadastro banco
                _context.Add(Usuario);
                await _context.SaveChangesAsync();

                return usuarioRegisterDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> VerificaSeExisteUsuarioEEmail(UsuarioRegisterDto usuarioRegisterDto)
        {
            try
            {

                var mesmoUsuario = await _context.Usuarios.FirstOrDefaultAsync(usuarioBanco => usuarioBanco.Email == usuarioRegisterDto.Email);

                if (mesmoUsuario == null)
                {
                    return true;
                }

                return false;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> RedefinirSenha(int id, string senha)
        {
            var usuarioNovaSenha = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuarioNovaSenha != null)
            {
                _autenticacaoInterface.CriarPasswordHash(senha, out byte[] passwordHash, out byte[] passwordSalt);
                usuarioNovaSenha.PasswordHash = passwordHash;
                usuarioNovaSenha.PasswordSalt = passwordSalt;

                _context.Update(usuarioNovaSenha);
                await _context.SaveChangesAsync(); ;
                return true;
            }
            return false;
        }

        public async Task<UsuarioEditarDto> Editar(UsuarioEditarDto usuarioEditado)
        {
            
            try
            {

                var usuarioEditar = await _context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Id == usuarioEditado.Id);               
                if (usuarioEditar != null)
                {
                    usuarioEditar.Nome = usuarioEditado.Nome;
                    usuarioEditar.Email = usuarioEditado.Email;                   
                    _context.Update(usuarioEditar);
                    await _context.SaveChangesAsync();

                    return usuarioEditado;
                }

                return usuarioEditado;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<UsuarioModel> MudarSituacaoUsuario(int idUsuario)
        {
            try
            {
                var usuarioMudarSituacao = await _context.Usuarios.FindAsync(idUsuario);

                if (usuarioMudarSituacao != null)
                {
                    if (usuarioMudarSituacao.Situação == true)
                    {
                        usuarioMudarSituacao.Situação = false;
                        usuarioMudarSituacao.DataAlteracao = DateTime.Now;

                    }
                    else
                    {
                        usuarioMudarSituacao.Situação = true;
                        usuarioMudarSituacao.DataAlteracao = DateTime.Now;
                    }


                    _context.Update(usuarioMudarSituacao);
                    await _context.SaveChangesAsync();

                    return usuarioMudarSituacao;
                }


                return usuarioMudarSituacao;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UsuarioModel> BuscarUsuarioPorId(int? idUsuario)
        {
            try
            {               
                var usuario = await _context.Usuarios.Include(up => up.UsuarioPermissoes).FirstOrDefaultAsync(usuario => usuario.Id == idUsuario);
                return usuario;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UsuarioModel>> BuscarUsuarios(int? id)
        {

            try
            {
                var registros = new List<UsuarioModel>();
                if (id != null)
                {
                    registros = await _context.Usuarios.Where(cliente => cliente.Id == id).ToListAsync();
                }
                else
                {
                    registros = await _context.Usuarios.ToListAsync();
                }

                return registros;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
