using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ConnStringsTelvista;
using Librerias;
using EncuestasEvaluacionLiderazgo.Models;

namespace EncuestasEvaluacionLiderazgo.Data
{
    /// <summary>
    /// DL (Data Access Layer) - Capa de Acceso a Datos
    /// Contiene todos los métodos para acceder a la base de datos
    /// Gestiona conexiones, comandos SQL y operaciones de datos
    /// </summary>
    public class DL
    {
        private static string ConStr = GetConEvaluaLiderazgo();
        private static string ConDWH = GetConDWH();

        /// <summary>
        /// Obtiene la cadena de conexión para el Data Warehouse (DWH)
        /// </summary>
        /// <returns>Cadena de conexión al DWH</returns>
        public static string GetConDWH()
        {
            string Coneccion = "";
            ConnectionWebApi cs_telvista = new ConnectionWebApi();
            Coneccion = cs_telvista.getConnectionString("2f78398c-55ec-4ba3-aa22-9cdd889240bf", Centros.HIP, true, false);
            return Coneccion;
        }

        
        public static string GetConEvaluaLiderazgo()
        {
            string Coneccion = "";
            ConnectionWebApi cs_telvista = new ConnectionWebApi();
            Coneccion = cs_telvista.getConnectionString("e03c659c-a2bc-409c-b1be-51d34218933c", Centros.HIP, true, false);
            return Coneccion;
        }

        /// <summary>
        /// Obtiene la cadena de conexión principal usando la lógica de negocio
        /// </summary>
        /// <returns>Cadena de conexión válida</returns>
        private static string CS_ConStr()
        {
            // Producción - Obtiene la cadena de conexión disponible
            return FL.GetWebServiceAvailable("438A9C25-EB3F-4816-9792-809B147BAA70", 4, true, false);
        }

        /// <summary>
        /// Abre una conexión SQL con la cadena de conexión principal
        /// </summary>
        /// <returns>SqlConnection abierta</returns>
        private static SqlConnection GetConnection()
        {
            SqlConnection Conn = new SqlConnection(ConStr);
            Conn.Open();
            return Conn;
        }

