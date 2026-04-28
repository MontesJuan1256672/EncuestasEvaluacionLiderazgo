using Microsoft.AspNetCore.Http;
using EncuestasEvaluacionLiderazgo.Models;

namespace EncuestasEvaluacionLiderazgo.Views.Home
{
    /// <summary>
    /// Code-behind para la vista Home/Index
    /// Contiene la lógica de presentación y métodos helper
    /// </summary>
    public class IndexModel
    {
        /// <summary>
        /// Obtiene el tipo de usuario actual desde la sesión
        /// </summary>
        /// <param name="session">Sesión HTTP del usuario</param>
        /// <returns>Tipo de usuario (0 = Indeterminado, 1 = Administrador, 2 = Evaluador)</returns>
        public static int GetUserType(ISession session)
        {
            return session.GetInt32("UserType") ?? 0;
        }

        /// <summary>
        /// Verifica si el usuario actual es administrador
        /// </summary>
        /// <param name="session">Sesión HTTP del usuario</param>
        /// <returns>True si es administrador, False en caso contrario</returns>
        public static bool IsAdmin(ISession session)
        {
            int userType = GetUserType(session);
            return userType == (int)TipoUsuario.Administrador;
        }

        /// <summary>
        /// Verifica si el usuario actual es consultor
        /// </summary>
        /// <param name="session">Sesión HTTP del usuario</param>
        /// <returns>True si es consultor, False en caso contrario</returns>
        public static bool IsConsultor(ISession session)
        {
            int userType = GetUserType(session);
            return userType == (int)TipoUsuario.Consulta;
        }        

        /// <summary>
        /// Verifica si el usuario actual es evaluador
        /// </summary>
        /// <param name="session">Sesión HTTP del usuario</param>
        /// <returns>True si es evaluador, False en caso contrario</returns>
        public static bool IsEvaluador(ISession session)
        {
            int userType = GetUserType(session);
            return userType == (int)TipoUsuario.Evaluador;
        }

        /// <summary>
        /// Obtiene el nombre del rol del usuario
        /// </summary>
        /// <param name="session">Sesión HTTP del usuario</param>
        /// <returns>Nombre del rol (Administrador, Evaluador o Indeterminado)</returns>
        public static string GetRoleName(ISession session)
        {
            int userType = GetUserType(session);
            return userType switch
            {
                (int)TipoUsuario.Administrador => "Administrador",
                (int)TipoUsuario.Consulta => "Consultor",
                (int)TipoUsuario.Evaluador => "Evaluador",
                _ => "Indeterminado"
            };
        }

        /// <summary>
        /// Obtiene el nombre de usuario actual desde la sesión
        /// </summary>
        /// <param name="session">Sesión HTTP del usuario</param>
        /// <returns>Nombre de usuario o string vacío si no existe</returns>
        public static string GetUserName(ISession session)
        {
            return session.GetString("UserName") ?? string.Empty;
        }

        /// <summary>
        /// Obtiene el ID de usuario actual desde la sesión
        /// </summary>
        /// <param name="session">Sesión HTTP del usuario</param>
        /// <returns>ID de usuario o 0 si no existe</returns>
        public static int GetUserId(ISession session)
        {
            return session.GetInt32("UserId") ?? 0;
        }

        /// <summary>
        /// Verifica si el usuario está autenticado
        /// </summary>
        /// <param name="session">Sesión HTTP del usuario</param>
        /// <returns>True si está autenticado, False en caso contrario</returns>
        public static bool IsAuthenticated(ISession session)
        {
            return session.GetInt32("UserId").HasValue;
        }

        /// <summary>
        /// Obtiene un mensaje de bienvenida personalizado según el rol del usuario
        /// </summary>
        /// <param name="session">Sesión HTTP del usuario</param>
        /// <returns>Mensaje de bienvenida</returns>
        public static string GetWelcomeMessage(ISession session)
        {
            if (!IsAuthenticated(session))
                return "Bienvenido a la Plataforma de Evaluación de Liderazgo";

            string userName = GetUserName(session);
            string rolName = GetRolName(session);

            return rolName switch
            {
                "Administrador" => $"Bienvenido, {userName}. Como administrador puedes crear y gestionar encuestas.",
                "Evaluador" => $"Bienvenido, {userName}. Como evaluador puedes responder encuestas.",
                _ => $"Bienvenido, {userName}."
            };
        }

        /// <summary>
        /// Obtiene el nombre del rol del usuario (variante corta del nombre del método)
        /// </summary>
        /// <param name="session">Sesión HTTP del usuario</param>
        /// <returns>Nombre del rol</returns>
        public static string GetRolName(ISession session)
        {
            return GetRoleName(session);
        }

        /// <summary>
        /// Obtiene el color de badge según el tipo de usuario
        /// </summary>
        /// <param name="session">Sesión HTTP del usuario</param>
        /// <returns>Clase Tailwind para el color (bg-blue-600 para admin, bg-green-600 para evaluador)</returns>
        public static string GetRolBadgeColor(ISession session)
        {
            int userType = GetUserType(session);
            return userType switch
            {
                (int)TipoUsuario.Administrador => "bg-blue-600",
                (int)TipoUsuario.Evaluador => "bg-green-600",
                _ => "bg-gray-600"
            };
        }
    }
}
