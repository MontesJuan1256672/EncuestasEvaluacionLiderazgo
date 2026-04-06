using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EncuestasEvaluacionLiderazgo.Models;
using EncuestasEvaluacionLiderazgo.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using ClosedXML.Excel;

namespace EncuestasEvaluacionLiderazgo.Controllers
{
    /// <summary>
    /// Controlador responsable de manejar los reportes de evaluación
    /// </summary>
    [Route("[controller]")]
    public class ReportesController : Controller
    {
        /// <summary>
        /// Verifica si el usuario está autenticado
        /// </summary>
        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetInt32("UserId").HasValue;
        }

        /// <summary>
        /// Obtiene el tipo de usuario actual desde la sesión
        /// </summary>
        private int GetCurrentUserType()
        {
            return HttpContext.Session.GetInt32("UserType") ?? 0;
        }

        /// <summary>
        /// Verifica si el usuario actual es administrador
        /// </summary>
        private bool IsAdmin()
        {
            return GetCurrentUserType() == (int)EncuestasEvaluacionLiderazgo.Models.TipoUsuario.Administrador;
        }

        /// <summary>
        /// GET: /Reportes
        /// Muestra la página principal del reporteador de resultados
        /// Solo administradores pueden acceder
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            var viewModel = new ReportesIndexViewModel();
            CargarFiltros(viewModel);

