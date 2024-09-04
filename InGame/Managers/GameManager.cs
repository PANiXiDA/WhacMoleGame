using Entities;
using InGame.IManagers;
using InGame.Models;
using System.Collections.Concurrent;

namespace InGame.Managers
{
    public class GameManager : IGameManager
    {
        public Game? CurrentGame { get; set; }
        public bool GameOver { get; private set; }
        public List<User> Players { get; set; } = new List<User>();
        public List<Mole> Moles { get; set; } = new List<Mole>();
        public List<Plant> Plants { get; set; } = new List<Plant>();
        public ConcurrentDictionary<string, int> PlayerScores { get; set; } = new ConcurrentDictionary<string, int>();

        private readonly object _lock = new object();
        private readonly Random _random = new Random();

        public void InitializeGame(Game game)
        {
            CurrentGame = game;
            Players = game.Sessions!.Select(s => s.Player).ToList();

            foreach (var player in Players)
            {
                PlayerScores.TryAdd(player.Login, 0);
                Moles.Add(new Mole(Guid.NewGuid(), Players.First().Id, GetRandomTileId()));
                Plants.Add(new Plant(Guid.NewGuid(), GetRandomTileId()));
            }

            GameOver = false;
        }

        public void UpdateGame()
        {
            if (GameOver) return;

            lock (_lock)
            {
                foreach (var mole in Moles)
                {
                    mole.TileId = GetRandomTileId();
                }

                foreach (var plant in Plants)
                {
                    plant.TileId = GetRandomTileId();
                }
            }
        }

        public void PlayerMove(string playerLogin, int tileId)
        {
            lock (_lock)
            {
                var hitMole = Moles.FirstOrDefault(m => m.TileId == tileId);
                if (hitMole != null)
                {
                    PlayerScores.AddOrUpdate(playerLogin, 10, (id, oldScore) => oldScore + 10);
                }

                var hitPlant = Plants.FirstOrDefault(p => p.TileId == tileId);
                if (hitPlant != null)
                {
                    GameOver = true;
                }

                UpdateGame();
            }
        }

        private int GetRandomTileId()
        {
            int id;
            HashSet<int> usedTileIds = new HashSet<int>(Moles.Select(m => m.TileId).Concat(Plants.Select(p => p.TileId)));

            do
            {
                id = _random.Next(0, 64);
            } while (usedTileIds.Contains(id));

            return id;
        }

        public GameState GetGameState()
        {
            return new GameState
            {
                MolePositions = Moles.Select(m => m.TileId).ToList(),
                PlantPositions = Plants.Select(p => p.TileId).ToList(),
                PlayerScores = PlayerScores.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                GameOver = GameOver
            };
        }
    }
}
