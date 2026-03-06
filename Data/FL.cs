using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using EncuestasEvaluacionLiderazgo.Models;

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
            try
            {
                return DL.TraeTiposEvaluacion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener tipos de evaluación: " + ex.Message);
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

        /// <summary>
        /// Actualiza la clave de acceso de una encuesta
        /// </summary>
        /// <param name="idTipoEvaluacion">ID del tipo de evaluación</param>
        /// <param name="cClaveAcceso">Nueva clave de acceso</param>
        /// <returns>True si la actualización fue exitosa, False en caso contrario</returns>
        public static bool ActualizaClaveAcceso(string idTipoEvaluacion, string cClaveAcceso)
        {
            try
            {
                // Validar parámetros
                if (string.IsNullOrWhiteSpace(idTipoEvaluacion))
                {
                    throw new Exception("El ID del tipo de evaluación no puede estar vacío.");
                }

                if (string.IsNullOrWhiteSpace(cClaveAcceso))
                {
                    throw new Exception("La clave de acceso no puede estar vacía.");
                }

                // Llamar al método DL para actualizar la clave de acceso
                return DL.ActualizaClaveAcceso(idTipoEvaluacion, cClaveAcceso);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la clave de acceso: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene las personas a evaluar según el tipo de evaluación
        /// Nota: Se completará cuando la tabla de personas esté lista en la BD
        /// </summary>
        /// <param name="idTipoEvaluacion">Identificador del tipo de evaluación</param>
        /// <returns>DataSet con las personas a evaluar</returns>
        public static DataSet TraePersonasEvaluar(int idTipoEvaluacion)
        {
            try
            {
                return DL.TraePersonasEvaluar(idTipoEvaluacion);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener personas a evaluar: " + ex.Message);
            }
        }

        /// <summary>
        /// Busca un empleado por su número y ubicación
        /// </summary>
        /// <param name="noemp">Número de empleado</param>
        /// <param name="idUbicacion">ID de la ubicación/ciudad</param>
        /// <returns>DataTable con la información del empleado</returns>
        public static DataTable BuscarEmpleado(int noemp, int idUbicacion)
        {
            try
            {
                string query = "SELECT * FROM Tr3ss.Personal WHERE NoEmp = " + noemp + " AND IDUbicacion = " + idUbicacion;
                return DL.QueryGenericoDWH(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar empleado: " + ex.Message);
            }
        }

        /// <summary>
        /// Inserta una relación de tipo de encuesta y personal
        /// Llama a DL.InsertaPorIdPersonal que ejecuta sp_Inserta_PorIdPersonal
        /// </summary>
        /// <param name="idTipoEncuesta">ID del tipo de encuesta</param>
        /// <param name="idPersonal">ID del personal</param>
        /// <returns>ID del registro insertado o 0 si no se pudo insertar</returns>
        public static int InsertaPorIdPersonal(int idTipoEncuesta, int idPersonal)
        {
            try
            {
                return DL.InsertaPorIdPersonal(idTipoEncuesta, idPersonal);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar relación tipo encuesta-personal: " + ex.Message);
            }
        }

        /// <summary>
        /// Elimina (soft-delete) una persona a evaluar
        /// Llama a DL.EliminaPersona que ejecuta sp_Elimina_PorIdPersonal
        /// </summary>
        /// <param name="idPersonal">ID del personal a eliminar</param>
        /// <returns>1 si la operación fue exitosa, 0 si no se encontró el registro</returns>
        public static int EliminaPersona(int idPersonal)
        {
            try
            {
                return DL.EliminaPersona(idPersonal);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar persona a evaluar: " + ex.Message);
            }
        }

        /// <summary>
        /// Inserta una evaluación de liderazgo
        /// </summary>
        /// <param name="respuesta">Objeto Respuesta con los datos de la evaluación</param>
        /// <returns>String con el resultado de la operación (ID de la evaluación insertada)</returns>
        public static string InsertaEvaluacion(Respuesta respuesta)
        {
            try
            {
                return DL.InsertaEvaluacion(respuesta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar evaluación: " + ex.Message);
            }
        }

        /// <summary>
        /// Inserta una respuesta de evaluación
        /// </summary>
        /// <param name="idEvaluacion">ID de la evaluación</param>
        /// <param name="idPregunta">ID de la pregunta</param>
        /// <param name="nRespuesta">Valor numérico de la respuesta (1-5)</param>
        /// <param name="cComentarios">Comentarios de la respuesta</param>
        /// <returns>String con el resultado de la operación</returns>
        public static string InsertaRespuesta(int idEvaluacion, int idPregunta, int nRespuesta, string cComentarios)
        {
            try
            {
                return DL.InsertaRespuesta(idEvaluacion, idPregunta, nRespuesta, cComentarios);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar respuesta: " + ex.Message);
            }
        }

        /// <summary>
        /// Inserta un registro en tblContesto indicando que el usuario contestó la encuesta
        /// </summary>
        /// <param name="idPersonalDWH">ID del personal en DWH</param>
        /// <returns>String con el ID del registro insertado</returns>
        public static string InseRegistro(int idPersonalDWH)
        {
            try
            {
                return DL.InseRegistro(idPersonalDWH);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar registro: " + ex.Message);
            }
        }

    }
}
