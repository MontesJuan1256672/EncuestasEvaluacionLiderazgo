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
        /// Título procesado de la encuesta basado en el tipo de evaluación
        /// </summary>
        public string TituloEncuesta { get; set; } = "";

        /// <summary>
        /// Lista de preguntas procesadas y ordenadas
        /// </summary>
        public List<PreguntaViewModel> Preguntas { get; set; } = new List<PreguntaViewModel>();

        /// <summary>
        /// Indica si hay preguntas disponibles para mostrar
        /// </summary>
        public bool TienePreguntasValidas => Preguntas != null && Preguntas.Any();

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

    /// <summary>
    /// Modelo de vista para una pregunta en el formulario de respuestas
    /// Contiene la información de la pregunta ya procesada para presentación
    /// </summary>
    public class PreguntaViewModel
    {
        /// <summary>
        /// ID único de la pregunta
        /// </summary>
        public int IdPregunta { get; set; }

        /// <summary>
        /// Número de orden (posición en la encuesta)
        /// </summary>
        public int NumeroPregunta { get; set; }

        /// <summary>
        /// Texto de la pregunta en español
        /// </summary>
        public string Texto { get; set; } = "";

        /// <summary>
        /// Texto de la pregunta en inglés
        /// </summary>
        public string TextoIngles { get; set; } = "";

        /// <summary>
        /// Nombre del campo en el formulario para la respuesta
        /// Formato: respuesta_{IdPregunta}
        /// </summary>
        public string NombreCampoRespuesta => $"respuesta_{IdPregunta}";

        /// <summary>
        /// Nombre del campo en el formulario para el comentario
        /// Formato: comentario_{IdPregunta}
        /// </summary>
        public string NombreCampoComentario => $"comentario_{IdPregunta}";
    }
}
