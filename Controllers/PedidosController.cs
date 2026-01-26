using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost]
        public async Task<ActionResult<PedidoResponseDto>> PostPedido([FromBody] PedidoCreateDto dto)
        {
            try
            {
                if (dto.Items == null || dto.Items.Count == 0)
                {
                    return BadRequest("El carrito no puede estar vacío.");
                }

                var resultado = await _pedidoService.CrearPedido(dto);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear el pedido: {ex.Message}");
            }
        }
    }
}