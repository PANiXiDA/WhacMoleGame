using Entities;
using InGame.IManagers;
using InGame.Models;
using System.Collections.Concurrent;

namespace InGame.Managers
{
    public class GameManager : IGameManager
    {
        public bool GameOver { get; private set; }
        public Game? CurrentGame { get; private set; }
        public List<User> Players { get; private set; } = new List<User>();
        public Mole? CurrMole { get; private set; }
        public Plant? CurrPlant { get; private set; }
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
            }

            CurrMole = new Mole(GenerateId(), Players.First().Id, GetRandomTileId());
            CurrPlant = new Plant(GenerateId(), GetRandomTileId());
            GameOver = false;
        }

        public void UpdateGame()
        {
            if (GameOver) return;

            lock (_lock)
            {
                if (CurrMole != null && CurrPlant != null)
                {
                    CurrMole.TileId = GetRandomTileId();
                    CurrPlant.TileId = GetRandomTileId();
                }
            }
        }

        public void PlayerMove(string playerLogin, int tileId)
        {
            lock (_lock)
            {
                if (CurrMole != null && CurrMole.TileId == tileId)
                {
                    PlayerScores.AddOrUpdate(playerLogin, 10, (id, oldScore) => oldScore + 10);
                }
                else if (CurrPlant != null && CurrPlant.TileId == tileId)
                {
                    GameOver = true;
                }

                UpdateGame();
            }
        }

        private int GetRandomTileId()
        {
            var id = _random.Next(0, 64);
            while (CurrPlant?.Id == id || CurrMole?.Id == id)
            {
                id = _random.Next(0, 64);
            }
            return id;
        }

        private int GenerateId()
        {
            return _random.Next(1000, 9999);
        }

        public GameState GetGameState()
        {
            return new GameState
            {
                MolePosition = CurrMole?.TileId ?? -1,
                PlantPosition = CurrPlant?.TileId ?? -1,
                PlayerScores = PlayerScores.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                GameOver = GameOver
            };
        }
    }
}
