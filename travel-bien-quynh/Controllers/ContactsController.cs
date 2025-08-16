using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using travel_bien_quynh.CoTravel_bien_quynh;
using travel_bien_quynh.Repositories.Interface;

namespace travel_bien_quynh.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IEmailConfiguration _emailConfig;

        public ContactController(IEmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public class ContactRequest
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Subject { get; set; }
            public string Message { get; set; }
        }

        [HttpPost("send")]
        public IActionResult SendContact([FromBody] ContactRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new { message = "Vui lòng nhập đầy đủ thông tin bắt buộc." });
            }

            string body = $@"
                <h3>Thông tin liên hệ</h3>
                <p><b>Họ tên:</b> {request.FullName}</p>
                <p><b>Email:</b> {request.Email}</p>
                <p><b>Số điện thoại:</b> {request.Phone}</p>
                <p><b>Chủ đề:</b> {request.Subject}</p>
                <p><b>Nội dung:</b> {request.Message}</p>
            ";

            try
            {
                var sendMail = new SendMail(_emailConfig);
                string result = sendMail.sendMailBySmtp(
                    TO: "dungjpit.fpt@gmail.com", // email nhận
                    CC: "",
                    SUBJECT: $"Liên hệ từ {request.FullName} - {request.Subject}",
                    BODY: body
                );

                if (!string.IsNullOrEmpty(result))
                {
                    return StatusCode(500, new { message = "Gửi email thất bại", error = result });
                }

                return Ok(new { message = "Gửi email thành công" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi gửi email", error = ex.Message });
            }
        }
    }
}
