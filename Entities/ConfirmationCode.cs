namespace Entities
{
    public class ConfirmationCode
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public ConfirmationCode(int id, string code)
        {
            Id = id;
            Code = code;
        }
    }
}
