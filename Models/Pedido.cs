using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

        [Required]
        public string NombreCliente { get; set; } = string.Empty;

        [Required]
        public string Telefono { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public decimal Total { get; set; }

        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }
}