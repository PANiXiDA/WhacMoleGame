namespace PL.MVC.Infrastructure.Responses
{
    public class ResponseAuthorizationModel
    {
        public bool IsSuccess { get; set; } = true;
        public string TextError { get; set; } = string.Empty;
    }
}
