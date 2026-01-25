using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Http;

namespace Backend.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductoService(IUnitOfWork unitOfWork, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Producto>> ObtenerTodos()
        {
            return await _unitOfWork.Productos.GetAllAsync();
        }

        public async Task<Producto> CrearProducto(ProductoCreateDto dto)
        {

            var nuevoProducto = new Producto
            {
                Nombre = dto.Nombre,
                Marca = dto.Marca,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock,
                CategoriaId = dto.CategoriaId,
                ImagenUrl = ""
            };

            if (dto.Imagen != null)
            {

                string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(dto.Imagen.FileName);

                string rutaCarpeta = Path.Combine(_env.WebRootPath, "imagenes");

                if (!Directory.Exists(rutaCarpeta)) Directory.CreateDirectory(rutaCarpeta);

                string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await dto.Imagen.CopyToAsync(stream);
                }

                var request = _httpContextAccessor.HttpContext!.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";
                nuevoProducto.ImagenUrl = $"{baseUrl}/imagenes/{nombreArchivo}";
            }

            await _unitOfWork.Productos.AddAsync(nuevoProducto);
            await _unitOfWork.CompleteAsync();

            return nuevoProducto;
        }

        public async Task EliminarProducto(int id)
        {
            var producto = await _unitOfWork.Productos.GetByIdAsync(id);
            if (producto != null)
            {
                _unitOfWork.Productos.Delete(producto);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<Producto?> ObtenerPorId(int id)
        {
            return await _unitOfWork.Productos.GetByIdAsync(id);
        }

        public async Task ActualizarProducto(int id, Producto producto)
        {
            var productoExistente = await _unitOfWork.Productos.GetByIdAsync(id);

            if (productoExistente != null)
            {
                productoExistente.Nombre = producto.Nombre;
                productoExistente.Marca = producto.Marca;
                productoExistente.Descripcion = producto.Descripcion;
                productoExistente.Precio = producto.Precio;
                productoExistente.ImagenUrl = producto.ImagenUrl;
                productoExistente.Stock = producto.Stock;
                productoExistente.CategoriaId = producto.CategoriaId;

                _unitOfWork.Productos.Update(productoExistente);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}