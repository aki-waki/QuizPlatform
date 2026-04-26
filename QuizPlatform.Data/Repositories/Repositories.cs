using Microsoft.EntityFrameworkCore;
using QuizPlatform.Core.Interfaces;
using QuizPlatform.Core.Models;
using QuizPlatform.Data.Context;

namespace QuizPlatform.Data.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly QuizDbContext _ctx;
    public PlayerRepository(QuizDbContext ctx) => _ctx = ctx;

    public async Task<Player?> GetByIdAsync(int id) =>
        await _ctx.Players.FindAsync(id);

    public async Task<Player?> GetByUsernameAsync(string username) =>
        await _ctx.Players.FirstOrDefaultAsync(p => p.Username == username);

    public async Task<IEnumerable<Player>> GetAllAsync() =>
        await _ctx.Players.ToListAsync();

    public async Task AddAsync(Player player) => await _ctx.Players.AddAsync(player);

    public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();
}

public class QuestionRepository : IQuestionRepository
{
    private readonly QuizDbContext _ctx;
    public QuestionRepository(QuizDbContext ctx) => _ctx = ctx;

    public async Task<Question?> GetByIdAsync(int id) =>
        await _ctx.Questions.Include(q => q.Answers).Include(q => q.Category).FirstOrDefaultAsync(q => q.Id == id);

    public async Task<IEnumerable<Question>> GetByCategoryAndDifficultyAsync(int categoryId, Difficulty difficulty) =>
        await _ctx.Questions
            .Include(q => q.Answers)
            .Where(q => q.CategoryId == categoryId && q.Difficulty == difficulty)
            .ToListAsync();

    public async Task<IEnumerable<Question>> GetAllAsync() =>
        await _ctx.Questions.Include(q => q.Answers).Include(q => q.Category).ToListAsync();

    public async Task AddAsync(Question question) => await _ctx.Questions.AddAsync(question);

    public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();
}

public class CategoryRepository : ICategoryRepository
{
    private readonly QuizDbContext _ctx;
    public CategoryRepository(QuizDbContext ctx) => _ctx = ctx;

    public async Task<Category?> GetByIdAsync(int id) =>
        await _ctx.Categories.FindAsync(id);

    public async Task<IEnumerable<Category>> GetAllAsync() =>
        await _ctx.Categories.ToListAsync();

    public async Task AddAsync(Category category) => await _ctx.Categories.AddAsync(category);

    public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();
}

public class GameResultRepository : IGameResultRepository
{
    private readonly QuizDbContext _ctx;
    public GameResultRepository(QuizDbContext ctx) => _ctx = ctx;

    public async Task AddAsync(GameResult result) => await _ctx.GameResults.AddAsync(result);

    public async Task<IEnumerable<GameResult>> GetByPlayerIdAsync(int playerId) =>
        await _ctx.GameResults
            .Include(g => g.Category)
            .Where(g => g.PlayerId == playerId)
            .OrderByDescending(g => g.PlayedAt)
            .ToListAsync();

    public async Task<IEnumerable<GameResult>> GetTopScoresAsync(int count = 10) =>
        await _ctx.GameResults
            .Include(g => g.Player)
            .Include(g => g.Category)
            .OrderByDescending(g => g.Score)
            .Take(count)
            .ToListAsync();

    public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();
}
