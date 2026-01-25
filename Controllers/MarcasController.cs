using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcasController : ControllerBase
    {
        private readonly IMarcaService _marcaService;

        public MarcasController(IMarcaService marcaService)
        {
            _marcaService = marcaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marca>>> GetMarcas()
        {
            var marcas = await _marcaService.ObtenerTodas();
            return Ok(marcas);
        }

        [HttpPost]
        public async Task<ActionResult<Marca>> PostMarca(Marca marca)
        {
            var nuevaMarca = await _marcaService.CrearMarca(marca);
            return CreatedAtAction(nameof(GetMarcas), new { id = nuevaMarca.Id }, nuevaMarca);
        }
    }
}