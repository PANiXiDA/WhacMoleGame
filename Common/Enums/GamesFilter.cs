using System.ComponentModel.DataAnnotations;

namespace Common.Enums
{
    public enum GamesFilter
    {
        [Display(Name = "Все игры")]
        All = 0,

        [Display(Name = "Мои игры")]
        My = 1,

        [Display(Name = "Активные")]
        Active = 2,

        [Display(Name = "Завершённые")]
        Finished = 3,
    }
}
