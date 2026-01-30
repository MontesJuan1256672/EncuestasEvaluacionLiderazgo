using System;
using System.Collections.Generic;

namespace EncuestasEvaluacionLiderazgo.Models
{
    /// <summary>
    /// Modelo que representa las respuestas de un usuario a una encuesta
    /// </summary>
    public class Respuesta
    {
        public int Id { get; set; }
        public int EncuestaId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaRespuesta { get; set; } = DateTime.Now;
        public bool Completada { get; set; } = false;

        // Relaciones
        public virtual Encuesta Encuesta { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<RespuestaDetalle> Detalles { get; set; }
    }

    /// <summary>
    /// Detalle de una respuesta individual a una pregunta
    /// </summary>
    public class RespuestaDetalle
    {
        public int Id { get; set; }
        public int RespuestaId { get; set; }
        public int PreguntaId { get; set; }
        public string Valor { get; set; }
        public DateTime FechaRespuesta { get; set; } = DateTime.Now;

        // Relaciones
        public virtual Respuesta Respuesta { get; set; }
        public virtual Pregunta Pregunta { get; set; }
    }
}
