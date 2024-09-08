namespace Dal.DbModels.Models
{
    public partial class ConfirmationCode
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
