using Entities;
using InGame.IManagers;
using InGame.Models;

namespace InGame.Managers
{
    public class GameManager : IGameManager
    {
        public List<GameSession> GameSessions { get; private set; } = new List<GameSession>();

        private readonly object _lock = new object();
        private readonly Random _random = new Random();

        public void InitializeGame(Game game)
        {
            var gameSession = GameSessions.FirstOrDefault(session => session.Game.Id == game.Id);

            if (gameSession == null)
            {
                gameSession = new GameSession(game);

                var players = game.Sessions!.Select(s => s.Player).ToList();
                gameSession.Players = players;

                for (int i = 0; i < 5; i++)
                {
                    gameSession.Moles.Add(new Mole(Guid.NewGuid(), GetRandomTileId(gameSession)));
                    gameSession.Plants.Add(new Plant(Guid.NewGuid(), GetRandomTileId(gameSession)));
                }

                foreach (var player in players)
                {
                    gameSession.PlayerScores.TryAdd(player.Login, 0);
                }

                GameSessions.Add(gameSession);
            }
            else
            {
                var newPlayers = game.Sessions!
                    .Select(s => s.Player)
                    .Where(p => !gameSession.Players.Any(existingPlayer => existingPlayer.Id == p.Id))
                    .ToList();

                gameSession.Players.AddRange(newPlayers);

                foreach (var player in newPlayers)
                {
                    gameSession.PlayerScores.TryAdd(player.Login, 0);
                }
            }
        }

        public void UpdateGame()
        {
            lock (_lock)
            {
                foreach (var gameSession in GameSessions)
                {
                    foreach (var mole in gameSession.Moles)
                    {
                        mole.TileId = GetRandomTileId(gameSession);
                    }
                    foreach (var plant in gameSession.Plants)
                    {
                        plant.TileId = GetRandomTileId(gameSession);
                    }
                }
            }
        }

        public void PlayerMove(string playerLogin, int tileId, int gameId)
        {
            lock (_lock)
            {
                var gameSession = GameSessions.FirstOrDefault(session => session.Game.Id == gameId);
                if (gameSession == null)
                {
                    return;
                }
                var hitMole = gameSession.Moles.FirstOrDefault(m => m.TileId == tileId);
                if (hitMole != null)
                {
                    gameSession.PlayerScores.AddOrUpdate(playerLogin, 10, (id, oldScore) => oldScore + 10);
                }

                var hitPlant = gameSession.Plants.FirstOrDefault(p => p.TileId == tileId);
                if (hitPlant != null)
                {
                    gameSession.GameOver = true;
                }
            }
        }

        private int GetRandomTileId(GameSession gameSession)
        {
            int id;
            HashSet<int> usedTileIds = new HashSet<int>(gameSession.Moles.Select(m => m.TileId)
                .Concat(gameSession.Plants.Select(p => p.TileId)));

            do
            {
                id = _random.Next(0, 64);
            } while (usedTileIds.Contains(id));

            return id;
        }

        public GameState GetGameState(int gameId)
        {
            var gameSession = GameSessions.FirstOrDefault(session => session.Game.Id == gameId);

            if (gameSession != null)
            {
                return new GameState
                {
                    MolePositions = gameSession.Moles.Select(m => m.TileId).ToList(),
                    PlantPositions = gameSession.Plants.Select(p => p.TileId).ToList(),
                    PlayerScores = gameSession.PlayerScores.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                    GameOver = gameSession.GameOver
                };
            }
            else
            {
                return new GameState
                {
                    GameOver = true
                };
            }
        }

        public void RemoveGameSession(int gameId)
        {
            var gameSession = GameSessions.First(session => session.Game.Id == gameId);

            if (gameSession != null)
            {
                GameSessions.Remove(gameSession);
            }
        }
    }
}
