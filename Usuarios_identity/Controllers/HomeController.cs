using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Usuarios_identity.Datos;
using Usuarios_identity.Models;
using Usuarios_identity.ViewModels;

namespace Usuarios_identity.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, ApplicationDbContext _context, IMapper mapper)
        {
            _logger = logger;
            this.userManager = userManager;
            context = _context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string user)
        {
            var usuarios =  context.AppUsuario.ToList();

            if(user is not null)
            {
                usuarios = usuarios.Where(x => x.Nombre.Contains(user)).ToList();
            }

            var usersVm = mapper.Map<List<UsuariosViewModel>>(usuarios);
            return View(usersVm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}