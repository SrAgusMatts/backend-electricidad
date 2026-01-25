using Backend.Models;

namespace Backend.Interfaces
{
    public interface IMarcaService
    {
        Task<IEnumerable<Marca>> ObtenerTodas();
        Task<Marca> CrearMarca(Marca marca);
    }
}