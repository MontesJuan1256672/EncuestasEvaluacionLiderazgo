using EncuestasEvaluacionLiderazgo.Models;
using EncuestasEvaluacionLiderazgo.Data;
using System.Data;
using EncuestaModel = EncuestasEvaluacionLiderazgo.Models.Encuesta;
using PreguntaModel = EncuestasEvaluacionLiderazgo.Models.Pregunta;
using OpcionModel = EncuestasEvaluacionLiderazgo.Models.OpcionRespuesta;

namespace EncuestasEvaluacionLiderazgo.Views.EditaEncuesta
{
    /// <summary>
    /// Code-behind para la vista Index.cshtml de EditaEncuesta
    /// Contiene los métodos helper para la presentación de datos de encuestas en edición
    /// </summary>
    public class IndexModel
    {
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
        /// Obtiene las clases CSS para el badge de estado de la encuesta
        /// </summary>
        public string GetEstadoBgClase(EstadoEncuesta estado)
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
        /// Valida si la encuesta tiene preguntas
        /// </summary>
        public bool TienePreguntasValidas(EncuestaModel encuesta)
        {
            return encuesta?.Preguntas != null && encuesta.Preguntas.Any();
        }

        /// <summary>
        /// Obtiene las preguntas ordenadas por orden de aparición
        /// </summary>
        public List<PreguntaModel> GetPreguntasOrdenadas(EncuestaModel encuesta)
        {
            return encuesta?.Preguntas?.OrderBy(p => p.Orden).ToList() ?? new List<PreguntaModel>();
        }

        /// <summary>
        /// Obtiene las opciones de una pregunta ordenadas por orden de aparición
        /// </summary>
        public List<OpcionModel> GetOpcionesOrdenadas(PreguntaModel pregunta)
        {
            return pregunta?.Opciones?.OrderBy(o => o.Orden).ToList() ?? new List<OpcionModel>();
        }

        /// <summary>
        /// Valida si la encuesta tiene respuestas
        /// </summary>
        public bool TieneRespuestasValidas(EncuestaModel encuesta)
        {
            return encuesta?.Respuestas != null && encuesta.Respuestas.Any();
        }

        /// <summary>
        /// Obtiene el texto del estado de una respuesta
        /// </summary>
        public string GetTextoEstadoRespuesta(bool completada)
        {
            return completada ? "Completada" : "En progreso";
        }

        /// <summary>
        /// Obtiene las clases CSS para el badge de estado de una respuesta
        /// </summary>
        public string GetClaseEstadoRespuesta(bool completada)
        {
            return completada 
                ? "bg-green-100 text-green-800" 
                : "bg-yellow-100 text-yellow-800";
        }

        /// <summary>
        /// Determina si el formulario de edición debe estar habilitado
        /// </summary>
        public bool DebeHabilitarEdicion(EstadoEncuesta estado)
        {
            return estado == EstadoEncuesta.Borrador;
        }

        /// <summary>
        /// Obtiene el atributo disabled si el formulario no debe editarse
        /// </summary>
        public string GetAtributoDisabled(EstadoEncuesta estado)
        {
            return DebeHabilitarEdicion(estado) ? "" : "disabled";
        }

