using Microsoft.AspNetCore.Http; // Necesario para IFormFile

namespace Backend.DTOs
{
    public class ProductoCreateDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
        public IFormFile? Imagen { get; set; }
        public string Marca { get; set; }
    }
}