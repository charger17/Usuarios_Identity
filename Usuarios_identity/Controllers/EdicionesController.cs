using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Usuarios_identity.Datos;
using Usuarios_identity.Utilidades;
using Usuarios_identity.ViewModels;

namespace Usuarios_identity.Controllers
{
    [Authorize]
    public class EdicionesController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
		private readonly SignInManager<IdentityUser> signInManager;
		private funcionesAdicionales adds = new funcionesAdicionales();

        public EdicionesController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, ApplicationDbContext _context, IMapper mapper, SignInManager<IdentityUser> signInManager)
        {
            this.logger = logger;
            this.userManager = userManager;
            context = _context;
            this.mapper = mapper;
			this.signInManager = signInManager;
		}

        [HttpGet]
        public async Task<IActionResult> Editar(string id)
        {
            var usuario = context.AppUsuario.Where(x => x.Id.Equals(id)).FirstOrDefault();

            if(usuario is null)
            {
                return NotFound();
            }
            var retorno = mapper.Map<UsuariosViewModel>(usuario);
            retorno.FechaNacimiento = DateTime.Parse(retorno.FechaNacimiento.ToString("yyyy-MM-dd"));
            return View(retorno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(UsuariosViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var usuario = await context.AppUsuario.FindAsync(user.Id);

            usuario.Nombre = user.Nombre;
            usuario.ApellidoPaterno = user.ApellidoPaterno;
            usuario.ApellidoMaterno = user.ApellidoMaterno;
            usuario.Telefono = user.Telefono;
            usuario.Celular = user.Celular;
            usuario.Direccion = user.Direccion;
            usuario.Edad = adds.CalculaEdad(user.FechaNacimiento);

            await userManager.UpdateAsync(usuario);

            return RedirectToAction("Index", "Home");
        }

		[HttpDelete]
        public async Task<IActionResult> Borrar(string id)
		{
            if(id is null)
			{
                return BadRequest();
			}

            var usuario = await context.AppUsuario.FindAsync(id);

            var result = User.Identity.Name;

            if (result == usuario.UserName)
			{
                return Problem(statusCode: 400, title: "No se puede elimianr al usuario loggeado.");
            }

            context.AppUsuario.Remove(usuario);
            await context.SaveChangesAsync();

            return Ok();
		}

        [HttpGet]
        public IActionResult CambiarPassword(string Id)
        {
            var datos = new CambioPasswordViewModel()
            {
                Id = Id
            };
            return View(datos);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarPassword(CambioPasswordViewModel cambioPasswordViewModel)
        {
			if (!ModelState.IsValid)
			{

                return View(cambioPasswordViewModel);
			}
            var usuario =  context.AppUsuario.Where(x => x.Id.Equals(cambioPasswordViewModel.Id)).FirstOrDefault();

            var userData = await userManager.FindByEmailAsync(usuario.Email);

            var token = await userManager.GeneratePasswordResetTokenAsync(userData);

            var resultado = await userManager.ResetPasswordAsync(userData, token, cambioPasswordViewModel.Password);

            if (resultado.Succeeded)
			{
                usuario = context.AppUsuario.Where(x => x.Id.Equals(cambioPasswordViewModel.Id)).FirstOrDefault();
                usuario.Password = cambioPasswordViewModel.Password;

                await userManager.UpdateAsync(usuario);

                return RedirectToAction("Index", "Home");
			}

            return View(cambioPasswordViewModel);

        }

    }
}
