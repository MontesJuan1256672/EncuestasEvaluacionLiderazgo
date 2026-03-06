using System;
using System.Collections.Generic;

namespace EncuestasEvaluacionLiderazgo.Models
{
    /// <summary>
    /// Modelo que representa una evaluación de liderazgo
    /// Corresponde a los parámetros del SP sp_inseEvaluacionDWH
    /// </summary>
    public class Respuesta
    {
        /// <summary>Tipo de evaluación (sp: @IdTipoEvaluacion int)</summary>
        public int IdTipoEvaluacion { get; set; }

        /// <summary>Id del centro en DWH (sp: @IdCentroDWH varchar(2))</summary>
        public string IdCentroDWH { get; set; }

        /// <summary>Id del jefe en DWH (sp: @IDPersonalDWH_Jefe int)</summary>
        public int IDPersonalDWH_Jefe { get; set; }

        /// <summary>Id de la persona evaluada en DWH (sp: @IDPersonalDWH_Evaluado int)</summary>
        public int IDPersonalDWH_Evaluado { get; set; }

        /// <summary>Nombre del evaluado (sp: @cNombreEvaluado varchar(100))</summary>
        public string cNombreEvaluado { get; set; }

        /// <summary>Comentarios de la evaluación (sp: @cComentarios varchar(1000))</summary>
        public string cComentarios { get; set; }

        /// <summary>Número de empleado del agente (sp: @nNoEmpAgente numeric)</summary>
        public decimal nNoEmpAgente { get; set; }

        /// <summary>Id de antigüedad / tiempo con el líder (sp: @IDAntig numeric)</summary>
        public decimal IDAntig { get; set; }
    }

    /// <summary>
    /// Detalle de una respuesta individual a una pregunta
    /// </summary>
    public class RespuestaDetalle
    {
        public int Id { get; set; }
        public int RespuestaId { get; set; }
        public int PreguntaId { get; set; }
        public string Valor { get; set; }
        public DateTime FechaRespuesta { get; set; } = DateTime.Now;

        // Relaciones
        public virtual Respuesta Respuesta { get; set; }
        public virtual Pregunta Pregunta { get; set; }
    }
}
