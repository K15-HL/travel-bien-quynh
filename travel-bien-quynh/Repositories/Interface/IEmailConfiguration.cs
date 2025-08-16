namespace travel_bien_quynh.Repositories.Interface
{
    public interface IEmailConfiguration
    {
        string FromName { get; set; }
        string SmtpServer { get; set; }
        int SmtpPort { get; set; }
        string SmtpUsername { get; set; }
        string SmtpPassword { get; set; }

        bool IsSystem { get; set; }
        string PopServer { get; set; }
        int PopPort { get; set; }
        string PopUsername { get; set; }
        string PopPassword { get; set; }
        string Url { get; set; }
    }
}
