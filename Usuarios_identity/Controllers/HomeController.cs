using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Usuarios_identity.Datos;
using Usuarios_identity.Models;

namespace Usuarios_identity.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, ApplicationDbContext _context)
        {
            _logger = logger;
            this.userManager = userManager;
            context = _context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios =  context.AppUsuario.ToList(); 
            return View();
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