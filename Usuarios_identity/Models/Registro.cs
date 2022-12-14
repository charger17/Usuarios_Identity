using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Usuarios_identity.Models
{
    [NotMapped]
    public class Registro
    {

        #region Variables NO Requridas
        [DataType(DataType.PhoneNumber, ErrorMessage = "No es un numero de telefono")]
        [MaxLength(10)]
        public string Telefono { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "No es un numero de telefono")]
        [MaxLength(10)]
        public string Celular { get; set; }
        #endregion

        #region Variables Requridas
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Las contraseñas no coinciden")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Direccion { get; set; }


        [Required(ErrorMessage = "El campo {0} es obligatorio")]
		[Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; } = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));


        public int Edad { get; set; }
        #endregion



    }
}
