using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Usuarios_identity.Datos;
using Usuarios_identity.Utilidades;
using Usuarios_identity.ViewModels;
using System.Collections.Generic;
using Usuarios_identity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        private readonly RoleManager<IdentityRole> roleManager;
        private funcionesAdicionales adds = new funcionesAdicionales();

        public EdicionesController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, ApplicationDbContext _context, IMapper mapper, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.logger = logger;
            this.userManager = userManager;
            context = _context;
            this.mapper = mapper;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Editar(string id)
        {
            var usuario = context.AppUsuario.Where(x => x.Id.Equals(id)).FirstOrDefault();

            if (usuario is null)
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
            if (id is null)
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarPassword(CambioPasswordViewModel cambioPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {

                return View(cambioPasswordViewModel);
            }
            var usuario = context.AppUsuario.Where(x => x.Id.Equals(cambioPasswordViewModel.Id)).FirstOrDefault();

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

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult EditarRoles(string error = null)
        {
            var EditRoles = setList();

            if(error != null)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return View(EditRoles);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarRoles(AsignarRol asignar)
        {

            if (!ModelState.IsValid)
            {
                var EditRoles = setList();
                return View(EditRoles);
            }

            var IdUser = asignar.Usuario;

            var roleName = asignar.Rol;

            if (IdUser is null || roleName is null)
            {
                ModelState.AddModelError(string.Empty, "No se puede agregar al rol actual.");
                var EditRoles = setList();
                return View(EditRoles);
            }

            var usuario = await userManager.FindByIdAsync(IdUser);

            var roles = await userManager.GetRolesAsync(usuario);

            foreach (var item in roles)
            {
                await userManager.RemoveFromRoleAsync(usuario, item);
            }

            await userManager.AddToRoleAsync(usuario, roleName);


            return RedirectToAction("EditarRoles", "Ediciones");
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string rol)
        {
            if(rol is null)
            {
                return RedirectToAction("EditarRoles", "Ediciones", new { error = "Debe agregar un rol para crear." });
            }

            var res = await roleManager.RoleExistsAsync(rol);

            if (res)
            {
                return RedirectToAction("EditarRoles", "Ediciones", new {error = "El rol ya existe"});
            }

            await roleManager.CreateAsync(new IdentityRole (rol));

            return View(setList());
        }

        [HttpGet]
        public async Task<IActionResult> Solicitar(string Name)
        {

            var data = context.SolicitarRol.Where(x => x.Name.Equals(Name) && x.EstatusAtencion == 0).FirstOrDefault();

            if(data is not null)
            {
                return RedirectToAction("Listo", "Ediciones", new MensajesViewModel { titulo = "Error", mensaje = "Ya cuentas con una solicitud." });
            }

            var usuario = await userManager.FindByEmailAsync(Name);

            SolicitarRol sol = new SolicitarRol()
            {
                Id = usuario.Id,
                Name = usuario.Email
            };

            return View(sol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Solicitar(SolicitarRol solicitar)
        {

            if (!ModelState.IsValid)
            {
                return View(solicitar);
            }


            if(solicitar is null)
            {
                return View(solicitar);
            }

            var usuarios = await userManager.FindByIdAsync(solicitar.Id);
            solicitar.EstatusAtencion = 0;
            solicitar.Name = usuarios.Email;
            await context.SolicitarRol.AddAsync(solicitar);
            context.SaveChanges();
            return RedirectToAction("Listo", "Ediciones", new MensajesViewModel { titulo = "Listo", mensaje = "Su solicitud será enviada a un administrador."});
        }

        [HttpGet]
        public IActionResult Listo(MensajesViewModel mensajes)
        {
            return View(mensajes);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult VerSolicitudes()
        {
            var soliciudes = context.SolicitarRol.ToList();


            return View(soliciudes);
        }


        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Diss(string id)
        {
            var solicitud = await context.SolicitarRol.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            context.SolicitarRol.Remove(solicitud);
            await context.SaveChangesAsync();

            return RedirectToAction("VerSolicitudes", "Ediciones");
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> appro(string id)
        {
            var usuario = await userManager.FindByIdAsync(id);

            var roles = await userManager.GetRolesAsync(usuario);

            foreach (var item in roles)
            {
                await userManager.RemoveFromRoleAsync(usuario, item);
            }

            await userManager.AddToRoleAsync(usuario, "Administrador");

            var solicitud = await context.SolicitarRol.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            context.SolicitarRol.Remove(solicitud);
            await context.SaveChangesAsync();


            return RedirectToAction("VerSolicitudes", "Ediciones");
        }


        [HttpGet]
        [Authorize(Roles="Registrado")]
        public async Task<IActionResult> EnviarComentarios(string Name)
        {

            var user = await userManager.FindByEmailAsync(Name);

            ComentarioAlAdministrador coments = new ComentarioAlAdministrador()
            {
                Id = user.Id,
                Email = user.Email
            };


            return View(coments);
        }

        [HttpPost]
        [Authorize(Roles = "Registrado")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnviarComentarios(ComentarioAlAdministrador comentarios)
        {
            if (!ModelState.IsValid)
            {
                return View(comentarios);
            }

            await context.AddAsync(comentarios);
            context.SaveChanges();

            return RedirectToAction("Listo", "Ediciones", new MensajesViewModel { titulo = "Listo", mensaje = "Sus comentarios han sido enviados al equipo administrativo." });
        }


        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> VerComentarios()
        {
            var coments = context.ComentarioAlAdministrador.ToList();

            return View(coments);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Read(int id)
        {
            var coment = await context.ComentarioAlAdministrador.Where(x => x.IdNo.Equals(id)).FirstOrDefaultAsync();

            context.Remove(coment);
            await context.SaveChangesAsync();
            return RedirectToAction("VerComentarios", "Ediciones");
        }


        private EditarRolesViewModel setList()
        {
            EditarRolesViewModel EditRoles = new EditarRolesViewModel();

            var roles = roleManager.Roles.ToList();
            var users = context.AppUsuario.ToList();
            var data = context.UsuariosRolesViewModel.ToList();


            List<SelectListItem> listaRoles = new List<SelectListItem>();
            List<SelectListItem> listaUsers = new List<SelectListItem>();

            foreach (var item in roles)
            {
                listaRoles.Add(new SelectListItem
                {
                    Value = item.Name,
                    Text = item.Name
                });
            }

            foreach (var item in users)
            {
                listaUsers.Add(new SelectListItem
                {
                    Value = item.Id,
                    Text = item.Email
                });
            }

            EditRoles.RolesList = listaRoles;
            EditRoles.Usuarios = listaUsers;
            EditRoles.UsuariosRole = data;

            return EditRoles;
        }
    }
}
