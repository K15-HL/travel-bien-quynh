using System.ComponentModel.DataAnnotations;

namespace travel_bien_quynh.Requests;

public class RegisterRequest
{
    public string Email { get; set; }

    [StringLength(100, ErrorMessage = "USERNAME_MIN_LENGTH", MinimumLength = 6)]
    public string Username { get; set; }

    [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
    public string Password { get; set; }
    public string TypeAccount { get; set; } 
    public string VerifyCode { get; set; }

}
