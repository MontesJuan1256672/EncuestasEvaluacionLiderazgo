namespace EncuestasEvaluacionLiderazgo.Models
{
    /// <summary>
    /// Modelo que representa a una persona a ser evaluada
    /// </summary>
    public class PersonaEvaluar
    {
        /// <summary>
        /// Identificador único de la persona
        /// </summary>
        public int IDPersona { get; set; }

        /// <summary>
        /// Número de empleado
        /// </summary>
        public string NumeroEmpleado { get; set; }

        /// <summary>
        /// Nombre completo de la persona
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Ciudad donde trabaja la persona
        /// </summary>
        public string Ciudad { get; set; }

        /// <summary>
        /// Puesto o cargo de la persona
        /// </summary>
        public string Puesto { get; set; }

        /// <summary>
        /// Indicador de si la persona está activa
        /// </summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
