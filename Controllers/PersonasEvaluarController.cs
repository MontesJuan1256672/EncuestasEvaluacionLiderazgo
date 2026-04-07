using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using EncuestasEvaluacionLiderazgo.Models;
using EncuestasEvaluacionLiderazgo.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncuestasEvaluacionLiderazgo.Controllers
{
    /// <summary>
    /// Controlador responsable de manejar la gestión de personas a evaluar
    /// </summary>
    public class PersonasEvaluarController : Controller
    {
        private List<PersonaEvaluar> personasEvaluar = new List<PersonaEvaluar>();

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
            return GetCurrentUserType() == (int)TipoUsuario.Administrador;
        }

        /// <summary>
        /// GET: /PersonasEvaluar
        /// Muestra la lista de personas a evaluar
        /// Solo administradores pueden acceder
        /// </summary>
        [HttpGet]
        public IActionResult Index(string idTipoEvaluacion = "")
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            // Obtener lista de personas a evaluar desde la base de datos
            // Usar el parámetro idTipoEvaluacion del combobox para filtrar
            // Si es nulo o 0, usar una lista vacía
            int idTipoEvaluacionInt = 0;
            if (!string.IsNullOrWhiteSpace(idTipoEvaluacion) && int.TryParse(idTipoEvaluacion, out int parsedId) && parsedId > 0)
            {
                idTipoEvaluacionInt = parsedId;
            }
            
            var personas = new List<PersonaEvaluar>();

            // Solo hacer la llamada a la BD si hay un tipo de evaluación válido
            if (idTipoEvaluacionInt > 0)
            {
                try
                {
                    var ds = FL.TraePersonasEvaluar(idTipoEvaluacionInt);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                        {
                            personas.Add(new PersonaEvaluar
                            {
                                IDPersona = row["IDPersonal"] != System.DBNull.Value ? Convert.ToInt32(row["IDPersonal"]) : 0,
                                NumeroEmpleado = row["NoEmp"] != System.DBNull.Value ? row["NoEmp"].ToString() : "",
                                Nombre = row["Nombre"] != System.DBNull.Value ? row["Nombre"].ToString() : "",
                                Ciudad = row["Ciudad"] != System.DBNull.Value ? row["Ciudad"].ToString() : "",
                                Puesto = row["cDescripcion"] != System.DBNull.Value ? row["cDescripcion"].ToString() : "",
                                Activo = true  // La columna Activo no viene en el resultado del SP, siempre es true
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al cargar personas: {ex.Message}";
                }
            }
            
            // Cargar tipos de evaluación para el filtro combobox
            ViewBag.TiposEvaluacion = ObtenerTiposEvaluacion();
            // Guardar el filtro actual en ViewBag para que el combobox muestre el valor seleccionado
            ViewBag.FiltroTipoEvaluacionActual = idTipoEvaluacion;

            return View(personas);
        }

        /// <summary>
        /// GET: /PersonasEvaluar/Create
        /// Muestra el formulario para agregar una nueva persona a evaluar
        /// </summary>
        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            var viewModel = new PersonaEvaluar();
            CargarCiudades();
            CargarTiposEvaluacion();

            return View(viewModel);
        }

        /// <summary>
        /// POST: /PersonasEvaluar/Create
        /// Agrega una nueva persona a evaluar
        /// </summary>
        [HttpPost]
        [Route("Create")]
        public IActionResult Create(PersonaEvaluar model)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            if (!ModelState.IsValid)
            {
                CargarCiudades();
                CargarTiposEvaluacion();
                return View(model);
            }

            try
            {
                // TODO: Cuando la tabla esté lista en BD, reemplazar con:
                // bool resultado = FL.InsertaPersonaEvaluar(model);
                // if (resultado)
                // {
                //     TempData["Success"] = "Persona agregada exitosamente";
                //     return RedirectToAction("Index");
                // }
                // else
                // {
                //     TempData["Error"] = "Error al agregar la persona";
                // }

                TempData["Success"] = "Persona agregada exitosamente (datos en memoria)";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al agregar la persona: {ex.Message}";
                CargarCiudades();
                CargarTiposEvaluacion();
                return View(model);
            }
        }

        /// <summary>
        /// GET: /PersonasEvaluar/Edit/{id}
        /// Redirige a Create (Edit no es necesario, usar Create para nueva asignación)
        /// </summary>
        [HttpGet]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            return RedirectToAction("Create");
        }

        /// <summary>
        /// POST: /PersonasEvaluar/Edit/{id}
        /// Redirige a Create (Edit no es necesario, usar Create para nueva asignación)
        /// </summary>
        [HttpPost]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id, PersonaEvaluar model)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            return RedirectToAction("Create");
        }

        /// <summary>
        /// POST: /PersonasEvaluar/Delete/{id}
        /// Elimina una persona a evaluar
        /// </summary>
        [HttpPost]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            try
            {
                int resultado = FL.EliminaPersona(id);
                if (resultado > 0)
                {
                    TempData["Success"] = "Persona eliminada exitosamente";
                }
                else
                {
                    TempData["Error"] = "La persona no fue encontrada o ya estaba eliminada";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar la persona: {ex.Message}";
                return RedirectToAction("Index");
            }
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

        /// <summary>
        /// Carga los tipos de evaluación desde la base de datos
        /// </summary>
        private void CargarTiposEvaluacion()
        {
            ViewBag.TiposEvaluacion = ObtenerTiposEvaluacion();
        }

        /// <summary>
        /// Obtiene la lista de tipos de evaluación desde la base de datos
        /// </summary>
        private List<SelectListItem> ObtenerTiposEvaluacion()
        {
            try
            {
                var tiposEvaluacion = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- Seleccione Puesto --" }
                };

                var dstipos = FL.TraeTiposEvaluacion();
                if (dstipos != null && dstipos.Tables.Count > 0 && dstipos.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow row in dstipos.Tables[0].Rows)
                    {
                        tiposEvaluacion.Add(new SelectListItem
                        {
                            Value = row["IDTipoEvaluacion"]?.ToString() ?? "",
                            Text = row["cDescripcion"]?.ToString() ?? ""
                        });
                    }
                }

                return tiposEvaluacion;
            }
            catch (Exception ex)
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Error al cargar tipos de evaluación: " + ex.Message }
                };
            }
        }

        /// <summary>
        /// GET: /PersonasEvaluar/GetEmpleado
        /// Busca la información de un empleado por su número y ciudad
        /// Retorna JSON con el nombre del empleado
        /// </summary>
        [HttpGet]
        [Route("GetEmpleado")]
        public IActionResult GetEmpleado(string numeroEmpleado, string ciudad)
        {
            if (!IsAuthenticated())
                return Unauthorized(new { success = false, message = "No autenticado" });

            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(numeroEmpleado))
                    return Json(new { success = false, message = "Número de empleado vacío" });

                if (string.IsNullOrWhiteSpace(ciudad))
                    return Json(new { success = false, message = "Ciudad no seleccionada" });

                // Convertir string a int
                if (!int.TryParse(numeroEmpleado, out int noemp))
                    return Json(new { success = false, message = "Número de empleado inválido" });

                if (!int.TryParse(ciudad, out int idUbicacionInt))
                    return Json(new { success = false, message = "ID de ciudad inválido" });

                // Mapeo de ID de ubicación a nombre de ciudad
                var mapaCiudades = new Dictionary<int, string>
                {
                    { 17, "Tijuana" },
                    { 18, "Mexicali" },
                    { 19, "CDMX" }
                };

                // Verificar que la ubicación sea válida
                if (!mapaCiudades.TryGetValue(idUbicacionInt, out var nombreCiudad))
                    return Json(new { success = false, message = "Ciudad no válida" });

                // Llamar al método FL.BuscarEmpleado
                var dtEmpleado = FL.BuscarEmpleado(noemp, idUbicacionInt);

                // Validar si se encontró el empleado
                if (dtEmpleado == null || dtEmpleado.Rows.Count == 0)
                    return Json(new { success = false, message = "Empleado no encontrado" });

                // Extraer datos del empleado
                var row = dtEmpleado.Rows[0];
                int idpersonal = row["IDPersonal"] != DBNull.Value ? Convert.ToInt32(row["IDPersonal"]) : 0;
                string nombreCompleto = row["Nombre"]?.ToString() ?? "Sin nombre";

                return Json(new { success = true, nombre = nombreCompleto, numeroEmpleado = numeroEmpleado, ciudad = nombreCiudad, idPersonal = idpersonal });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        /// <summary>
        /// POST: /PersonasEvaluar/GuardarPersona
        /// Guarda una nueva relación de tipo de encuesta y personal
        /// Llamado vía AJAX desde el botón guardar
        /// </summary>
        [HttpPost]
        [Route("GuardarPersona")]
        public IActionResult GuardarPersona(int idTipoEncuesta, int idPersonal)
        {
            if (!IsAuthenticated())
                return Unauthorized(new { success = false, message = "No autenticado" });

            if (!IsAdmin())
                return Json(new { success = false, message = "No tienes permisos para realizar esta acción" });

            try
            {
                // Validaciones
                if (idTipoEncuesta <= 0)
                    return Json(new { success = false, message = "Tipo de encuesta no seleccionado" });

                if (idPersonal <= 0)
                    return Json(new { success = false, message = "Personal no seleccionado" });

                // Llamar a FL.InsertaPorIdPersonal
                int resultado = FL.InsertaPorIdPersonal(idTipoEncuesta, idPersonal);

                if (resultado > 0)
                {
                    return Json(new 
                    { 
                        success = true, 
                        message = "Persona guardada exitosamente",
                        id = resultado
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Error al guardar la relación en la base de datos" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al guardar: {ex.Message}" });
            }
        }
    }
}
