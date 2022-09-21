using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Usuarios_identity.Models
{
    public class ComentarioAlAdministrador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdNo { get; set; }

        public string Id { get; set; }

        public string Email { get; set; }

        [Required]
        [Display(Name = "Escriba sus comentarios o sugerencias para la administración: ")]
        public string Comentario { get; set; }
    }
}
