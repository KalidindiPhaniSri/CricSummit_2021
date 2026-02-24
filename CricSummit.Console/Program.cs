using CricSummit.Application.Interfaces;
using CricSummit.Application.Services;
using CricSummit.Console;
using CricSummit.Console.Challenges;
using CricSummit.Console.InputProviders;
using CricSummit.Console.Interfaces;
using CricSummit.Domain.DomainServices;
using CricSummit.Domain.Rules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel
    .Debug()
    // .WriteTo
    // .Console()
    .WriteTo
    .File("logs/cricket-summit-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

//create DI container
var services = new ServiceCollection();

//Add logging to DI
services.AddLogging(builder =>
{
    builder.ClearProviders();
    builder.AddSerilog();
});

//Register services
services.AddSingleton<CombinationRuleProvider>();
services.AddSingleton<CrickSummit>();
services.AddSingleton<IInputProvider, ConsoleInputProvider>();

services.AddSingleton<PredictOutcomeHandler>();
services.AddSingleton<CommentaryHandler>();
services.AddSingleton<SuperOverHandler>();

services.AddSingleton<IPredictScoreService, PredictScoreService>();
services.AddSingleton<IScoreCommentaryService, ScoreCommentaryService>();
services.AddSingleton<ISuperOverCommentaryService, SuperOverCommentaryService>();

services.AddSingleton<ICombinationRuleProvider, CombinationRuleProvider>();
services.AddSingleton<IScoreRuleProvider, ScoreRuleProvider>();
services.AddSingleton<ICommentaryRuleProvider, CommentaryRuleProvider>();
services.AddSingleton<IScoreCommentaryService, ScoreCommentaryService>();
services.AddSingleton<ISuperOverService, SuperOverService>();

//Builds the DI container, it will create objects, inject dependencies, resolve services
var serviceProvider = services.BuildServiceProvider();

//Get logger from DI
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Application started");

var app = serviceProvider.GetRequiredService<CrickSummit>();
app.Run();
