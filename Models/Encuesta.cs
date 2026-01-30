using System;
using System.Collections.Generic;

namespace EncuestasEvaluacionLiderazgo.Models
{
    /// <summary>
    /// Modelo que representa una encuesta de evaluación de liderazgo
    /// </summary>
    public class Encuesta
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaVencimiento { get; set; }
        public EstadoEncuesta Estado { get; set; } = EstadoEncuesta.Borrador;
        public int UsuarioCreadorId { get; set; }
        public bool Activa { get; set; } = true;

        // Relaciones
        public virtual Usuario UsuarioCreador { get; set; }
        public virtual ICollection<Pregunta> Preguntas { get; set; }
        public virtual ICollection<Respuesta> Respuestas { get; set; }
    }

    /// <summary>
    /// Estados posibles de una encuesta
    /// </summary>
    public enum EstadoEncuesta
    {
        Borrador = 1,
        Publicada = 2,
        Cerrada = 3,
        Archivada = 4
    }
}
