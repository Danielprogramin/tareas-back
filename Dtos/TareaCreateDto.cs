using TareasApi.Models;

namespace TareasApi.Dtos
{
    public class TareaCreateDto
    {
        public string Descripcion { get; set; } = string.Empty;
        public Prioridad Prioridad { get; set; }
        public DateTime? FechaVencimiento { get; set; }
    }

}
