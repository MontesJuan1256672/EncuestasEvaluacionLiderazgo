using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using EncuestasEvaluacionLiderazgo.Services;
using EncuestasEvaluacionLiderazgo.Models;
using EncuestasEvaluacionLiderazgo.Data;
using System.Data;

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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EncuestaController(IEncuestaService encuestaService, IRespuestaService respuestaService, IHttpContextAccessor httpContextAccessor)
        {
            _encuestaService = encuestaService;
            _respuestaService = respuestaService;
            _httpContextAccessor = httpContextAccessor;
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
        /// Obtiene el tipo de usuario actual desde la sesión
        /// </summary>
        private int GetCurrentUserType()
        {
            return HttpContext.Session.GetInt32("UserType") ?? 0;
        }

        /// <summary>
        /// Verifica si el usuario actual es administrador
        /// </summary>
        private bool IsAdmin()
        {
            return GetCurrentUserType() == (int)TipoUsuario.Administrador;
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

            // Crear instancia del IndexModel con métodos helper
            var indexModel = new Views.Encuesta.IndexModel(_httpContextAccessor);
            indexModel.CargaComboboxFiltroTipoEvaluacion();

            // Obtener elementos del card header de la primera encuesta
            Views.Encuesta.IndexModel.ElementosCardHeader elementosCardHeader = null;
            var primeraEncuesta = encuestas.FirstOrDefault();
            if (primeraEncuesta != null)
            {
                elementosCardHeader = indexModel.GetElementosCardHeader(primeraEncuesta.IdTipoEvaluacion, primeraEncuesta.Estado);
            }

            // Crear el ViewModel
            var viewModel = new EncuestaIndexViewModel
            {
                Encuestas = encuestas,
                FiltroTipoEvaluacion = "",
                TiposEvaluacion = indexModel.TiposEvaluacion,
                IndexModel = indexModel,
                ElementosCardHeader = elementosCardHeader,
                IsAdmin = IsAdmin()
            };

            return View(viewModel);
        }

        /// <summary>
        /// POST: /Encuesta
        /// Procesa las acciones POST como filtrado
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Index(string accion, string FiltroTipoEvaluacion)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            try
            {
                // Procesar según la acción recibida
                switch (accion)
                {
                    case "actualizar":
                        // Recargar los tipos de evaluación
                        break;

                    case "filtrar":
                        // Actualizar el filtro seleccionado
                        // El filtroTipoEvaluacion ya está actualizado via parámetro
                        break;

                    default:
                        break;
                }

                int userId = GetCurrentUserId();
                var encuestas = await _encuestaService.GetEncuestasAsync(userId);

                // Crear instancia del IndexModel con métodos helper
                var indexModel = new Views.Encuesta.IndexModel(_httpContextAccessor);
                indexModel.CargaComboboxFiltroTipoEvaluacion();

                // Obtener elementos del card header de la primera encuesta
                Views.Encuesta.IndexModel.ElementosCardHeader elementosCardHeader = null;
                var primeraEncuesta = encuestas.FirstOrDefault();
                if (primeraEncuesta != null)
                {
                    elementosCardHeader = indexModel.GetElementosCardHeader(Convert.ToInt32(FiltroTipoEvaluacion), primeraEncuesta.Estado);
                }

                // Crear el ViewModel
                var viewModel = new EncuestaIndexViewModel
                {
                    Encuestas = encuestas,
                    FiltroTipoEvaluacion = FiltroTipoEvaluacion,
                    TiposEvaluacion = indexModel.TiposEvaluacion,
                    IndexModel = indexModel,
                    ElementosCardHeader = elementosCardHeader,
                    IsAdmin = IsAdmin()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
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
        /// Solo administradores pueden crear encuestas
        /// </summary>
        [HttpGet("Create")]
        public IActionResult Create()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
            {
                TempData["Error"] = "Solo los administradores pueden crear encuestas.";
                return RedirectToAction("Index");
            }

            return View();
        }

        /// <summary>
        /// POST: /Encuesta/Create
        /// Crea una nueva encuesta
        /// Solo administradores pueden crear encuestas
        /// </summary>
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Encuesta encuesta)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
            {
                TempData["Error"] = "Solo los administradores pueden crear encuestas.";
                return RedirectToAction("Index");
            }

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
