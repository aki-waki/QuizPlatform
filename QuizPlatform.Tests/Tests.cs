using Moq;
using QuizPlatform.Core.Interfaces;
using QuizPlatform.Core.Models;
using QuizPlatform.Services;

namespace QuizPlatform.Tests;

public class AuthServiceTests
{
    private Mock<IPlayerRepository> MakeRepo(Player? existing = null)
    {
        var repo = new Mock<IPlayerRepository>();
        repo.Setup(r => r.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(existing);
        repo.Setup(r => r.AddAsync(It.IsAny<Player>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        return repo;
    }

    [Fact]
    public async Task Register_WithNewUsername_ReturnsPlayer()
    {
        var repo = MakeRepo(null);
        var svc = new AuthService(repo.Object);

        var result = await svc.RegisterAsync("testuser", "password123");

        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
    }

    [Fact]
    public async Task Register_WithExistingUsername_ReturnsNull()
    {
        var existing = new Player { Username = "taken" };
        var repo = MakeRepo(existing);
        var svc = new AuthService(repo.Object);

        var result = await svc.RegisterAsync("taken", "password");

        Assert.Null(result);
    }

    [Fact]
    public async Task Register_WithEmptyUsername_ReturnsNull()
    {
        var repo = MakeRepo();
        var svc = new AuthService(repo.Object);

        var result = await svc.RegisterAsync("", "password");

        Assert.Null(result);
    }

    [Fact]
    public async Task Login_WithCorrectCredentials_ReturnsPlayer()
    {
        // Hash for "secret": pre-compute expected hash
        var hash = Convert.ToHexString(
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes("secret")));

        var player = new Player { Username = "user", PasswordHash = hash };
        var repo = MakeRepo(player);
        var svc = new AuthService(repo.Object);

        var result = await svc.LoginAsync("user", "secret");

        Assert.NotNull(result);
        Assert.Equal("user", result.Username);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsNull()
    {
        var player = new Player { Username = "user", PasswordHash = "wronghash" };
        var repo = MakeRepo(player);
        var svc = new AuthService(repo.Object);

        var result = await svc.LoginAsync("user", "wrongpassword");

        Assert.Null(result);
    }
}

public class QuizServiceTests
{
    [Fact]
    public async Task GetQuestions_ReturnsAtMostRequestedCount()
    {
        var questions = Enumerable.Range(1, 10).Select(i => new Question
        {
            Id = i,
            Text = $"Question {i}",
            Difficulty = Difficulty.Easy,
            CategoryId = 1,
            CreatedByPlayerId = 1,
            Answers = new List<Answer> { new() { Text = "A", IsCorrect = true } }
        }).ToList();

        var qRepo = new Mock<IQuestionRepository>();
        qRepo.Setup(r => r.GetByCategoryAndDifficultyAsync(1, Difficulty.Easy))
             .ReturnsAsync(questions);

        var rRepo = new Mock<IGameResultRepository>();
        var svc = new QuizService(qRepo.Object, rRepo.Object);

        var result = (await svc.GetQuestionsAsync(1, Difficulty.Easy, 5)).ToList();

        Assert.Equal(5, result.Count);
    }

    [Fact]
    public async Task SubmitResult_SavesCorrectly()
    {
        var qRepo = new Mock<IQuestionRepository>();
        var rRepo = new Mock<IGameResultRepository>();

        GameResult? saved = null;
        rRepo.Setup(r => r.AddAsync(It.IsAny<GameResult>()))
             .Callback<GameResult>(r => saved = r)
             .Returns(Task.CompletedTask);
        rRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var svc = new QuizService(qRepo.Object, rRepo.Object);
        await svc.SubmitResultAsync(1, 1, Difficulty.Medium, 4, 5);

        Assert.NotNull(saved);
        Assert.Equal(4, saved!.Score);
        Assert.Equal(5, saved.TotalQuestions);
    }
}

public class GameResultTests
{
    [Fact]
    public void Percentage_CalculatesCorrectly()
    {
        var result = new GameResult { Score = 3, TotalQuestions = 5 };
        Assert.Equal(60.0, result.Percentage);
    }

    [Fact]
    public void Percentage_WhenZeroTotal_ReturnsZero()
    {
        var result = new GameResult { Score = 0, TotalQuestions = 0 };
        Assert.Equal(0.0, result.Percentage);
    }
}
