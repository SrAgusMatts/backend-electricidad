using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Request
{
    public class DetallePedidoDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
    public class PedidoCreateDto
    {
        [Required]
        public string NombreCliente { get; set; } = string.Empty;

        [Required]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public List<DetallePedidoDto> Items { get; set; } = new List<DetallePedidoDto>();
    }
}