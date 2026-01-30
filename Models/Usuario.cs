using System;
using System.Collections.Generic;

namespace EncuestasEvaluacionLiderazgo.Models
{
    /// <summary>
    /// Modelo que representa un usuario del sistema
    /// Soporta dos tipos: Administrador y Evaluador
    /// </summary>
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaUltimoAcceso { get; set; }

        // Relaciones
        public virtual ICollection<Encuesta> EncuestasCreadas { get; set; }
        public virtual ICollection<Respuesta> RespuestasEnviadas { get; set; }
    }

    /// <summary>
    /// Tipos de usuarios del sistema
    /// </summary>
    public enum TipoUsuario
    {
        Administrador = 1,
        Evaluador = 2
    }
}
