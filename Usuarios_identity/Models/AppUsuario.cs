using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Usuarios_identity.Models
{
    public class AppUsuario : IdentityUser
    {
        public string Nombre { get; set; }

        public string ApellidoPaterno { get; set; }


        public string ApellidoMaterno { get; set; }

        public string Password { get; set; }


        public string ConfirmPassword { get; set; }


        public int Telefono { get; set; }


        public string Direccion { get; set; }


        public DateTime FechaNacimiento { get; set; } = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
    }
}
