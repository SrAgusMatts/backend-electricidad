using Backend.Models;

namespace Backend.Interfaces
{
    public interface IAuthService
    {
        Task<bool> SolicitarRecuperacion(string email);
        Task<bool> ResetearPassword(string email, string token, string nuevaPassword);
        string GenerarTokenJwt(Usuario usuario);
    }
}