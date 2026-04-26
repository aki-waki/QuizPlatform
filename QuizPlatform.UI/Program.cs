using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizPlatform.Core.Interfaces;
using QuizPlatform.Core.Models;
using QuizPlatform.Data.Context;
using QuizPlatform.Data.Repositories;
using QuizPlatform.Services;

// ── DI setup ──────────────────────────────────────────────
var services = new ServiceCollection();
services.AddDbContext<QuizDbContext>(opt =>
    opt.UseSqlite("Data Source=quiz.db"));

services.AddScoped<IPlayerRepository, PlayerRepository>();
services.AddScoped<IQuestionRepository, QuestionRepository>();
services.AddScoped<ICategoryRepository, CategoryRepository>();
services.AddScoped<IGameResultRepository, GameResultRepository>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IQuizService, QuizService>();
services.AddScoped<ILeaderboardService, LeaderboardService>();

var provider = services.BuildServiceProvider();

// ── DB init ────────────────────────────────────────────────
using (var scope = provider.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<QuizDbContext>();
    ctx.Database.EnsureCreated();
}

// ── App state ──────────────────────────────────────────────
Player? currentPlayer = null;

// ── Entry ──────────────────────────────────────────────────
while (true)
{
    Console.Clear();
    PrintBanner();

    if (currentPlayer == null)
        await GuestMenuAsync();
    else
        await MainMenuAsync();
}

// ══════════════════════════════════════════════════════════
// MENUS
// ══════════════════════════════════════════════════════════

async Task GuestMenuAsync()
{
    Console.WriteLine("  [1] Register");
    Console.WriteLine("  [2] Login");
    Console.WriteLine("  [0] Exit");
    Console.Write("\n> ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1": await RegisterAsync(); break;
        case "2": await LoginAsync(); break;
        case "0": Environment.Exit(0); break;
    }
}

async Task MainMenuAsync()
{
    Console.WriteLine($"  Logged in as: {currentPlayer!.Username}");
    Console.WriteLine();
    Console.WriteLine("  [1] Play Quiz");
    Console.WriteLine("  [2] Add Question");
    Console.WriteLine("  [3] Leaderboard");
    Console.WriteLine("  [4] My History");
    Console.WriteLine("  [5] Logout");
    Console.WriteLine("  [0] Exit");
    Console.Write("\n> ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1": await PlayQuizAsync(); break;
        case "2": await AddQuestionAsync(); break;
        case "3": await ShowLeaderboardAsync(); break;
        case "4": await ShowHistoryAsync(); break;
        case "5": currentPlayer = null; break;
        case "0": Environment.Exit(0); break;
    }
}

// ══════════════════════════════════════════════════════════
// AUTH
// ══════════════════════════════════════════════════════════

async Task RegisterAsync()
{
    Console.Clear();
    PrintHeader("REGISTER");
    var username = Prompt("Username");
    var password = Prompt("Password");

    using var scope = provider.CreateScope();
    var auth = scope.ServiceProvider.GetRequiredService<IAuthService>();
    var player = await auth.RegisterAsync(username, password);

    if (player == null)
        Error("Username taken or invalid input.");
    else
    {
        currentPlayer = player;
        Success($"Welcome, {player.Username}!");
    }
    Pause();
}

async Task LoginAsync()
{
    Console.Clear();
    PrintHeader("LOGIN");
    var username = Prompt("Username");
    var password = Prompt("Password");

    using var scope = provider.CreateScope();
    var auth = scope.ServiceProvider.GetRequiredService<IAuthService>();
    var player = await auth.LoginAsync(username, password);

    if (player == null)
        Error("Invalid credentials.");
    else
    {
        currentPlayer = player;
        Success($"Welcome back, {player.Username}!");
    }
    Pause();
}

// ══════════════════════════════════════════════════════════
// QUIZ
// ══════════════════════════════════════════════════════════

