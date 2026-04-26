using Microsoft.EntityFrameworkCore;
using QuizPlatform.Core.Models;

namespace QuizPlatform.Data.Context;

public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }

    public DbSet<Player> Players => Set<Player>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<GameResult> GameResults => Set<GameResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(e =>
        {
            e.HasIndex(p => p.Username).IsUnique();
        });

        modelBuilder.Entity<Question>(e =>
        {
            e.HasOne(q => q.Category).WithMany(c => c.Questions).HasForeignKey(q => q.CategoryId);
            e.HasOne(q => q.CreatedByPlayer).WithMany(p => p.CreatedQuestions).HasForeignKey(q => q.CreatedByPlayerId);
        });

        modelBuilder.Entity<Answer>(e =>
        {
            e.HasOne(a => a.Question).WithMany(q => q.Answers).HasForeignKey(a => a.QuestionId);
        });

        modelBuilder.Entity<GameResult>(e =>
        {
            e.HasOne(g => g.Player).WithMany(p => p.GameResults).HasForeignKey(g => g.PlayerId);
            e.HasOne(g => g.Category).WithMany().HasForeignKey(g => g.CategoryId);
        });

        // Seed categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "General Knowledge" },
            new Category { Id = 2, Name = "Science" },
            new Category { Id = 3, Name = "History" },
            new Category { Id = 4, Name = "Technology" }
        );

        // Seed a system player for seeded questions
        modelBuilder.Entity<Player>().HasData(
            new Player { Id = 1, Username = "admin", PasswordHash = "admin_seed", CreatedAt = DateTime.UtcNow }
        );

        // Seed some starter questions
        modelBuilder.Entity<Question>().HasData(
            new Question { Id = 1, Text = "What is the capital of France?", Difficulty = Difficulty.Easy, CategoryId = 1, CreatedByPlayerId = 1, CreatedAt = DateTime.UtcNow },
            new Question { Id = 2, Text = "What is 2 + 2?", Difficulty = Difficulty.Easy, CategoryId = 1, CreatedByPlayerId = 1, CreatedAt = DateTime.UtcNow },
            new Question { Id = 3, Text = "What is the chemical symbol for water?", Difficulty = Difficulty.Easy, CategoryId = 2, CreatedByPlayerId = 1, CreatedAt = DateTime.UtcNow },
            new Question { Id = 4, Text = "What planet is closest to the Sun?", Difficulty = Difficulty.Medium, CategoryId = 2, CreatedByPlayerId = 1, CreatedAt = DateTime.UtcNow },
            new Question { Id = 5, Text = "In what year did World War II end?", Difficulty = Difficulty.Medium, CategoryId = 3, CreatedByPlayerId = 1, CreatedAt = DateTime.UtcNow },
            new Question { Id = 6, Text = "Who invented the telephone?", Difficulty = Difficulty.Medium, CategoryId = 3, CreatedByPlayerId = 1, CreatedAt = DateTime.UtcNow },
            new Question { Id = 7, Text = "What does CPU stand for?", Difficulty = Difficulty.Easy, CategoryId = 4, CreatedByPlayerId = 1, CreatedAt = DateTime.UtcNow },
            new Question { Id = 8, Text = "What does HTTP stand for?", Difficulty = Difficulty.Medium, CategoryId = 4, CreatedByPlayerId = 1, CreatedAt = DateTime.UtcNow }
        );

        modelBuilder.Entity<Answer>().HasData(
            // Q1: Capital of France
            new Answer { Id = 1, Text = "Paris", IsCorrect = true, QuestionId = 1 },
            new Answer { Id = 2, Text = "London", IsCorrect = false, QuestionId = 1 },
            new Answer { Id = 3, Text = "Berlin", IsCorrect = false, QuestionId = 1 },
            new Answer { Id = 4, Text = "Madrid", IsCorrect = false, QuestionId = 1 },
            // Q2: 2+2
            new Answer { Id = 5, Text = "4", IsCorrect = true, QuestionId = 2 },
            new Answer { Id = 6, Text = "3", IsCorrect = false, QuestionId = 2 },
            new Answer { Id = 7, Text = "5", IsCorrect = false, QuestionId = 2 },
            new Answer { Id = 8, Text = "22", IsCorrect = false, QuestionId = 2 },
            // Q3: H2O
            new Answer { Id = 9, Text = "H2O", IsCorrect = true, QuestionId = 3 },
            new Answer { Id = 10, Text = "CO2", IsCorrect = false, QuestionId = 3 },
            new Answer { Id = 11, Text = "O2", IsCorrect = false, QuestionId = 3 },
            new Answer { Id = 12, Text = "NaCl", IsCorrect = false, QuestionId = 3 },
            // Q4: Mercury
            new Answer { Id = 13, Text = "Mercury", IsCorrect = true, QuestionId = 4 },
            new Answer { Id = 14, Text = "Venus", IsCorrect = false, QuestionId = 4 },
            new Answer { Id = 15, Text = "Earth", IsCorrect = false, QuestionId = 4 },
            new Answer { Id = 16, Text = "Mars", IsCorrect = false, QuestionId = 4 },
            // Q5: WW2 1945
            new Answer { Id = 17, Text = "1945", IsCorrect = true, QuestionId = 5 },
            new Answer { Id = 18, Text = "1939", IsCorrect = false, QuestionId = 5 },
            new Answer { Id = 19, Text = "1941", IsCorrect = false, QuestionId = 5 },
            new Answer { Id = 20, Text = "1918", IsCorrect = false, QuestionId = 5 },
            // Q6: Telephone
            new Answer { Id = 21, Text = "Alexander Graham Bell", IsCorrect = true, QuestionId = 6 },
            new Answer { Id = 22, Text = "Thomas Edison", IsCorrect = false, QuestionId = 6 },
            new Answer { Id = 23, Text = "Nikola Tesla", IsCorrect = false, QuestionId = 6 },
            new Answer { Id = 24, Text = "Benjamin Franklin", IsCorrect = false, QuestionId = 6 },
            // Q7: CPU
            new Answer { Id = 25, Text = "Central Processing Unit", IsCorrect = true, QuestionId = 7 },
            new Answer { Id = 26, Text = "Core Processing Unit", IsCorrect = false, QuestionId = 7 },
            new Answer { Id = 27, Text = "Computer Processing Unit", IsCorrect = false, QuestionId = 7 },
            new Answer { Id = 28, Text = "Central Program Unit", IsCorrect = false, QuestionId = 7 },
            // Q8: HTTP
            new Answer { Id = 29, Text = "HyperText Transfer Protocol", IsCorrect = true, QuestionId = 8 },
            new Answer { Id = 30, Text = "HyperText Transmission Protocol", IsCorrect = false, QuestionId = 8 },
            new Answer { Id = 31, Text = "High Transfer Text Protocol", IsCorrect = false, QuestionId = 8 },
            new Answer { Id = 32, Text = "HyperText Transport Protocol", IsCorrect = false, QuestionId = 8 }
        );
    }
}
