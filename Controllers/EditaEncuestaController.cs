using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using EncuestasEvaluacionLiderazgo.Services;
using EncuestasEvaluacionLiderazgo.Models;
using EncuestasEvaluacionLiderazgo.Data;

namespace EncuestasEvaluacionLiderazgo.Controllers
{
    /// <summary>
    /// Controlador responsable de manejar la edición de encuestas
    /// </summary>
    [Route("[controller]")]
    public class EditaEncuestaController : Controller
    {
        private readonly IEncuestaService _encuestaService;

        public EditaEncuestaController(IEncuestaService encuestaService)
        {
            _encuestaService = encuestaService;
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
        /// GET: /EditaEncuesta/{id}
        /// Muestra el formulario para editar una encuesta existente
        /// Solo administradores pueden editar encuestas
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id, string filtroTipo = "")
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            var encuesta = await _encuestaService.GetEncuestaByIdAsync(id);

            if (encuesta == null)
                return NotFound();

            // Verificar que el usuario sea el creador de la encuesta
            if (encuesta.UsuarioCreadorId != GetCurrentUserId())
                return Forbid();

            // Pasar filtroTipo a la vista mediante ViewBag
            ViewBag.FiltroTipo = filtroTipo;

            return View(encuesta);
        }

        /// <summary>
        /// POST: /EditaEncuesta/Update
        /// Actualiza una encuesta existente
        /// Solo administradores pueden editar encuestas
        /// </summary>
        [HttpPost("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Encuesta encuesta)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            if (!ModelState.IsValid)
                return View("Index", encuesta);

            // Verificar que el usuario sea el creador
            var encuestaExistente = await _encuestaService.GetEncuestaByIdAsync(encuesta.Id);
            if (encuestaExistente.UsuarioCreadorId != GetCurrentUserId())
                return Forbid();

            var (success, message) = await _encuestaService.UpdateEncuestaAsync(encuesta);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return View("Index", encuesta);
            }

            TempData["Success"] = "Encuesta actualizada correctamente.";
            return RedirectToAction("Index", "Encuesta");
        }

        /// <summary>
        /// POST: /EditaEncuesta/Publish
        /// Publica una encuesta
        /// Solo administradores pueden publicar encuestas
        /// </summary>
        [HttpPost("Publish")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            var encuesta = await _encuestaService.GetEncuestaByIdAsync(id);
            if (encuesta == null)
                return NotFound();

            if (encuesta.UsuarioCreadorId != GetCurrentUserId())
                return Forbid();

            var (success, message) = await _encuestaService.PublishEncuestaAsync(id);

            if (!success)
            {
                TempData["Error"] = message;
                return RedirectToAction("Index", new { id = id });
            }

            TempData["Success"] = "Encuesta publicada correctamente.";
            return RedirectToAction("Index", "Encuesta");
        }

        /// <summary>
        /// POST: /EditaEncuesta/Delete
        /// Elimina una encuesta
        /// Solo administradores pueden eliminar encuestas
        /// </summary>
        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            var encuesta = await _encuestaService.GetEncuestaByIdAsync(id);
            if (encuesta == null)
                return NotFound();

            if (encuesta.UsuarioCreadorId != GetCurrentUserId())
                return Forbid();

            var (success, message) = await _encuestaService.DeleteEncuestaAsync(id);

            if (!success)
            {
                TempData["Error"] = message;
                return RedirectToAction("Index", new { id = id });
            }

