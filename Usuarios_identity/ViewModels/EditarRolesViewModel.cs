using Microsoft.AspNetCore.Mvc.Rendering;
using Usuarios_identity.Models;

namespace Usuarios_identity.ViewModels
{
    public class EditarRolesViewModel
    {

        public string Usuario { get; set; }

        public string Rol { get; set; }


        public IEnumerable<SelectListItem> RolesList { get; set; }

        public IEnumerable<SelectListItem> Usuarios { get; set; }

        public IEnumerable<UsuariosRoleViewModel> UsuariosRole { get; set; }

    }
}
