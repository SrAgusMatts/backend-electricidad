using System.Linq.Expressions;

namespace Backend.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression); // Para buscar con filtros
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}