async Task PlayQuizAsync()
{
    Console.Clear();
    PrintHeader("PLAY QUIZ");

    using var scope = provider.CreateScope();
    var catRepo = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
    var quiz = scope.ServiceProvider.GetRequiredService<IQuizService>();

    var categories = (await catRepo.GetAllAsync()).ToList();
    Console.WriteLine("  Categories:");
    foreach (var c in categories)
        Console.WriteLine($"    [{c.Id}] {c.Name}");

    Console.Write("\n  Choose category: ");
    if (!int.TryParse(Console.ReadLine(), out int catId) || categories.All(c => c.Id != catId))
    { Error("Invalid category."); Pause(); return; }

    Console.WriteLine("\n  Difficulty: [1] Easy  [2] Medium  [3] Hard");
    Console.Write("  Choose: ");
    if (!int.TryParse(Console.ReadLine(), out int diffInput) || diffInput < 1 || diffInput > 3)
    { Error("Invalid difficulty."); Pause(); return; }

    var difficulty = (Difficulty)diffInput;
    var questions = (await quiz.GetQuestionsAsync(catId, difficulty, 5)).ToList();

    if (questions.Count == 0)
    { Error("No questions available for this selection."); Pause(); return; }

    int score = 0;

    for (int i = 0; i < questions.Count; i++)
    {
        Console.Clear();
        PrintHeader($"QUESTION {i + 1} / {questions.Count}");
        var q = questions[i];
        Console.WriteLine($"  {q.Text}");
        Console.WriteLine();

        var answers = q.Answers.OrderBy(_ => Guid.NewGuid()).ToList();
        for (int j = 0; j < answers.Count; j++)
            Console.WriteLine($"    [{j + 1}] {answers[j].Text}");

        Console.Write("\n  Your answer: ");
        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > answers.Count)
        { Console.WriteLine("  Skipped."); Pause(); continue; }

        var selected = answers[choice - 1];
        if (selected.IsCorrect)
        { score++; Success("Correct!"); }
        else
        {
            Error($"Wrong. Correct: {answers.First(a => a.IsCorrect).Text}");
        }
        Pause();
    }

    // Save result
    await quiz.SubmitResultAsync(currentPlayer!.Id, catId, difficulty, score, questions.Count);

    Console.Clear();
    PrintHeader("RESULT");
    Console.WriteLine($"  Score: {score} / {questions.Count}  ({score * 100 / questions.Count}%)");
    Console.WriteLine();
    if (score == questions.Count) Success("Perfect score!");
    else if (score >= questions.Count / 2) Success("Good job!");
    else Error("Better luck next time.");
    Pause();
}

// ══════════════════════════════════════════════════════════
// ADD QUESTION
// ══════════════════════════════════════════════════════════

async Task AddQuestionAsync()
{
    Console.Clear();
    PrintHeader("ADD QUESTION");

    using var scope = provider.CreateScope();
    var catRepo = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
    var qRepo = scope.ServiceProvider.GetRequiredService<IQuestionRepository>();

    var categories = (await catRepo.GetAllAsync()).ToList();
    foreach (var c in categories)
        Console.WriteLine($"  [{c.Id}] {c.Name}");

    Console.Write("\n  Category: ");
    if (!int.TryParse(Console.ReadLine(), out int catId) || categories.All(c => c.Id != catId))
    { Error("Invalid."); Pause(); return; }

    Console.WriteLine("  Difficulty: [1] Easy  [2] Medium  [3] Hard");
    Console.Write("  Choose: ");
    if (!int.TryParse(Console.ReadLine(), out int diffInput) || diffInput < 1 || diffInput > 3)
    { Error("Invalid."); Pause(); return; }

    Console.Write("  Question text: ");
    var text = Console.ReadLine()?.Trim();
    if (string.IsNullOrEmpty(text)) { Error("Empty text."); Pause(); return; }

    var answers = new List<Answer>();
    Console.WriteLine("  Enter 4 answers. Mark the correct one with *");
    bool hasCorrect = false;

    for (int i = 1; i <= 4; i++)
    {
        Console.Write($"  Answer {i} (prefix * for correct): ");
        var line = Console.ReadLine()?.Trim() ?? "";
        bool isCorrect = line.StartsWith("*");
        var answerText = isCorrect ? line[1..].Trim() : line;
        if (isCorrect) hasCorrect = true;
        answers.Add(new Answer { Text = answerText, IsCorrect = isCorrect });
    }

    if (!hasCorrect) { Error("No correct answer marked."); Pause(); return; }

    var question = new Question
    {
        Text = text,
        Difficulty = (Difficulty)diffInput,
        CategoryId = catId,
        CreatedByPlayerId = currentPlayer!.Id,
        CreatedAt = DateTime.UtcNow,
        Answers = answers
    };

    await qRepo.AddAsync(question);
    await qRepo.SaveChangesAsync();
    Success("Question added!");
    Pause();
}

