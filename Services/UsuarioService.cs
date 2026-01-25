using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Usuario> Registrar(Usuario usuario)
        {
            var existe = _unitOfWork.Usuarios.Find(u => u.Email == usuario.Email).FirstOrDefault();
            if (existe != null) throw new Exception("El correo ya existe.");

            usuario.Rol = "Cliente";
            await _unitOfWork.Usuarios.AddAsync(usuario);
            await _unitOfWork.CompleteAsync();

            return usuario;
        }

        public Usuario? ValidarLogin(string email, string password)
        {
            return _unitOfWork.Usuarios
                .Find(u => u.Email == email && u.Password == password)
                .FirstOrDefault();
        }
    }
}