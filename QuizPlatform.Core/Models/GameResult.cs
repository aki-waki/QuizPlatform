namespace QuizPlatform.Core.Models;

public class GameResult
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public int CategoryId { get; set; }
    public Difficulty Difficulty { get; set; }
    public int Score { get; set; }
    public int TotalQuestions { get; set; }
    public DateTime PlayedAt { get; set; } = DateTime.UtcNow;

    public Player Player { get; set; } = null!;
    public Category Category { get; set; } = null!;

    public double Percentage => TotalQuestions == 0 ? 0 : (double)Score / TotalQuestions * 100;
}
