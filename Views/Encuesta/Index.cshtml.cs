using EncuestasEvaluacionLiderazgo.Models;
using Microsoft.AspNetCore.Http;
using System.Data;
using EncuestasEvaluacionLiderazgo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EncuestasEvaluacionLiderazgo.Views.Encuesta
{
    /// <summary>
    /// Code-behind para la vista Index.cshtml
    /// Contiene los métodos helper para la presentación de datos
    /// </summary>
    public class IndexModel
    {
        /// <summary>
        /// Clase interna para encapsular los elementos del card header
        /// </summary>
        public class ElementosCardHeader
        {
            public string Titulo { get; set; }
            public string EstadoClase { get; set; }
            public string EstadoTexto { get; set; }
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        [BindProperty] public string filtroTipoEvaluacion { get; set; }
        public List<SelectListItem> TiposEvaluacion { get; set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            TiposEvaluacion = new List<SelectListItem>();
        }

        /// <summary>
        /// Carga el combobox con los tipos de evaluación disponibles
        /// </summary>
        public void CargaComboboxFiltroTipoEvaluacion()
        {
            try
            {
                // Obtener los tipos de evaluación desde la BD
                var dataSet = FL.TraeTiposEvaluacion();
                
                TiposEvaluacion = new List<SelectListItem>();
                
                // Agregar opción por defecto
                TiposEvaluacion.Add(new SelectListItem
                {
                    Value = "",
                    Text = "Filtrar por tipo de evaluación..."
                });
                
                // Llenar el combobox con los datos del DataSet
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        TiposEvaluacion.Add(new SelectListItem
                        {
                            Value = row["IDTipoEvaluacion"]?.ToString() ?? "",
                            Text = row["cDescripcion"]?.ToString() ?? "",
                            Selected = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // En caso de error, solo dejar la opción por defecto
                TiposEvaluacion = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "",
                        Text = "Error al cargar tipos de evaluación"
                    }
                };
            }
        }

        /// <summary>
        /// Verifica si el usuario actual es administrador
        /// </summary>
        /// <returns>True si es administrador, False en caso contrario</returns>
        public bool IsAdmin()
        {
            var userType = _httpContextAccessor?.HttpContext?.Session?.GetInt32("UserType");
            return userType.HasValue && userType == (int)TipoUsuario.Administrador;
        }

        /// <summary>
        /// Obtiene el título de la encuesta basado en su tipo de evaluación
        /// </summary>
        /// <param name="idTipoEvaluacion">ID del tipo de evaluación</param>
        /// <returns>Descripción del tipo de evaluación o "Sin título" si no se encuentra</returns>
        public string GetTituloEncuesta(int idTipoEvaluacion)
        {
            try
            {
                var datosEncuesta = FL.TraeEncuestaEvaluaLiderazgo(idTipoEvaluacion.ToString());
                
                if (datosEncuesta != null && datosEncuesta.Tables.Count > 0 && datosEncuesta.Tables[0].Rows.Count > 0)
                {
                    DataRow row = datosEncuesta.Tables[0].Rows[0];
                    return row["cDescripcion"]?.ToString() ?? "Sin título";
                }
                
                return "Sin título";
            }
            catch
            {
                return "Sin título";
            }
        }

        /// <summary>
        /// Obtiene todos los elementos necesarios para el card header (título, estado y clases CSS)
        /// </summary>
        /// <param name="idTipoEvaluacion">ID del tipo de evaluación</param>
        /// <param name="estado">Estado de la encuesta</param>
        /// <returns>Objeto ElementosCardHeader con todos los elementos del card header</returns>
        public ElementosCardHeader GetElementosCardHeader(int idTipoEvaluacion, EstadoEncuesta estado)
        {
            return new ElementosCardHeader
            {
                Titulo = GetTituloEncuesta(idTipoEvaluacion),
                EstadoClase = GetEstadoClase(estado),
                EstadoTexto = GetEstadoTexto(estado)
            };
        }

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
    }
}





   


