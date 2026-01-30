using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using EncuestasEvaluacionLiderazgo.Services;
using EncuestasEvaluacionLiderazgo.Models;

namespace EncuestasEvaluacionLiderazgo.Controllers
{
    /// <summary>
    /// Controlador responsable de manejar la autenticación de usuarios
    /// </summary>
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// GET: /Auth/Login
        /// Muestra la página de login
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            // Si el usuario ya está autenticado, redirigir al home
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        /// <summary>
        /// POST: /Auth/Login
        /// Procesa el login del usuario
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message, usuario) = await _authService.LoginAsync(model.Email, model.Contraseña);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(model);
            }

            // Guardar información del usuario en la sesión
            HttpContext.Session.SetInt32("UserId", usuario.Id);
            HttpContext.Session.SetString("UserName", usuario.Nombre);
            HttpContext.Session.SetInt32("UserType", (int)usuario.TipoUsuario);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// GET: /Auth/Logout
        /// Cierra la sesión del usuario
        /// </summary>
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// GET: /Auth/Register
        /// Muestra la página de registro
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// POST: /Auth/Register
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message) = await _authService.RegisterAsync(
                model.Nombre,
                model.Email,
                model.Contraseña,
                model.TipoUsuario
            );

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(model);
            }

            TempData["Success"] = "Registro exitoso. Por favor inicia sesión.";
            return RedirectToAction("Login");
        }
    }

    /// <summary>
    /// ViewModel para el formulario de login
    /// </summary>
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Contraseña { get; set; }
    }

    /// <summary>
    /// ViewModel para el formulario de registro
    /// </summary>
    public class RegisterViewModel
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public string ConfirmarContraseña { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
    }
}
