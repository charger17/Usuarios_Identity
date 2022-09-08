using System.ComponentModel.DataAnnotations;

namespace Usuarios_identity.ViewModels
{
    public class CambioPasswordViewModel
    {
		public string Id { get; set; }

		[Required(ErrorMessage = "El campo {0} es requerido")]
		[DataType(DataType.Password)]
		[Display(Name = "Nueva Contraseña")]
		public string Password { get; set; }

		[Required(ErrorMessage = "El campo {0} es requerido")]
		[DataType(DataType.Password)] 
		[Display(Name = "Confirme Nueva Contraseña")]
		[Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
		public string ConfirmPassword { get; set; }
	}
}
