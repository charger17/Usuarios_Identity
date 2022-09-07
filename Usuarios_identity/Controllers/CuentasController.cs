using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Usuarios_identity.Models;
using Usuarios_identity.Utilidades;

namespace Usuarios_identity.Controllers
{
    public class CuentasController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private funcionesAdicionales adds = new funcionesAdicionales();

        public CuentasController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Acceso access = new Acceso();
            return View(access);
        }

        [HttpPost]
        public async Task< IActionResult> Index(Acceso access)
        {
            if (!ModelState.IsValid)
            {
                return View(access);
            }

            var resultado = await signInManager.PasswordSignInAsync(access.Email, access.Password, access.RememberMe, lockoutOnFailure:false);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Acceso Inválido");
            return View(access);

        }

        [HttpGet]
        public async Task<IActionResult> Registro()
        {
            Registro registro = new Registro();

            return View(registro);
        }

        [HttpPost]
        public async Task<IActionResult> Registro(Registro registro)
        {
            if (!ModelState.IsValid)
            {
                return View(registro);
            }

            var usuario = new AppUsuario
            {
                UserName = registro.Email,
                Email = registro.Email,
                Telefono = registro.Telefono,
                Celular = registro.Celular,
                Password = registro.Password,
                Nombre = registro.Nombre,
                ApellidoPaterno = registro.ApellidoPaterno,
                ApellidoMaterno = registro.ApellidoMaterno,
                FechaNacimiento = registro.FechaNacimiento,
                Direccion = registro.Direccion,
                Edad = adds.CalculaEdad(registro.FechaNacimiento)
            };

            if(usuario.Edad <= 18)
            {
                ModelState.AddModelError(string.Empty, "Edad no válida.");
                return View(registro);
            }

            var resultado = await userManager.CreateAsync(usuario, registro.Password);

            if (resultado.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }
            ValidarErrores(resultado);
            return View(registro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalirAplicacion()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Cuentas");
        }


        private void ValidarErrores(IdentityResult resultado)
        {
            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(String.Empty, error.Description); //Manejador de errores
            }
        }
    }
}
