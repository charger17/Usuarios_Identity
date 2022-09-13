using System.ComponentModel.DataAnnotations;

namespace Usuarios_identity.ViewModels
{
    public class OlvidoPasswordViewModel
    {
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
        [EmailAddress]
        public string Email { get; set; }

        public string code { get; set; }

    }
}
