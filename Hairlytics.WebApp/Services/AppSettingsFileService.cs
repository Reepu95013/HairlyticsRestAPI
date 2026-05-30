using System.Text.Json;
using System.Text.Json.Nodes;
using Hairlytics.Application.DTOs.HelperDTOs;
using Microsoft.Extensions.Configuration;

namespace Hairlytics.WebApp.Services
{
    public class AppSettingsFileService
    {
        private readonly string _settingsPath;
        private readonly IConfiguration _configuration;

        public AppSettingsFileService(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _settingsPath = Path.Combine(environment.ContentRootPath, "appsettings.json");
            _configuration = configuration;
        }

        public AppSettingsDto Load()
        {
            return new AppSettingsDto
            {
                DefaultConnection = _configuration.GetConnectionString("DefaultConnection") ?? "",
                FileStorageBasePath = _configuration["FileStorage:BasePath"] ?? "",
                SmtpServer = _configuration["EmailSettings:SmtpServer"] ?? "",
                SmtpPort = int.TryParse(_configuration["EmailSettings:Port"], out var port) ? port : 465,
                EmailFrom = _configuration["EmailSettings:From"] ?? "",
                EmailUsername = _configuration["EmailSettings:Username"] ?? "",
                EmailPassword = "",
                SessionTimeoutMinutes = int.TryParse(_configuration["AdminSettings:SessionTimeoutMinutes"], out var mins) ? mins : 20
            };
        }

        public async Task<(bool Success, string Message)> SaveAsync(AppSettingsDto dto)
        {
            try
            {
                if (!File.Exists(_settingsPath))
                    return (false, "appsettings.json not found.");

                var json = await File.ReadAllTextAsync(_settingsPath);
                var root = JsonNode.Parse(json)?.AsObject();
                if (root == null)
                    return (false, "Invalid appsettings.json format.");

                root["ConnectionStrings"] ??= new JsonObject();
                root["ConnectionStrings"]!["DefaultConnection"] = dto.DefaultConnection;

                root["FileStorage"] ??= new JsonObject();
                root["FileStorage"]!["BasePath"] = dto.FileStorageBasePath;

                root["EmailSettings"] ??= new JsonObject();
                root["EmailSettings"]!["SmtpServer"] = dto.SmtpServer;
                root["EmailSettings"]!["Port"] = dto.SmtpPort;
                root["EmailSettings"]!["From"] = dto.EmailFrom;
                root["EmailSettings"]!["Username"] = dto.EmailUsername;
                if (!string.IsNullOrWhiteSpace(dto.EmailPassword))
                    root["EmailSettings"]!["Password"] = dto.EmailPassword;

                root["AdminSettings"] ??= new JsonObject();
                root["AdminSettings"]!["SessionTimeoutMinutes"] = dto.SessionTimeoutMinutes;

                var options = new JsonSerializerOptions { WriteIndented = true };
                await File.WriteAllTextAsync(_settingsPath, root.ToJsonString(options));

                return (true, "Settings saved. Restart the application for database, storage, and session changes to take effect.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
