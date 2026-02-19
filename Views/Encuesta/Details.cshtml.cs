using EncuestasEvaluacionLiderazgo.Models;
using EncuestasEvaluacionLiderazgo.Data;
using System.Data;
using EncuestaModel = EncuestasEvaluacionLiderazgo.Models.Encuesta;
using PreguntaModel = EncuestasEvaluacionLiderazgo.Models.Pregunta;
using OpcionModel = EncuestasEvaluacionLiderazgo.Models.OpcionRespuesta;

namespace EncuestasEvaluacionLiderazgo.Views.Encuesta
{
    /// <summary>
    /// Code-behind para la vista Details.cshtml
    /// Contiene los métodos helper para la presentación de datos de encuestas
    /// </summary>
    public class DetailsModel
    {
        /// <summary>
        /// Obtiene las preguntas ordenadas por orden de aparición
        /// </summary>
        public List<PreguntaModel> GetPreguntasOrdenadas(IEnumerable<PreguntaModel> preguntas)
        {
            return preguntas?.OrderBy(p => p.Orden).ToList() ?? new List<PreguntaModel>();
        }

        /// <summary>
        /// Obtiene el índice de una pregunta en la lista (para mostrar número)
        /// </summary>
        public int GetIndicePregunta(PreguntaModel pregunta, IEnumerable<PreguntaModel> preguntas)
        {
            var preguntasOrdenadas = GetPreguntasOrdenadas(preguntas);
            return preguntasOrdenadas.IndexOf(pregunta) + 1;
        }

        /// <summary>
        /// Obtiene el atributo required si la pregunta es requerida
        /// </summary>
        public string GetAtributoRequerido(bool esRequerida)
        {
            return esRequerida ? "required" : "";
        }

        /// <summary>
        /// Valida si hay preguntas para mostrar
        /// </summary>
        public bool TienePreguntasValidas(EncuestaModel encuesta)
        {
            return encuesta?.Preguntas != null && encuesta.Preguntas.Any();
        }

        /// <summary>
        /// Obtiene las opciones ordenadas de una pregunta
        /// </summary>
        public List<OpcionModel> GetOpcionesOrdenadas(PreguntaModel pregunta)
        {
            return pregunta?.Opciones?.OrderBy(o => o.Orden).ToList() ?? new List<OpcionModel>();
        }

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

        /// <summary>
        /// Obtiene el título de la encuesta basado en el tipo de evaluación
        /// </summary>
        public string GetTituloEncuesta(string filtroTipo)
        {
            // Si el filtroTipo está vacío, retornar el título por defecto
            if (string.IsNullOrEmpty(filtroTipo))
            {
                return "Sin título";
            }

            try
            {
                // Convertir filtroTipo a int
                if (int.TryParse(filtroTipo, out int idTipoEvaluacion))
                {
                    var datosEncuesta = FL.TraeEncuestaEvaluaLiderazgo(idTipoEvaluacion.ToString());
                    
                    if (datosEncuesta != null && datosEncuesta.Tables.Count > 0 && datosEncuesta.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = datosEncuesta.Tables[0].Rows[0];
                        return row["cDescripcion"]?.ToString() ?? "Sin título";
                    }
                }

                return "Sin título";
            }
            catch
            {
                return "Sin título";
            }
        }

        /// <summary>
        /// Obtiene las preguntas desde la BD usando FL.TraePreguntasII
        /// </summary>
        public DataSet ObtenerPreguntasDeBaseDatos(string idTipoEvaluacion)
        {
            if (string.IsNullOrEmpty(idTipoEvaluacion))
            {
                return new DataSet();
            }

            try
            {
                return FL.TraePreguntasII(idTipoEvaluacion);
            }
            catch
            {
                return new DataSet();
            }
        }

        /// <summary>
        /// Valida si el DataSet de preguntas tiene datos válidos
        /// </summary>
        public bool TienePreguntasEnDataSet(DataSet dataSet)
        {
            return dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// Obtiene las filas del DataSet de preguntas ordenadas por nOrden
        /// </summary>
        public List<DataRow> ObtenerFilasPreguntasOrdenadas(DataSet dataSet)
        {
            if (!TienePreguntasEnDataSet(dataSet))
            {
                return new List<DataRow>();
            }

            try
            {
                return dataSet.Tables[0].Rows.Cast<DataRow>()
                    .OrderBy(r => Convert.ToInt32(r["nOrden"] ?? 0))
                    .ToList();
            }
            catch
            {
                return new List<DataRow>();
            }
        }

        /// <summary>
        /// Obtiene el texto de la pregunta desde una fila del DataSet
        /// </summary>
        public string ObtenerTextoPregunta(DataRow fila)
        {
            return fila?["cPregunta"]?.ToString() ?? "";
        }

        /// <summary>
        /// Obtiene el número de orden de la pregunta desde una fila del DataSet
        /// </summary>
        public int ObtenerNumeroPregunta(DataRow fila)
        {
            try
            {
                return Convert.ToInt32(fila?["nOrden"] ?? 0);
            }
            catch
            {
                return 0;
            }
        }
    }
}
