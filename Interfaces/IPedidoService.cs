using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResponseDto> CrearPedido(PedidoCreateDto dto);
        Task<List<PedidoResponseDto>> ObtenerPedidos();
        Task<PedidoResponseDto?> ObtenerPedidoPorId(int id);
        Task<bool> CambiarEstado(int id, string nuevoEstado);
    }
}