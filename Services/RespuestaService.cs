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

        /// <summary>
        /// Guarda una respuesta de encuesta
        /// </summary>
        public Task<(bool Success, string Message)> SaveRespuestaAsync(Respuesta respuesta)
        {
            // Validar entrada
            if (respuesta == null || respuesta.IdTipoEvaluacion <= 0)
            {
                return Task.FromResult<(bool, string)>(
                    (false, "Datos de respuesta inválidos")
                );
            }

            try
            {
                string resultado = Data.FL.InsertaEvaluacion(respuesta);

                if (!string.IsNullOrEmpty(resultado) && !resultado.StartsWith("Error"))
                {
                    return Task.FromResult<(bool, string)>(
                        (true, "Evaluación guardada exitosamente. ID: " + resultado)
                    );
                }
                else
                {
                    return Task.FromResult<(bool, string)>(
                        (false, resultado ?? "Error al guardar la evaluación")
                    );
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult<(bool, string)>(
                    (false, "Error al guardar: " + ex.Message)
                );
            }
        }

        /// <summary>
        /// Obtiene una respuesta por ID
        /// </summary>
        public Task<Respuesta> GetRespuestaAsync(int respuestaId)
        {
            return Task.FromResult<Respuesta>(null);
        }

        /// <summary>
        /// Obtiene todas las respuestas de una encuesta
        /// </summary>
        public Task<IEnumerable<Respuesta>> GetRespuestasEncuestaAsync(int encuestaId)
        {
            return Task.FromResult<IEnumerable<Respuesta>>(new List<Respuesta>());
        }

        /// <summary>
        /// Marca una respuesta como completada
        /// </summary>
        public Task<(bool Success, string Message)> CompleteRespuestaAsync(int respuestaId)
        {
            return Task.FromResult<(bool, string)>(
                (false, "No implementado")
            );
        }

        /// <summary>
        /// Elimina una respuesta
        /// </summary>
        public Task<(bool Success, string Message)> DeleteRespuestaAsync(int respuestaId)
        {
            return Task.FromResult<(bool, string)>(
                (false, "No implementado")
            );
        }
    }
}