            TempData["Success"] = "Encuesta eliminada correctamente.";
            return RedirectToAction("Index", "Encuesta");
        }

        /// <summary>
        /// Actualiza el estado activo de una encuesta (Activo/Baja)
        /// </summary>
        /// <param name="filtroTipo">ID del tipo de evaluación (filtroTipo)</param>
        /// <param name="id">ID de la encuesta</param>
        /// <param name="estado">Estado a actualizar (true = Activo, false = Baja)</param>
        [HttpPost]
        [Route("UpdateEstado/{filtroTipo}")]
        public async Task<IActionResult> UpdateEstado(int filtroTipo, int id, string estado)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            try
            {
                // Convertir el estado string a int (true = 1, false = 0)
                int bActivo = estado?.ToLower() == "true" ? 1 : 0;

                // Llamar al método FL.ActualizaEstadoEncuesta
                bool resultado = FL.ActualizaEstadoEncuesta(filtroTipo.ToString(), bActivo);

                if (resultado)
                {
                    TempData["Success"] = "Estado de la encuesta actualizado correctamente.";
                }
                else
                {
                    TempData["Error"] = "No se pudo actualizar el estado de la encuesta.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al actualizar el estado: " + ex.Message;
            }

            // Redirigir de vuelta a la página de edición con ambos parámetros
            return RedirectToAction("Index", new { id = id, filtroTipo = filtroTipo });
        }

        /// <summary>
        /// Crea una nueva pregunta en una encuesta
        /// </summary>
        [HttpPost]
        [Route("CreatePregunta/{filtroTipo}")]
        public async Task<IActionResult> CreatePregunta(int filtroTipo, int encuestaId, string cPregunta, 
                                                        string cPregunta_Ingles, int cCompetencia, 
                                                        int cActividad, int cDescripcion, int nOrden)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            try
            {
                // Validar que los campos requeridos no estén vacíos
                if (string.IsNullOrWhiteSpace(cPregunta) || string.IsNullOrWhiteSpace(cPregunta_Ingles))
                {
                    TempData["Error"] = "La pregunta en español e inglés son requeridas.";
                    return RedirectToAction("Index", new { id = encuestaId, filtroTipo = filtroTipo });
                }

                // Aquí insertar la pregunta
                bool resultado = FL.InsertaPreguntaII(
                    filtroTipo, 
                    cPregunta, 
                    cPregunta_Ingles, 
                    cCompetencia, 
                    cActividad, 
                    cDescripcion, 
                    nOrden
                );

                if (resultado)
                {
                    TempData["Success"] = "Pregunta agregada correctamente.";
                }
                else
                {
                    TempData["Error"] = "No se pudo agregar la pregunta. Intente nuevamente.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al agregar la pregunta: " + ex.Message;
            }

            return RedirectToAction("Index", new { id = encuestaId, filtroTipo = filtroTipo });
        }

        /// <summary>
        /// Actualiza una pregunta existente en una encuesta
        /// </summary>
        [HttpPost]
        [Route("UpdatePregunta/{filtroTipo}")]
        public async Task<IActionResult> UpdatePregunta(int filtroTipo, int encuestaId, int idPregunta, 
                                                        string cPregunta, string cPregunta_Ingles, 
                                                        int cCompetencia, int cActividad, int cDescripcion, int nOrden)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            try
            {
                // Validar que los campos requeridos no estén vacíos
                if (string.IsNullOrWhiteSpace(cPregunta) || string.IsNullOrWhiteSpace(cPregunta_Ingles))
                {
                    TempData["Error"] = "La pregunta en español e inglés son requeridas.";
                    return RedirectToAction("Index", new { id = encuestaId, filtroTipo = filtroTipo });
                }

                // Validar que el ID de pregunta sea válido
                if (idPregunta <= 0)
                {
                    TempData["Error"] = "ID de pregunta inválido.";
                    return RedirectToAction("Index", new { id = encuestaId, filtroTipo = filtroTipo });
                }

                // Llamar al método FL para actualizar la pregunta
                bool resultado = EncuestasEvaluacionLiderazgo.Data.FL.UpdatePreguntaII(
                    idPregunta,
                    filtroTipo, 
                    cPregunta, 
                    cPregunta_Ingles, 
                    cCompetencia, 
                    cActividad, 
                    cDescripcion, 
                    nOrden
                );

                if (resultado)
                {
                    TempData["Success"] = "Pregunta actualizada correctamente.";
                }
                else
                {
                    TempData["Error"] = "No se pudo actualizar la pregunta. Intente nuevamente.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al actualizar la pregunta: " + ex.Message;
            }

            return RedirectToAction("Index", new { id = encuestaId, filtroTipo = filtroTipo });
        }

        /// <summary>
        /// Elimina (desactiva) una pregunta existente de una encuesta
        /// </summary>
        [HttpPost]
        [Route("DeletePregunta")]
        public IActionResult DeletePregunta(int idPregunta)
        {
            if (!IsAuthenticated())
                return Unauthorized(new { success = false, message = "No autenticado" });

            try
            {
                // Validar parámetros
                if (idPregunta <= 0)
                {
                    return Json(new { success = false, message = "ID de pregunta inválido." });
                }

                // Llamar al método FL para desactivar la pregunta (bActivo = 0)
                bool resultado = FL.ActualizaEstadoPregunta(idPregunta, 0);

                if (resultado)
                {
                    return Json(new { success = true, message = "Pregunta eliminada correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo eliminar la pregunta." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar la pregunta: " + ex.Message });
            }
        }

        /// <summary>
        /// Mueve una pregunta hacia arriba (disminuye su número de orden)
        /// Intercambia el orden con la pregunta anterior
        /// </summary>
        [HttpPost]
        [Route("MoverPreguntaArriba")]
        public IActionResult MoverPreguntaArriba(int idPregunta, int nOrden, int idPreguntaAnterior)
        {
            if (!IsAuthenticated())
                return Unauthorized(new { success = false, message = "No autenticado" });

            try
            {
                // Validar parámetros
                if (idPregunta <= 0)
                {
                    return Json(new { success = false, message = "ID de pregunta inválido." });
                }

                if (idPreguntaAnterior <= 0)
                {
                    return Json(new { success = false, message = "ID de pregunta anterior inválido." });
                }

                // Validar que no sea la primera pregunta (nOrden = 1)
                if (nOrden <= 1)
                {
                    return Json(new { success = false, message = "No se puede mover la pregunta hacia arriba." });
                }

                // Calcular el nuevo orden para ambas preguntas
                int nuevoNOrdenActual = nOrden - 1;
                int nuevoNOrdenAnterior = nOrden;

                // Actualizar ambas preguntas
                bool resultado1 = FL.ActualizaNOrden(idPregunta, nuevoNOrdenActual);
                bool resultado2 = FL.ActualizaNOrden(idPreguntaAnterior, nuevoNOrdenAnterior);

                if (resultado1 && resultado2)
                {
                    return Json(new { success = true, message = "Pregunta movida hacia arriba correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo mover la pregunta." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al mover la pregunta: " + ex.Message });
            }
        }

        /// <summary>
        /// Mueve una pregunta hacia abajo (aumenta su número de orden)
        /// Intercambia el orden con la pregunta siguiente
        /// </summary>
        [HttpPost]
        [Route("MoverPreguntaAbajo")]
        public IActionResult MoverPreguntaAbajo(int idPregunta, int nOrden, int idPreguntaSiguiente)
        {
            if (!IsAuthenticated())
                return Unauthorized(new { success = false, message = "No autenticado" });

            try
            {
                // Validar parámetros
                if (idPregunta <= 0)
                {
                    return Json(new { success = false, message = "ID de pregunta inválido." });
                }

                if (idPreguntaSiguiente <= 0)
                {
                    return Json(new { success = false, message = "ID de pregunta siguiente inválido." });
                }

                // Validar que no sea la última pregunta (verificamos que nOrden sea válido)
                if (nOrden <= 0)
                {
                    return Json(new { success = false, message = "Número de orden inválido." });
                }

                // Calcular el nuevo orden para ambas preguntas
                int nuevoNOrdenActual = nOrden + 1;
                int nuevoNOrdenSiguiente = nOrden;

                // Actualizar ambas preguntas
                bool resultado1 = FL.ActualizaNOrden(idPregunta, nuevoNOrdenActual);
                bool resultado2 = FL.ActualizaNOrden(idPreguntaSiguiente, nuevoNOrdenSiguiente);

                if (resultado1 && resultado2)
                {
                    return Json(new { success = true, message = "Pregunta movida hacia abajo correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo mover la pregunta." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al mover la pregunta: " + ex.Message });
            }
        }

        /// <summary>
        /// Actualiza la clave de acceso de una encuesta
        /// </summary>
        [HttpPost]
        [Route("ActualizaClaveAcceso")]
        public IActionResult ActualizaClaveAcceso(int filtroTipo, string cClaveAcceso)
        {
            if (!IsAuthenticated())
                return Unauthorized(new { success = false, message = "No autenticado" });

            try
            {
                // Validar parámetros
                if (filtroTipo <= 0)
                {
                    return Json(new { success = false, message = "ID de tipo de evaluación inválido." });
                }

                if (string.IsNullOrWhiteSpace(cClaveAcceso))
                {
                    return Json(new { success = false, message = "La clave de acceso no puede estar vacía." });
                }

                // Llamar al método FL para actualizar la clave de acceso
                bool resultado = FL.ActualizaClaveAcceso(filtroTipo.ToString(), cClaveAcceso);

                if (resultado)
                {
                    return Json(new { success = true, message = "Clave de acceso actualizada correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No se pudo actualizar la clave de acceso." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al actualizar la clave de acceso: " + ex.Message });
            }
        }
    
    }
}
