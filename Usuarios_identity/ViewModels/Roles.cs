using System.ComponentModel.DataAnnotations.Schema;

namespace Usuarios_identity.ViewModels
{
    public class Roles
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}
