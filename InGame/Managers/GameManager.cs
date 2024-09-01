using Entities;
using InGame.IManagers;
using InGame.Models;
using System.Collections.Concurrent;

namespace InGame.Managers
{
    public class GameManager : IGameManager
    {
        public bool GameOver { get; private set; }
        public Game CurrentGame { get; private set; }
        public List<User> Players { get; private set; } = new List<User>();
        public Mole CurrMole { get; private set; }
        public Plant CurrPlant { get; private set; }

        private readonly ConcurrentDictionary<int, int> PlayerScores = new ConcurrentDictionary<int, int>();
        private readonly object _lock = new object();
        private readonly Random _random = new Random();

        public GameManager() { }

        public void InitializeGame(Game game)
        {
            CurrentGame = game;
            Players = game.Sessions.Select(s => s.Player).ToList();

            // Инициализация крота и растения на случайных позициях
            CurrMole = new Mole(GenerateId(), Players.First().Id, GetRandomTileId());
            CurrPlant = new Plant(GenerateId(), GetRandomTileId());
            GameOver = false;
        }

        public void UpdateGame()
        {
            if (GameOver) return;

            lock (_lock)
            {
                // Перемещение крота и растения на новые позиции
                CurrMole.TileId = GetRandomTileId();
                CurrPlant.TileId = GetRandomTileId();

                // Проверка столкновений между кротом и растением
                if (CurrMole.TileId == CurrPlant.TileId)
                {
                    GameOver = true; // Игра заканчивается, если крот и растение на одной плитке
                }
            }
        }

        public void PlayerMove(int playerId, int tileId)
        {
            lock (_lock)
            {
                if (GameOver) return;

                // Проверка: игрок нажал на крота?
                if (CurrMole != null && CurrMole.TileId == tileId)
                {
                    // Увеличиваем очки игрока
                    PlayerScores.AddOrUpdate(playerId, 10, (id, oldScore) => oldScore + 10);
                }
                else if (CurrPlant != null && CurrPlant.TileId == tileId)
                {
                    // Игрок нажал на растение, игра заканчивается
                    GameOver = true;
                }

                // Перемещаем крота и растение на новые позиции
                MoveMoleAndPlant();
            }
        }

        private void MoveMoleAndPlant()
        {
            // Перемещение крота и растения на новые плитки
            CurrMole.TileId = GetRandomTileId();
            CurrPlant.TileId = GetRandomTileId();
        }

        private int GetRandomTileId()
        {
            // Логика получения случайного ID плитки (от 0 до 63)
            return _random.Next(0, 64);
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
