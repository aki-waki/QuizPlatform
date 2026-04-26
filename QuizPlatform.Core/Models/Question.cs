namespace QuizPlatform.Core.Models;

public enum Difficulty
{
    Easy = 1,
    Medium = 2,
    Hard = 3
}

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public Difficulty Difficulty { get; set; }
    public int CategoryId { get; set; }
    public int CreatedByPlayerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Category Category { get; set; } = null!;
    public Player CreatedByPlayer { get; set; } = null!;
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
}