        /// <summary>
        /// Obtiene el texto descriptivo para el estado de una encuesta
        /// </summary>
        public string GetDescripcionEstado(EstadoEncuesta estado)
        {
            return estado switch
            {
                EstadoEncuesta.Borrador => "Aún estás en fase de edición. Publica cuando estés listo.",
                EstadoEncuesta.Publicada => "Los participantes pueden responder esta encuesta.",
                EstadoEncuesta.Cerrada => "Esta encuesta ya no acepta respuestas.",
                EstadoEncuesta.Archivada => "Esta encuesta ha sido archivada.",
                _ => ""
            };
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
        /// Obtiene las filas de preguntas desde el DataSet retornado por ObtenerPreguntasDeBaseDatos
        /// </summary>
        public List<DataRow> ObtenerFilasPreguntasDeBaseDatos(string idTipoEvaluacion)
        {
            try
            {
                var ds = ObtenerPreguntasDeBaseDatos(idTipoEvaluacion);
                if (ds?.Tables.Count > 0)
                {
                    return ds.Tables[0].AsEnumerable().ToList();
                }
            }
            catch
            {
                // Retorna lista vacía en caso de error
            }

            return new List<DataRow>();
        }

        /// <summary>
        /// Obtiene el valor de una celda del DataRow, retornando string.Empty si no existe
        /// </summary>
        public string ObtenerValorDataRow(DataRow row, string columnName)
        {
            try
            {
                if (row != null && row.Table.Columns.Contains(columnName))
                {
                    var valor = row[columnName];
                    return valor != DBNull.Value ? valor.ToString() : string.Empty;
                }
            }
            catch
            {
                // Retorna vacío en caso de error
            }

            return string.Empty;
        }

        /// <summary>
        /// Obtiene el título de la encuesta desde la BD usando FL.TraeEncuestaEvaluaLiderazgo
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
        /// Obtiene el estado activo de la encuesta desde la BD usando FL.TraeEncuestaEvaluaLiderazgo
        /// Retorna true si bActivo = 1, false si bActivo = 0
        /// </summary>
        public bool GetEstadoActivoEncuesta(string filtroTipo)
        {
            // Por defecto retornar true (activo)
            if (string.IsNullOrEmpty(filtroTipo))
            {
                return true;
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
                        var bActivo = row["bActivo"];
                        
                        // Si bActivo es 1, retorna true; si es 0, retorna false
                        if (bActivo != DBNull.Value)
                        {
                            return Convert.ToInt32(bActivo) == 1;
                        }
                    }
                }

                return true;
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// Obtiene la clave de acceso de la encuesta desde la BD usando FL.TraeEncuestaEvaluaLiderazgo
        /// </summary>
        public string GetClaveAccesoEncuesta(string filtroTipo)
        {
            // Si el filtroTipo está vacío, retornar vacío
            if (string.IsNullOrEmpty(filtroTipo))
            {
                return string.Empty;
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
                        return row["cClaveAcceso"]?.ToString() ?? string.Empty;
                    }
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Obtiene la lista de competencias para el select con IDCatalogo
        /// </summary>
        public List<KeyValuePair<string, string>> ObtenerCompetencias()
        {
            List<KeyValuePair<string, string>> competencias = new List<KeyValuePair<string, string>>();
            try
            {
                DataSet ds = FL.TraeCompetencias();
                if (ds != null && ds.Tables.Count > 0)
                {
                    // Verificar si hay error en la primera tabla
                    if (ds.Tables[0].Columns.Contains("Error"))
                    {
                        System.Diagnostics.Debug.WriteLine("Error en TraeCompetencias: " + ds.Tables[0].Rows[0]["Error"]);
                        return competencias;
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string competencia = ObtenerValorDataRow(row, "Descripcion");
                            string idCatalogo = ObtenerValorDataRow(row, "IDCatalogo");
                            if (!string.IsNullOrWhiteSpace(competencia) && !string.IsNullOrWhiteSpace(idCatalogo))
                            {
                                bool exists = competencias.Any(c => c.Key == competencia);
                                if (!exists)
                                {
                                    competencias.Add(new KeyValuePair<string, string>(competencia, idCatalogo));
                                }
                            }
                        }
                    }
                }
                competencias = competencias.OrderBy(c => c.Key).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en ObtenerCompetencias: " + ex.Message);
            }

            return competencias;
        }

        /// <summary>
        /// Obtiene la lista de actividades para el select con IDCatalogo
        /// </summary>
        public List<KeyValuePair<string, string>> ObtenerActividades()
        {
            List<KeyValuePair<string, string>> actividades = new List<KeyValuePair<string, string>>();
            try
            {
                DataSet ds = EncuestasEvaluacionLiderazgo.Data.FL.TraeActividades();
                if (ds != null && ds.Tables.Count > 0)
                {
                    // Verificar si hay error en la primera tabla
                    if (ds.Tables[0].Columns.Contains("Error"))
                    {
                        System.Diagnostics.Debug.WriteLine("Error en TraeActividades: " + ds.Tables[0].Rows[0]["Error"]);
                        return actividades;
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string actividad = ObtenerValorDataRow(row, "Descripcion");
                            string idCatalogo = ObtenerValorDataRow(row, "IDCatalogo");
                            if (!string.IsNullOrWhiteSpace(actividad) && !string.IsNullOrWhiteSpace(idCatalogo))
                            {
                                bool exists = actividades.Any(a => a.Key == actividad);
                                if (!exists)
                                {
                                    actividades.Add(new KeyValuePair<string, string>(actividad, idCatalogo));
                                }
                            }
                        }
                    }
                }
                actividades = actividades.OrderBy(a => a.Key).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en ObtenerActividades: " + ex.Message);
            }

            return actividades;
        }

        /// <summary>
        /// Obtiene la lista de descripciones para el select con IDCatalogo
        /// </summary>
        public List<KeyValuePair<string, string>> ObtenerDescripciones()
        {
            List<KeyValuePair<string, string>> descripciones = new List<KeyValuePair<string, string>>();
            try
            {
                DataSet ds = EncuestasEvaluacionLiderazgo.Data.FL.TraeDescripciones();
                if (ds != null && ds.Tables.Count > 0)
                {
                    // Verificar si hay error en la primera tabla
                    if (ds.Tables[0].Columns.Contains("Error"))
                    {
                        System.Diagnostics.Debug.WriteLine("Error en TraeDescripciones: " + ds.Tables[0].Rows[0]["Error"]);
                        return descripciones;
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string descripcion = ObtenerValorDataRow(row, "cDescripcion");
                            string idCatalogo = ObtenerValorDataRow(row, "IDPerfilPregunta");
                            if (!string.IsNullOrWhiteSpace(descripcion) && !string.IsNullOrWhiteSpace(idCatalogo))
                            {
                                bool exists = descripciones.Any(d => d.Key == descripcion);
                                if (!exists)
                                {
                                    descripciones.Add(new KeyValuePair<string, string>(descripcion, idCatalogo));
                                }
                            }
                        }
                    }
                }
                descripciones = descripciones.OrderBy(d => d.Key).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en ObtenerDescripciones: " + ex.Message);
            }

            return descripciones;
        }

        /// <summary>
        /// Obtiene el próximo número de orden (máximo actual + 1)
        /// </summary>
        public int ObtenerProximoNumeroOrden(string filtroTipo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filtroTipo))
                    return 1;

                var filasPreguntas = ObtenerFilasPreguntasDeBaseDatos(filtroTipo);
                if (filasPreguntas.Count == 0)
                    return 1;

                int maxOrden = 0;
                foreach (var fila in filasPreguntas)
                {
                    string valorOrden = ObtenerValorDataRow(fila, "nOrden");
                    if (int.TryParse(valorOrden, out int orden))
                    {
                        if (orden > maxOrden)
                            maxOrden = orden;
                    }
                }

                return maxOrden + 1;
            }
            catch
            {
                return 1;
            }
        }
    }
}
