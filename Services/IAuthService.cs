using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EncuestasEvaluacionLiderazgo.Services
{
    /// <summary>
    /// Interfaz para el servicio de autenticación
    /// Define las operaciones relacionadas con la autenticación de usuarios
    /// </summary>
    public interface IAuthService
    {
        Task<(bool Success, string Message, Models.Usuario Usuario)> LoginAsync(string email, string contraseña);
        Task<(bool Success, string Message)> RegisterAsync(string nombre, string email, string contraseña, Models.TipoUsuario tipoUsuario);
        Task<bool> ValidateUserAsync(int usuarioId);
        Task<Models.Usuario> GetUserByIdAsync(int id);
        Task<Models.Usuario> GetUserByEmailAsync(string email);
    }
}
