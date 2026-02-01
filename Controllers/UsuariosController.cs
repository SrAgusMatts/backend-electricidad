using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAuthService _authService;

        public UsuariosController(IUsuarioService usuarioService, IAuthService authService)
        {
            _usuarioService = usuarioService;
            _authService = authService;
        }

        [HttpPost("registro")]
        public async Task<ActionResult<Usuario>> Registrarse(Usuario usuario)
        {
            try
            {
                var nuevoUsuario = await _usuarioService.Registrar(usuario);
                return Ok(nuevoUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public ActionResult<object> Login([FromBody] Usuario loginData)
        {
            var usuario = _usuarioService.ValidarLogin(loginData.Email, loginData.Password);

            if (usuario == null)
            {
                return Unauthorized("Correo o contraseña incorrectos.");
            }

            var tokenJwt = _authService.GenerarTokenJwt(usuario);

            return Ok(new
            {
                token = tokenJwt,
                usuario = new
                {
                    id = usuario.Id,
                    nombre = usuario.Nombre,
                    email = usuario.Email,
                    rol = usuario.Rol,
                }
            });
        }
    }
}