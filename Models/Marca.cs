using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Marca
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;
    }
}