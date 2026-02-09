using Microsoft.AspNetCore.Mvc.Rendering;
using EncuestasEvaluacionLiderazgo.Views.Encuesta;

namespace EncuestasEvaluacionLiderazgo.Models
{
    /// <summary>
    /// ViewModel para la vista Index de Encuestas
    /// Contiene la información necesaria para mostrar el listado con filtros
    /// </summary>
    public class EncuestaIndexViewModel
    {
        /// <summary>
        /// Lista de encuestas a mostrar
        /// </summary>
        public IEnumerable<Encuesta> Encuestas { get; set; } = new List<Encuesta>();

        /// <summary>
        /// Filtro seleccionado del tipo de evaluación
        /// </summary>
        public string FiltroTipoEvaluacion { get; set; } = "";

        /// <summary>
        /// Lista de tipos de evaluación para el combobox
        /// </summary>
        public List<SelectListItem> TiposEvaluacion { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Modelo de la vista con métodos helper
        /// </summary>
        public IndexModel IndexModel { get; set; }

        /// <summary>
        /// Elementos del card header de la primera encuesta
        /// </summary>
        public IndexModel.ElementosCardHeader ElementosCardHeader { get; set; }
    }
}
