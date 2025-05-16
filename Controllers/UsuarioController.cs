using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TareasApi.Dtos;


namespace TareasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; 

        public UsuarioController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("asignar-rol")]
        public async Task<IActionResult> AsignarRol([FromBody] AsignarRolDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UsuarioId);
            if (user == null) return NotFound("Usuario no encontrado");

            if (!await _roleManager.RoleExistsAsync(dto.Rol))
                return BadRequest("El rol no existe");

            if (await _userManager.IsInRoleAsync(user, dto.Rol))
                return BadRequest("El usuario ya tiene ese rol");

            var result = await _userManager.AddToRoleAsync(user, dto.Rol);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Rol asignado correctamente");
        }

        [HttpPost("remover-rol")]
        public async Task<IActionResult> RemoverRol([FromBody] RemoverRolDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UsuarioId);
            if (user == null) return NotFound("Usuario no encontrado");

            if (!await _roleManager.RoleExistsAsync(dto.Rol))
                return BadRequest("El rol no existe");

            if (!await _userManager.IsInRoleAsync(user, dto.Rol))
                return BadRequest("El usuario no tiene este rol");

            var result = await _userManager.RemoveFromRoleAsync(user, dto.Rol);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Rol removido correctamente");
        }


        [HttpGet]
        public async Task<IActionResult> GetUsuariosConRoles()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    user.Id,
                    user.Email,
                    Roles = roles
                });
            }

            return Ok(userList);
        }
    }
}
