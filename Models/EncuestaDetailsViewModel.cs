namespace EncuestasEvaluacionLiderazgo.Models
{
    /// <summary>
    /// ViewModel para la vista Details de Encuestas
    /// Contiene la encuesta y propiedades adicionales necesarias para la presentación
    /// </summary>
    public class EncuestaDetailsViewModel
    {
        /// <summary>
        /// La encuesta a mostrar
        /// </summary>
        public Encuesta Encuesta { get; set; }

        /// <summary>
        /// Filtro de tipo de evaluación (si viene de Index)
        /// </summary>
        public string FiltroTipo { get; set; } = "";

        /// <summary>
        /// Obtiene las clases CSS para el badge de estado de la encuesta
        /// </summary>
        public string GetEstadoClase(EstadoEncuesta estado)
        {
            return estado switch
            {
                EstadoEncuesta.Borrador => "bg-gray-100 text-gray-800",
                EstadoEncuesta.Publicada => "bg-green-100 text-green-800",
                EstadoEncuesta.Cerrada => "bg-orange-100 text-orange-800",
                EstadoEncuesta.Archivada => "bg-blue-100 text-blue-800",
                _ => "bg-gray-100 text-gray-800"
            };
        }

        /// <summary>
        /// Obtiene el texto a mostrar para el estado de la encuesta
        /// </summary>
        public string GetEstadoTexto(EstadoEncuesta estado)
        {
            return estado switch
            {
                EstadoEncuesta.Borrador => "Borrador",
                EstadoEncuesta.Publicada => "Publicada",
                EstadoEncuesta.Cerrada => "Cerrada",
                EstadoEncuesta.Archivada => "Archivada",
                _ => "Desconocido"
            };
        }

        /// <summary>
        /// Obtiene el texto descriptivo del tipo de pregunta
        /// </summary>
        public string GetTipoPreguntaTexto(TipoPregunta tipo)
        {
            return tipo switch
            {
                TipoPregunta.TextoCorto => "Texto corto",
                TipoPregunta.TextoLargo => "Párrafo",
                TipoPregunta.OpcionUnica => "Opción única",
                TipoPregunta.OpcionMultiple => "Opciones múltiples",
                TipoPregunta.Escala => "Escala",
                _ => "Desconocido"
            };
        }

        /// <summary>
        /// Obtiene las estrellas para mostrar una escala visual
        /// </summary>
        public string GetEstrella(int valor)
        {
            return valor switch
            {
                1 => "⭐",
                2 => "⭐⭐",
                3 => "⭐⭐⭐",
                4 => "⭐⭐⭐⭐",
                5 => "⭐⭐⭐⭐⭐",
                _ => ""
            };
        }
    }
}
