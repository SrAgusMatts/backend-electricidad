namespace Backend.DTOs.Response
{
    public class PedidoResponseDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public decimal Total { get; set; }

        public List<DetallePedidoResponseDto> Items { get; set; } = new List<DetallePedidoResponseDto>();
    }
}