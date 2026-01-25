using Backend.DTOs;
using Backend.Models;

namespace Backend.Interfaces // O namespace Backend.Services si preferís
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> ObtenerTodos();
        Task<Producto?> ObtenerPorId(int id);
        Task<Producto> CrearProducto(ProductoCreateDto productoDto);
        Task EliminarProducto(int id);
        Task ActualizarProducto(int id, Producto producto);
    }
}