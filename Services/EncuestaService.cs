using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EncuestasEvaluacionLiderazgo.Models;

namespace EncuestasEvaluacionLiderazgo.Services
{
    /// <summary>
    /// Implementación del servicio de encuestas
    /// En producción, esto se conectaría a una base de datos real
    /// </summary>
    public class EncuestaService : IEncuestaService
    {
        // Simulación de base de datos en memoria
        private static readonly List<Encuesta> _encuestas = new();
        private static int _encuestaIdCounter = 1;

        public EncuestaService()
        {
            // Inicializar con datos de ejemplo
            if (_encuestas.Count == 0)
            {
                _encuestas.Add(new Encuesta
                {
                    Id = _encuestaIdCounter++,
                    Titulo = "Evaluación de Liderazgo 2026",
                    Descripcion = "Encuesta para evaluar competencias de liderazgo",
                    FechaCreacion = DateTime.Now.AddDays(-5),
                    FechaVencimiento = DateTime.Now.AddDays(30),
                    Estado = EstadoEncuesta.Publicada,
                    UsuarioCreadorId = 1,
                    Activa = true,
                    Preguntas = new List<Pregunta>
                    {
                        new Pregunta
                        {
                            Id = 1,
                            EncuestaId = 1,
                            Texto = "¿Cómo evalúas el liderazgo?",
                            Tipo = TipoPregunta.OpcionUnica,
                            Orden = 1,
                            Requerida = true,
                            Opciones = new List<OpcionRespuesta>
                            {
                                new OpcionRespuesta { Id = 1, PreguntaId = 1, Texto = "Excelente", Valor = 5, Orden = 1 },
                                new OpcionRespuesta { Id = 2, PreguntaId = 1, Texto = "Bueno", Valor = 4, Orden = 2 },
                                new OpcionRespuesta { Id = 3, PreguntaId = 1, Texto = "Regular", Valor = 3, Orden = 3 }
                            }
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Obtiene todas las encuestas de un usuario
        /// </summary>
        public Task<IEnumerable<Encuesta>> GetEncuestasAsync(int usuarioId)
        {
            var encuestas = _encuestas
                .Where(e => e.UsuarioCreadorId == usuarioId || e.Estado == EstadoEncuesta.Publicada)
                .ToList();

            return Task.FromResult<IEnumerable<Encuesta>>(encuestas);
        }

        /// <summary>
        /// Obtiene una encuesta por ID
        /// </summary>
        public Task<Encuesta> GetEncuestaByIdAsync(int id)
        {
            var encuesta = _encuestas.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(encuesta);
        }

        /// <summary>
        /// Crea una nueva encuesta
        /// </summary>
        public Task<(bool Success, string Message, int EncuestaId)> CreateEncuestaAsync(Encuesta encuesta)
        {
            // Validar entrada
            if (string.IsNullOrEmpty(encuesta.Titulo) || encuesta.Titulo.Length < 5)
            {
                return Task.FromResult<(bool, string, int)>(
                    (false, "El título debe tener al menos 5 caracteres", 0)
                );
            }

            if (string.IsNullOrEmpty(encuesta.Descripcion) || encuesta.Descripcion.Length < 10)
            {
                return Task.FromResult<(bool, string, int)>(
                    (false, "La descripción debe tener al menos 10 caracteres", 0)
                );
            }

            if (encuesta.FechaVencimiento <= DateTime.Now)
            {
                return Task.FromResult<(bool, string, int)>(
                    (false, "La fecha de vencimiento debe ser en el futuro", 0)
                );
            }

            // Asignar ID y crear
            encuesta.Id = _encuestaIdCounter++;
            encuesta.FechaCreacion = DateTime.Now;
            encuesta.Estado = EstadoEncuesta.Borrador;
            encuesta.Activa = true;
            encuesta.Preguntas = new List<Pregunta>();
            encuesta.Respuestas = new List<Respuesta>();

            _encuestas.Add(encuesta);

            return Task.FromResult<(bool, string, int)>(
                (true, "Encuesta creada exitosamente", encuesta.Id)
            );
        }

        /// <summary>
        /// Actualiza una encuesta existente
        /// </summary>
        public Task<(bool Success, string Message)> UpdateEncuestaAsync(Encuesta encuesta)
        {
            // Validar entrada
            if (string.IsNullOrEmpty(encuesta.Titulo) || encuesta.Titulo.Length < 5)
            {
                return Task.FromResult<(bool, string)>(
                    (false, "El título debe tener al menos 5 caracteres")
                );
            }

            var encuestaExistente = _encuestas.FirstOrDefault(e => e.Id == encuesta.Id);

            if (encuestaExistente == null)
            {
                return Task.FromResult<(bool, string)>(
                    (false, "Encuesta no encontrada")
                );
            }

            // Solo permitir edición en estado Borrador
            if (encuestaExistente.Estado != EstadoEncuesta.Borrador)
            {
                return Task.FromResult<(bool, string)>(
                    (false, "No se pueden editar encuestas publicadas o cerradas")
                );
            }

            encuestaExistente.Titulo = encuesta.Titulo;
            encuestaExistente.Descripcion = encuesta.Descripcion;
            encuestaExistente.FechaVencimiento = encuesta.FechaVencimiento;

            return Task.FromResult<(bool, string)>(
                (true, "Encuesta actualizada exitosamente")
            );
        }

        /// <summary>
        /// Elimina una encuesta
        /// </summary>
        public Task<(bool Success, string Message)> DeleteEncuestaAsync(int id)
        {
            var encuesta = _encuestas.FirstOrDefault(e => e.Id == id);

            if (encuesta == null)
            {
                return Task.FromResult<(bool, string)>(
                    (false, "Encuesta no encontrada")
                );
            }

            if (encuesta.Respuestas != null && encuesta.Respuestas.Any())
            {
                return Task.FromResult<(bool, string)>(
                    (false, "No se pueden eliminar encuestas con respuestas")
                );
            }

            _encuestas.Remove(encuesta);

            return Task.FromResult<(bool, string)>(
                (true, "Encuesta eliminada exitosamente")
            );
        }

        /// <summary>
        /// Publica una encuesta (la hace disponible para responder)
        /// </summary>
        public Task<(bool Success, string Message)> PublishEncuestaAsync(int id)
        {
            var encuesta = _encuestas.FirstOrDefault(e => e.Id == id);

            if (encuesta == null)
            {
                return Task.FromResult<(bool, string)>(
                    (false, "Encuesta no encontrada")
                );
            }

            if (encuesta.Estado != EstadoEncuesta.Borrador)
            {
                return Task.FromResult<(bool, string)>(
                    (false, "Solo se pueden publicar encuestas en estado Borrador")
                );
            }

            if (encuesta.Preguntas == null || !encuesta.Preguntas.Any())
            {
                return Task.FromResult<(bool, string)>(
                    (false, "La encuesta debe tener al menos una pregunta antes de publicarse")
                );
            }

            encuesta.Estado = EstadoEncuesta.Publicada;

            return Task.FromResult<(bool, string)>(
                (true, "Encuesta publicada exitosamente")
            );
        }

        /// <summary>
        /// Cierra una encuesta (detiene de aceptar respuestas)
        /// </summary>
        public Task<(bool Success, string Message)> CloseEncuestaAsync(int id)
        {
            var encuesta = _encuestas.FirstOrDefault(e => e.Id == id);

            if (encuesta == null)
            {
                return Task.FromResult<(bool, string)>(
                    (false, "Encuesta no encontrada")
                );
            }

            encuesta.Estado = EstadoEncuesta.Cerrada;

            return Task.FromResult<(bool, string)>(
                (true, "Encuesta cerrada exitosamente")
            );
        }
    }
}