        /// <summary>
        /// Abre una conexión SQL con una cadena de conexión específica
        /// </summary>
        /// <param name="myconn">Cadena de conexión personalizada</param>
        /// <returns>SqlConnection abierta</returns>
        private static SqlConnection GetConnection2(string myconn)
        {
            SqlConnection Conn = new SqlConnection(myconn);
            Conn.Open();
            return Conn;
        }

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
        /// Desactiva la base de datos ejecutando un procedimiento almacenado
        /// </summary>
        public static void Desactiva_BD()
        {
            SqlConnection SqlCon = null;
            try
            {
                SqlCon = GetConnection();
                using (SqlCommand cmd = new SqlCommand("Desactiva_BD", SqlCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (SqlCon != null)
                    SqlCon.Dispose();
                GC.Collect();
            }
        }

        /// <summary>
        /// Actualiza el registro de declaración de un personal
        /// </summary>
        /// <param name="IdPersonal">ID del personal</param>
        /// <param name="Año">Año de la declaración</param>
        /// <param name="Aceptado">Estado de aceptación</param>
        /// <returns>ID del registro insertado o 0 si no se pudo insertar</returns>
        public static int ActualizaLogDeclaracion(int IdPersonal, int Año, int Aceptado)
        {
            SqlConnection SqlCon = null;
            DataTable dt = new DataTable();
            try
            {
                SqlCon = GetConnection();
                using (SqlCommand cmd = new SqlCommand("Inserta_LogDeclaracion", SqlCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdPersonal", SqlDbType.Int).Value = IdPersonal;
                    cmd.Parameters.Add("@Año", SqlDbType.Int).Value = Año;
                    cmd.Parameters.Add("@IdAceptado", SqlDbType.VarChar).Value = Aceptado;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    dt = ds.Tables[0];

                    if (dt.Rows.Count > 0)
                        return int.Parse(dt.Rows[0][0].ToString());
                    else
                        return 0;
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (SqlCon != null)
                    SqlCon.Dispose();
                GC.Collect();
                dt.Dispose();
            }
        }

        /// <summary>
        /// Actualiza la confirmación de convenio para un personal
        /// </summary>
        /// <param name="IdPersonal">ID del personal</param>
        /// <param name="IdCentro">ID del centro o ubicación</param>
        /// <param name="NoEmp">Número de empleado</param>
        /// <param name="Aceptado">Estado de aceptación (1 = Aceptado, 0 = Rechazado)</param>
        public static void ActualizaConfirmacionConvenio(int IdPersonal, int IdCentro, int NoEmp, int Aceptado)
        {
            SqlConnection SqlCon = null;
            try
            {
                SqlCon = GetConnection();
                using (SqlCommand cmd = new SqlCommand("jrd.Inserta_ConfirmacionConvenio", SqlCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdPersonal", SqlDbType.Int).Value = IdPersonal;
                    cmd.Parameters.Add("@IdUbicacion", SqlDbType.Int).Value = IdCentro;
                    cmd.Parameters.Add("@NoEmp", SqlDbType.Int).Value = NoEmp;
                    cmd.Parameters.Add("@Aceptado", SqlDbType.Int).Value = Aceptado;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (SqlCon != null)
                    SqlCon.Dispose();
                GC.Collect();
            }
        }

        /// <summary>
        /// Ejecuta una consulta genérica contra el Data Warehouse
        /// </summary>
        /// <param name="Query">Consulta SQL a ejecutar</param>
        /// <returns>DataTable con los resultados o tabla de error si falla</returns>
        public static DataTable QueryGenericoDWH(string Query)
        {
            SqlCommand sqlcommand = new SqlCommand();
            SqlDataAdapter sqldataadapter = new SqlDataAdapter();
            DataTable datatable = new DataTable();
            try
            {
                sqlcommand = new SqlCommand(Query);
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Connection = new SqlConnection(ConDWH);
                sqldataadapter = new SqlDataAdapter(sqlcommand);

                sqlcommand.CommandTimeout = 0;
                sqlcommand.Connection.Open();
                sqldataadapter.Fill(datatable);
                sqlcommand.Connection.Close();
                return datatable;
            }
            catch (Exception e)
            {
                DataRow ren;
                datatable = new DataTable();
                datatable.Columns.Add(new DataColumn("Id", typeof(int)));
                datatable.Columns.Add(new DataColumn("Error", typeof(string)));

                ren = datatable.NewRow();
                ren["Id"] = 0;
                ren["Error"] = e.Message;
                datatable.Rows.Add(ren);

                return datatable;
            }
            finally
            {
                if ((sqlcommand != null) && (sqlcommand.Connection != null))
                {
                    if (sqlcommand.Connection.State == ConnectionState.Open)
                        sqlcommand.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Ejecuta una consulta genérica contra la base de datos de Políticas Telvista
        /// </summary>
        /// <param name="Query">Consulta SQL a ejecutar</param>
        /// <returns>DataTable con los resultados o tabla de error si falla</returns>
        public static DataTable QueryGenericoPoliticasTelvista(string Query)
        {
            SqlCommand sqlcommand = new SqlCommand();
            SqlDataAdapter sqldataadapter = new SqlDataAdapter();
            DataTable datatable = new DataTable();
            try
            {
                sqlcommand = new SqlCommand(Query);
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Connection = new SqlConnection(ConStr);

                sqldataadapter = new SqlDataAdapter(sqlcommand);

                sqlcommand.CommandTimeout = 0;
                sqlcommand.Connection.Open();
                sqldataadapter.Fill(datatable);
                sqlcommand.Connection.Close();
                return datatable;
            }
            catch (Exception e)
            {
                DataRow ren;
                datatable = new DataTable();
                datatable.Columns.Add(new DataColumn("Id", typeof(int)));
                datatable.Columns.Add(new DataColumn("Error", typeof(string)));

                ren = datatable.NewRow();
                ren["Id"] = 0;
                ren["Error"] = e.Message;
                datatable.Rows.Add(ren);

                return datatable;
            }
            finally
            {
                if ((sqlcommand != null) && (sqlcommand.Connection != null))
                {
                    if (sqlcommand.Connection.State == ConnectionState.Open)
                        sqlcommand.Connection.Close();
                }
            }
        }
    
        public static DataSet queryGenericoEvaluacionLiderazgo(string Query)
        {
            SqlCommand sqlcommand = new SqlCommand();
            SqlDataAdapter sqldataadapter = new SqlDataAdapter();
            DataSet dataset = new DataSet();
            try
            {
                sqlcommand = new SqlCommand(Query);
                sqlcommand.CommandType = CommandType.Text;
                sqlcommand.Connection = new SqlConnection(ConStr);
                sqldataadapter = new SqlDataAdapter(sqlcommand);

                sqlcommand.CommandTimeout = 0;
                sqlcommand.Connection.Open();
                sqldataadapter.Fill(dataset);
                sqlcommand.Connection.Close();
                return dataset;
            }
            catch (Exception e)
            {
                DataTable datatable = new DataTable();
                DataRow ren;
                datatable.Columns.Add(new DataColumn("Id", typeof(int)));
                datatable.Columns.Add(new DataColumn("Error", typeof(string)));

                ren = datatable.NewRow();
                ren["Id"] = 0;
                ren["Error"] = e.Message;
                datatable.Rows.Add(ren);

                dataset.Tables.Add(datatable);
                return dataset;
            }
            finally
            {
                if ((sqlcommand != null) && (sqlcommand.Connection != null))
                {
                    if (sqlcommand.Connection.State == ConnectionState.Open)
                        sqlcommand.Connection.Close();
                }
            }
        }

        public static DataSet TraeTiposEvaluacion()
        {
            SqlConnection sqlConnection = null;
            SqlDataAdapter sqlDataAdapter = null;
            DataSet dataSet = new DataSet();

            try
            {
                string connectionString = ConStr;
                
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
        /// Inserta una nueva pregunta en la encuesta de evaluación de liderazgo
        /// </summary>
        public static bool InsertaPreguntaII(int idTipoEvaluacion, string cPregunta, string cPregunta_Ingles,
                                             int cCompetencia, int cActividad, int cDescripcion, int nOrden)
        {
            SqlCommand sqlcommand = null;
            try
            {
                sqlcommand = new SqlCommand("sp_insertaPreguntaII", new SqlConnection(ConStr));
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandTimeout = 0;

                // Agregar parámetros
                sqlcommand.Parameters.Add("@IdTipoEvaluacion", SqlDbType.Int).Value = idTipoEvaluacion;
                sqlcommand.Parameters.Add("@cPregunta", SqlDbType.VarChar).Value = cPregunta;
                sqlcommand.Parameters.Add("@cPregunta_Ingles", SqlDbType.VarChar).Value = cPregunta_Ingles;
                sqlcommand.Parameters.Add("@IdCompetencia", SqlDbType.Int).Value = cCompetencia;
                sqlcommand.Parameters.Add("@IdActividad", SqlDbType.Int).Value = cActividad;
                sqlcommand.Parameters.Add("@IDPerfilPregunta", SqlDbType.Int).Value = cDescripcion;
                sqlcommand.Parameters.Add("@nOrden", SqlDbType.Int).Value = nOrden;

                sqlcommand.Connection.Open();
                sqlcommand.ExecuteNonQuery();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (sqlcommand != null && sqlcommand.Connection != null)
                {
                    if (sqlcommand.Connection.State == ConnectionState.Open)
                        sqlcommand.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Actualiza una pregunta existente en la encuesta de evaluación de liderazgo
        /// </summary>
        public static bool UpdatePreguntaII(int idPregunta, int idTipoEvaluacion, string cPregunta, string cPregunta_Ingles,
                                            int cCompetencia, int cActividad, int cDescripcion, int nOrden)
        {
            SqlCommand sqlcommand = null;
            try
            {
                sqlcommand = new SqlCommand("sp_actuPreguntaIII", new SqlConnection(ConStr));
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandTimeout = 0;

                // Agregar parámetros
                sqlcommand.Parameters.Add("@IdPregunta", SqlDbType.Int).Value = idPregunta;
                sqlcommand.Parameters.Add("@IdTipoEvaluacion", SqlDbType.Int).Value = idTipoEvaluacion;
                sqlcommand.Parameters.Add("@cPregunta", SqlDbType.VarChar).Value = cPregunta;
                sqlcommand.Parameters.Add("@cPregunta_Ingles", SqlDbType.VarChar).Value = cPregunta_Ingles;
                sqlcommand.Parameters.Add("@IdCompetencia", SqlDbType.Int).Value = cCompetencia;
                sqlcommand.Parameters.Add("@IdActividad", SqlDbType.Int).Value = cActividad;
                sqlcommand.Parameters.Add("@IDPerfilPregunta", SqlDbType.Int).Value = cDescripcion;
                sqlcommand.Parameters.Add("@nOrden", SqlDbType.Int).Value = nOrden;

                sqlcommand.Connection.Open();
                sqlcommand.ExecuteNonQuery();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (sqlcommand != null && sqlcommand.Connection != null)
                {
                    if (sqlcommand.Connection.State == ConnectionState.Open)
                        sqlcommand.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Obtiene las competencias únicas de una evaluación
        /// </summary>
        public static DataSet TraeCompetencias()
        {
            SqlCommand sqlcommand = null;
            SqlDataAdapter sqldataadapter = null;
            DataSet dataset = new DataSet();
            try
            {
                sqlcommand = new SqlCommand("sp_traeCompetencias", new SqlConnection(ConStr));
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandTimeout = 0;

                sqldataadapter = new SqlDataAdapter(sqlcommand);
                sqldataadapter.Fill(dataset);

                return dataset;
            }
            catch (Exception ex)
            {
                DataTable datatable = new DataTable();
                DataRow ren;
                datatable.Columns.Add(new DataColumn("Id", typeof(int)));
                datatable.Columns.Add(new DataColumn("Error", typeof(string)));

                ren = datatable.NewRow();
                ren["Id"] = 0;
                ren["Error"] = ex.Message;
                datatable.Rows.Add(ren);

                dataset.Tables.Add(datatable);
                return dataset;
            }
            finally
            {
                if (sqlcommand != null && sqlcommand.Connection != null)
                {
                    if (sqlcommand.Connection.State == ConnectionState.Open)
                        sqlcommand.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Obtiene las actividades únicas de una evaluación
        /// </summary>
        public static DataSet TraeActividades()
        {
            SqlCommand sqlcommand = null;
            SqlDataAdapter sqldataadapter = null;
            DataSet dataset = new DataSet();
            try
            {
                sqlcommand = new SqlCommand("sp_traeActividades", new SqlConnection(ConStr));
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandTimeout = 0;

                sqldataadapter = new SqlDataAdapter(sqlcommand);
                sqldataadapter.Fill(dataset);

                return dataset;
            }
            catch (Exception ex)
            {
                DataTable datatable = new DataTable();
                DataRow ren;
                datatable.Columns.Add(new DataColumn("Id", typeof(int)));
                datatable.Columns.Add(new DataColumn("Error", typeof(string)));

                ren = datatable.NewRow();
                ren["Id"] = 0;
                ren["Error"] = ex.Message;
                datatable.Rows.Add(ren);

                dataset.Tables.Add(datatable);
                return dataset;
            }
            finally
            {
                if (sqlcommand != null && sqlcommand.Connection != null)
                {
                    if (sqlcommand.Connection.State == ConnectionState.Open)
                        sqlcommand.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Obtiene las descripciones únicas de una evaluación
        /// </summary>
        public static DataSet TraeDescripciones()
        {
            SqlCommand sqlcommand = null;
            SqlDataAdapter sqldataadapter = null;
            DataSet dataset = new DataSet();
            try
            {
                sqlcommand = new SqlCommand("sp_traeDescripciones", new SqlConnection(ConStr));
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandTimeout = 0;

                sqldataadapter = new SqlDataAdapter(sqlcommand);
                sqldataadapter.Fill(dataset);

                return dataset;
            }
            catch (Exception ex)
            {
                DataTable datatable = new DataTable();
                DataRow ren;
                datatable.Columns.Add(new DataColumn("Id", typeof(int)));
                datatable.Columns.Add(new DataColumn("Error", typeof(string)));

                ren = datatable.NewRow();
                ren["Id"] = 0;
                ren["Error"] = ex.Message;
                datatable.Rows.Add(ren);

                dataset.Tables.Add(datatable);
                return dataset;
            }
            finally
            {
                if (sqlcommand != null && sqlcommand.Connection != null)
                {
                    if (sqlcommand.Connection.State == ConnectionState.Open)
                        sqlcommand.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Obtiene las preguntas de un tipo de evaluación específico
        /// </summary>
        /// <param name="IdTipoEvaluacion">ID del tipo de evaluación</param>
        /// <returns>DataSet con las preguntas</returns>
        public static DataSet TraePreguntasII(string IdTipoEvaluacion)
        {
            SqlCommand sqlcommand = null;
            SqlDataAdapter sqldataadapter = null;
            DataSet dataset = new DataSet();
            try
            {
                sqlcommand = new SqlCommand("sp_traePreguntasII", new SqlConnection(ConStr));
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandTimeout = 0;
                sqlcommand.Parameters.Add("@IdTipoEvaluacion", SqlDbType.VarChar).Value = IdTipoEvaluacion;

                sqldataadapter = new SqlDataAdapter(sqlcommand);
                sqldataadapter.Fill(dataset);

                return dataset;
            }
            catch (Exception ex)
            {
                DataTable datatable = new DataTable();
                DataRow ren;
                datatable.Columns.Add(new DataColumn("Id", typeof(int)));
                datatable.Columns.Add(new DataColumn("Error", typeof(string)));

                ren = datatable.NewRow();
                ren["Id"] = 0;
                ren["Error"] = ex.Message;
                datatable.Rows.Add(ren);

                dataset.Tables.Add(datatable);
                return dataset;
            }
            finally
            {
                if (sqlcommand != null && sqlcommand.Connection != null)
                {
                    if (sqlcommand.Connection.State == ConnectionState.Open)
                        sqlcommand.Connection.Close();
                }
            }
        }

        /// <summary>
        /// Actualiza el estado de una encuesta en la base de datos
        /// </summary>
        /// <param name="IdTipoEvaluacion">ID del tipo de evaluación</param>
        /// <param name="bActivo">Estado activo (1 = Activo, 0 = Baja)</param>
        /// <returns>True si la actualización fue exitosa, False en caso contrario</returns>
        public static bool ActualizaEstadoEncuesta(string IdTipoEvaluacion, int bActivo)
        {
            SqlConnection SqlCon = null;
            try
            {
                SqlCon = new SqlConnection(ConStr);
                SqlCon.Open();
                
                using (SqlCommand cmd = new SqlCommand("sp_ActualizaEstadoEncuesta", SqlCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdTipoEvaluacion", SqlDbType.VarChar).Value = IdTipoEvaluacion;
                    cmd.Parameters.Add("@bActivo", SqlDbType.Int).Value = bActivo;
                    
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlCon != null && SqlCon.State == ConnectionState.Open)
                    SqlCon.Close();
                SqlCon?.Dispose();
                GC.Collect();
            }
        }

        /// <summary>
        /// Actualiza el estado activo de una pregunta en la base de datos
        /// </summary>
        /// <param name="idPregunta">ID de la pregunta</param>
        /// <param name="bActivo">Estado activo (1 = Activo, 0 = Inactivo)</param>
        /// <returns>True si la actualización fue exitosa, False en caso contrario</returns>
        public static bool ActualizaEstadoPregunta(int idPregunta, int bActivo)
        {
            SqlConnection SqlCon = null;
            try
            {
                SqlCon = new SqlConnection(ConStr);
                SqlCon.Open();
                
                using (SqlCommand cmd = new SqlCommand("sp_actuPreguntaStat", SqlCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdPregunta", SqlDbType.Int).Value = idPregunta;
                    cmd.Parameters.Add("@bActivo", SqlDbType.Bit).Value = (bActivo == 1);
                    
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlCon != null && SqlCon.State == ConnectionState.Open)
                    SqlCon.Close();
                SqlCon?.Dispose();
                GC.Collect();
            }
        }

        /// <summary>
        /// Actualiza el número de orden de una pregunta en la base de datos
        /// </summary>
        /// <param name="idPregunta">ID de la pregunta</param>
        /// <param name="nOrden">Nuevo número de orden</param>
        /// <returns>True si la actualización fue exitosa, False en caso contrario</returns>
        public static bool ActualizaNOrden(int idPregunta, int nOrden)
        {
            SqlConnection SqlCon = null;
            try
            {
                SqlCon = new SqlConnection(ConStr);
                SqlCon.Open();
                
                using (SqlCommand cmd = new SqlCommand("sp_actuPreguntaOrden", SqlCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdPregunta", SqlDbType.Int).Value = idPregunta;
                    cmd.Parameters.Add("@nOrden", SqlDbType.Int).Value = nOrden;
                    
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlCon != null && SqlCon.State == ConnectionState.Open)
                    SqlCon.Close();
                SqlCon?.Dispose();
                GC.Collect();
            }
        }

        /// <summary>
        /// Actualiza la clave de acceso de una encuesta en la base de datos
        /// </summary>
        /// <param name="idTipoEvaluacion">ID del tipo de evaluación</param>
        /// <param name="cClaveAcceso">Nueva clave de acceso</param>
        /// <returns>True si la actualización fue exitosa, False en caso contrario</returns>
        public static bool ActualizaClaveAcceso(string idTipoEvaluacion, string cClaveAcceso)
        {
            SqlConnection SqlCon = null;
            try
            {
                SqlCon = new SqlConnection(ConStr);
                SqlCon.Open();
                
                using (SqlCommand cmd = new SqlCommand("sp_ActualizaClaveEncuesta", SqlCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdTipoEvaluacion", SqlDbType.VarChar).Value = idTipoEvaluacion;
                    cmd.Parameters.Add("@cClaveAcceso", SqlDbType.VarChar).Value = cClaveAcceso;
                    
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlCon != null && SqlCon.State == ConnectionState.Open)
                    SqlCon.Close();
                SqlCon?.Dispose();
                GC.Collect();
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
            SqlConnection sqlConnection = null;
            SqlDataAdapter sqlDataAdapter = null;
            DataSet dataSet = new DataSet();

            try
            {
                string connectionString = ConDWH; 
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("No se pudo obtener la cadena de conexión del EvaluaLiderazgo");
                }

                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                // Nota: Usar procedure almacenado o consulta SQL según la estructura final de la BD
                using (SqlCommand command = new SqlCommand("sp_traePersonasEvaluar", sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IdTipoEncuesta", SqlDbType.Int).Value = idTipoEvaluacion;
                    command.CommandTimeout = 0;

                    sqlDataAdapter = new SqlDataAdapter(command);
                    sqlDataAdapter.Fill(dataSet);
                }

                return dataSet;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener personas a evaluar: " + ex.Message);
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
        /// Inserta una relación de tipo de encuesta y personal
        /// Ejecuta el SP sp_Inserta_PorIdPersonal
        /// </summary>
        /// <param name="IdTipoEncuesta">ID del tipo de encuesta</param>
        /// <param name="IdPersonal">ID del personal</param>
        /// <returns>1 si la operación fue exitosa, 0 si no se pudo insertar</returns>
        public static int InsertaPorIdPersonal(int IdTipoEncuesta, int IdPersonal)
        {
            SqlConnection SqlCon = null;
            try
            {
                string connectionString = ConStr;
                SqlCon = new SqlConnection(connectionString);
                SqlCon.Open();
                using (SqlCommand cmd = new SqlCommand("sp_Inserta_PorIdPersonal", SqlCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdTipoEncuesta", SqlDbType.Int).Value = IdTipoEncuesta;
                    cmd.Parameters.Add("@IdPersonal", SqlDbType.Int).Value = IdPersonal;

                    cmd.ExecuteNonQuery();
                    
                    // ExecuteNonQuery() retorna -1 cuando hay SET NOCOUNT ON en el SP
                    // Si llegamos aquí sin excepción, la operación fue exitosa
                    return 1;
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (SqlCon != null && SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    SqlCon.Dispose();
                }

                GC.Collect();
            }
        }

        /// <summary>
        /// Elimina (soft-delete) una persona a evaluar por su ID
        /// Ejecuta el SP sp_Elimina_PorIdPersonal que establece Activo = 0
        /// </summary>
        /// <param name="idPersonal">ID del personal a eliminar</param>
        /// <returns>1 si la operación fue exitosa, 0 si no se encontró el registro</returns>
        public static int EliminaPersona(int idPersonal)
        {
            SqlConnection SqlCon = null;
            DataTable dt = new DataTable();
            try
            {
                string connectionString = ConStr;
                SqlCon = new SqlConnection(connectionString);
                SqlCon.Open();
                using (SqlCommand cmd = new SqlCommand("sp_Elimina_PorIdPersonal", SqlCon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdPersonal", SqlDbType.Int).Value = idPersonal;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    dt = ds.Tables[0];

                    // El SP retorna SELECT @@ROWCOUNT
                    if (dt.Rows.Count > 0)
                    {
                        int filasAfectadas = int.Parse(dt.Rows[0][0].ToString());
                        return filasAfectadas > 0 ? 1 : 0;
                    }
                    else
                        return 0;
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (SqlCon != null && SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                    SqlCon.Dispose();
                }

                GC.Collect();
            }
        }

        /// <summary>
        /// Inserta una evaluación de liderazgo en la base de datos
        /// </summary>
        /// <param name="respuesta">Objeto Respuesta con los datos de la evaluación</param>
        /// <returns>String con el resultado de la operación (ID de la evaluación insertada)</returns>
        public static string InsertaEvaluacion(Respuesta respuesta)
        {
            string resultado = "";
            SqlCommand sqlcommand = null;
            try
            {
                sqlcommand = new SqlCommand("sp_inseEvaluacionDWH", new SqlConnection(ConStr));
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandTimeout = 0;

                sqlcommand.Parameters.Add("@IdTipoEvaluacion", SqlDbType.Int).Value = respuesta.IdTipoEvaluacion;
                sqlcommand.Parameters.Add("@IdCentroDWH", SqlDbType.VarChar, 2).Value = respuesta.IdCentroDWH;
                sqlcommand.Parameters.Add("@IDPersonalDWH_Jefe", SqlDbType.Int).Value = respuesta.IDPersonalDWH_Jefe;
                sqlcommand.Parameters.Add("@IDPersonalDWH_Evaluado", SqlDbType.Int).Value = respuesta.IDPersonalDWH_Evaluado;
                sqlcommand.Parameters.Add("@cNombreEvaluado", SqlDbType.VarChar, 100).Value = respuesta.cNombreEvaluado;
                sqlcommand.Parameters.Add("@cComentarios", SqlDbType.VarChar, 1000).Value = respuesta.cComentarios;
                sqlcommand.Parameters.Add("@nNoEmpAgente", SqlDbType.Decimal).Value = respuesta.nNoEmpAgente;
                sqlcommand.Parameters.Add("@IDAntig", SqlDbType.Decimal).Value = respuesta.IDAntig;

                sqlcommand.Connection.Open();
                var result = sqlcommand.ExecuteScalar();
                resultado = result != null ? result.ToString() : "";
            }
            catch (Exception ex)
            {
                resultado = "Error: " + ex.Message;
            }
            finally
            {
                if (sqlcommand != null && sqlcommand.Connection != null)
                {
                    if (sqlcommand.Connection.State == ConnectionState.Open)
                        sqlcommand.Connection.Close();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Inserta una respuesta de evaluación en la base de datos
        /// </summary>
        /// <param name="idEvaluacion">ID de la evaluación</param>
        /// <param name="idPregunta">ID de la pregunta</param>
        /// <param name="nRespuesta">Valor numérico de la respuesta (1-5)</param>
        /// <param name="cComentarios">Comentarios de la respuesta</param>
        /// <returns>String con el resultado de la operación</returns>
        public static string InsertaRespuesta(int idEvaluacion, int idPregunta, int nRespuesta, string cComentarios)
        {
            string resultado = "";
            SqlCommand sqlcommand = null;
            try
            {
                sqlcommand = new SqlCommand("sp_inseRespuestas", new SqlConnection(ConStr));
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandTimeout = 0;

                sqlcommand.Parameters.Add("@IdEvaluacion", SqlDbType.Int).Value = idEvaluacion;
                sqlcommand.Parameters.Add("@IdPregunta", SqlDbType.Int).Value = idPregunta;
                sqlcommand.Parameters.Add("@nRespuesta", SqlDbType.Int).Value = nRespuesta;
                sqlcommand.Parameters.Add("@cComentarios", SqlDbType.VarChar, 500).Value = cComentarios ?? "";

                sqlcommand.Connection.Open();
                var result = sqlcommand.ExecuteScalar();
                resultado = result != null ? result.ToString() : "";
            }
            catch (Exception ex)
            {
                resultado = "Error: " + ex.Message;
            }
            finally
            {
                if (sqlcommand != null && sqlcommand.Connection != null)
                {
                    if (sqlcommand.Connection.State == ConnectionState.Open)
                        sqlcommand.Connection.Close();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Inserta un registro en tblContesto indicando que el usuario contestó la encuesta
        /// </summary>
        /// <param name="idPersonalDWH">ID del personal en DWH</param>
        /// <returns>String con el ID del registro insertado</returns>
        public static string InseRegistro(int idPersonalDWH)
        {
            string resultado = "";
            SqlCommand sqlcommand = null;
            try
            {
                sqlcommand = new SqlCommand("sp_inseRegistro", new SqlConnection(ConStr));
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandTimeout = 0;

                sqlcommand.Parameters.Add("@IDPersonalDWH", SqlDbType.Int).Value = idPersonalDWH;

                sqlcommand.Connection.Open();
                var result = sqlcommand.ExecuteScalar();
                resultado = result != null ? result.ToString() : "";
            }
            catch (Exception ex)
            {
                resultado = "Error: " + ex.Message;
            }
            finally
            {
                if (sqlcommand != null && sqlcommand.Connection != null)
                {
                    if (sqlcommand.Connection.State == ConnectionState.Open)
                        sqlcommand.Connection.Close();
                }
            }

            return resultado;
        }


    }
}
