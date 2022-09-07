using System.ComponentModel.DataAnnotations;

namespace Usuarios_identity.ViewModels
{
    public class UsuariosViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "No es un numero de telefono")]
        [MaxLength(10)]
        public string Telefono { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "No es un numero de telefono")]
        [MaxLength(10)]
        public string Celular { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
		[Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; } = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

        public int Edad { get; set; }
    }
}
