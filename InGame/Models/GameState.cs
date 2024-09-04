namespace InGame.Models
{
    public class GameState
    {
        public List<int> MolePositions { get; set; } = new List<int>();
        public List<int> PlantPositions { get; set; } = new List<int>();
        public Dictionary<string, int> PlayerScores { get; set; } = new Dictionary<string, int>();
        public bool GameOver { get; set; }
    }
}
