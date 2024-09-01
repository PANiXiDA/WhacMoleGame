using Entities;

namespace PL.MVC.Infrastructure.Models
{
    public class SessionModel
    {
        public int Id { get; set; }
        public UserModel Player { get; set; } = new UserModel();
        public GameModel Game { get; set; } = new GameModel();
        public bool IsActive { get; set; }

        public static SessionModel FromEntity(Session obj)
        {
            return new SessionModel
            {
                Id = obj.Id,
                Player = UserModel.FromEntity(obj.Player),
                Game = GameModel.FromEntity(obj.Game),
                IsActive = obj.IsActive,
            };
        }

        public static Session ToEntity(SessionModel obj)
        {
            return new Session(
                obj.Id,
                UserModel.ToEntity(obj.Player),
                GameModel.ToEntity(obj.Game),
                obj.IsActive
                );
        }

        public static List<SessionModel> FromEntitiesList(IEnumerable<Session> list)
        {
            return list?.Select(FromEntity).Where(x => x != null).Cast<SessionModel>().ToList() ?? new List<SessionModel>();
        }

        public static List<Session> ToEntitiesList(IEnumerable<SessionModel> list)
        {
            return list?.Select(ToEntity).Where(x => x != null).Cast<Session>().ToList() ?? new List<Session>();
        }
    }
}
