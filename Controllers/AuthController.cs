using Backend.DTOs;
using Backend.Interfaces;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("olvide-password")]
        public async Task<IActionResult> OlvidePassword([FromBody] OlvidePasswordDto dto)
        {
            await _authService.SolicitarRecuperacion(dto.Email);
            return Ok(new { message = "Si el correo existe, se enviaron las instrucciones." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var exito = await _authService.ResetearPassword(dto.Email, dto.Token, dto.NuevaPassword);

            if (!exito) return BadRequest("Enlace inválido o expirado.");

            return Ok(new { message = "Contraseña actualizada correctamente." });
        }
    }
}