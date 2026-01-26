using Backend.Data;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IGenericRepository<Producto> _productos;
        private IGenericRepository<Usuario> _usuarios;
        private IGenericRepository<Marca> _marcas;
        private IGenericRepository<Pedido> _pedidos;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Producto> Productos => _productos ??= new GenericRepository<Producto>(_context);
        public IGenericRepository<Usuario> Usuarios => _usuarios ??= new GenericRepository<Usuario>(_context);
        public IGenericRepository<Marca> Marcas => _marcas ??= new GenericRepository<Marca>(_context);
        public IGenericRepository<Pedido> Pedidos => _pedidos ??= new GenericRepository<Pedido>(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}