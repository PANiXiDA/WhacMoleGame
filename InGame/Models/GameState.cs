namespace InGame.Models
{
    public class GameState
    {
        public int MolePosition { get; set; }
        public int PlantPosition { get; set; }
        public Dictionary<int, int> PlayerScores { get; set; } = new Dictionary<int, int>();
        public bool GameOver { get; set; }
    }
}
