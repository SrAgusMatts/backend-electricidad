using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
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
        public ActionResult<Usuario> Login(Usuario loginData)
        {
            var usuario = _usuarioService.ValidarLogin(loginData.Email, loginData.Password);

            if (usuario == null)
            {
                return Unauthorized("Correo o contraseña incorrectos.");
            }

            return Ok(usuario);
        }
    }
}