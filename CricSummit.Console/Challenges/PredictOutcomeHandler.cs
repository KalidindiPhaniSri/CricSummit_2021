using CricSummit.Application.Interfaces;
using CricSummit.Console.DTO;
using CricSummit.Console.Interfaces;
using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CricSummit.Console.Challenges
{
    public class PredictOutcomeHandler
    {
        private readonly ILogger<PredictOutcomeHandler> _logger;
        private readonly IInputProvider _inputProvider;
        private readonly IPredictScoreService _predictScoreService;

        public PredictOutcomeHandler(
            ILogger<PredictOutcomeHandler> logger,
            IInputProvider inputProvider,
            IPredictScoreService predictScoreService
        )
        {
            _logger = logger;
            _inputProvider = inputProvider;
            _predictScoreService = predictScoreService;
        }

        public void Execute()
        {
            while (true)
            {
                _logger.LogInformation("Start predicting the score");
                System.Console.WriteLine("\n Format");
                System.Console.WriteLine("BowlingType_BattingType_ShotTiming");
                System.Console.WriteLine("\n Example");
                System.Console.WriteLine("Fast_PullShot_Perfect \n");
                var input = _inputProvider.ReadAll();
                if (input == null || !input.Any())
                {
                    System.Console.WriteLine("Invalid input. Please try again");
                    continue;
                }
                bool isValid = true;
                List<BallPlayInputDto> enums =  [ ];
                foreach (string[] entry in input)
                {
                    if (entry == null || entry.Length < 3)
                    {
                        System
                            .Console
                            .WriteLine($"Invalid format: {string.Join(" ,", entry ?? [ ])}");
                        isValid = false;
                        break;
                    }
                    if (
                        !Enum.TryParse(entry[0], true, out BowlingType bowl)
                        || !Enum.TryParse(entry[1], true, out BattingType bat)
                        || !Enum.TryParse(entry[2], true, out ShotTiming timing)
                    )
                    {
                        System.Console.WriteLine("Invalid input. Please try again");

                        _logger.LogWarning(
                            "Invalid bowling/batting/shot timing values: {row}",
                            string.Join(",", entry)
                        );
                        isValid = false;

                        break;
                    }
                    enums.Add(
                        new BallPlayInputDto
                        {
                            Bat = bat,
                            Bowl = bowl,
                            Timing = timing
                        }
                    );
                }
                if (!isValid)
                    continue;
                foreach (BallPlayInputDto inp in enums)
                {
                    Score score = _predictScoreService.EvaluateScore(inp.Bowl, inp.Bat, inp.Timing);
                    string runs = ScoreExtensions.Runs(score);
                    _logger.LogInformation(
                        "Score for the given bowling type : {bowl} batting type : {bat} shot timing : {timing} is {runs}",
                        inp.Bowl,
                        inp.Bat,
                        inp.Timing,
                        runs
                    );
                    System.Console.WriteLine($"\n {runs}");
                }
                break;
            }
        }
    }
}
