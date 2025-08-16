namespace travel_bien_quynh.Requests
{
    public class ResetPassword
    {
        public string email {  get; set; }  
        public string password { get; set; }
        public string confirmPassword { get; set; }

        public string vericode { get; set; }
    }
}
