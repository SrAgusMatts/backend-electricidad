using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoResponseDto>> ObtenerTodos();

        Task<ProductoResponseDto?> ObtenerPorId(int id);

        Task<Producto> CrearProducto(ProductoRequestDto dto);

        Task ActualizarProducto(int id, ProductoRequestDto dto);

        Task EliminarProducto(int id);
    }
}