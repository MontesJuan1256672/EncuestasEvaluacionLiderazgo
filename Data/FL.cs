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
        /// Obtiene las preguntas según el tipo de evaluación ejecutando el SP sp_traePreguntasII
        /// </summary>
        /// <param name="IdTipoEvaluacion">Identificador del tipo de evaluación</param>
        /// <returns>DataSet con las preguntas</returns>
        public static DataSet TraePreguntasII(string IdTipoEvaluacion)
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
                using (SqlCommand command = new SqlCommand("sp_traePreguntasII", sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 0;

                    // Agregar el parámetro
                    command.Parameters.AddWithValue("@IdTipoEvaluacion", IdTipoEvaluacion);

                    // Ejecutar el stored procedure
                    sqlDataAdapter = new SqlDataAdapter(command);
                    sqlDataAdapter.Fill(dataSet);
                }

                return dataSet;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener preguntas: " + ex.Message);
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
    }
}

