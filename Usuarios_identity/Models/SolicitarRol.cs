using System.ComponentModel.DataAnnotations;

namespace Usuarios_identity.Models
{
	public class SolicitarRol
	{
		public string Id { get; set; }

		[Display(Name = "Usuario")]
		public string Name { get; set; }

		[Required]
        [MinLength(100, ErrorMessage = "Se requieren al menos 100 caracteres para tu solicitud.")]
		public string Descripcion { get; set; }

        public int EstatusAtencion { get; set; }

    }
}
