using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Usuarios_identity.Datos;
using Usuarios_identity.Models;
using Usuarios_identity.Servicios;
using Usuarios_identity.Utilidades;
using Usuarios_identity.ViewModels;

namespace Usuarios_identity.Controllers
{
    public class CuentasController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext contexto;
        private readonly IEmailSender emailSender;
        private readonly RoleManager<IdentityRole> roleManager;
        private funcionesAdicionales adds = new funcionesAdicionales();

        public CuentasController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext contexto, IEmailSender emailSender, RoleManager<IdentityRole> roleManager )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.contexto = contexto;
            this.emailSender = emailSender;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index(string returnurl=null)
        {
            ViewData["ReturnUrl"] = returnurl;
            Acceso access = new Acceso();
            return View(access);
        }

        [HttpPost]
        public async Task< IActionResult> Index(Acceso access, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            if (!ModelState.IsValid)
            {
                return View(access);
            }

            var resultado = await signInManager.PasswordSignInAsync(access.Email, access.Password, access.RememberMe, lockoutOnFailure:false);

            if (resultado.Succeeded)
            {
                if(returnurl.Length > 2)
				{
                    if(returnurl.Contains("Den"))

                    return Redirect(returnurl);
                }
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Acceso Inválido");
            return View(access);

        }

        [HttpGet]
        public async Task<IActionResult> Registro()
        {
            Registro registro = new Registro();

            //Para la creacion de los roles

            if (!await roleManager.RoleExistsAsync("Administrador"))
            {
                //Creacion del rol usuairo Administrador
                await roleManager.CreateAsync(new IdentityRole("Administrador"));
            }

            if (!await roleManager.RoleExistsAsync("Registrado"))
            {
                //Creacion del rol usuairo registrado
                await roleManager.CreateAsync(new IdentityRole("Registrado"));
            }



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
                await userManager.AddToRoleAsync(usuario, "Registrado");

                await signInManager.SignInAsync(usuario, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }
            ValidarErrores(resultado);
            return View(registro);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalirAplicacion()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Cuentas");
        }


        [HttpGet]
        public IActionResult RecuperarContrasenia()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecuperarContrasenia(Acceso acceso)
        {
            if (ModelState.IsValid)
            {
                return View(acceso);
            }

            var resultado = await userManager.FindByEmailAsync(acceso.Email);

            if (resultado is null)
            {
                ModelState.AddModelError(string.Empty, "No existe el usuario.");
                return View(acceso);
            }

            var codigo = await userManager.GeneratePasswordResetTokenAsync(resultado);

            var urlRetorno = Url.Action("OlvidoPassword", "Cuentas", new { userId = resultado.Id, code = codigo }, protocol: HttpContext.Request.Scheme);

            await emailSender.SendEmailAsync(acceso.Email, "Recuperar Contraseña - Usuarios Idendity" ,"Por favor confirme su cuenta dando clic aqui: " + urlRetorno + "&email=" + resultado.Email);

            return RedirectToAction("ConfirmacionOlvidoPassword", "Cuentas");
        }

        [HttpGet]
        public IActionResult OlvidoPassword(string code, string email)
        {
            OlvidoPasswordViewModel ovps = new OlvidoPasswordViewModel()
            {
                Email = email
            };
            return code == null ? View("Error", "Cuentas"):View(ovps);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OlvidoPassword(OlvidoPasswordViewModel olvidoPassword )
        {
            if (!ModelState.IsValid)
            {

                return View(olvidoPassword);
            }


            var usuario = await userManager.FindByEmailAsync(olvidoPassword.Email);

            if (usuario is null)
            {
                return RedirectToAction("Error", "Cuentas");
            }

            var resultado = await userManager.ResetPasswordAsync(usuario, olvidoPassword.code, olvidoPassword.Password);

            if (resultado.Succeeded)
            {
                var usuarioDos = contexto.AppUsuario.Where(x => x.Email.Equals(olvidoPassword.Email)).FirstOrDefault();
                usuarioDos.Password = olvidoPassword.Password;

                await userManager.UpdateAsync(usuarioDos);

                return RedirectToAction("Index", "Home");
            }

            return View(olvidoPassword);
        }
    

        [HttpGet]
        public IActionResult ConfirmacionOlvidoPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
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
