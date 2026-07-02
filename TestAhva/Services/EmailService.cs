using System;
using System.IO;
using System.Threading.Tasks;

namespace TestAhva.Services
{
    public interface IEmailService
    {
        Task SendLockoutEmailAsync(string targetEmail, string userDoc);
    }

    public class LocalFileEmailService : IEmailService
    {
        public async Task SendLockoutEmailAsync(string targetEmail, string userDoc)
        {
            // Write logs to a local temp folder by default (safer than hard-coded drive paths).
            string emailLogDir = Path.Combine(Path.GetTempPath(), "SentEmailsMock");
            Directory.CreateDirectory(emailLogDir);

            // Sanitize userDoc so it can be used safely in a filename.
            var invalidChars = Path.GetInvalidFileNameChars();
            var safeUserDoc = string.IsNullOrWhiteSpace(userDoc)
                ? "unknown"
                : string.Join("_", userDoc.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
            if (safeUserDoc.Length > 100) safeUserDoc = safeUserDoc.Substring(0, 100);

            string fileName = $"Lockout_Alert_{safeUserDoc}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string filePath = Path.Combine(emailLogDir, fileName);

            string emailContent = $@"
========================================================================
DE: seguridad@ceplan.gob.pe
PARA: {targetEmail}
ASUNTO: Alerta de Seguridad: Cuenta Bloqueada Temporalmente
========================================================================

Estimado Usuario,

Le informamos que su cuenta asociada al documento ({userDoc}) ha sido 
bloqueada temporalmente debido a que se superó el límite máximo de 5 
intentos fallidos de inicio de sesión.

Por motivos de seguridad, el acceso al sistema permanecerá restringido 
durante los próximos 15 minutos.

Centro Nacional de Planeamiento Estratégico (CEPLAN)
Fecha/Hora del Evento: {DateTime.Now}
========================================================================";

            await File.WriteAllTextAsync(filePath, emailContent);
        }
    }
}
