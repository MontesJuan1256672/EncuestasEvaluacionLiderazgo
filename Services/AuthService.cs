using System;
using System.Threading.Tasks;
using EncuestasEvaluacionLiderazgo.Models;

namespace EncuestasEvaluacionLiderazgo.Services
{
    /// <summary>
    /// Implementación del servicio de autenticación
    /// En producción, esto se conectaría a una base de datos real
    /// </summary>
    public class AuthService : IAuthService
    {
        // Simulación de base de datos en memoria (para demostración)
        private static readonly List<Usuario> _usuarios = new()
        {
            new Usuario 
            { 
                Id = 1, 
                Nombre = "Admin", 
                Email = "admin@test.com", 
                Contraseña = "Admin@123",
                TipoUsuario = TipoUsuario.Administrador,
                FechaCreacion = DateTime.Now,
                Activo = true
            },
            new Usuario 
            { 
                Id = 2, 
                Nombre = "Evaluador 1", 
                Email = "evaluador@test.com", 
                Contraseña = "Eval@123",
                TipoUsuario = TipoUsuario.Evaluador,
                FechaCreacion = DateTime.Now,
                Activo = true
            }
        };

        /// <summary>
        /// Autentica un usuario con email y contraseña
        /// </summary>
        public Task<(bool Success, string Message, Usuario Usuario)> LoginAsync(string email, string contraseña)
        {
            // Validar entrada
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contraseña))
            {
                return Task.FromResult<(bool, string, Usuario)>(
                    (false, "Email y contraseña son requeridos", null)
                );
            }

            // Buscar usuario
            var usuario = _usuarios.FirstOrDefault(u => 
                u.Email.ToLower() == email.ToLower() && 
                u.Activo
            );

            if (usuario == null)
            {
                return Task.FromResult<(bool, string, Usuario)>(
                    (false, "Email o contraseña incorrectos", null)
                );
            }

            // Verificar contraseña (en producción usar hashing)
            if (usuario.Contraseña != contraseña)
            {
                return Task.FromResult<(bool, string, Usuario)>(
                    (false, "Email o contraseña incorrectos", null)
                );
            }

            // Actualizar último acceso
            usuario.FechaUltimoAcceso = DateTime.Now;

            return Task.FromResult<(bool, string, Usuario)>(
                (true, "Login exitoso", usuario)
            );
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema
        /// </summary>
        public Task<(bool Success, string Message)> RegisterAsync(string nombre, string email, string contraseña, TipoUsuario tipoUsuario)
        {
            // Validar entrada
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contraseña))
            {
                return Task.FromResult<(bool, string)>(
                    (false, "Todos los campos son requeridos")
                );
            }

            // Validar que el email sea único
            if (_usuarios.Any(u => u.Email.ToLower() == email.ToLower()))
            {
                return Task.FromResult<(bool, string)>(
                    (false, "Este email ya está registrado")
                );
            }

            // Validar contraseña
            if (contraseña.Length < 6)
            {
                return Task.FromResult<(bool, string)>(
                    (false, "La contraseña debe tener al menos 6 caracteres")
                );
            }

            // Crear nuevo usuario
            var nuevoUsuario = new Usuario
            {
                Id = _usuarios.Count + 1,
                Nombre = nombre,
                Email = email,
                Contraseña = contraseña, // En producción, aplicar hashing
                TipoUsuario = tipoUsuario,
                FechaCreacion = DateTime.Now,
                Activo = true
            };

            _usuarios.Add(nuevoUsuario);

            return Task.FromResult<(bool, string)>(
                (true, "Registro exitoso")
            );
        }

        /// <summary>
        /// Valida si un usuario existe y está activo
        /// </summary>
        public Task<bool> ValidateUserAsync(int usuarioId)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.Id == usuarioId && u.Activo);
            return Task.FromResult(usuario != null);
        }

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        public Task<Usuario> GetUserByIdAsync(int id)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(usuario);
        }

        /// <summary>
        /// Obtiene un usuario por su email
        /// </summary>
        public Task<Usuario> GetUserByEmailAsync(string email)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            return Task.FromResult(usuario);
        }
    }
}
