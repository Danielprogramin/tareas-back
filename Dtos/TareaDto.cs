using TareasApi.Models;

namespace TareasApi.Dtos
{
    public class TareaDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public bool Completado { get; set; }
        public Prioridad Prioridad { get; set; }
        public DateTime? FechaVencimiento { get; set; }
    }
}
