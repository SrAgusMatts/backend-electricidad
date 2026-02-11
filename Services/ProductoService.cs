using Backend.DTOs.Request;
using Backend.DTOs.Response;
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

        public async Task<IEnumerable<ProductoResponseDto>> ObtenerTodos()
        {
            var productos = await _unitOfWork.Productos.GetAllAsync(includeProperties: "Marca,Categoria");

            return productos.Select(p => new ProductoResponseDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                ImagenUrl = p.ImagenUrl,
                Marca = p.Marca != null ? new MarcaResponseDto
                {
                    Id = p.Marca.Id,
                    Nombre = p.Marca.Nombre
                } : null,
                Categoria = p.Categoria != null ? new CategoriaResponseDto
                {
                    Id = p.Categoria.Id,
                    Nombre = p.Categoria.Nombre
                } : null
            });
        }

        public async Task<ProductoResponseDto?> ObtenerPorId(int id)
        {
            var p = await _unitOfWork.Productos.GetByIdAsync(id);

            if (p == null) return null;

            return new ProductoResponseDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                ImagenUrl = p.ImagenUrl,
                Categoria = p.Categoria != null ? new CategoriaResponseDto
                {
                    Id = p.Categoria.Id,
                    Nombre = p.Categoria.Nombre
                } : null,
                Marca = p.Marca != null ? new MarcaResponseDto
                {
                    Id = p.Marca.Id,
                    Nombre = p.Marca.Nombre
                } : null
            };
        }

        public async Task<Producto> CrearProducto(ProductoRequestDto dto)
        {
            var nuevoProducto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock,
                CategoriaId = dto.CategoriaId,
                MarcaId = dto.MarcaId,
                ImagenUrl = ""
            };

            if (dto.Imagen != null)
            {
                nuevoProducto.ImagenUrl = await GuardarImagen(dto.Imagen);
            }

            await _unitOfWork.Productos.AddAsync(nuevoProducto);
            await _unitOfWork.CompleteAsync();

            return nuevoProducto;
        }

        public async Task ActualizarProducto(int id, ProductoRequestDto dto)
        {
            var productoExistente = await _unitOfWork.Productos.GetByIdAsync(id);

            if (productoExistente != null)
            {
                productoExistente.Nombre = dto.Nombre;
                productoExistente.Descripcion = dto.Descripcion;
                productoExistente.Precio = dto.Precio;
                productoExistente.Stock = dto.Stock;
                productoExistente.CategoriaId = dto.CategoriaId;
                productoExistente.MarcaId = dto.MarcaId;

                if (dto.Imagen != null)
                {
                    productoExistente.ImagenUrl = await GuardarImagen(dto.Imagen);
                }

                _unitOfWork.Productos.Update(productoExistente);
                await _unitOfWork.CompleteAsync();
            }
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

        private async Task<string> GuardarImagen(IFormFile imagen)
        {
            string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
            string rutaCarpeta = Path.Combine(_env.WebRootPath, "imagenes");

            if (!Directory.Exists(rutaCarpeta)) Directory.CreateDirectory(rutaCarpeta);

            string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await imagen.CopyToAsync(stream);
            }

            var request = _httpContextAccessor.HttpContext!.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            return $"{baseUrl}/imagenes/{nombreArchivo}";
        }
    }
}