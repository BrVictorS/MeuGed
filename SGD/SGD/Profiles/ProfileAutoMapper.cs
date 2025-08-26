using AutoMapper;
using SGD.Dtos.Projeto;
using SGD.Dtos.Usuarios;
using SGD.Models;

namespace SGD.Profiles
{
    public class ProfileAutoMapper:Profile
    {
        public ProfileAutoMapper() 
        {
            CreateMap<UsuarioModel, UsuarioEditarDto>();
            CreateMap<UsuarioEditarDto, UsuarioModel>();
            CreateMap<ProjetoEditarDto, ProjetoModel>();
            CreateMap<UsuarioModel, UsuarioDto>();
        }
    }
}
