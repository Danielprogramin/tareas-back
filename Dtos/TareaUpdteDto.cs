using System.ComponentModel.DataAnnotations;
using TareasApi.Models;

namespace TareasApi.Dtos
{
    public class TareaUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La prioridad es obligatoria.")]
        public Prioridad Prioridad { get; set; }

        public bool Completado { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaVencimiento { get; set; }
    }
}
