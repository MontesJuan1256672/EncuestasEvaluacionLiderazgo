using EncuestasEvaluacionLiderazgo.Models;

namespace EncuestasEvaluacionLiderazgo.Views.Encuesta
{
    /// <summary>
    /// Code-behind para la vista Details.cshtml
    /// Contiene los métodos helper para la presentación de datos de encuestas
    /// </summary>
    public class DetailsModel
    {
        /// <summary>
        /// Obtiene las clases CSS para el badge de estado de la encuesta
        /// </summary>
        /// <param name="estado">Estado de la encuesta</param>
        /// <returns>Clases CSS de Tailwind para el estado</returns>
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
        /// <param name="estado">Estado de la encuesta</param>
        /// <returns>Texto del estado en español</returns>
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
        /// <param name="tipo">Tipo de pregunta</param>
        /// <returns>Descripción del tipo en español</returns>
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
        /// <param name="valor">Valor de 1 a 5</param>
        /// <returns>String con estrellas repetidas</returns>
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