            return View(viewModel);
        }

        /// <summary>
        /// POST: /Reportes/Buscar
        /// Ejecuta la búsqueda de reportes según los filtros seleccionados
        /// </summary>
        [HttpPost]
        [Route("Buscar")]
        public IActionResult Buscar(ReportesIndexViewModel filtros)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            CargarFiltros(filtros);

            // Recuperar los valores seleccionados en los filtros
            string idTipoEncuesta = filtros.EncuestaId ?? "";
            string idUbicacion = filtros.Ciudad ?? "";
            string idPersonalPersonaAEvaluar = filtros.PersonaEvaluar ?? "";
            string nombrePersonaEvaluarText = filtros.PersonaEvaluarText ?? "";
            
            // Evaluar el tipo de reporte para determinar rango de fechas
            string fechaInicio = "";
            string fechaFinal = "";
            
            if (filtros.TipoReporte == "2") // Reporte Histórico
            {
                fechaInicio = "20000101"; // Inicio desde año 2000
                fechaFinal = DateTime.Now.ToString("yyyyMMdd"); // Fecha actual
            }
            else // Reporte Normal o sin especificar
            {
                fechaInicio = filtros.FechaInicio?.ToString("yyyyMMdd") ?? "";
                fechaFinal = filtros.FechaFinal?.ToString("yyyyMMdd") ?? "";
            }

            // Validar que la fecha final no sea anterior a la de inicio
            if (filtros.FechaInicio.HasValue && filtros.FechaFinal.HasValue && filtros.FechaFinal < filtros.FechaInicio)
            {
                TempData["Error"] = "La fecha final no puede ser anterior a la fecha de inicio.";
                return View("Index", filtros);
            }

            int idtipoEvaluacionInt = int.TryParse(idTipoEncuesta, out int tipoEvaluacion) ? tipoEvaluacion : 0;
            int idUbicacionInt = int.TryParse(idUbicacion, out int ubicacion) ? ubicacion : 0;
            int idPersonalAEvaluarInt = int.TryParse(idPersonalPersonaAEvaluar, out int personalAEvaluar) ? personalAEvaluar : 0;

            // Obtener tabla de promedios
            DataTable dtReporteDePromediosPorEvaluado = new DataTable();
            DataTable dtAgentesQueContestaronEncuesta = new DataTable();

            // Evaluar si se deben procesar todas las personas o solo una
            bool todoPersonas = !string.IsNullOrEmpty(filtros.TodoPersonas) && bool.TryParse(filtros.TodoPersonas, out bool result) && result;

            if (todoPersonas && idtipoEvaluacionInt > 0)
            {
                // Obtener todas las personas para el tipo de evaluación seleccionado
                try
                {
                    var dataSetPersonas = FL.TraePersonasEvaluar(idtipoEvaluacionInt);

                    if (dataSetPersonas != null && dataSetPersonas.Tables.Count > 0 && dataSetPersonas.Tables[0].Rows.Count > 0)
                    {
                        bool esFirstRow = true;

                        foreach (System.Data.DataRow rowPersona in dataSetPersonas.Tables[0].Rows)
                        {
                            var idPersona = rowPersona["IdPersonal"]?.ToString() ?? "";
                            var nombrePersona = rowPersona["Nombre"]?.ToString() ?? "";
                            var noEmpPersona = rowPersona["NoEmp"]?.ToString() ?? "";

                            if (!string.IsNullOrEmpty(idPersona) && !string.IsNullOrEmpty(nombrePersona))
                            {
                                int idPersonaInt = int.TryParse(idPersona, out int pId) ? pId : 0;
                                
                                // Llamar a PromediosPorEvaluado para cada persona
                                DataTable dtPersona = PromediosPorEvaluado(idPersonaInt, idtipoEvaluacionInt, nombrePersona, fechaInicio, fechaFinal, noEmpPersona);
                                DataTable dtAgentes = FL.AgentesQueContestaronEncuesta(idPersonaInt, fechaInicio, fechaFinal);

                                // En la primera iteración, se crea la estructura
                                if (esFirstRow)
                                {
                                    dtReporteDePromediosPorEvaluado = dtPersona.Copy();
                                    dtAgentesQueContestaronEncuesta = dtAgentes.Copy();
                                    esFirstRow = false;
                                }
                                else
                                {
                                    // Importar las filas de cada persona si tienen evaluaciones
                                    if (dtPersona.Rows.Count > 0)
                                    {
                                        // Validar que Total de evaluaciones sea mayor a 0
                                        var totalEvaluacionesValue = dtPersona.Rows[0]["Total de evaluaciones"];
                                        int totalEvaluaciones = 0;
                                        
                                        if (totalEvaluacionesValue != null && totalEvaluacionesValue != DBNull.Value)
                                        {
                                            if (decimal.TryParse(totalEvaluacionesValue.ToString(), out decimal temp))
                                            {
                                                totalEvaluaciones = (int)temp;
                                            }
                                        }
                                        
                                        // Solo importar si hay evaluaciones
                                        if (totalEvaluaciones > 0)
                                        {
                                            dtReporteDePromediosPorEvaluado.ImportRow(dtPersona.Rows[0]);
                                        }
                                    }
                                    if (dtAgentes != null && dtAgentes.Rows.Count > 0)
                                    {
                                        foreach (DataRow row in dtAgentes.Rows)
                                        {
                                            dtAgentesQueContestaronEncuesta.ImportRow(row);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error al procesar todas las personas: {ex.Message}";
                }
            }
            else if (idPersonalAEvaluarInt > 0)
            {
                // Procesar solo una persona
                dtReporteDePromediosPorEvaluado = PromediosPorEvaluado(idPersonalAEvaluarInt, idtipoEvaluacionInt, nombrePersonaEvaluarText, fechaInicio, fechaFinal, filtros.NoEmp ?? "");
                dtAgentesQueContestaronEncuesta = FL.AgentesQueContestaronEncuesta(idPersonalAEvaluarInt, fechaInicio, fechaFinal);

 
            }

            // Asignar el DataTable al ViewModel para mostrar en la vista
            filtros.ReporteDePromedios = dtReporteDePromediosPorEvaluado;
            filtros.AgentesQueContestaronEncuesta = dtAgentesQueContestaronEncuesta;

            return View("Index", filtros);
        }

        /// <summary>
        /// GET: /Reportes/GetPersonasEvaluar
        /// Obtiene las personas a evaluar dinámicamente para un tipo de evaluación específico
        /// Llamado vía AJAX desde la vista
        /// </summary>
        [HttpGet]
        [Route("GetPersonasEvaluar")]
        public IActionResult GetPersonasEvaluar(string idTipoEvaluacion)
        {
            try
            {
                if (string.IsNullOrEmpty(idTipoEvaluacion))
                {
                    return Json(new { success = false, message = "ID de tipo de evaluación no proporcionado" });
                }

                
                var dataSet = FL.TraePersonasEvaluar(Convert.ToInt32(idTipoEvaluacion));
                
                List<object> personas = new List<object>();

                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    // Llenar la lista con las personas
                    foreach (System.Data.DataRow row in dataSet.Tables[0].Rows)
                    {
                        // Columnas retornadas por sp_traePersonasEvaluar: IdPersonal, NoEmp, Ciudad, Nombre, cDescripcion
                        var idPersona = row["IdPersonal"]?.ToString() ?? "";
                        var nombrePersona = row["Nombre"]?.ToString() ?? "";
                        var noEmp = row["NoEmp"]?.ToString() ?? "";
                        
                        if (!string.IsNullOrEmpty(idPersona) && !string.IsNullOrEmpty(nombrePersona))
                        {
                            personas.Add(new { value = idPersona, text = nombrePersona, noEmp = noEmp });
                        }
                    }
                }

                if (personas.Count == 0)
                {
                    personas.Add(new { value = "", text = "Sin personas disponibles" });
                }

                return Json(new { success = true, personas = personas });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        /// <summary>
        /// GET: /Reportes/PorEncuesta
        /// Muestra reportes agrupados por encuesta
        /// </summary>
        [HttpGet]
        [Route("PorEncuesta")]
        public IActionResult PorEncuesta()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            ViewBag.TipoReporte = "Por Encuesta";
            return View("Index");
        }

        /// <summary>
        /// GET: /Reportes/Comparativo
        /// Muestra análisis comparativo de múltiples encuestas
        /// </summary>
        [HttpGet]
        [Route("Comparativo")]
        public IActionResult Comparativo()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            ViewBag.TipoReporte = "Análisis Comparativo";
            return View("Index");
        }

        /// <summary>
        /// GET: /Reportes/Competencias
        /// Muestra análisis por competencias
        /// </summary>
        [HttpGet]
        [Route("Competencias")]
        public IActionResult Competencias()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            ViewBag.TipoReporte = "Análisis de Competencias";
            return View("Index");
        }

        /// <summary>
        /// GET: /Reportes/Participantes
        /// Muestra análisis de participantes y tasas de respuesta
        /// </summary>
        [HttpGet]
        [Route("Participantes")]
        public IActionResult Participantes()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            ViewBag.TipoReporte = "Análisis de Participantes";
            return View("Index");
        }

        /// <summary>
        /// GET: /Reportes/Tendencias
        /// Muestra tendencias y evolución de competencias
        /// </summary>
        [HttpGet]
        [Route("Tendencias")]
        public IActionResult Tendencias()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            ViewBag.TipoReporte = "Tendencias y Evolución";
            return View("Index");
        }

        /// <summary>
        /// GET: /Reportes/Exportar
        /// Muestra opciones de exportación de reportes
        /// </summary>
        [HttpGet]
        [Route("Exportar")]
        public IActionResult Exportar()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            if (!IsAdmin())
                return Forbid();

            ViewBag.TipoReporte = "Exportar Datos";
            return View("Index");
        }

        /// <summary>
        /// Carga los tipos de evaluación y ciudades en el ViewModel
        /// </summary>
        private void CargarFiltros(ReportesIndexViewModel viewModel)
        {
            try
            {
                var dataSet = FL.TraeTiposEvaluacion();

                viewModel.TiposEvaluacion.Add(new SelectListItem { Value = "", Text = "-- Seleccionar encuesta --" });

                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Data.DataRow row in dataSet.Tables[0].Rows)
                    {
                        viewModel.TiposEvaluacion.Add(new SelectListItem
                        {
                            Value = row["IDTipoEvaluacion"]?.ToString() ?? "",
                            Text = row["cDescripcion"]?.ToString() ?? "",
                            Selected = row["IDTipoEvaluacion"]?.ToString() == viewModel.EncuestaId
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                viewModel.TiposEvaluacion.Add(new SelectListItem { Value = "", Text = "-- Todas las encuestas --" });
                TempData["Error"] = $"Error al cargar tipos de evaluación: {ex.Message}";
            }

            CargarCiudades();
        }

        /// <summary>
        /// Carga la lista de ciudades disponibles
        /// </summary>
        private void CargarCiudades()
        {
            try
            {
                var ciudades = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- Seleccione ciudad --" },
                    new SelectListItem { Value = "17", Text = "Tijuana" },
                    new SelectListItem { Value = "18", Text = "Mexicali" },
                    new SelectListItem { Value = "19", Text = "CDMX" }
                };

                ViewBag.Ciudades = ciudades;
            }
            catch (Exception ex)
            {
                ViewBag.Ciudades = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Error al cargar ciudades" }
                };
            }
        }
        
        private string TreaJefeDePersonaAEvaluar(string idPersonaEvaluar)
        {
            DataTable empleado = FL.BuscarEmpleadoPorId(idPersonaEvaluar);

            if (empleado != null && empleado.Rows.Count > 0)
            {
                string idJefe = empleado.Rows[0]["IDPersonal_Jefe"]?.ToString();

                if (!string.IsNullOrEmpty(idJefe))
                {
                    return idJefe;
                }
            }

            return "";
        }

        /// <summary>
        /// Calcula la calificación total promediando todos los valores del dataset,
        /// excluyendo la fila "Total de evaluaciones"
        /// </summary>
        /// <param name="dataSet">DataSet con columnas "Competencia" y "PromedioRespuesta"</param>
        /// <returns>Promedio de todos los valores excluyendo "Total de evaluaciones"</returns>
        private decimal CalculaCalificacionTotal(DataSet dataSet)
        {
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                return 0;
            }

            DataTable sourceTable = dataSet.Tables[0];
            List<decimal> valores = new List<decimal>();

            foreach (DataRow row in sourceTable.Rows)
            {
                string competencia = row["Competencia"]?.ToString() ?? "";
                
                // Excluir la fila "Total de evaluaciones"
                if (competencia.Equals("Total de evaluaciones", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                decimal promedio = 0;
                if (decimal.TryParse(row["PromedioRespuesta"]?.ToString() ?? "0", out decimal valor))
                {
                    promedio = valor;
                }

                valores.Add(promedio);
            }

            // Calcular el promedio de los valores
            if (valores.Count == 0)
            {
                return 0;
            }

            return valores.Sum() / valores.Count;
        }

        /// <summary>
        /// Genera un DataTable con los promedios por evaluado con columnas ordenadas específicamente
        /// </summary>
        /// <param name="idPersonalAEvaluar">ID de la persona a evaluar</param>
        /// <param name="idtipoEvaluacion">ID del tipo de evaluación</param>
        /// <param name="nombrePersonaEvaluarText">Nombre de la persona a evaluar</param>
        /// <returns>DataTable con estructura ordenada de promedios</returns>
        private DataTable PromediosPorEvaluado(int idPersonalAEvaluar, int idtipoEvaluacion, string nombrePersonaEvaluarText, string fechaInicio, string fechaFin, string noEmp = "")
        {
            DataTable dtReporteDePromedios = new DataTable();
            List<string> columnasOrdenadas = new List<string>
            {
                "Nombre",
                "Total de evaluaciones",
                "Credibilidad Técnica",
                "Orientacion al cliente interno/externo",
                "Desarrollo de personas",
                "Capacidad de supervisión",
                "Comunicación efectiva",
                "Liderazgo",
                "Orientación a resultados",
                "Resolución de problemas",
                "Percepción General",
                "Calificación Total",
                "Numero de empleado",
                "Menos de 1 mes",
                "1 a 3 meses",
                "3 a 6 meses",
                "Mas de 6 meses",
                "Fecha de última encuesta"
            };

            /// Por cada id competencia, calcular el promedio por evaluado
            var promedioPorEvaluado = FL.PromedioPorEvaluado(idPersonalAEvaluar, idtipoEvaluacion, fechaInicio, fechaFin);
            
            // Calcular calificación total excluyendo "Total de evaluaciones"
            decimal calificacionTotal = Math.Round(CalculaCalificacionTotal(promedioPorEvaluado), 2);
            
            DataTable promedios = PivotarPromediosPorCompetencia(promedioPorEvaluado);

            // Unir promedios a dtReporteDePromedios con orden específico
            // Agregar columnas en orden deseado
            foreach (string nombreColumna in columnasOrdenadas)
            {
                if (!dtReporteDePromedios.Columns.Contains(nombreColumna))
                {
                    if (nombreColumna == "Nombre")
                    {
                        dtReporteDePromedios.Columns.Add(nombreColumna, typeof(string));
                    }
                    else if (nombreColumna == "Numero de empleado" || nombreColumna == "Fecha de última encuesta")
                    {
                        dtReporteDePromedios.Columns.Add(nombreColumna, typeof(string));
                    }
                    else if (nombreColumna == "Calificación Total" || nombreColumna.Contains("mes"))
                    {
                        dtReporteDePromedios.Columns.Add(nombreColumna, typeof(decimal));
                    }
                    else if (promedios.Columns.Contains(nombreColumna))
                    {
                        dtReporteDePromedios.Columns.Add(nombreColumna, promedios.Columns[nombreColumna].DataType);
                    }
                    else
                    {
                        dtReporteDePromedios.Columns.Add(nombreColumna, typeof(decimal));
                    }
                }
            }

            // Agregar la fila de promedios
            if (promedios.Rows.Count > 0)
            {
                DataRow nuevaFila = dtReporteDePromedios.NewRow();
                // Asignar nombre de persona a evaluar
                nuevaFila["Nombre"] = nombrePersonaEvaluarText;
                // Asignar calificación total
                nuevaFila["Calificación Total"] = calificacionTotal;
                // Copiar valores de la primera fila de promedios en orden
                foreach (string nombreColumna in columnasOrdenadas)
                {
                    if (nombreColumna != "Nombre" && nombreColumna != "Calificación Total" && dtReporteDePromedios.Columns.Contains(nombreColumna))
                    {
                        if (promedios.Columns.Contains(nombreColumna))
                        {
                            nuevaFila[nombreColumna] = promedios.Rows[0][nombreColumna];
                        }
                    }
                }

                nuevaFila["Numero de empleado"] = !string.IsNullOrEmpty(noEmp) ? noEmp : (HttpContext.Session.GetString("NoEmp") ?? "");
                nuevaFila["1 a 3 meses"] = 0;
                nuevaFila["3 a 6 meses"] = 0;
                nuevaFila["Mas de 6 meses"] = 0;
                nuevaFila["Menos de 1 mes"] = 0;

                DataSet dsAntiguedad = GetAntiguedadConJefe(idPersonalAEvaluar);
                if (dsAntiguedad != null && dsAntiguedad.Tables.Count > 0 && dsAntiguedad.Tables[0].Rows.Count > 0)
                {
                    DataTable tblAntiguedad = dsAntiguedad.Tables[0];
                    
                    // Iterate sobre las filas para encontrar coincidencias
                    foreach (DataRow row in tblAntiguedad.Rows)
                    {
                        string rango = row[0]?.ToString() ?? "";
                        int valor = 0;
                        
                        if (int.TryParse(row[1]?.ToString() ?? "0", out int v))
                        {
                            valor = v;
                        }
                        
                        if (rango.Contains("1 a 3 meses") || rango.Contains("1 a 3"))
                            nuevaFila["1 a 3 meses"] = valor;
                        else if (rango.Contains("3 a 6 meses") || rango.Contains("3 a 6"))
                            nuevaFila["3 a 6 meses"] = valor;
                        else if (rango.Contains("Mas de 6 meses") || rango.Contains("Mas de 6"))
                            nuevaFila["Mas de 6 meses"] = valor;
                        else if (rango.Contains("Menos de 1 mes") || rango.Contains("Menos de 1"))
                            nuevaFila["Menos de 1 mes"] = valor;
                    }
                }

                nuevaFila["Fecha de última encuesta"] = FL.GetFechaUltimaEvaluacion(idPersonalAEvaluar) ?? "N/A";
                dtReporteDePromedios.Rows.Add(nuevaFila);
            }

            return dtReporteDePromedios;
        }

        private DataSet GetAntiguedadConJefe(int idPersonalAEvaluar)
        {
            DataSet dsAntiguedad = new DataSet();
            
            dsAntiguedad = FL.GetAntiguedadConJefePorEvaluacion(idPersonalAEvaluar);
            return dsAntiguedad;
        }

        /// <summary>
        /// Transforma un DataSet de formato largo (long format) a formato ancho (wide format)
        /// Convierte cada competencia única en una columna y sus promedios en una fila
        /// </summary>
        /// <param name="dataSet">DataSet con columnas "Competencia" y "PromedioRespuesta"</param>
        /// <returns>DataTable pivotado con competencias como columnas</returns>
        private DataTable PivotarPromediosPorCompetencia(DataSet dataSet)
        {
            DataTable resultadoTable = new DataTable();

            if (dataSet == null || dataSet.Tables.Count == 0)
            {
                return resultadoTable;
            }

            DataTable sourceTable = dataSet.Tables[0];

            if (sourceTable.Rows.Count == 0)
            {
                return resultadoTable;
            }

            Dictionary<string, decimal> competenciasPromedios = new Dictionary<string, decimal>();

            foreach (DataRow row in sourceTable.Rows)
            {
                string competencia = row["Competencia"]?.ToString() ?? "";
                decimal promedio = 0;

                if (decimal.TryParse(row["PromedioRespuesta"]?.ToString() ?? "0", out decimal valor))
                {
                    promedio = valor;
                }

                // Si la competencia ya existe, tomar el promedio más reciente o calcular si es necesario
                if (!competenciasPromedios.ContainsKey(competencia))
                {
                    competenciasPromedios.Add(competencia, promedio);
                }
            }

            // Crear columnas en el DataTable basadas en las competencias únicas
            foreach (var competencia in competenciasPromedios.Keys)
            {
                if (!string.IsNullOrEmpty(competencia))
                {
                    resultadoTable.Columns.Add(competencia, typeof(decimal));
                }
            }

            // Si no hay competencias válidas, retornar tabla vacía
            if (resultadoTable.Columns.Count == 0)
            {
                return resultadoTable;
            }

            // Crear una sola fila con los promedios
            DataRow nuevaFila = resultadoTable.NewRow();

            foreach (var kvp in competenciasPromedios)
            {
                if (resultadoTable.Columns.Contains(kvp.Key))
                {
                    nuevaFila[kvp.Key] = kvp.Value;
                }
            }

            resultadoTable.Rows.Add(nuevaFila);

            return resultadoTable;
        }

        /// <summary>
        /// Método para descargar el reporte en formato Excel
        /// Genera un archivo con dos sheets: uno para el reporte de promedios y otro para agentes
        /// </summary>
        [HttpPost]
        [Route("DescargarReporte")]
        [IgnoreAntiforgeryToken]
        public IActionResult DescargarReporte([FromBody] ExcelReporteRequest request)
        {
            try
            {
                if (!IsAuthenticated())
                    return Unauthorized();

                // Crear un nuevo workbook
                using (var workbook = new XLWorkbook())
                {
                    // SHEET 1: Reporte de Promedios
                    if (request.ReporteDePromedios != null && request.ReporteDePromedios.Rows.Count > 0)
                    {
                        var dtPromedios = request.ReporteDePromedios.ToDataTable();
                        var worksheet1 = workbook.Worksheets.Add("Promedios por evaluado");
                        
                        // Agregar headers
                        int colIndex = 1;
                        foreach (var columnName in request.ReporteDePromedios.Columns)
                        {
                            worksheet1.Cell(1, colIndex).Value = columnName;
                            worksheet1.Cell(1, colIndex).Style.Font.Bold = true;
                            worksheet1.Cell(1, colIndex).Style.Fill.BackgroundColor = XLColor.Blue;
                            worksheet1.Cell(1, colIndex).Style.Font.FontColor = XLColor.White;
                            colIndex++;
                        }

                        // Agregar datos
                        int rowIndex = 2;
                        foreach (var rowData in request.ReporteDePromedios.Rows)
                        {
                            colIndex = 1;
                            foreach (var columnName in request.ReporteDePromedios.Columns)
                            {
                                var value = rowData.ContainsKey(columnName) ? rowData[columnName] : "";
                                
                                if (!string.IsNullOrEmpty(value) && value != "N/A")
                                {
                                    // Intentar convertir a decimal si es posible
                                    if (decimal.TryParse(value, out decimal decimalValue))
                                    {
                                        worksheet1.Cell(rowIndex, colIndex).Value = decimalValue;
                                        worksheet1.Cell(rowIndex, colIndex).Style.NumberFormat.Format = "0.00";
                                    }
                                    else
                                    {
                                        worksheet1.Cell(rowIndex, colIndex).Value = value;
                                    }
                                }
                                else
                                {
                                    worksheet1.Cell(rowIndex, colIndex).Value = value;
                                }
                                colIndex++;
                            }
                            rowIndex++;
                        }

                        // Ajustar ancho de columnas automáticamente
                        worksheet1.Columns().AdjustToContents();
                    }

                    // SHEET 2: Agentes que Contestaron Encuesta
                    if (request.AgentesQueContestaronEncuesta != null && request.AgentesQueContestaronEncuesta.Rows.Count > 0)
                    {
                        var dtAgentes = request.AgentesQueContestaronEncuesta.ToDataTable();
                        var worksheet2 = workbook.Worksheets.Add("Agentes respondentes");
                        
                        // Agregar headers
                        int colIndex = 1;
                        foreach (var columnName in request.AgentesQueContestaronEncuesta.Columns)
                        {
                            worksheet2.Cell(1, colIndex).Value = columnName;
                            worksheet2.Cell(1, colIndex).Style.Font.Bold = true;
                            worksheet2.Cell(1, colIndex).Style.Fill.BackgroundColor = XLColor.Green;
                            worksheet2.Cell(1, colIndex).Style.Font.FontColor = XLColor.White;
                            colIndex++;
                        }

                        // Agregar datos
                        int rowIndex = 2;
                        foreach (var rowData in request.AgentesQueContestaronEncuesta.Rows)
                        {
                            colIndex = 1;
                            foreach (var columnName in request.AgentesQueContestaronEncuesta.Columns)
                            {
                                var value = rowData.ContainsKey(columnName) ? rowData[columnName] : "";
                                
                                if (!string.IsNullOrEmpty(value) && value != "N/A")
                                {
                                    // Intentar convertir a decimal si es posible
                                    if (decimal.TryParse(value, out decimal decimalValue))
                                    {
                                        worksheet2.Cell(rowIndex, colIndex).Value = decimalValue;
                                        worksheet2.Cell(rowIndex, colIndex).Style.NumberFormat.Format = "0.00";
                                    }
                                    else
                                    {
                                        worksheet2.Cell(rowIndex, colIndex).Value = value;
                                    }
                                }
                                else
                                {
                                    worksheet2.Cell(rowIndex, colIndex).Value = value;
                                }
                                colIndex++;
                            }
                            rowIndex++;
                        }

                        // Ajustar ancho de columnas automáticamente
                        worksheet2.Columns().AdjustToContents();
                    }

                    // Guardar el archivo en un MemoryStream y retornarlo
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte_Evaluaciones.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al generar el archivo Excel", error = ex.Message });
            }
        }

    }
}