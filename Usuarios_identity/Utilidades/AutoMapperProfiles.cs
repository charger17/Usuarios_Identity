using AutoMapper;
using Usuarios_identity.Models;
using Usuarios_identity.ViewModels;

namespace Usuarios_identity.Utilidades
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UsuariosViewModel, AppUsuario>();
            CreateMap<AppUsuario, UsuariosViewModel>();
        }

    }
}
