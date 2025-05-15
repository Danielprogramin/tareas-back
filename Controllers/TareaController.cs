using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TareasApi.Dtos;
using TareasApi.Services.Interfaces;

namespace TareasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TareaController : ControllerBase
    {
        private readonly ITareaService _tareaService;
        private readonly UserManager<IdentityUser> _userManager;

        public TareaController(ITareaService tareaService, UserManager<IdentityUser> userManager)
        {
            _tareaService = tareaService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TareaDto>>> GetTareas()
        {
            var userId = _userManager.GetUserId(User);
            var tareas = await _tareaService.GetTareasAsync(userId);
            return Ok(tareas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TareaDto>> GetTarea(int id)
        {
            var userId = _userManager.GetUserId(User);
            var tarea = await _tareaService.GetTareaAsync(id, userId);

            if (tarea == null)
                return NotFound();

            return Ok(tarea);
        }

        [HttpPost]
        public async Task<ActionResult<TareaDto>> PostTarea([FromBody] TareaCreateDto dto)
        {
            var userId = _userManager.GetUserId(User);
            var tarea = await _tareaService.CreateTareaAsync(userId, dto);
            return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id }, tarea);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarea(int id, TareaUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("El ID de la ruta no coincide con el ID del cuerpo.");

            var userId = _userManager.GetUserId(User);
            var actualizado = await _tareaService.UpdateTareaAsync(userId, dto);

            if (!actualizado)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(int id)
        {
            var userId = _userManager.GetUserId(User);
            var eliminado = await _tareaService.DeleteTareaAsync(id, userId);

            if (!eliminado)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/completar")]
        public async Task<IActionResult> Completar(int id)  
        {
            var userId = _userManager.GetUserId(User);
            var completado = await _tareaService.CompletarTareaAsync(id, userId);

            if (!completado)
                return NotFound();

            return NoContent();
        }

        [HttpGet("estadisticas")]
        public async Task<IActionResult> ObtenerEstadisticas()
        {
            var userId = _userManager.GetUserId(User);
            var stats = await _tareaService.GetEstadisticasAsync(userId);
            return Ok(stats);
        }
    }
}
