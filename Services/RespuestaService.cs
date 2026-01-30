using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EncuestasEvaluacionLiderazgo.Models;

namespace EncuestasEvaluacionLiderazgo.Services
{
    /// <summary>
    /// Implementación del servicio de respuestas
    /// En producción, esto se conectaría a una base de datos real
    /// </summary>
    public class RespuestaService : IRespuestaService
    {
        // Simulación de base de datos en memoria
        private static readonly List<Respuesta> _respuestas = new();
        private static int _respuestaIdCounter = 1;

        /// <summary>
        /// Guarda una respuesta de encuesta
        /// </summary>
        public Task<(bool Success, string Message)> SaveRespuestaAsync(Respuesta respuesta)
        {
            // Validar entrada
            if (respuesta == null || respuesta.EncuestaId <= 0 || respuesta.UsuarioId <= 0)
            {
                return Task.FromResult<(bool, string)>(
                    (false, "Datos de respuesta inválidos")
                );
            }

            // Asignar ID
            respuesta.Id = _respuestaIdCounter++;
            respuesta.FechaRespuesta = DateTime.Now;

            _respuestas.Add(respuesta);

            return Task.FromResult<(bool, string)>(
                (true, "Respuesta guardada exitosamente")
            );
        }

        /// <summary>
        /// Obtiene una respuesta por ID
        /// </summary>
        public Task<Respuesta> GetRespuestaAsync(int respuestaId)
        {
            var respuesta = _respuestas.FirstOrDefault(r => r.Id == respuestaId);
            return Task.FromResult(respuesta);
        }

        /// <summary>
        /// Obtiene todas las respuestas de una encuesta
        /// </summary>
        public Task<IEnumerable<Respuesta>> GetRespuestasEncuestaAsync(int encuestaId)
        {
            var respuestas = _respuestas
                .Where(r => r.EncuestaId == encuestaId)
                .ToList();

            return Task.FromResult<IEnumerable<Respuesta>>(respuestas);
        }

        /// <summary>
        /// Marca una respuesta como completada
        /// </summary>
        public Task<(bool Success, string Message)> CompleteRespuestaAsync(int respuestaId)
        {
            var respuesta = _respuestas.FirstOrDefault(r => r.Id == respuestaId);

            if (respuesta == null)
            {
                return Task.FromResult<(bool, string)>(
                    (false, "Respuesta no encontrada")
                );
            }

            respuesta.Completada = true;

            return Task.FromResult<(bool, string)>(
                (true, "Respuesta marcada como completada")
            );
        }

        /// <summary>
        /// Elimina una respuesta
        /// </summary>
        public Task<(bool Success, string Message)> DeleteRespuestaAsync(int respuestaId)
        {
            var respuesta = _respuestas.FirstOrDefault(r => r.Id == respuestaId);

            if (respuesta == null)
            {
                return Task.FromResult<(bool, string)>(
                    (false, "Respuesta no encontrada")
                );
            }

            _respuestas.Remove(respuesta);

            return Task.FromResult<(bool, string)>(
                (true, "Respuesta eliminada exitosamente")
            );
        }
    }
}
