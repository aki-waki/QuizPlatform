using QuizPlatform.Core.Models;

namespace QuizPlatform.Core.Interfaces;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(int id);
    Task<Player?> GetByUsernameAsync(string username);
    Task<IEnumerable<Player>> GetAllAsync();
    Task AddAsync(Player player);
    Task SaveChangesAsync();
}

public interface IQuestionRepository
{
    Task<Question?> GetByIdAsync(int id);
    Task<IEnumerable<Question>> GetByCategoryAndDifficultyAsync(int categoryId, Difficulty difficulty);
    Task<IEnumerable<Question>> GetAllAsync();
    Task AddAsync(Question question);
    Task SaveChangesAsync();
}

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task AddAsync(Category category);
    Task SaveChangesAsync();
}

public interface IGameResultRepository
{
    Task AddAsync(GameResult result);
    Task<IEnumerable<GameResult>> GetByPlayerIdAsync(int playerId);
    Task<IEnumerable<GameResult>> GetTopScoresAsync(int count = 10);
    Task SaveChangesAsync();
}
