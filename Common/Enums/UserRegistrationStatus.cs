using System.ComponentModel.DataAnnotations;

namespace Common.Enums
{
    public enum UserRegistrationStatus
    {
        [Display(Name = "Не подтвержден")]
        Unconfirmed = 0,
        [Display(Name = "Подтвержден")]
        Confirmed = 1
    }
}
