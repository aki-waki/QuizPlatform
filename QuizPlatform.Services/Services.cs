using System.Security.Cryptography;
using System.Text;
using QuizPlatform.Core.Interfaces;
using QuizPlatform.Core.Models;

namespace QuizPlatform.Services;

public class AuthService : IAuthService
{
    private readonly IPlayerRepository _players;

    public AuthService(IPlayerRepository players) => _players = players;

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    public async Task<Player?> RegisterAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return null;

        var existing = await _players.GetByUsernameAsync(username);
        if (existing != null)
            return null;

        var player = new Player
        {
            Username = username.Trim(),
            PasswordHash = HashPassword(password),
            CreatedAt = DateTime.UtcNow
        };

        await _players.AddAsync(player);
        await _players.SaveChangesAsync();
        return player;
    }

    public async Task<Player?> LoginAsync(string username, string password)
    {
        var player = await _players.GetByUsernameAsync(username.Trim());
        if (player == null) return null;

        return player.PasswordHash == HashPassword(password) ? player : null;
    }
}

public class QuizService : IQuizService
{
    private readonly IQuestionRepository _questions;
    private readonly IGameResultRepository _results;

    public QuizService(IQuestionRepository questions, IGameResultRepository results)
    {
        _questions = questions;
        _results = results;
    }

    public async Task<IEnumerable<Question>> GetQuestionsAsync(int categoryId, Difficulty difficulty, int count = 10)
    {
        var all = await _questions.GetByCategoryAndDifficultyAsync(categoryId, difficulty);
        return all.OrderBy(_ => Guid.NewGuid()).Take(count);
    }

    public async Task<GameResult> SubmitResultAsync(int playerId, int categoryId, Difficulty difficulty, int score, int total)
    {
        var result = new GameResult
        {
            PlayerId = playerId,
            CategoryId = categoryId,
            Difficulty = difficulty,
            Score = score,
            TotalQuestions = total,
            PlayedAt = DateTime.UtcNow
        };

        await _results.AddAsync(result);
        await _results.SaveChangesAsync();
        return result;
    }
}

public class LeaderboardService : ILeaderboardService
{
    private readonly IGameResultRepository _results;

    public LeaderboardService(IGameResultRepository results) => _results = results;

    public Task<IEnumerable<GameResult>> GetTopScoresAsync(int count = 10) =>
        _results.GetTopScoresAsync(count);

    public Task<IEnumerable<GameResult>> GetPlayerHistoryAsync(int playerId) =>
        _results.GetByPlayerIdAsync(playerId);
}
