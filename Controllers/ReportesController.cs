using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EncuestasEvaluacionLiderazgo.Models;
using EncuestasEvaluacionLiderazgo.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EncuestasEvaluacionLiderazgo.Controllers
{
    /// <summary>
    /// Controlador responsable de manejar los reportes de evaluación
    /// </summary>
    [Route("[controller]")]
    public class ReportesController : Controller
    {
        /// <summary>
        /// Verifica si el usuario está autenticado
        /// </summary>
        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetInt32("UserId").HasValue;
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
            return GetCurrentUserType() == (int)EncuestasEvaluacionLiderazgo.Models.TipoUsuario.Administrador;
        }

        /// <summary>
        /// GET: /Reportes
        /// Muestra la página principal del reporteador de resultados
        /// Solo administradores pueden acceder
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            // Crear el ViewModel
            var viewModel = new ReportesIndexViewModel();

            // Cargar los tipos de evaluación desde la base de datos
            try
            {
                var dataSet = FL.TraeTiposEvaluacion();

                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    // Agregar opción por defecto
                    viewModel.TiposEvaluacion.Add(new SelectListItem
                    {
                        Value = "",
                        Text = "-- Seleccionar encuesta --"
                    });

                    // Llenar con los datos de la BD
                    foreach (System.Data.DataRow row in dataSet.Tables[0].Rows)
                    {
                        viewModel.TiposEvaluacion.Add(new SelectListItem
                        {
                            Value = row["IDTipoEvaluacion"]?.ToString() ?? "",
                            Text = row["cDescripcion"]?.ToString() ?? ""
                        });
                    }
                }
                else
                {
                    viewModel.TiposEvaluacion.Add(new SelectListItem
                    {
                        Value = "",
                        Text = "-- Seleccionar encuesta --"
                    });
                }
            }
            catch (Exception ex)
            {
                // En caso de error, mostrar solo la opción por defecto
                viewModel.TiposEvaluacion.Add(new SelectListItem
                {
                    Value = "",
                    Text = "-- Todas las encuestas --"
                });
                TempData["Error"] = $"Error al cargar tipos de evaluación: {ex.Message}";
            }

            // Cargar las ciudades disponibles
            CargarCiudades();

            return View(viewModel);
        }

        /// <summary>
        /// GET: /Reportes/GetPersonasEvaluar
        /// Obtiene las personas a evaluar dinámicamente para un tipo de evaluación específico
        /// Llamado vía AJAX desde la vista
        /// </summary>
        [HttpGet]
        [Route("GetPersonasEvaluar")]
        public IActionResult GetPersonasEvaluar(string idTipoEvaluacion)
        {
            try
            {
                if (string.IsNullOrEmpty(idTipoEvaluacion))
                {
                    return Json(new { success = false, message = "ID de tipo de evaluación no proporcionado" });
                }

                
                var dataSet = FL.TraePersonasEvaluar(Convert.ToInt32(idTipoEvaluacion));
                
                List<object> personas = new List<object>();

                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    // Llenar la lista con las personas
                    foreach (System.Data.DataRow row in dataSet.Tables[0].Rows)
                    {
                        // Columnas retornadas por sp_traePersonasEvaluar: IdPersonal, NoEmp, Ciudad, Nombre, cDescripcion
                        var idPersona = row["IdPersonal"]?.ToString() ?? "";
                        var nombrePersona = row["Nombre"]?.ToString() ?? "";
                        
                        if (!string.IsNullOrEmpty(idPersona) && !string.IsNullOrEmpty(nombrePersona))
                        {
                            personas.Add(new { value = idPersona, text = nombrePersona });
                        }
                    }
                }

                if (personas.Count == 0)
                {
                    personas.Add(new { value = "", text = "Sin personas disponibles" });
                }

                return Json(new { success = true, personas = personas });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        /// <summary>
        /// GET: /Reportes/PorEncuesta
        /// Muestra reportes agrupados por encuesta
        /// </summary>
        [HttpGet]
        [Route("PorEncuesta")]
        public IActionResult PorEncuesta()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            ViewBag.TipoReporte = "Por Encuesta";
            return View("Index");
        }

        /// <summary>
        /// GET: /Reportes/Comparativo
        /// Muestra análisis comparativo de múltiples encuestas
        /// </summary>
        [HttpGet]
        [Route("Comparativo")]
        public IActionResult Comparativo()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            ViewBag.TipoReporte = "Análisis Comparativo";
            return View("Index");
        }

        /// <summary>
        /// GET: /Reportes/Competencias
        /// Muestra análisis por competencias
        /// </summary>
        [HttpGet]
        [Route("Competencias")]
        public IActionResult Competencias()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            ViewBag.TipoReporte = "Análisis de Competencias";
            return View("Index");
        }

        /// <summary>
        /// GET: /Reportes/Participantes
        /// Muestra análisis de participantes y tasas de respuesta
        /// </summary>
        [HttpGet]
        [Route("Participantes")]
        public IActionResult Participantes()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            ViewBag.TipoReporte = "Análisis de Participantes";
            return View("Index");
        }

        /// <summary>
        /// GET: /Reportes/Tendencias
        /// Muestra tendencias y evolución de competencias
        /// </summary>
        [HttpGet]
        [Route("Tendencias")]
        public IActionResult Tendencias()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            ViewBag.TipoReporte = "Tendencias y Evolución";
            return View("Index");
        }

        /// <summary>
        /// GET: /Reportes/Exportar
        /// Muestra opciones de exportación de reportes
        /// </summary>
        [HttpGet]
        [Route("Exportar")]
        public IActionResult Exportar()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            ViewBag.TipoReporte = "Exportar Datos";
            return View("Index");
        }

            /// <summary>
        /// Carga la lista de ciudades disponibles
        /// </summary>
        private void CargarCiudades()
        {
            try
            {
                var ciudades = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- Seleccione ciudad --" },
                    new SelectListItem { Value = "17", Text = "Tijuana" },
                    new SelectListItem { Value = "18", Text = "Mexicali" },
                    new SelectListItem { Value = "19", Text = "CDMX" }
                };

                ViewBag.Ciudades = ciudades;
            }
            catch (Exception ex)
            {
                ViewBag.Ciudades = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Error al cargar ciudades" }
                };
            }
        }
        
    }
}
