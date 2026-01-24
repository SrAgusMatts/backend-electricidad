using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Producto> Productos { get; set; } = new List<Producto>();
    }
}