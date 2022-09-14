using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Usuarios_identity.Models;

namespace Usuarios_identity.Datos
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //Agregamos modelos
        public DbSet<AppUsuario> AppUsuario { get; set; }

        public DbSet<UsuariosRoleViewModel> UsuariosRolesViewModel { get; set; }

        public DbSet<SolicitarRol> SolicitarRol { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UsuariosRoleViewModel>().HasNoKey().ToView("GetUserRole");

            base.OnModelCreating(modelBuilder);
        }
    }
}
