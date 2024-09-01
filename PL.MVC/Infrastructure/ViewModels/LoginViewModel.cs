namespace PL.MVC.Infrastructure.ViewModel
{
    public class LoginViewModel
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Remember { get; set; }
    }
}
