using System.ComponentModel.DataAnnotations;

namespace travel_bien_quynh.Requests;

public class GoogleRegisterRequest
{
    public string Email { get; set; }

    [StringLength(100, ErrorMessage = "USERNAME_MIN_LENGTH", MinimumLength = 6)]
    public string Username { get; set; }

    public string Password { get; set; }

    public string TypeAccount { get; set; } 

}
