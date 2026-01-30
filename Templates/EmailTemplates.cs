using System;

namespace Backend.Templates
{
    public static class EmailTemplates
    {
        public static string RecuperarPassword(string nombre, string enlace)
        {
            // Colores de tu marca
            string colorPrimario = "#2563EB"; // Tu azul actual
            string colorFondo = "#F3F4F6";    // Gris muy suave
            string colorTexto = "#1F2937";    // Gris oscuro casi negro

            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Recuperar Contraseña</title>
            </head>
            <body style='margin: 0; padding: 0; background-color: {colorFondo}; font-family: ""Helvetica Neue"", Helvetica, Arial, sans-serif;'>
                
                <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%' style='background-color: {colorFondo}; padding: 40px 0;'>
                    <tr>
                        <td align='center'>
                            
                            <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='600' style='background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                                
                                <tr>
                                    <td bgcolor='#111827' style='padding: 30px; text-align: center;'>
                                        <h1 style='margin: 0; color: #ffffff; font-size: 24px; letter-spacing: 1px; font-weight: bold;'>
                                            ⚡ ELECTRICIDAD MATTOS
                                        </h1>
                                    </td>
                                </tr>

                                <tr>
                                    <td style='padding: 40px 30px;'>
                                        <h2 style='margin-top: 0; color: {colorTexto}; font-size: 20px;'>Hola, {nombre} 👋</h2>
                                        
                                        <p style='color: #4B5563; font-size: 16px; line-height: 1.6;'>
                                            Recibimos una solicitud para restablecer la contraseña de tu cuenta. 
                                            Si fuiste tú, hacé clic en el botón de abajo para crear una nueva clave segura.
                                        </p>

                                        <div style='text-align: center; margin: 35px 0;'>
                                            <a href='{enlace}' style='background-color: {colorPrimario}; color: #ffffff; padding: 14px 32px; text-decoration: none; border-radius: 6px; font-weight: bold; font-size: 16px; display: inline-block; box-shadow: 0 2px 4px rgba(37, 99, 235, 0.3);'>
                                                Restablecer mi Contraseña
                                            </a>
                                        </div>

                                        <p style='color: #4B5563; font-size: 14px; margin-bottom: 0;'>
                                            Este enlace expirará en <strong>15 minutos</strong> por seguridad.
                                        </p>

                                        <hr style='border: 0; border-top: 1px solid #E5E7EB; margin: 30px 0;'>

                                        <p style='color: #6B7280; font-size: 12px; margin-bottom: 10px;'>
                                            ¿El botón no funciona? Copiá y pegá el siguiente enlace en tu navegador:
                                        </p>
                                        <p style='margin: 0; font-size: 12px; word-break: break-all;'>
                                            <a href='{enlace}' style='color: {colorPrimario}; text-decoration: underline;'>
                                                {enlace}
                                            </a>
                                        </p>
                                    </td>
                                </tr>

                                <tr>
                                    <td bgcolor='#F9FAFB' style='padding: 20px; text-align: center; border-top: 1px solid #E5E7EB;'>
                                        <p style='margin: 0; color: #9CA3AF; font-size: 12px;'>
                                            Si no solicitaste este cambio, podés ignorar este correo tranquilamente. Tu cuenta sigue segura.
                                        </p>
                                        <p style='margin: 10px 0 0; color: #9CA3AF; font-size: 12px;'>
                                            © {DateTime.Now.Year} Electricidad Mattos. Todos los derechos reservados.
                                        </p>
                                    </td>
                                </tr>

                            </table>
                            </td>
                    </tr>
                </table>

            </body>
            </html>";
        }
    }
}