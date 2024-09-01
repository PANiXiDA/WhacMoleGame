using Common.Enums;
using Entities;

namespace PL.MVC.Infrastructure.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime GameStartTime { get; set; }
        public DateTime? GameEndTime { get; set; }
        public UserModel? Winner { get; set; }
        public int? MaxPointsCount { get; set; }
        public bool IsActive { get; set; }

        public List<SessionModel> Sessions { get; set; } = new List<SessionModel>();

        public static GameModel FromEntity(Game obj)
        {
            return new GameModel
            {
                Id = obj.Id,
                Name = obj.Name,
                GameStartTime = obj.GameStartTime,
                GameEndTime = obj.GameEndTime,
                Winner = obj.Winner == null ? null : UserModel.FromEntity(obj.Winner),
                MaxPointsCount = obj.MaxPointsCount,
                IsActive = obj.IsActive,
                Sessions = obj.Sessions == null? new List<SessionModel>() : SessionModel.FromEntitiesList(obj.Sessions)
            };
        }

        public static Game ToEntity(GameModel obj)
        {
            return new Game(
                obj.Id,
                obj.Name,
                obj.GameStartTime,
                obj.GameEndTime,
                obj.Winner == null ? null : UserModel.ToEntity(obj.Winner),
                obj.MaxPointsCount,
                obj.IsActive,
                SessionModel.ToEntitiesList(obj.Sessions));
        }

        public static List<GameModel> FromEntitiesList(IEnumerable<Game> list)
        {
            return list?.Select(FromEntity).Where(x => x != null).Cast<GameModel>().ToList() ?? new List<GameModel>();
        }

        public static List<Game> ToEntitiesList(IEnumerable<GameModel> list)
        {
            return list?.Select(ToEntity).Where(x => x != null).Cast<Game>().ToList() ?? new List<Game>();
        }
    }
}
