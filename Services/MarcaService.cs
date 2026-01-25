using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class MarcaService : IMarcaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MarcaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Marca>> ObtenerTodas()
        {
            var marcas = await _unitOfWork.Marcas.GetAllAsync();
            return marcas.OrderBy(m => m.Nombre);
        }

        public async Task<Marca> CrearMarca(Marca marca)
        {
            await _unitOfWork.Marcas.AddAsync(marca);
            await _unitOfWork.CompleteAsync();
            return marca;
        }
    }
}