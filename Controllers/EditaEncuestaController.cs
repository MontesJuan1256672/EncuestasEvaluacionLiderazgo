using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using EncuestasEvaluacionLiderazgo.Services;
using EncuestasEvaluacionLiderazgo.Models;

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
        /// GET: /EditaEncuesta/{id}
        /// Muestra el formulario para editar una encuesta existente
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var encuesta = await _encuestaService.GetEncuestaByIdAsync(id);

            if (encuesta == null)
                return NotFound();

            // Verificar que el usuario sea el creador de la encuesta
            if (encuesta.UsuarioCreadorId != GetCurrentUserId())
                return Forbid();

            return View(encuesta);
        }

        /// <summary>
        /// POST: /EditaEncuesta/Update
        /// Actualiza una encuesta existente
        /// </summary>
        [HttpPost("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Encuesta encuesta)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

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
        /// Publica una encuesta (la hace visible para responder)
        /// </summary>
        [HttpPost("Publish")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

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
        /// </summary>
        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

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
    }
}
