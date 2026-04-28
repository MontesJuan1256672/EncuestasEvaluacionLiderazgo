using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using EncuestasEvaluacionLiderazgo.Services;
using EncuestasEvaluacionLiderazgo.Models;
using EncuestasEvaluacionLiderazgo.Data;
using EncuestasEvaluacionLiderazgo.Utilities;
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

        /// <summary>
        /// ID del tipo de evaluación (filtro) que se recibe como parámetro
        /// </summary>
        public string IdEncuesta { get; set; }

        public EncuestaController(IEncuestaService encuestaService, IRespuestaService respuestaService, IHttpContextAccessor httpContextAccessor)
        {
            _encuestaService = encuestaService;
            _respuestaService = respuestaService;
            _httpContextAccessor = httpContextAccessor;
            IdEncuesta = string.Empty;
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
        /// Obtiene las preguntas de un tipo de evaluación específico
        /// </summary>
        /// <param name="idEncuesta">ID del tipo de evaluación</param>
        /// <returns>DataSet con las preguntas</returns>
        private DataSet TraePreguntasII(string idEncuesta)
        {
            if (string.IsNullOrEmpty(idEncuesta))
            {
                return new DataSet();
            }

            try
            {
                return FL.TraePreguntasII(idEncuesta);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al obtener preguntas: {ex.Message}";
                return new DataSet();
            }
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

            if (!IsAdmin())
                return RedirectToAction("Login", "Auth");

            int userId = GetCurrentUserId();
            var encuestas = await _encuestaService.GetEncuestasAsync(userId);

            // Obtener preguntas por tipo de evaluación
            var preguntas = TraePreguntasII(IdEncuesta);

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

            // Asignar el filtro a IdEncuesta
            IdEncuesta = FiltroTipoEvaluacion ?? string.Empty;

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

                // Obtener preguntas por tipo de evaluación
                var preguntas = TraePreguntasII(IdEncuesta);

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
        public async Task<IActionResult> Details(int id, string filtroTipo = "")
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            // Asignar el filtroTipo a la propiedad IdEncuesta
            IdEncuesta = filtroTipo ?? string.Empty;

            var encuesta = await _encuestaService.GetEncuestaByIdAsync(id);

            if (encuesta == null)
                return NotFound();

            // Procesar las preguntas en el controlador (lógica de negocio)
            var detailsModel = new EncuestasEvaluacionLiderazgo.Views.Encuesta.DetailsModel();
            var preguntasDataSet = detailsModel.ObtenerPreguntasDeBaseDatos(filtroTipo);
            var filasPreguntas = detailsModel.ObtenerFilasPreguntasOrdenadas(preguntasDataSet);

            // Mapear DataRows a List<PreguntaViewModel>
            var preguntas = new List<PreguntaViewModel>();
            int numeroOrden = 1;

            foreach (var fila in filasPreguntas)
            {
                try
                {
                    int idPregunta = Convert.ToInt32(fila["IdPregunta"] ?? 0);
                    string textoPregunta = detailsModel.ObtenerTextoPregunta(fila);
                    string textoPreguntaIngles = detailsModel.ObtenerTextoPreguntaIngles(fila);

                    preguntas.Add(new PreguntaViewModel
                    {
                        IdPregunta = idPregunta,
                        NumeroPregunta = numeroOrden,
                        Texto = textoPregunta,
                        TextoIngles = textoPreguntaIngles
                    });

                    numeroOrden++;
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al procesar pregunta: {ex.Message}";
                }
            }

            // Llenar el ViewModel con todos los datos procesados
            var viewModel = new EncuestaDetailsViewModel
            {
                Encuesta = encuesta,
                FiltroTipo = filtroTipo ?? "",
                TituloEncuesta = detailsModel.GetTituloEncuesta(filtroTipo),
                Preguntas = preguntas
            };

            return View(viewModel);
        }

        /// <summary>
        /// POST: /Encuesta/Submit
        /// Guarda las respuestas de una encuesta
        /// </summary>
        [HttpPost("Submit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int encuestaId, string filtroTipo = "", [FromForm] Dictionary<string, string> respuestas = null)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            // Asignar el filtroTipo a la propiedad IdEncuesta
            IdEncuesta = filtroTipo ?? string.Empty;

            int userId = GetCurrentUserId();
            string idPersonal = SessionHelper.GetIdPersonal(HttpContext.Session);
            string idCentro = SessionHelper.GetIdCentro(HttpContext.Session);
            string noEmp = SessionHelper.GetNoEmp(HttpContext.Session);

            // Extraer personaEvaluar, nombrePersonaEvaluar y tiempoConPersona del diccionario de respuestas
            string idPersonalEvaluado = respuestas != null && respuestas.TryGetValue("personaEvaluar", out string personaEvaluar) ? personaEvaluar : string.Empty;
            string nombrePersonaEvaluar = respuestas != null && respuestas.TryGetValue("nombrePersonaEvaluar", out string nombreEvaluado) ? nombreEvaluado : string.Empty;
            string tiempoConPersona = respuestas != null && respuestas.TryGetValue("tiempoConPersona", out string tiempo) ? tiempo : string.Empty;
            string comentarioGeneral = respuestas != null && respuestas.TryGetValue("comentarioGeneral", out string comentario) ? comentario : string.Empty;

            var respuesta = new Respuesta
            {
                IdTipoEvaluacion = Convert.ToInt32(filtroTipo),
                IdCentroDWH = idCentro,
                IDPersonalDWH_Evaluado = int.TryParse(idPersonalEvaluado, out int idEvaluado) ? idEvaluado : 0,
                IDPersonalDWH_Jefe = FL.IdPersonalDWHJefeDeEvaluado(Convert.ToInt32(idPersonalEvaluado)), // int.TryParse(idPersonal, out int idJefe) ? idJefe : 0,
                cNombreEvaluado = nombrePersonaEvaluar,
                cComentarios = comentarioGeneral,
                nNoEmpAgente = decimal.TryParse(noEmp, out decimal nEmp) ? nEmp : 0,
                IDAntig = int.TryParse(tiempoConPersona, out int idAntig) ? idAntig : 0
            };

            var (success, message) = await _respuestaService.SaveRespuestaAsync(respuesta);

            // Insertar cada respuesta individual por pregunta
            if (success && respuestas != null)
            {
                // Extraer el IdEvaluacion del mensaje (formato: "Evaluación guardada exitosamente. ID: {id}")
                string idEvalStr = message.Contains("ID: ") ? message.Substring(message.LastIndexOf("ID: ") + 4).Trim() : "0";
                int idEvaluacion = int.TryParse(idEvalStr, out int idEval) ? idEval : 0;

                foreach (var kvp in respuestas)
                {
                    // Filtrar solo los campos de respuesta (respuesta_{IdPregunta})
                    if (kvp.Key.StartsWith("respuesta_") && !string.IsNullOrEmpty(kvp.Value))
                    {
                        string idPreguntaStr = kvp.Key.Replace("respuesta_", "");
                        int idPregunta = int.TryParse(idPreguntaStr, out int idPreg) ? idPreg : 0;
                        int nRespuesta = int.TryParse(kvp.Value, out int nResp) ? nResp : 0;

                        // Buscar el comentario correspondiente a esta pregunta
                        string comentarioPregunta = respuestas.TryGetValue($"comentario_{idPreguntaStr}", out string coment) ? coment : "";

                        FL.InsertaRespuesta(idEvaluacion, idPregunta, nRespuesta, comentarioPregunta);
                    }
                }

                // Registrar que el usuario contestó la encuesta
                FL.InseRegistro(int.TryParse(idPersonal, out int idPers) ? idPers : 0);
            }

            if (!success)
            {
                TempData["Error"] = message;
                return RedirectToAction("Details", new { id = encuestaId, filtroTipo = filtroTipo });
            }

            TempData["Success"] = "Encuesta respondida correctamente.";

            // Volver a Index con el filtro si se proporcionó
            if (!string.IsNullOrEmpty(filtroTipo))
            {
                return RedirectToAction("Index", new { FiltroTipoEvaluacion = filtroTipo });
            }

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
