using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ConnStringsTelvista;
using Librerias;

namespace EncuestasEvaluacionLiderazgo.Data
{
    /// <summary>
    /// DL (Data Access Layer) - Capa de Acceso a Datos
    /// Contiene todos los métodos para acceder a la base de datos
    /// Gestiona conexiones, comandos SQL y operaciones de datos
    /// </summary>
    public class DL
    {
        private static string ConStr = CS_ConStr();

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
        /// Busca datos ejecutando una consulta SQL
        /// </summary>
        /// <returns>DataTable con los resultados</returns>
        public static DataTable BuscaDatos()
        {
            SqlConnection SqlCon = null;

            try
            {
                SqlCon = GetConnection();
                using (SqlCommand cmd = new SqlCommand("Busca_Datos2", SqlCon))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    return ds.Tables[0];
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
                sqlcommand.Connection = new SqlConnection(DL.GetConDWH());
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
                sqlcommand.Connection = new SqlConnection(DL.CS_ConStr());

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
                sqlcommand.Connection = new SqlConnection(DL.GetConEvaluaLiderazgo());
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

        /// <summary>
        /// Inserta una nueva pregunta en la encuesta de evaluación de liderazgo
        /// </summary>
        public static bool InsertaPreguntaII(string idTipoEvaluacion, string cPregunta, string cPregunta_Ingles,
                                             string cCompetencia, string cActividad, string cDescripcion, int nOrden)
        {
            SqlCommand sqlcommand = null;
            try
            {
                sqlcommand = new SqlCommand("sp_insertaPreguntaII", new SqlConnection(DL.GetConEvaluaLiderazgo()));
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandTimeout = 0;

                // Agregar parámetros
                sqlcommand.Parameters.Add("@IdTipoEvaluacion", SqlDbType.VarChar).Value = idTipoEvaluacion;
                sqlcommand.Parameters.Add("@cPregunta", SqlDbType.VarChar).Value = cPregunta;
                sqlcommand.Parameters.Add("@cPregunta_Ingles", SqlDbType.VarChar).Value = cPregunta_Ingles;
                sqlcommand.Parameters.Add("@cCompetencia", SqlDbType.VarChar).Value = cCompetencia;
                sqlcommand.Parameters.Add("@cActividad", SqlDbType.VarChar).Value = cActividad;
                sqlcommand.Parameters.Add("@cDescripcion", SqlDbType.VarChar).Value = cDescripcion;
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
                sqlcommand = new SqlCommand("sp_traeCompetencias", new SqlConnection(DL.GetConEvaluaLiderazgo()));
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
                sqlcommand = new SqlCommand("sp_traeActividades", new SqlConnection(DL.GetConEvaluaLiderazgo()));
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
                sqlcommand = new SqlCommand("sp_traeDescripciones", new SqlConnection(DL.GetConEvaluaLiderazgo()));
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
                sqlcommand = new SqlCommand("sp_traePreguntasII", new SqlConnection(DL.GetConEvaluaLiderazgo()));
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
                SqlCon = new SqlConnection(GetConEvaluaLiderazgo());
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

    }
}
