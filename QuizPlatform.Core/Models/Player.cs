namespace QuizPlatform.Core.Models;

public class Player
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<GameResult> GameResults { get; set; } = new List<GameResult>();
    public ICollection<Question> CreatedQuestions { get; set; } = new List<Question>();
}
