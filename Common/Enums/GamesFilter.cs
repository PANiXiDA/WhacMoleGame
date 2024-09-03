using System.ComponentModel.DataAnnotations;

namespace Common.Enums
{
    public enum GamesFilter
    {
        [Display(Name = "All games")]
        All = 0,

        [Display(Name = "My games")]
        My = 1,

        [Display(Name = "Active")]
        Active = 2,

        [Display(Name = "Finished")]
        Finished = 3,
    }
}
