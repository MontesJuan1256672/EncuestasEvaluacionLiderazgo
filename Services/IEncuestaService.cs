using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EncuestasEvaluacionLiderazgo.Models;

namespace EncuestasEvaluacionLiderazgo.Services
{
    /// <summary>
    /// Interfaz para el servicio de encuestas
    /// Define las operaciones CRUD para encuestas
    /// </summary>
    public interface IEncuestaService
    {
        Task<IEnumerable<Encuesta>> GetEncuestasAsync(int usuarioId);
        Task<Encuesta> GetEncuestaByIdAsync(int id);
        Task<(bool Success, string Message, int EncuestaId)> CreateEncuestaAsync(Encuesta encuesta);
        Task<(bool Success, string Message)> UpdateEncuestaAsync(Encuesta encuesta);
        Task<(bool Success, string Message)> DeleteEncuestaAsync(int id);
        Task<(bool Success, string Message)> PublishEncuestaAsync(int id);
        Task<(bool Success, string Message)> CloseEncuestaAsync(int id);
    }
}
