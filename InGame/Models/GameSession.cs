using Entities;
using System.Collections.Concurrent;

namespace InGame.Models
{
    public class GameSession
    {
        public Game Game { get; set; }
        public bool GameOver { get; set; } = false;
        public List<User> Players { get; set; } = new List<User>();
        public List<Mole> Moles { get; set; } = new List<Mole>();
        public List<Plant> Plants { get; set; } = new List<Plant>();
        public ConcurrentDictionary<string, int> PlayerScores { get; set; } = new ConcurrentDictionary<string, int>();

        public GameSession (Game game)
        {
            Game = game;
        }
    }
}
