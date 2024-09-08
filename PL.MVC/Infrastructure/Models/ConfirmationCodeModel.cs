using Entities;

namespace PL.MVC.Infrastructure.Models
{
    public class ConfirmationCodeModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;

        public static ConfirmationCodeModel FromEntity(ConfirmationCode obj)
        {
            return new ConfirmationCodeModel
            {
                Id = obj.Id,
                Code = obj.Code,
            };
        }

        public static ConfirmationCode ToEntity(ConfirmationCodeModel obj)
        {
            return new ConfirmationCode(
                obj.Id,
                obj.Code);
        }

        public static List<ConfirmationCodeModel> FromEntitiesList(IEnumerable<ConfirmationCode> list)
        {
            return list?.Select(FromEntity).Where(x => x != null).Cast<ConfirmationCodeModel>().ToList() ?? new List<ConfirmationCodeModel>();
        }

        public static List<ConfirmationCode> ToEntitiesList(IEnumerable<ConfirmationCodeModel> list)
        {
            return list?.Select(ToEntity).Where(x => x != null).Cast<ConfirmationCode>().ToList() ?? new List<ConfirmationCode>();
        }
    }
}
