namespace InGame.Models
{
    public class GameState
    {
        public int MolePosition { get; set; }
        public int PlantPosition { get; set; }
        public Dictionary<string, int> PlayerScores { get; set; } = new Dictionary<string, int>();
        public bool GameOver { get; set; }
    }
}
