using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using travel_bien_quynh.CoTravel_bien_quynh;
using travel_bien_quynh.Entities;
using travel_bien_quynh.Repositories.Interface;
using travel_bien_quynh.Requests;
using travel_bien_quynh.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;


namespace travel_bien_quynh.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;
    private readonly IVerificationCode _verificationCode;

    private readonly IVerificationService _verificationService;
    private readonly IEmailConfiguration _emailConfiguration;
    PasswordHasher hasher = new PasswordHasher();

    public UsersController(IConfiguration config, IUserRepository userRepository, IEmailConfiguration emailConfiguration, IVerificationCode verificationCode, IVerificationService verificationService)
    {
        _config = config;
        _userRepository = userRepository;
        _emailConfiguration = emailConfiguration;
        _verificationCode = verificationCode;
        _verificationService = verificationService;
    }

    //[Authorize]
    [HttpGet]
    public async Task<IActionResult> GetInfoUser()
    {
        string username = User.FindFirst(JwtRegisteredClaimNames.PreferredUsername)?.Value;

        if (username == null)
        {
            return Unauthorized(new { msg = "User not found" });
        }


        var user = await _userRepository.GetAsyncByField("username", username);

        if (user == null)
        {
            return NotFound(new { msg = $"User not found " });
        }
        return StatusCode(200, new { user.Email, user.Username, user.Id, user.Money, user.Role, user.TypeAccount });
    }

    [Authorize]
    [HttpGet]

    public async Task<IActionResult> GetAllUser()
    {
        string username = User.FindFirst(JwtRegisteredClaimNames.PreferredUsername).Value;

        if (username == null)
        {
            return Unauthorized(new { msg = "User not found" });
        }

        var user = await _userRepository.GetAsyncByField("username", username);

        if (user == null)
        {
            return BadRequest(new { msg = "User Not Found" });
        }

        var alluser = await _userRepository.GetAsync();

        return StatusCode(200, new { data = alluser });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        IActionResult response = Unauthorized();

        var users = await _userRepository.GetAsync();


        // Find the user with matching credentials
        var user = users.FirstOrDefault(x => (x.Username == request.Username || x.Email == request.Username) && hasher.VerifyPassword(request.Password, x.Password));

        if (user != null)
        {
            if (user.Ban)
            {
                return BadRequest(new { msg = "Tài Khoản của bạn bị khóa liên hệ admin để biết thêm chi tiết" });
            }
            var tokenString = GenerateJSONWebToken(user);
            response = StatusCode(200, new { token = tokenString, user = user });
        }
        return response;
    }

    [HttpPost]
    public async Task<IActionResult> LoginbyGoogle(GoogleRegisterRequest request)
    {
        var users = await _userRepository.GetAsync();

        var googleuser = users.FirstOrDefault(x => x.Email == request.Email || x.Username == request.Username);

        IActionResult response = Unauthorized();

        if (googleuser != null)
        {
            var tokenString = GenerateJSONWebToken(googleuser);
            response = StatusCode(200, new { token = tokenString });

            return response;
        }


        await _userRepository.CreateAsync(new User
        {
            Email = request.Email,
            Username = request.Username,
            Password = request.Password,
            TypeAccount = request.TypeAccount,
            Money = 0,
            CreatedAt = DateTime.Now,
            Role = "User",
            Ban = false
        });

        var useraftercreate = await _userRepository.GetAsync();

        var googleuserafter = useraftercreate.FirstOrDefault(x => x.Email == request.Email || x.Username == request.Username);

        IActionResult newresponse = Unauthorized();

        if (googleuserafter != null)
        {
            var tokenString = GenerateJSONWebToken(googleuserafter);
            newresponse = StatusCode(200, new { token = tokenString });

            return newresponse;
        }

        return StatusCode(200, new { msg = "Đăng nhập không thành công" });
    }


    private string GenerateJSONWebToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[] {

                new Claim(JwtRegisteredClaimNames.PreferredUsername, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Role", user.Role),
            };

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(RegisterRequest request)
    {

        var regex = new Regex(RegexValidationRule.EmailPattern, RegexOptions.IgnoreCase);
        if (!regex.IsMatch(request.Email))
        {
            return BadRequest("Email không hợp lệ");
        }
        var regexUserName = new Regex(RegexValidationRule.UserNamePattern, RegexOptions.IgnoreCase);
        if (!regexUserName.IsMatch(request.Username))
        {
            return BadRequest("Tên tài khoản không hợp lệ");
        }
        var regexPass = new Regex(RegexValidationRule.PasswordPattern, RegexOptions.IgnoreCase);
        if (!regexPass.IsMatch(request.Password))
        {
            return BadRequest("Mật Khẩu không hợp lệ");
        }

        var users = await _userRepository.GetAsync();

        var user = users.FirstOrDefault(x => x.Email == request.Email || x.Username == request.Username);

        var code = await _verificationCode.GetAsyncByField("email", request.Email);

        if (user != null)
        {
            return Conflict(new { msg = "Tài Khoản hoặc Email đã tồn tại" });
        }

        string role = User.FindFirst("Role")?.Value;


        if (role == null)
        {
            if (code == null || code.ExpiryTime < DateTime.UtcNow || code.Code != request.VerifyCode)
            {

                return BadRequest(new { msg = $"Mã Hết Hạn Hoặc không tồn tại" });
            }
        }

        await _userRepository.CreateAsync(new User
        {
            Email = request.Email,
            Username = request.Username,
            Password = hasher.HashPassword(request.Password),
            TypeAccount = request.TypeAccount,
            Money = 0,
            CreatedAt = DateTime.Now,
            Role = "User",
            Ban = false
        });
        if (role == null)
        {
            await _verificationCode.DeleteAsync(code.Id.ToString());
        }


        return StatusCode(200, new { msg = "Tạo Tài Khoản Thành Công", status = 200 });
    }

    [HttpPost]
    public async Task<IActionResult> GetCodeForgotpassword(GetCodeVerify request)
    {

        var user = await _userRepository.GetAsyncByField("email", request.email);

        if (user == null || user.TypeAccount != "Default")
        {
            return BadRequest(new { msg = "Tài Khoản Hoặc Email Không Tồn Tại Trong Hệ Thống" });
        }

        try
        {
            string body = string.Empty;
            string code = string.Empty;
            code = await _verificationService.GenerateVerificationCodeAsync(request.email);
            string content = $"Bạn đã yêu cầu khôi phục mật khẩu cho tài khoản của mình trên <b>E_Commerce</b>. Vui lòng sử dụng mã sau để đặt lại mật khẩu của bạn:<b style=\"color\">{code}</b>";
            body = _verificationService.GetContent(request.email, code, content);
            var sendmail = new SendMail(_emailConfiguration);
            string msg = sendmail.sendMailBySmtp(request.email, request.email, "NATECHJSC", body);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }


        return StatusCode(200, new { msg = "Mã đã được gửi", status = 200 });
    }

    [HttpPost]
    public async Task<IActionResult> GetCodeVerify(GetCodeVerify request)
    {
        string body = string.Empty;
        string code = string.Empty;

        code = await _verificationService.GenerateVerificationCodeAsync(request.email);
        string content = $"Bạn đã đăng ký tài khoản trên <b>E_Commerce</b>. Vui lòng sử dụng mã sau để kích hoạt tài khoản của bạn\r\n  <b style=\"color:red\">{code}</b>";
        body = _verificationService.GetContent(request.email, code, content);
        var sendmail = new SendMail(_emailConfiguration);

        string msg = sendmail.sendMailBySmtp(request.email, request.email, "NATECHJSC", body);

        return StatusCode(200, new { msg = "Gửi mã thành công", status = 200 });

    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { msg = "Invalid request data" });
        }

        var regexPass = new Regex(RegexValidationRule.PasswordPattern, RegexOptions.IgnoreCase);
        if (!regexPass.IsMatch(request.NewPassword))
        {
            return BadRequest("Mật Khẩu không hợp lệ");
        }

        if (String.Compare(request.NewPassword, request.ConfirmPassword) == 0)
        {
            string username = User.FindFirst(JwtRegisteredClaimNames.PreferredUsername)?.Value;

            if (username == null)
            {
                return Unauthorized(new { msg = "User not found" });
            }


            var user = await _userRepository.GetAsyncByField("username", username);

            if (user == null)
            {
                return NotFound(new { msg = $"User not found " });
            }


            if (!hasher.VerifyPassword(request.OldPassword, user.Password))
            {
                return BadRequest(new { msg = "Mật khẩu không chính xác" });
            }

            user.Password = hasher.HashPassword(request.NewPassword);

            await _userRepository.UpdateAsync(user.Id, user);


            return StatusCode(200, new { msg = "Đổi mật khẩu thành công" });
        }
        else
        {
            return BadRequest(new { msg = "Nhập lại mật khẩu không trùng khớp" });
        }


    }

    [HttpPut]
    public async Task<IActionResult> ResetPassword(ResetPassword request)
    {


        var regexPass = new Regex(RegexValidationRule.PasswordPattern, RegexOptions.IgnoreCase);
        if (!regexPass.IsMatch(request.password))
        {
            return BadRequest("Mật Khẩu không hợp lệ");
        }

        if (String.Compare(request.password, request.confirmPassword) == 0)
        {
            var user = await _userRepository.GetAsyncByField("email", request.email);

            if (user == null)
            {
                return NotFound(new { msg = $"User not found " });
            }
            var code = await _verificationCode.GetAsyncByField("email", request.email);

            if (code == null || code.ExpiryTime < DateTime.UtcNow || code.Code != request.vericode)
            {

                return BadRequest(new { msg = $"Mã Hết Hạn Hoặc không tồn tại" });
            }



            user.Password = hasher.HashPassword(request.password);

            await _userRepository.UpdateAsync(user.Id, user);



        }

        return StatusCode(200, new { msg = "Cập nhật mật khẩu thành công", status = 200 });

    }

}
