# Quiz Platform

A console-based quiz game built with .NET 8 / C# as a university lab project.

## Features

- User registration & login (SHA-256 password hashing)
- Quiz game with categories and difficulty levels (Easy / Medium / Hard)
- User-created questions
- Game result history per player
- Global leaderboard (top 10)
- SQLite database via Entity Framework Core
- Unit tests with xUnit + Moq

## Architecture

```
QuizPlatform.Core      — Models, Interfaces (no dependencies)
QuizPlatform.Data      — EF Core DbContext, Repositories, Migrations
QuizPlatform.Services  — Business logic (AuthService, QuizService, LeaderboardService)
QuizPlatform.UI        — Console UI / entry point
QuizPlatform.Tests     — Unit tests (xUnit + Moq)
```

## How to Run

```bash
cd QuizPlatform.UI
dotnet run
```

## How to Run Tests

```bash
cd QuizPlatform.Tests
dotnet test
```

## Requirements

- .NET 8 SDK
- No external DB needed (SQLite file created automatically)

- ## Contributors
- Nuran Balzhan (69647) 
- Bexultan Zhylkybay (69642)
- Nurbolat Zhanuzak (70997)

