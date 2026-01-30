using System;

namespace EncuestasEvaluacionLiderazgo.Models
{
    /// <summary>
    /// Modelo que representa una opción de respuesta para una pregunta
    /// </summary>
    public class OpcionRespuesta
    {
        public int Id { get; set; }
        public int PreguntaId { get; set; }
        public string Texto { get; set; }
        public int Valor { get; set; }
        public int Orden { get; set; }

        // Relaciones
        public virtual Pregunta Pregunta { get; set; }
    }
}
