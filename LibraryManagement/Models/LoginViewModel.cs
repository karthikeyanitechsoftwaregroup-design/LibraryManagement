namespace LibraryManagement.Models
{
    public class ValidateUserModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    
    public class LoginViewModel
    {
        public class ValidateUserModel
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}