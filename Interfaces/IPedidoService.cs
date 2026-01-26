using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResponseDto> CrearPedido(PedidoCreateDto dto);
    }
}