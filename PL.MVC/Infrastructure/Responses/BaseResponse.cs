namespace PL.MVC.Infrastructure.Responses
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string TextError { get; set; } = string.Empty;
    }
}
