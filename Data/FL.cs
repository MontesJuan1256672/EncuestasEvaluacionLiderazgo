using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace EncuestasEvaluacionLiderazgo.Data
{
    /// <summary>
    /// FL (Funciones Lógicas) - Capa de Lógica de Negocio
    /// Contiene las reglas de negocio y validaciones de la aplicación
    /// </summary>
    public class FL
    {
        /// <summary>
        /// Valida si un objeto es numérico
        /// </summary>
        /// <param name="Expression">Objeto a validar</param>
        /// <returns>True si es numérico, False en caso contrario</returns>
        public static bool IsNumeric(object Expression)
        {
            bool isNum;
            double retNum;
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        /// <summary>
        /// Obtiene la cadena de conexión disponible según el centro primario
        /// 
        /// idPrimaryCenter = 1 - Hipodromo
        /// idPrimaryCenter = 2 - Torres - Campestre
        /// idPrimaryCenter = 3 - Mexicali
        /// idPrimaryCenter = 4 - Centro Histórico
        /// 
        /// El método intenta conectar al centro solicitado y, si no está disponible,
        /// intenta con los otros centros en orden de prioridad.
        /// </summary>
        /// <param name="idConnectionString">Identificador de la cadena de conexión</param>
        /// <param name="idPrimaryCenter">ID del centro primario</param>
        /// <param name="isProduction">Indica si es ambiente de producción</param>
        /// <param name="useSSL">Indica si se debe usar SSL para la conexión</param>
        /// <returns>Cadena de conexión disponible o vacía si ninguna está disponible</returns>
        public static string GetWebServiceAvailable(string idConnectionString, int idPrimaryCenter, bool isProduction, bool useSSL)
        {
            // Nota: Esta es una estructura adaptada. En producción se usaría ConnStringsTelvista.ConnectionWebApi
            // ConnStringsTelvista.ConnectionWebApi con = new ConnStringsTelvista.ConnectionWebApi();

            if (!FL.IsNumeric(idPrimaryCenter))
            {
                return string.Empty;
            }

            string connStrResult = string.Empty;

            switch (idPrimaryCenter)
            {
                case 1: //HIPODROMO
                    try
                    {
                        // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.HIP, isProduction, useSSL);
                        // if (connStrResult.Length > 0) return connStrResult;

                        // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.CAM, isProduction, useSSL);
                        // if (connStrResult.Length > 0) return connStrResult;

                        // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.MXL, isProduction, useSSL);
                        // if (connStrResult.Length > 0) return connStrResult;

                        // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.CH, isProduction, useSSL);
                        // if (connStrResult.Length > 0) return connStrResult;
                    }
                    catch (Exception Ex)
                    {
                        string exep = Ex.Message;
                        return string.Empty;
                    }

                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.HIP, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.CAM, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.MXL, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.CH, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    break;
                case 2: //TORRES || CAMPESTRE
                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.CAM, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.HIP, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.MXL, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.CH, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    break;
                case 3: //MEXICALI
                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.MXL, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.HIP, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.CAM, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.CH, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    break;
                case 4: //CENTRO HISTORICO
                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.CH, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.HIP, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.CAM, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    // connStrResult = con.getConnectionString(idConnectionString, ConnStringsTelvista.Centros.MXL, isProduction, useSSL);
                    // if (connStrResult.Length > 0) return connStrResult;

                    break;
            }
            return "";
        }

        /// <summary>
        /// Obtiene la cadena de conexión específica para un centro
        /// </summary>
        /// <param name="center">Centro solicitado (HIP, CAM, MXL, CH)</param>
        /// <returns>Cadena de conexión para el centro</returns>
        public static string GetConnectionStringByCenter(string center)
        {
            // Implementar según la lógica de negocio específica
            // Por ahora retorna vacío
            return string.Empty;
        }

        /// <summary>
        /// Valida la disponibilidad de un servicio web
        /// </summary>
        /// <param name="serviceName">Nombre del servicio</param>
        /// <returns>True si el servicio está disponible, False en caso contrario</returns>
        public static bool IsServiceAvailable(string serviceName)
        {
            try
            {
                // Implementar lógica de validación de disponibilidad
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene los tipos de evaluación disponibles ejecutando el SP sp_traeTiposEvaluacion
        /// </summary>
        /// <returns>DataSet con los tipos de evaluación</returns>
        public static DataSet TraeTiposEvaluacion()
        {
            SqlConnection sqlConnection = null;
            SqlDataAdapter sqlDataAdapter = null;
            DataSet dataSet = new DataSet();

            try
            {
                // Obtener la conexión del DWH
                string connectionString = DL.GetConEvaluaLiderazgo();
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("No se pudo obtener la cadena de conexión del EvaluaLiderazgo");
                }

                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                // Crear comando para ejecutar el SP
                using (SqlCommand command = new SqlCommand("sp_traeTiposEvaluacion", sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 0;

                    // Ejecutar el stored procedure
                    sqlDataAdapter = new SqlDataAdapter(command);
                    sqlDataAdapter.Fill(dataSet);
                }

                return dataSet;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener tipos de evaluación: " + ex.Message);
            }
            finally
            {
                if (sqlConnection != null && sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                    sqlConnection.Dispose();
                }

                if (sqlDataAdapter != null)
                {
                    sqlDataAdapter.Dispose();
                }

                GC.Collect();
            }
        }

        /// <summary>
        /// Obtiene las preguntas según el tipo de evaluación
        /// </summary>
        /// <param name="IdTipoEvaluacion">Identificador del tipo de evaluación</param>
        /// <returns>DataSet con las preguntas</returns>
        public static DataSet TraePreguntasII(string IdTipoEvaluacion)
        {
            return DL.TraePreguntasII(IdTipoEvaluacion);
        }

        /// <summary>
        /// Ejecuta una consulta genérica SQL según el tipo de evaluación
        /// </summary>
        /// <param name="idTipoEvaluacion">Identificador del tipo de evaluación</param>
        /// <returns>DataSet con los resultados de la consulta</returns>
        public static DataSet TraeEncuestaEvaluaLiderazgo(string idTipoEvaluacion)
        {
            try
            {
                // Formar la consulta SQL según el tipo de evaluación
                string query = $"SELECT * FROM cat_TiposEvaluacion where IDTipoEvaluacion = '{idTipoEvaluacion}'";
                
                // Ejecutar la consulta a través de DL
                return DL.queryGenericoEvaluacionLiderazgo(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar consulta genérica: " + ex.Message);
            }
        }

        /// <summary>
        /// Actualiza el estado activo de una encuesta
        /// </summary>
        /// <param name="idTipoEvaluacion">ID del tipo de evaluación</param>
        /// <param name="bActivo">Estado activo (1 = Activo, 0 = Baja)</param>
        /// <returns>True si la actualización fue exitosa, False en caso contrario</returns>
        public static bool ActualizaEstadoEncuesta(string idTipoEvaluacion, int bActivo)
        {
            try
            {
                // Validar parámetros
                if (string.IsNullOrEmpty(idTipoEvaluacion))
                {
                    throw new Exception("El ID del tipo de evaluación no puede estar vacío.");
                }

                if (bActivo != 0 && bActivo != 1)
                {
                    throw new Exception("El estado activo debe ser 0 o 1.");
                }

                // Llamar al método DL para actualizar el estado
                return DL.ActualizaEstadoEncuesta(idTipoEvaluacion, bActivo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el estado de la encuesta: " + ex.Message);
            }
        }

        /// <summary>
        /// Inserta una nueva pregunta en la encuesta
        /// </summary>
        public static bool InsertaPreguntaII(int idTipoEvaluacion, string cPregunta, string cPregunta_Ingles,
                                             int cCompetencia, int cActividad, int cDescripcion, int nOrden)
        {
            try
            {
                // Validar parámetros
                if (idTipoEvaluacion <= 0)
                    throw new Exception("El ID del tipo de evaluación debe ser mayor a 0.");
                
                if (string.IsNullOrWhiteSpace(cPregunta))
                    throw new Exception("La pregunta no puede estar vacía.");
                
                if (cCompetencia <= 0)
                    throw new Exception("El ID de competencia debe ser mayor a 0.");
                
                if (cActividad <= 0)
                    throw new Exception("El ID de actividad debe ser mayor a 0.");
                
                if (cDescripcion <= 0)
                    throw new Exception("El ID de descripción debe ser mayor a 0.");
                
                if (nOrden <= 0)
                    throw new Exception("El número de orden debe ser mayor a 0.");

                // Llamar al método DL para insertar la pregunta
                return DL.InsertaPreguntaII(idTipoEvaluacion, cPregunta, cPregunta_Ingles, 
                                           cCompetencia, cActividad, cDescripcion, nOrden);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar la pregunta: " + ex.Message);
            }
        }

        /// <summary>
        /// Actualiza una pregunta existente en la encuesta
        /// </summary>
        public static bool UpdatePreguntaII(int idPregunta, int idTipoEvaluacion, string cPregunta, string cPregunta_Ingles,
                                            int cCompetencia, int cActividad, int cDescripcion, int nOrden)
        {
            try
            {
                // Validar parámetros
                if (idPregunta <= 0)
                    throw new Exception("El ID de la pregunta debe ser mayor a 0.");
                
                if (idTipoEvaluacion <= 0)
                    throw new Exception("El ID del tipo de evaluación debe ser mayor a 0.");
                
                if (string.IsNullOrWhiteSpace(cPregunta))
                    throw new Exception("La pregunta no puede estar vacía.");
                
                if (cCompetencia <= 0)
                    throw new Exception("El ID de competencia debe ser mayor a 0.");
                
                if (cActividad <= 0)
                    throw new Exception("El ID de actividad debe ser mayor a 0.");
                
                if (cDescripcion <= 0)
                    throw new Exception("El ID de descripción debe ser mayor a 0.");
                
                if (nOrden <= 0)
                    throw new Exception("El número de orden debe ser mayor a 0.");

                // Llamar al método DL para actualizar la pregunta
                return DL.UpdatePreguntaII(idPregunta, idTipoEvaluacion, cPregunta, cPregunta_Ingles, 
                                          cCompetencia, cActividad, cDescripcion, nOrden);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la pregunta: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene las competencias únicas de una evaluación
        /// </summary>
        public static DataSet TraeCompetencias()
        {
            return DL.TraeCompetencias();
        }

        /// <summary>
        /// Obtiene las actividades únicas de una evaluación
        /// </summary>
        public static DataSet TraeActividades()
        {
            return DL.TraeActividades();
        }

        /// <summary>
        /// Obtiene las descripciones únicas de una evaluación
        /// </summary>
        public static DataSet TraeDescripciones()
        {
            return DL.TraeDescripciones();
        }

        /// <summary>
        /// Actualiza el estado activo de una pregunta
        /// </summary>
        /// <param name="idPregunta">ID de la pregunta</param>
        /// <param name="bActivo">Estado activo (1 = Activo, 0 = Inactivo)</param>
        /// <returns>True si la actualización fue exitosa, False en caso contrario</returns>
        public static bool ActualizaEstadoPregunta(int idPregunta, int bActivo)
        {
            try
            {
                // Validar parámetros
                if (idPregunta <= 0)
                {
                    throw new Exception("El ID de la pregunta debe ser mayor a 0.");
                }

                if (bActivo != 0 && bActivo != 1)
                {
                    throw new Exception("El estado debe ser 0 o 1.");
                }

                // Llamar al método DL para actualizar el estado
                return DL.ActualizaEstadoPregunta(idPregunta, bActivo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el estado de la pregunta: " + ex.Message);
            }
        }

        /// <summary>
        /// Actualiza el número de orden de una pregunta
        /// </summary>
        /// <param name="idPregunta">ID de la pregunta</param>
        /// <param name="nOrden">Nuevo número de orden</param>
        /// <returns>True si la actualización fue exitosa, False en caso contrario</returns>
        public static bool ActualizaNOrden(int idPregunta, int nOrden)
        {
            try
            {
                // Validar parámetros
                if (idPregunta <= 0)
                {
                    throw new Exception("El ID de la pregunta debe ser mayor a 0.");
                }

                if (nOrden <= 0)
                {
                    throw new Exception("El número de orden debe ser mayor a 0.");
                }

                // Llamar al método DL para actualizar el número de orden
                return DL.ActualizaNOrden(idPregunta, nOrden);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el número de orden: " + ex.Message);
            }
        }
    }
}
