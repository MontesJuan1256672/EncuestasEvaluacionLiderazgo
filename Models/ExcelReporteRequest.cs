using System.Data;

namespace EncuestasEvaluacionLiderazgo.Models
{
    /// <summary>
    /// Clase que encapsula los datos necesarios para generar un reporte en Excel
    /// </summary>
    public class ExcelReporteRequest
    {
        /// <summary>
        /// Datos del reporte de promedios
        /// </summary>
        public TablaReporteData ReporteDePromedios { get; set; }

        /// <summary>
        /// Datos de agentes que contestaron encuesta
        /// </summary>
        public TablaReporteData AgentesQueContestaronEncuesta { get; set; }
    }

    /// <summary>
    /// Clase para representar los datos de una tabla en formato JSON
    /// </summary>
    public class TablaReporteData
    {
        /// <summary>
        /// Nombres de las columnas
        /// </summary>
        public List<string> Columns { get; set; } = new List<string>();

        /// <summary>
        /// Filas de datos, donde cada fila es un diccionario de columna -> valor
        /// </summary>
        public List<Dictionary<string, string>> Rows { get; set; } = new List<Dictionary<string, string>>();

        /// <summary>
        /// Convierte este objeto a un DataTable
        /// </summary>
        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable();

            // Crear columnas
            foreach (var columnName in Columns)
            {
                dt.Columns.Add(columnName, typeof(string));
            }

            // Crear filas
            foreach (var rowData in Rows)
            {
                DataRow row = dt.NewRow();
                foreach (var columnName in Columns)
                {
                    if (rowData.ContainsKey(columnName))
                    {
                        row[columnName] = rowData[columnName] ?? "";
                    }
                    else
                    {
                        row[columnName] = "";
                    }
                }
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
