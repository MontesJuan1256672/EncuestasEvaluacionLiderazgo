using System;
using System.Collections.Generic;

namespace EncuestasEvaluacionLiderazgo.Models
{
    /// <summary>
    /// Modelo que representa una pregunta dentro de una encuesta
    /// </summary>
    public class Pregunta
    {
        public int Id { get; set; }
        public int EncuestaId { get; set; }
        public string Texto { get; set; }
        public TipoPregunta Tipo { get; set; }
        public int Orden { get; set; }
        public bool Requerida { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relaciones
        public virtual Encuesta Encuesta { get; set; }
        public virtual ICollection<OpcionRespuesta> Opciones { get; set; }
        public virtual ICollection<RespuestaDetalle> Respuestas { get; set; }
    }

    /// <summary>
    /// Tipos de preguntas soportadas
    /// </summary>
    public enum TipoPregunta
    {
        TextoCorto = 1,
        TextoLargo = 2,
        OpcionUnica = 3,
        OpcionMultiple = 4,
        Escala = 5
    }
}
