using Backend.Models;

namespace Backend.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Producto> Productos { get; }
        IGenericRepository<Usuario> Usuarios { get; }
        Task<int> CompleteAsync(); // SaveChanges
    }
}