// ══════════════════════════════════════════════════════════
// LEADERBOARD
// ══════════════════════════════════════════════════════════

async Task ShowLeaderboardAsync()
{
    Console.Clear();
    PrintHeader("LEADERBOARD — TOP 10");

    using var scope = provider.CreateScope();
    var lb = scope.ServiceProvider.GetRequiredService<ILeaderboardService>();
    var top = (await lb.GetTopScoresAsync(10)).ToList();

    if (top.Count == 0) { Console.WriteLine("  No results yet."); Pause(); return; }

    Console.WriteLine($"  {"#",-4} {"Player",-20} {"Category",-20} {"Score",-10} {"Date"}");
    Console.WriteLine(new string('─', 70));

    for (int i = 0; i < top.Count; i++)
    {
        var r = top[i];
        Console.WriteLine($"  {i + 1,-4} {r.Player.Username,-20} {r.Category.Name,-20} {r.Score + "/" + r.TotalQuestions,-10} {r.PlayedAt:yyyy-MM-dd}");
    }
    Pause();
}

// ══════════════════════════════════════════════════════════
// HISTORY
// ══════════════════════════════════════════════════════════

async Task ShowHistoryAsync()
{
    Console.Clear();
    PrintHeader("MY HISTORY");

    using var scope = provider.CreateScope();
    var lb = scope.ServiceProvider.GetRequiredService<ILeaderboardService>();
    var history = (await lb.GetPlayerHistoryAsync(currentPlayer!.Id)).ToList();

    if (history.Count == 0) { Console.WriteLine("  No games played yet."); Pause(); return; }

    Console.WriteLine($"  {"Category",-20} {"Difficulty",-12} {"Score",-10} {"Date"}");
    Console.WriteLine(new string('─', 60));

    foreach (var r in history)
        Console.WriteLine($"  {r.Category.Name,-20} {r.Difficulty,-12} {r.Score + "/" + r.TotalQuestions,-10} {r.PlayedAt:yyyy-MM-dd HH:mm}");

    Pause();
}

// ══════════════════════════════════════════════════════════
// HELPERS
// ══════════════════════════════════════════════════════════

void PrintBanner()
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╔══════════════════════════════════╗");
    Console.WriteLine("║        QUIZ PLATFORM  v1.0       ║");
    Console.WriteLine("╚══════════════════════════════════╝");
    Console.ResetColor();
    Console.WriteLine();
}

void PrintHeader(string title)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"  ── {title} ──");
    Console.ResetColor();
    Console.WriteLine();
}

string Prompt(string label)
{
    Console.Write($"  {label}: ");
    return Console.ReadLine()?.Trim() ?? string.Empty;
}

void Success(string msg)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"  ✓ {msg}");
    Console.ResetColor();
}

void Error(string msg)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"  ✗ {msg}");
    Console.ResetColor();
}

void Pause()
{
    Console.WriteLine();
    Console.Write("  Press Enter to continue...");
    Console.ReadLine();
}
