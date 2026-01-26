using Backend.Models;

namespace Backend.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Producto> Productos { get; }
        IGenericRepository<Usuario> Usuarios { get; }
        IGenericRepository<Marca> Marcas { get; }
        IGenericRepository<Pedido> Pedidos { get; }
        Task<int> CompleteAsync();
    }
}