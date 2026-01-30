using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using EncuestasEvaluacionLiderazgo.Services;
using EncuestasEvaluacionLiderazgo.Models;

namespace EncuestasEvaluacionLiderazgo.Controllers
{
    /// <summary>
    /// Controlador responsable de manejar las operaciones de encuestas
    /// </summary>
    [Route("[controller]")]
    public class EncuestaController : Controller
    {
        private readonly IEncuestaService _encuestaService;
        private readonly IRespuestaService _respuestaService;

        public EncuestaController(IEncuestaService encuestaService, IRespuestaService respuestaService)
        {
            _encuestaService = encuestaService;
            _respuestaService = respuestaService;
        }

        /// <summary>
        /// Verifica si el usuario está autenticado
        /// </summary>
        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetInt32("UserId").HasValue;
        }

        /// <summary>
        /// Obtiene el ID del usuario actual desde la sesión
        /// </summary>
        private int GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId") ?? 0;
        }

        /// <summary>
        /// GET: /Encuesta
        /// Muestra la lista de encuestas del usuario actual
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            int userId = GetCurrentUserId();
            var encuestas = await _encuestaService.GetEncuestasAsync(userId);

            return View(encuestas);
        }

        /// <summary>
        /// GET: /Encuesta/Details/{id}
        /// Muestra los detalles de una encuesta para responder
        /// </summary>
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var encuesta = await _encuestaService.GetEncuestaByIdAsync(id);

            if (encuesta == null)
                return NotFound();

            return View(encuesta);
        }

        /// <summary>
        /// POST: /Encuesta/Submit
        /// Guarda las respuestas de una encuesta
        /// </summary>
        [HttpPost("Submit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int encuestaId, [FromForm] Dictionary<string, string> respuestas)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            int userId = GetCurrentUserId();
            var respuesta = new Respuesta
            {
                EncuestaId = encuestaId,
                UsuarioId = userId,
                Completada = true
            };

            var (success, message) = await _respuestaService.SaveRespuestaAsync(respuesta);

            if (!success)
            {
                TempData["Error"] = message;
                return RedirectToAction("Details", new { id = encuestaId });
            }

            TempData["Success"] = "Encuesta respondida correctamente.";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET: /Encuesta/Create
        /// Muestra el formulario para crear una nueva encuesta
        /// </summary>
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        /// <summary>
        /// POST: /Encuesta/Create
        /// Crea una nueva encuesta
        /// </summary>
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Encuesta encuesta)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(encuesta);

            encuesta.UsuarioCreadorId = GetCurrentUserId();

            var (success, message, encuestaId) = await _encuestaService.CreateEncuestaAsync(encuesta);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(encuesta);
            }

            TempData["Success"] = "Encuesta creada correctamente.";
            return RedirectToAction("Edita", new { id = encuestaId });
        }
    }
}
