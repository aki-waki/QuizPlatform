using QuizPlatform.Core.Models;

namespace QuizPlatform.Core.Interfaces;

public interface IAuthService
{
    Task<Player?> RegisterAsync(string username, string password);
    Task<Player?> LoginAsync(string username, string password);
}

public interface IQuizService
{
    Task<IEnumerable<Question>> GetQuestionsAsync(int categoryId, Difficulty difficulty, int count = 10);
    Task<GameResult> SubmitResultAsync(int playerId, int categoryId, Difficulty difficulty, int score, int total);
}

public interface ILeaderboardService
{
    Task<IEnumerable<GameResult>> GetTopScoresAsync(int count = 10);
    Task<IEnumerable<GameResult>> GetPlayerHistoryAsync(int playerId);
}
