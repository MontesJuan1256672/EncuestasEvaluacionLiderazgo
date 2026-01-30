using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EncuestasEvaluacionLiderazgo.Models;

namespace EncuestasEvaluacionLiderazgo.Services
{
    /// <summary>
    /// Interfaz para el servicio de respuestas
    /// Define las operaciones para gestionar respuestas de encuestas
    /// </summary>
    public interface IRespuestaService
    {
        Task<(bool Success, string Message)> SaveRespuestaAsync(Respuesta respuesta);
        Task<Respuesta> GetRespuestaAsync(int respuestaId);
        Task<IEnumerable<Respuesta>> GetRespuestasEncuestaAsync(int encuestaId);
        Task<(bool Success, string Message)> CompleteRespuestaAsync(int respuestaId);
        Task<(bool Success, string Message)> DeleteRespuestaAsync(int respuestaId);
    }
}
