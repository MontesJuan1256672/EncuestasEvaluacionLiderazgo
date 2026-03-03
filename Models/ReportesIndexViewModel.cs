using Microsoft.AspNetCore.Mvc.Rendering;

namespace EncuestasEvaluacionLiderazgo.Models
{
    /// <summary>
    /// ViewModel para la vista Index del controlador Reportes
    /// Contiene los datos necesarios para mostrar los filtros de búsqueda con valores dinámicos
    /// </summary>
    public class ReportesIndexViewModel
    {
        /// <summary>
        /// Lista de tipos de evaluación para el combobox encuestaId
        /// </summary>
        public List<SelectListItem> TiposEvaluacion { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Filtro seleccionado del tipo de evaluación
        /// </summary>
        public string EncuestaId { get; set; } = "";

        /// <summary>
        /// Filtro seleccionado del tipo de reporte
        /// </summary>
        public string TipoReporte { get; set; } = "";

        /// <summary>
        /// Fecha de inicio del período de búsqueda
        /// </summary>
        public DateTime? FechaInicio { get; set; }

        /// <summary>
        /// Fecha final del período de búsqueda
        /// </summary>
        public DateTime? FechaFinal { get; set; }

        /// <summary>
        /// ID de la persona a evaluar seleccionada
        /// </summary>
        public string PersonaEvaluar { get; set; } = "";
    }
}

