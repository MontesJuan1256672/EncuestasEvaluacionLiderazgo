using Microsoft.AspNetCore.Http;

namespace EncuestasEvaluacionLiderazgo.Utilities
{
    /// <summary>
    /// Utilidades para manejar sesiones y autenticación
    /// </summary>
    public static class SessionHelper
    {
        /// <summary>
        /// Obtiene el ID del usuario actual desde la sesión
        /// </summary>
        public static int? GetUserId(ISession session)
        {
            return session.GetInt32("UserId");
        }

        /// <summary>
        /// Obtiene el nombre del usuario actual desde la sesión
        /// </summary>
        public static string GetUserName(ISession session)
        {
            return session.GetString("UserName") ?? "Invitado";
        }

        /// <summary>
        /// Obtiene el tipo de usuario desde la sesión (1 = Admin, 2 = Evaluador)
        /// </summary>
        public static int? GetUserType(ISession session)
        {
            return session.GetInt32("UserType");
        }

        /// <summary>
        /// Verifica si el usuario está autenticado
        /// </summary>
        public static bool IsAuthenticated(ISession session)
        {
            return GetUserId(session).HasValue;
        }

        /// <summary>
        /// Verifica si el usuario es administrador
        /// </summary>
        public static bool IsAdmin(ISession session)
        {
            return GetUserType(session) == 1;
        }

        /// <summary>
        /// Verifica si el usuario es evaluador
        /// </summary>
        public static bool IsEvaluador(ISession session)
        {
            return GetUserType(session) == 2;
        }

        /// <summary>
        /// Crea la sesión del usuario
        /// </summary>
        public static void SetUserSession(ISession session, Models.Usuario usuario)
        {
            session.SetInt32("UserId", usuario.Id);
            session.SetString("UserName", usuario.Nombre);
            session.SetInt32("UserType", (int)usuario.TipoUsuario);
        }

        /// <summary>
        /// Limpia la sesión del usuario
        /// </summary>
        public static void ClearUserSession(ISession session)
        {
            session.Remove("UserId");
            session.Remove("UserName");
            session.Remove("UserType");
        }
    }
}
