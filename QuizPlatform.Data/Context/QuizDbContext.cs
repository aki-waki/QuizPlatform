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
        modelBuilder.Entity<Player>(e => { e.HasIndex(p => p.Username).IsUnique(); });
        modelBuilder.Entity<Question>(e => {
            e.HasOne(q => q.Category).WithMany(c => c.Questions).HasForeignKey(q => q.CategoryId);
            e.HasOne(q => q.CreatedByPlayer).WithMany(p => p.CreatedQuestions).HasForeignKey(q => q.CreatedByPlayerId);
        });
        modelBuilder.Entity<Answer>(e => { e.HasOne(a => a.Question).WithMany(q => q.Answers).HasForeignKey(a => a.QuestionId); });
        modelBuilder.Entity<GameResult>(e => {
            e.HasOne(g => g.Player).WithMany(p => p.GameResults).HasForeignKey(g => g.PlayerId);
            e.HasOne(g => g.Category).WithMany().HasForeignKey(g => g.CategoryId);
        });

        var d = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "General Knowledge" },
            new Category { Id = 2, Name = "Science" },
            new Category { Id = 3, Name = "History" },
            new Category { Id = 4, Name = "Technology" }
        );

        modelBuilder.Entity<Player>().HasData(
            new Player { Id = 1, Username = "admin", PasswordHash = "admin_seed", CreatedAt = d }
        );

        modelBuilder.Entity<Question>().HasData(
            new Question { Id=1,  Text="How many days are in a week?",                              Difficulty=Difficulty.Easy,   CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=2,  Text="What color is the sky on a clear day?",                     Difficulty=Difficulty.Easy,   CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=3,  Text="How many months are in a year?",                            Difficulty=Difficulty.Easy,   CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=4,  Text="What is the largest ocean on Earth?",                       Difficulty=Difficulty.Easy,   CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=5,  Text="How many legs does a spider have?",                         Difficulty=Difficulty.Easy,   CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=6,  Text="What is the capital of Australia?",                         Difficulty=Difficulty.Medium, CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=7,  Text="How many continents are there?",                            Difficulty=Difficulty.Medium, CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=8,  Text="What language is spoken in Brazil?",                        Difficulty=Difficulty.Medium, CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=9,  Text="What is the smallest country in the world?",                Difficulty=Difficulty.Medium, CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=10, Text="What currency does Japan use?",                             Difficulty=Difficulty.Medium, CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=11, Text="What is the capital of Kazakhstan?",                        Difficulty=Difficulty.Hard,   CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=12, Text="How many bones are in the human body?",                     Difficulty=Difficulty.Hard,   CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=13, Text="What is the longest river in the world?",                   Difficulty=Difficulty.Hard,   CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=14, Text="In what country is the Amazon rainforest mostly located?",  Difficulty=Difficulty.Hard,   CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=15, Text="What is the most spoken language in the world?",            Difficulty=Difficulty.Hard,   CategoryId=1, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=16, Text="What planet do we live on?",                                Difficulty=Difficulty.Easy,   CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=17, Text="What do plants need to grow?",                              Difficulty=Difficulty.Easy,   CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=18, Text="How many legs does an insect have?",                        Difficulty=Difficulty.Easy,   CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=19, Text="What is the boiling point of water in Celsius?",            Difficulty=Difficulty.Easy,   CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=20, Text="What gas do humans breathe in?",                            Difficulty=Difficulty.Easy,   CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=21, Text="What is the powerhouse of the cell?",                       Difficulty=Difficulty.Medium, CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=22, Text="What is the atomic number of carbon?",                      Difficulty=Difficulty.Medium, CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=23, Text="What force keeps us on the ground?",                        Difficulty=Difficulty.Medium, CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=24, Text="What is the chemical symbol for gold?",                     Difficulty=Difficulty.Medium, CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=25, Text="What is the chemical symbol for water?",                    Difficulty=Difficulty.Medium, CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=26, Text="What particle has no electric charge?",                     Difficulty=Difficulty.Hard,   CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=27, Text="What is the most abundant gas in Earth's atmosphere?",      Difficulty=Difficulty.Hard,   CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=28, Text="What is the SI unit of electric resistance?",               Difficulty=Difficulty.Hard,   CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=29, Text="What theory did Einstein publish in 1915?",                 Difficulty=Difficulty.Hard,   CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=30, Text="What is the half-life of Carbon-14 in years?",              Difficulty=Difficulty.Hard,   CategoryId=2, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=31, Text="Who was the first US president?",                           Difficulty=Difficulty.Easy,   CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=32, Text="In what country was Napoleon Bonaparte born?",              Difficulty=Difficulty.Easy,   CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=33, Text="What year did World War I begin?",                          Difficulty=Difficulty.Easy,   CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=34, Text="Who built the pyramids?",                                   Difficulty=Difficulty.Easy,   CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=35, Text="What wall divided East and West Berlin?",                   Difficulty=Difficulty.Easy,   CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=36, Text="In what year did the Titanic sink?",                        Difficulty=Difficulty.Medium, CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=37, Text="Who was the first man to walk on the Moon?",                Difficulty=Difficulty.Medium, CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=38, Text="What empire was Julius Caesar part of?",                    Difficulty=Difficulty.Medium, CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=39, Text="In what city was JFK assassinated?",                        Difficulty=Difficulty.Medium, CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=40, Text="What year did the Soviet Union collapse?",                  Difficulty=Difficulty.Medium, CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=41, Text="Who wrote the Communist Manifesto?",                        Difficulty=Difficulty.Hard,   CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=42, Text="What was the name of the first artificial satellite?",      Difficulty=Difficulty.Hard,   CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=43, Text="In what year did Genghis Khan die?",                        Difficulty=Difficulty.Hard,   CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=44, Text="What battle ended Napoleon's rule?",                        Difficulty=Difficulty.Hard,   CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=45, Text="Who was the first female Prime Minister of the UK?",        Difficulty=Difficulty.Hard,   CategoryId=3, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=46, Text="What does USB stand for?",                                  Difficulty=Difficulty.Easy,   CategoryId=4, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=47, Text="Who founded Microsoft?",                                    Difficulty=Difficulty.Easy,   CategoryId=4, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=48, Text="What does RAM stand for?",                                  Difficulty=Difficulty.Easy,   CategoryId=4, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=49, Text="What company made the iPhone?",                             Difficulty=Difficulty.Easy,   CategoryId=4, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=50, Text="What does WWW stand for?",                                  Difficulty=Difficulty.Easy,   CategoryId=4, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=51, Text="What does SQL stand for?",                                  Difficulty=Difficulty.Medium, CategoryId=4, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=52, Text="Who invented the World Wide Web?",                          Difficulty=Difficulty.Medium, CategoryId=4, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=53, Text="What does GPU stand for?",                                  Difficulty=Difficulty.Medium, CategoryId=4, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=54, Text="What year was the first iPhone released?",                  Difficulty=Difficulty.Medium, CategoryId=4, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=55, Text="What does CPU stand for?",                                  Difficulty=Difficulty.Medium, CategoryId=4, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=56, Text="What sorting algorithm has average O(n log n)?",            Difficulty=Difficulty.Hard,   CategoryId=4, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=57, Text="What does HTTPS stand for?",                                Difficulty=Difficulty.Hard,   CategoryId=4, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=58, Text="What is the binary representation of 10?",                  Difficulty=Difficulty.Hard,   CategoryId=4, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=59, Text="Who invented the C programming language?",                  Difficulty=Difficulty.Hard,   CategoryId=4, CreatedByPlayerId=1, CreatedAt=d },
            new Question { Id=60, Text="What design pattern separates UI from business logic?",     Difficulty=Difficulty.Hard,   CategoryId=4, CreatedByPlayerId=1, CreatedAt=d }
        );

        modelBuilder.Entity<Answer>().HasData(
            new Answer{Id=1,  Text="7",               IsCorrect=true,  QuestionId=1},  new Answer{Id=2,  Text="5",               IsCorrect=false, QuestionId=1},  new Answer{Id=3,  Text="6",               IsCorrect=false, QuestionId=1},  new Answer{Id=4,  Text="8",               IsCorrect=false, QuestionId=1},
            new Answer{Id=5,  Text="Blue",            IsCorrect=true,  QuestionId=2},  new Answer{Id=6,  Text="Green",           IsCorrect=false, QuestionId=2},  new Answer{Id=7,  Text="Red",             IsCorrect=false, QuestionId=2},  new Answer{Id=8,  Text="Yellow",          IsCorrect=false, QuestionId=2},
            new Answer{Id=9,  Text="12",              IsCorrect=true,  QuestionId=3},  new Answer{Id=10, Text="10",              IsCorrect=false, QuestionId=3},  new Answer{Id=11, Text="11",              IsCorrect=false, QuestionId=3},  new Answer{Id=12, Text="13",              IsCorrect=false, QuestionId=3},
            new Answer{Id=13, Text="Pacific Ocean",   IsCorrect=true,  QuestionId=4},  new Answer{Id=14, Text="Atlantic Ocean",  IsCorrect=false, QuestionId=4},  new Answer{Id=15, Text="Indian Ocean",    IsCorrect=false, QuestionId=4},  new Answer{Id=16, Text="Arctic Ocean",    IsCorrect=false, QuestionId=4},
            new Answer{Id=17, Text="8",               IsCorrect=true,  QuestionId=5},  new Answer{Id=18, Text="6",               IsCorrect=false, QuestionId=5},  new Answer{Id=19, Text="10",              IsCorrect=false, QuestionId=5},  new Answer{Id=20, Text="4",               IsCorrect=false, QuestionId=5},
            new Answer{Id=21, Text="Canberra",        IsCorrect=true,  QuestionId=6},  new Answer{Id=22, Text="Sydney",          IsCorrect=false, QuestionId=6},  new Answer{Id=23, Text="Melbourne",       IsCorrect=false, QuestionId=6},  new Answer{Id=24, Text="Brisbane",        IsCorrect=false, QuestionId=6},
            new Answer{Id=25, Text="7",               IsCorrect=true,  QuestionId=7},  new Answer{Id=26, Text="5",               IsCorrect=false, QuestionId=7},  new Answer{Id=27, Text="6",               IsCorrect=false, QuestionId=7},  new Answer{Id=28, Text="8",               IsCorrect=false, QuestionId=7},
            new Answer{Id=29, Text="Portuguese",      IsCorrect=true,  QuestionId=8},  new Answer{Id=30, Text="Spanish",         IsCorrect=false, QuestionId=8},  new Answer{Id=31, Text="English",         IsCorrect=false, QuestionId=8},  new Answer{Id=32, Text="French",          IsCorrect=false, QuestionId=8},
            new Answer{Id=33, Text="Vatican City",    IsCorrect=true,  QuestionId=9},  new Answer{Id=34, Text="Monaco",          IsCorrect=false, QuestionId=9},  new Answer{Id=35, Text="San Marino",      IsCorrect=false, QuestionId=9},  new Answer{Id=36, Text="Liechtenstein",   IsCorrect=false, QuestionId=9},
            new Answer{Id=37, Text="Yen",             IsCorrect=true,  QuestionId=10}, new Answer{Id=38, Text="Won",             IsCorrect=false, QuestionId=10}, new Answer{Id=39, Text="Yuan",            IsCorrect=false, QuestionId=10}, new Answer{Id=40, Text="Baht",            IsCorrect=false, QuestionId=10},
            new Answer{Id=41, Text="Astana",          IsCorrect=true,  QuestionId=11}, new Answer{Id=42, Text="Almaty",          IsCorrect=false, QuestionId=11}, new Answer{Id=43, Text="Shymkent",        IsCorrect=false, QuestionId=11}, new Answer{Id=44, Text="Karaganda",       IsCorrect=false, QuestionId=11},
            new Answer{Id=45, Text="206",             IsCorrect=true,  QuestionId=12}, new Answer{Id=46, Text="196",             IsCorrect=false, QuestionId=12}, new Answer{Id=47, Text="216",             IsCorrect=false, QuestionId=12}, new Answer{Id=48, Text="186",             IsCorrect=false, QuestionId=12},
            new Answer{Id=49, Text="Nile",            IsCorrect=true,  QuestionId=13}, new Answer{Id=50, Text="Amazon",          IsCorrect=false, QuestionId=13}, new Answer{Id=51, Text="Yangtze",         IsCorrect=false, QuestionId=13}, new Answer{Id=52, Text="Mississippi",     IsCorrect=false, QuestionId=13},
            new Answer{Id=53, Text="Brazil",          IsCorrect=true,  QuestionId=14}, new Answer{Id=54, Text="Peru",            IsCorrect=false, QuestionId=14}, new Answer{Id=55, Text="Colombia",        IsCorrect=false, QuestionId=14}, new Answer{Id=56, Text="Venezuela",       IsCorrect=false, QuestionId=14},
            new Answer{Id=57, Text="Mandarin Chinese",IsCorrect=true,  QuestionId=15}, new Answer{Id=58, Text="English",         IsCorrect=false, QuestionId=15}, new Answer{Id=59, Text="Spanish",         IsCorrect=false, QuestionId=15}, new Answer{Id=60, Text="Hindi",           IsCorrect=false, QuestionId=15},
            new Answer{Id=61, Text="Earth",           IsCorrect=true,  QuestionId=16}, new Answer{Id=62, Text="Mars",            IsCorrect=false, QuestionId=16}, new Answer{Id=63, Text="Venus",           IsCorrect=false, QuestionId=16}, new Answer{Id=64, Text="Jupiter",         IsCorrect=false, QuestionId=16},
            new Answer{Id=65, Text="Sunlight",        IsCorrect=true,  QuestionId=17}, new Answer{Id=66, Text="Darkness",        IsCorrect=false, QuestionId=17}, new Answer{Id=67, Text="Salt",            IsCorrect=false, QuestionId=17}, new Answer{Id=68, Text="Sand",            IsCorrect=false, QuestionId=17},
            new Answer{Id=69, Text="6",               IsCorrect=true,  QuestionId=18}, new Answer{Id=70, Text="8",               IsCorrect=false, QuestionId=18}, new Answer{Id=71, Text="4",               IsCorrect=false, QuestionId=18}, new Answer{Id=72, Text="10",              IsCorrect=false, QuestionId=18},
            new Answer{Id=73, Text="100",             IsCorrect=true,  QuestionId=19}, new Answer{Id=74, Text="90",              IsCorrect=false, QuestionId=19}, new Answer{Id=75, Text="110",             IsCorrect=false, QuestionId=19}, new Answer{Id=76, Text="80",              IsCorrect=false, QuestionId=19},
            new Answer{Id=77, Text="Oxygen",          IsCorrect=true,  QuestionId=20}, new Answer{Id=78, Text="Carbon dioxide",  IsCorrect=false, QuestionId=20}, new Answer{Id=79, Text="Nitrogen",        IsCorrect=false, QuestionId=20}, new Answer{Id=80, Text="Hydrogen",        IsCorrect=false, QuestionId=20},
            new Answer{Id=81, Text="Mitochondria",    IsCorrect=true,  QuestionId=21}, new Answer{Id=82, Text="Nucleus",         IsCorrect=false, QuestionId=21}, new Answer{Id=83, Text="Ribosome",        IsCorrect=false, QuestionId=21}, new Answer{Id=84, Text="Vacuole",         IsCorrect=false, QuestionId=21},
            new Answer{Id=85, Text="6",               IsCorrect=true,  QuestionId=22}, new Answer{Id=86, Text="8",               IsCorrect=false, QuestionId=22}, new Answer{Id=87, Text="12",              IsCorrect=false, QuestionId=22}, new Answer{Id=88, Text="4",               IsCorrect=false, QuestionId=22},
            new Answer{Id=89, Text="Gravity",         IsCorrect=true,  QuestionId=23}, new Answer{Id=90, Text="Magnetism",       IsCorrect=false, QuestionId=23}, new Answer{Id=91, Text="Friction",        IsCorrect=false, QuestionId=23}, new Answer{Id=92, Text="Inertia",         IsCorrect=false, QuestionId=23},
            new Answer{Id=93, Text="Au",              IsCorrect=true,  QuestionId=24}, new Answer{Id=94, Text="Ag",              IsCorrect=false, QuestionId=24}, new Answer{Id=95, Text="Fe",              IsCorrect=false, QuestionId=24}, new Answer{Id=96, Text="Go",              IsCorrect=false, QuestionId=24},
            new Answer{Id=97, Text="H2O",             IsCorrect=true,  QuestionId=25}, new Answer{Id=98, Text="CO2",             IsCorrect=false, QuestionId=25}, new Answer{Id=99, Text="O2",              IsCorrect=false, QuestionId=25}, new Answer{Id=100,Text="NaCl",            IsCorrect=false, QuestionId=25},
            new Answer{Id=101,Text="Neutron",         IsCorrect=true,  QuestionId=26}, new Answer{Id=102,Text="Proton",          IsCorrect=false, QuestionId=26}, new Answer{Id=103,Text="Electron",        IsCorrect=false, QuestionId=26}, new Answer{Id=104,Text="Photon",          IsCorrect=false, QuestionId=26},
            new Answer{Id=105,Text="Nitrogen",        IsCorrect=true,  QuestionId=27}, new Answer{Id=106,Text="Oxygen",          IsCorrect=false, QuestionId=27}, new Answer{Id=107,Text="Carbon dioxide",  IsCorrect=false, QuestionId=27}, new Answer{Id=108,Text="Argon",           IsCorrect=false, QuestionId=27},
            new Answer{Id=109,Text="Ohm",             IsCorrect=true,  QuestionId=28}, new Answer{Id=110,Text="Volt",            IsCorrect=false, QuestionId=28}, new Answer{Id=111,Text="Ampere",          IsCorrect=false, QuestionId=28}, new Answer{Id=112,Text="Watt",            IsCorrect=false, QuestionId=28},
            new Answer{Id=113,Text="General Theory of Relativity",IsCorrect=true, QuestionId=29}, new Answer{Id=114,Text="Special Theory of Relativity",IsCorrect=false,QuestionId=29}, new Answer{Id=115,Text="Quantum Theory",IsCorrect=false,QuestionId=29}, new Answer{Id=116,Text="Theory of Everything",IsCorrect=false,QuestionId=29},
            new Answer{Id=117,Text="5,730 years",     IsCorrect=true,  QuestionId=30}, new Answer{Id=118,Text="1,000 years",     IsCorrect=false, QuestionId=30}, new Answer{Id=119,Text="10,000 years",    IsCorrect=false, QuestionId=30}, new Answer{Id=120,Text="500 years",       IsCorrect=false, QuestionId=30},
            new Answer{Id=121,Text="George Washington",IsCorrect=true, QuestionId=31}, new Answer{Id=122,Text="Abraham Lincoln", IsCorrect=false, QuestionId=31}, new Answer{Id=123,Text="Thomas Jefferson",IsCorrect=false, QuestionId=31}, new Answer{Id=124,Text="John Adams",      IsCorrect=false, QuestionId=31},
            new Answer{Id=125,Text="Corsica (France)",IsCorrect=true,  QuestionId=32}, new Answer{Id=126,Text="mainland France", IsCorrect=false, QuestionId=32}, new Answer{Id=127,Text="Italy",           IsCorrect=false, QuestionId=32}, new Answer{Id=128,Text="Spain",           IsCorrect=false, QuestionId=32},
            new Answer{Id=129,Text="1914",            IsCorrect=true,  QuestionId=33}, new Answer{Id=130,Text="1912",            IsCorrect=false, QuestionId=33}, new Answer{Id=131,Text="1916",            IsCorrect=false, QuestionId=33}, new Answer{Id=132,Text="1918",            IsCorrect=false, QuestionId=33},
            new Answer{Id=133,Text="Ancient Egyptians",IsCorrect=true, QuestionId=34}, new Answer{Id=134,Text="Romans",          IsCorrect=false, QuestionId=34}, new Answer{Id=135,Text="Greeks",          IsCorrect=false, QuestionId=34}, new Answer{Id=136,Text="Babylonians",     IsCorrect=false, QuestionId=34},
            new Answer{Id=137,Text="Berlin Wall",     IsCorrect=true,  QuestionId=35}, new Answer{Id=138,Text="Iron Curtain",    IsCorrect=false, QuestionId=35}, new Answer{Id=139,Text="Great Wall",      IsCorrect=false, QuestionId=35}, new Answer{Id=140,Text="Checkpoint Wall", IsCorrect=false, QuestionId=35},
            new Answer{Id=141,Text="1912",            IsCorrect=true,  QuestionId=36}, new Answer{Id=142,Text="1910",            IsCorrect=false, QuestionId=36}, new Answer{Id=143,Text="1914",            IsCorrect=false, QuestionId=36}, new Answer{Id=144,Text="1908",            IsCorrect=false, QuestionId=36},
            new Answer{Id=145,Text="Neil Armstrong",  IsCorrect=true,  QuestionId=37}, new Answer{Id=146,Text="Buzz Aldrin",     IsCorrect=false, QuestionId=37}, new Answer{Id=147,Text="Yuri Gagarin",    IsCorrect=false, QuestionId=37}, new Answer{Id=148,Text="John Glenn",      IsCorrect=false, QuestionId=37},
            new Answer{Id=149,Text="Roman Empire",    IsCorrect=true,  QuestionId=38}, new Answer{Id=150,Text="Greek Empire",    IsCorrect=false, QuestionId=38}, new Answer{Id=151,Text="Ottoman Empire",  IsCorrect=false, QuestionId=38}, new Answer{Id=152,Text="Byzantine Empire",IsCorrect=false, QuestionId=38},
            new Answer{Id=153,Text="Dallas",          IsCorrect=true,  QuestionId=39}, new Answer{Id=154,Text="Houston",         IsCorrect=false, QuestionId=39}, new Answer{Id=155,Text="Washington DC",   IsCorrect=false, QuestionId=39}, new Answer{Id=156,Text="Chicago",         IsCorrect=false, QuestionId=39},
            new Answer{Id=157,Text="1991",            IsCorrect=true,  QuestionId=40}, new Answer{Id=158,Text="1989",            IsCorrect=false, QuestionId=40}, new Answer{Id=159,Text="1993",            IsCorrect=false, QuestionId=40}, new Answer{Id=160,Text="1985",            IsCorrect=false, QuestionId=40},
            new Answer{Id=161,Text="Karl Marx",       IsCorrect=true,  QuestionId=41}, new Answer{Id=162,Text="Lenin",           IsCorrect=false, QuestionId=41}, new Answer{Id=163,Text="Engels alone",    IsCorrect=false, QuestionId=41}, new Answer{Id=164,Text="Stalin",          IsCorrect=false, QuestionId=41},
            new Answer{Id=165,Text="Sputnik",         IsCorrect=true,  QuestionId=42}, new Answer{Id=166,Text="Vostok",          IsCorrect=false, QuestionId=42}, new Answer{Id=167,Text="Explorer 1",      IsCorrect=false, QuestionId=42}, new Answer{Id=168,Text="Mir",             IsCorrect=false, QuestionId=42},
            new Answer{Id=169,Text="1227",            IsCorrect=true,  QuestionId=43}, new Answer{Id=170,Text="1206",            IsCorrect=false, QuestionId=43}, new Answer{Id=171,Text="1241",            IsCorrect=false, QuestionId=43}, new Answer{Id=172,Text="1300",            IsCorrect=false, QuestionId=43},
            new Answer{Id=173,Text="Battle of Waterloo",IsCorrect=true,QuestionId=44}, new Answer{Id=174,Text="Battle of Austerlitz",IsCorrect=false,QuestionId=44}, new Answer{Id=175,Text="Battle of Trafalgar",IsCorrect=false,QuestionId=44}, new Answer{Id=176,Text="Battle of Leipzig",IsCorrect=false,QuestionId=44},
            new Answer{Id=177,Text="Margaret Thatcher",IsCorrect=true, QuestionId=45}, new Answer{Id=178,Text="Theresa May",     IsCorrect=false, QuestionId=45}, new Answer{Id=179,Text="Angela Merkel",   IsCorrect=false, QuestionId=45}, new Answer{Id=180,Text="Indira Gandhi",   IsCorrect=false, QuestionId=45},
            new Answer{Id=181,Text="Universal Serial Bus",IsCorrect=true,QuestionId=46}, new Answer{Id=182,Text="Unified System Bus",IsCorrect=false,QuestionId=46}, new Answer{Id=183,Text="Universal System Bridge",IsCorrect=false,QuestionId=46}, new Answer{Id=184,Text="Unified Serial Bridge",IsCorrect=false,QuestionId=46},
            new Answer{Id=185,Text="Bill Gates",      IsCorrect=true,  QuestionId=47}, new Answer{Id=186,Text="Steve Jobs",      IsCorrect=false, QuestionId=47}, new Answer{Id=187,Text="Elon Musk",       IsCorrect=false, QuestionId=47}, new Answer{Id=188,Text="Mark Zuckerberg", IsCorrect=false, QuestionId=47},
            new Answer{Id=189,Text="Random Access Memory",IsCorrect=true,QuestionId=48}, new Answer{Id=190,Text="Read Access Memory",IsCorrect=false,QuestionId=48}, new Answer{Id=191,Text="Rapid Access Module",IsCorrect=false,QuestionId=48}, new Answer{Id=192,Text="Random Allocation Memory",IsCorrect=false,QuestionId=48},
            new Answer{Id=193,Text="Apple",           IsCorrect=true,  QuestionId=49}, new Answer{Id=194,Text="Samsung",         IsCorrect=false, QuestionId=49}, new Answer{Id=195,Text="Google",          IsCorrect=false, QuestionId=49}, new Answer{Id=196,Text="Nokia",           IsCorrect=false, QuestionId=49},
            new Answer{Id=197,Text="World Wide Web",  IsCorrect=true,  QuestionId=50}, new Answer{Id=198,Text="World Web Wide",  IsCorrect=false, QuestionId=50}, new Answer{Id=199,Text="Wide World Web",  IsCorrect=false, QuestionId=50}, new Answer{Id=200,Text="Web World Wide",  IsCorrect=false, QuestionId=50},
            new Answer{Id=201,Text="Structured Query Language",IsCorrect=true,QuestionId=51}, new Answer{Id=202,Text="Simple Query Language",IsCorrect=false,QuestionId=51}, new Answer{Id=203,Text="Standard Query Language",IsCorrect=false,QuestionId=51}, new Answer{Id=204,Text="Sequential Query Language",IsCorrect=false,QuestionId=51},
            new Answer{Id=205,Text="Tim Berners-Lee", IsCorrect=true,  QuestionId=52}, new Answer{Id=206,Text="Bill Gates",      IsCorrect=false, QuestionId=52}, new Answer{Id=207,Text="Vint Cerf",       IsCorrect=false, QuestionId=52}, new Answer{Id=208,Text="Steve Jobs",      IsCorrect=false, QuestionId=52},
            new Answer{Id=209,Text="Graphics Processing Unit",IsCorrect=true,QuestionId=53}, new Answer{Id=210,Text="General Processing Unit",IsCorrect=false,QuestionId=53}, new Answer{Id=211,Text="Graphical Program Unit",IsCorrect=false,QuestionId=53}, new Answer{Id=212,Text="Graphics Program Utility",IsCorrect=false,QuestionId=53},
            new Answer{Id=213,Text="2007",            IsCorrect=true,  QuestionId=54}, new Answer{Id=214,Text="2005",            IsCorrect=false, QuestionId=54}, new Answer{Id=215,Text="2009",            IsCorrect=false, QuestionId=54}, new Answer{Id=216,Text="2003",            IsCorrect=false, QuestionId=54},
            new Answer{Id=217,Text="Central Processing Unit",IsCorrect=true,QuestionId=55}, new Answer{Id=218,Text="Core Processing Unit",IsCorrect=false,QuestionId=55}, new Answer{Id=219,Text="Computer Processing Unit",IsCorrect=false,QuestionId=55}, new Answer{Id=220,Text="Central Program Unit",IsCorrect=false,QuestionId=55},
            new Answer{Id=221,Text="Merge Sort",      IsCorrect=true,  QuestionId=56}, new Answer{Id=222,Text="Bubble Sort",     IsCorrect=false, QuestionId=56}, new Answer{Id=223,Text="Insertion Sort",  IsCorrect=false, QuestionId=56}, new Answer{Id=224,Text="Selection Sort",  IsCorrect=false, QuestionId=56},
            new Answer{Id=225,Text="HyperText Transfer Protocol Secure",IsCorrect=true,QuestionId=57}, new Answer{Id=226,Text="HyperText Transmission Protocol Secure",IsCorrect=false,QuestionId=57}, new Answer{Id=227,Text="High Transfer Text Protocol Secure",IsCorrect=false,QuestionId=57}, new Answer{Id=228,Text="HyperText Transport Protocol Secure",IsCorrect=false,QuestionId=57},
            new Answer{Id=229,Text="1010",            IsCorrect=true,  QuestionId=58}, new Answer{Id=230,Text="1100",            IsCorrect=false, QuestionId=58}, new Answer{Id=231,Text="0110",            IsCorrect=false, QuestionId=58}, new Answer{Id=232,Text="1001",            IsCorrect=false, QuestionId=58},
            new Answer{Id=233,Text="Dennis Ritchie",  IsCorrect=true,  QuestionId=59}, new Answer{Id=234,Text="Bjarne Stroustrup",IsCorrect=false,QuestionId=59}, new Answer{Id=235,Text="Linus Torvalds",  IsCorrect=false, QuestionId=59}, new Answer{Id=236,Text="James Gosling",   IsCorrect=false, QuestionId=59},
            new Answer{Id=237,Text="MVC",             IsCorrect=true,  QuestionId=60}, new Answer{Id=238,Text="Singleton",       IsCorrect=false, QuestionId=60}, new Answer{Id=239,Text="Observer",        IsCorrect=false, QuestionId=60}, new Answer{Id=240,Text="Factory",         IsCorrect=false, QuestionId=60}
        );
    }
}