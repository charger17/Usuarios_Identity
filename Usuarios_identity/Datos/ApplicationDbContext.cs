using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Usuarios_identity.Models;

namespace Usuarios_identity.Datos
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        //Agregamos modelos
        public DbSet<AppUsuario> AppUsuario { get; set; }
    }
}
