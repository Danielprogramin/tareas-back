using TareasApi.Dtos;

namespace TareasApi.Services.Interfaces
{
    public interface ITareaService
    {
        Task<IEnumerable<TareaDto>> GetTareasAsync(string userId);
        Task<TareaDto?> GetTareaAsync(int id, string userId);
        Task<TareaDto> CreateTareaAsync(string userId, TareaCreateDto dto);
        Task<bool> UpdateTareaAsync(string userId, TareaUpdateDto dto);
        Task<bool> DeleteTareaAsync(int id, string userId);
        Task<bool> CompletarTareaAsync(int id, string userId);
        Task<EstadisticaDto> GetEstadisticasAsync(string userId);

    }
}
