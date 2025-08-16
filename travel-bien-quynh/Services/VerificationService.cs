using MongoDB.Driver.Linq;
using travel_bien_quynh.Entities;
using travel_bien_quynh.Repositories;
using travel_bien_quynh.Entities;
using travel_bien_quynh.Repositories.Interface;

namespace travel_bien_quynh.Services
{
    public interface IVerificationService
    {
        Task<string> GenerateVerificationCodeAsync(string email);
        string GetContent(string accountName, string code, string body);
    }

    public class VerificationService : IVerificationService
    {
        private readonly IVerificationCode _verificationCode;
        private readonly IWebHostEnvironment _environment;

        public VerificationService(IVerificationCode verificationCode, IWebHostEnvironment environment)
        {
            _verificationCode = verificationCode;
            _environment = environment;
        }

        public async Task<string> GenerateVerificationCodeAsync(string email)
        {
            var code = new Random().Next(100000, 999999).ToString();
            var existingVerificationCode = await _verificationCode.GetAsyncByField("email", email);

            if (existingVerificationCode != null)
            {
                await _verificationCode.DeleteAsync(existingVerificationCode.Id.ToString());
            }

            var expiryTime = DateTime.UtcNow.AddMinutes(10);
            await _verificationCode.CreateAsync(new VerificationCode
            {
                Email = email,
                Code = code,
                ExpiryTime = expiryTime
            });

            return code;
        }
        public string GetContent(string accountName, string code, string body)
        {
            string content = string.Empty;
            string path = Path.Combine(_environment.ContentRootPath, "Content", "Templates", "Template_Form_VN.html");

            using (StreamReader reader = new StreamReader(path))
            {
                content = reader.ReadToEnd();
            }

            content = content.Replace("{AccountName}", accountName);
            content = content.Replace("{Code}", code);
            content = content.Replace("{Body}", body);
            content = content.Replace("{TimeHour}", DateTime.Now.ToString("HH:mm"));
            content = content.Replace("{TimeDate}", DateTime.Now.ToString("dd/MM/yyyy"));
            return content;
        }
    }
}
