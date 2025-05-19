using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using TareasApi.configuration;
using TareasApi.Dtos;


namespace TareasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly EmailSettings _emailSettings;

        public EmailController(UserManager<IdentityUser> userManager, EmailSettings emailSettings)
        {
            _userManager = userManager;
            _emailSettings = emailSettings;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return BadRequest("Usuario no encontrado");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var frontendUrl = "http://localhost:3001"; 
            var callbackUrl = $"{frontendUrl}/reset-password?email={dto.Email}&token={encodedToken}";

            var emailService = new EmailService(); 
            var mensajeHtml = $@"
                 <h3>Recuperación de contraseña</h3>
                 <p>Haz clic en el siguiente enlace para restablecer tu contraseña:</p>
                 <p><a href='{callbackUrl}'>{callbackUrl}</a></p>";

            await emailService.SendEmailAsync(dto.Email, "Recuperación de contraseña", mensajeHtml);

            return Ok("Correo enviado correctamente.");
        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return BadRequest("Usuario no encontrado.");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);

            if (result.Succeeded)
                return Ok("Contraseña restablecida exitosamente.");

            return BadRequest(result.Errors);
        }
    }
}

