using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TareasApi.Models
{
    public enum Prioridad { Baja, Media, Alta }

    public class Tarea
    {
        public int Id { get; set; }

        public string UsuarioId { get; set; } = string.Empty;

        [JsonIgnore]
        public IdentityUser? Usuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Descripcion { get; set; } = string.Empty;

        public bool Completado { get; set; }

        [Required]
        public Prioridad Prioridad { get; set; }

        [DataType(DataType.Date)]
        [CustomValidation(typeof(Tarea), nameof(ValidarFecha))]
        public DateTime? FechaVencimiento { get; set; }

        public static ValidationResult? ValidarFecha(DateTime? fecha, ValidationContext context)
        {
            if (fecha.HasValue && fecha.Value < DateTime.Today)
                return new ValidationResult("La fecha no puede estar en el pasado");
            return ValidationResult.Success;
        }
    }
}
