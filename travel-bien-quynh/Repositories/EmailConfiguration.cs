using travel_bien_quynh.Repositories.Interface;

namespace travel_bien_quynh.Repositories
{
    public class EmailConfiguration : IEmailConfiguration
    {
        public string FromName { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }


        public bool IsSystem { get; set; }
        public string PopServer { get; set; }
        public int PopPort { get; set; }
        public string PopUsername { get; set; }
        public string PopPassword { get; set; }
        public string Url { get; set; }
    }
}
