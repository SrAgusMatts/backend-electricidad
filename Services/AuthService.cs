using Backend.Interfaces;
using Backend.Models;
using System.IdentityModel.Tokens.Jwt;
using Backend.Templates;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
            var usuario = usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null) return true;

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

            usuario.ResetToken = null;
            usuario.ResetTokenExpira = null;

            _unitOfWork.Usuarios.Update(usuario);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public string GenerarTokenJwt(Usuario usuario)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var tokenKey = _config["AppSettings:Token"] ?? throw new Exception("Falta el Token en appsettings.json");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}