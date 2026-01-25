using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Request
{
    public class MarcaRequestDto
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;
    }
}