using AutoMapper;
using TareasApi.Data;
using TareasApi.Dtos;
using TareasApi.Models;
using TareasApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace TareasApi.Services
{
    public class TareaService : ITareaService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TareaService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TareaDto>> GetTareasAsync(string userId)
        {
            var tareas = await _context.Tareas
                .Where(t => t.UsuarioId == userId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TareaDto>>(tareas);
        }

        public async Task<TareaDto?> GetTareaAsync(int id, string userId)
        {
            var tarea = await _context.Tareas
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == userId);

            return tarea == null ? null : _mapper.Map<TareaDto>(tarea);
        }

        public async Task<TareaDto> CreateTareaAsync(string userId, TareaCreateDto dto)
        {
            var tarea = _mapper.Map<Tarea>(dto);
            tarea.UsuarioId = userId;

            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();

            return _mapper.Map<TareaDto>(tarea);
        }

        public async Task<bool> UpdateTareaAsync(string userId, TareaUpdateDto dto)
        {
            var tarea = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == dto.Id && t.UsuarioId == userId);
            if (tarea == null) return false;

            _mapper.Map(dto, tarea);
            tarea.UsuarioId = userId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTareaAsync(int id, string userId)
        {
            var tarea = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == userId);
            if (tarea == null) return false;

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompletarTareaAsync(int id, string userId)
        {
            var tarea = await _context.Tareas
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == userId);

            if (tarea == null)
                return false;

            tarea.Completado = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<EstadisticaDto> GetEstadisticasAsync(string userId)
        {
            var total = await _context.Tareas.CountAsync(t => t.UsuarioId == userId);
            var completadas = await _context.Tareas.CountAsync(t => t.UsuarioId == userId && t.Completado);
            var pendientes = total - completadas;

            return new EstadisticaDto
            {
                Total = total,
                Completadas = completadas,
                Pendientes = pendientes
            };
        }


    }
}
