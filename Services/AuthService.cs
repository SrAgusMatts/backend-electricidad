using Backend.Interfaces;
using Backend.Templates;

namespace Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public AuthService(IUnitOfWork unitOfWork, IEmailService emailService, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _config = config;
        }

        public async Task<bool> SolicitarRecuperacion(string email)
        {
            // 👇 CORRECCIÓN AQUÍ:
            // Como es un repo genérico, traemos la lista y filtramos manualmente.
            // (Si tu repo tiene un método Find(x => ...), úsalo, si no, usa GetAll)
            var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
            var usuario = usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null) return true;

            // Generar Token
            usuario.ResetToken = Guid.NewGuid().ToString();
            usuario.ResetTokenExpira = DateTime.UtcNow.AddMinutes(15);

            _unitOfWork.Usuarios.Update(usuario);
            await _unitOfWork.CompleteAsync();

            var baseUrl = _config["AppSettings:BaseUrl"];
            var enlace = $"{baseUrl}/reset-password?token={usuario.ResetToken}&email={email}";

            var html = EmailTemplates.RecuperarPassword(usuario.Nombre, enlace);

            await _emailService.SendEmailAsync(email, "Recuperar Contraseña", html);
            return true;
        }

        public async Task<bool> ResetearPassword(string email, string token, string nuevaPassword)
        {

            var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
            var usuario = usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null || usuario.ResetToken != token || usuario.ResetTokenExpira < DateTime.UtcNow)
                return false;

            usuario.Password = BCrypt.Net.BCrypt.HashPassword(nuevaPassword);

            // Limpiar token
            usuario.ResetToken = null;
            usuario.ResetTokenExpira = null;

            _unitOfWork.Usuarios.Update(usuario);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}