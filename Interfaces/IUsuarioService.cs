using Backend.Models;

namespace Backend.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> Registrar(Usuario usuario);
        Usuario? ValidarLogin(string email, string password);
    }
}