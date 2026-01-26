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
        public async Task<ActionResult<PedidoResponseDto>> Crear([FromBody] PedidoCreateDto dto)
        {
            var resultado = await _pedidoService.CrearPedido(dto);
            return Ok(resultado);
        }

        [HttpGet]
        public async Task<ActionResult<List<PedidoResponseDto>>> ObtenerTodos()
        {
            var pedidos = await _pedidoService.ObtenerPedidos();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoResponseDto>> ObtenerPorId(int id)
        {
            var pedido = await _pedidoService.ObtenerPedidoPorId(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] string nuevoEstado)
        {
            var exito = await _pedidoService.CambiarEstado(id, nuevoEstado);
            if (!exito) return BadRequest("No se pudo actualizar el estado.");
            return NoContent();
        }
    }
}