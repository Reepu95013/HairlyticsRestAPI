namespace Hairlytics.Application.DTOs.HelperDTOs
{
    public class AppSettingsDto
    {
        public string DefaultConnection { get; set; } = "";
        public string FileStorageBasePath { get; set; } = "";
        public string SmtpServer { get; set; } = "";
        public int SmtpPort { get; set; } = 465;
        public string EmailFrom { get; set; } = "";
        public string EmailUsername { get; set; } = "";
        public string EmailPassword { get; set; } = "";
        public int SessionTimeoutMinutes { get; set; } = 20;
    }
}
