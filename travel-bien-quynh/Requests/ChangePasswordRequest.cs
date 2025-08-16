using System.ComponentModel.DataAnnotations;

namespace travel_bien_quynh.Requests
{
    
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; }
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string NewPassword { get; set; }
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string ConfirmPassword { get; set;}

    }
}
