using CricSummit.Application.Interfaces;
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
            _logger.LogInformation("Start predicting the score");

            System.Console.WriteLine("\n Format");
            System.Console.WriteLine("BowlingType_BattingType_ShotTiming");
            System.Console.WriteLine("\n Example");
            System.Console.WriteLine("Fast_PullShot_Perfect \n");
            var input = _inputProvider.ReadAll();
            if (input == null || !input.Any())
            {
                _logger.LogWarning("Input should not be empty");
                throw new InvalidOperationException("Input should not be empty");
            }

            foreach (string[] entry in input)
            {
                try
                {
                    bool validBowlType = Enum.TryParse(entry[0], true, out BowlingType bowl);
                    bool validBatType = Enum.TryParse(entry[1], true, out BattingType bat);
                    bool validTimingType = Enum.TryParse(entry[2], true, out ShotTiming timing);
                    Score score = _predictScoreService.EvaluateScore(bowl, bat, timing);
                    string runs = ScoreExtensions.Runs(score);
                    _logger.LogInformation(
                        "Score for the given bowling type : {bowl} batting type : {bat} shot timing : {timing} is {runs}",
                        bowl,
                        bat,
                        timing,
                        runs
                    );
                    System.Console.WriteLine($"\n {runs}");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    _logger.LogWarning(ex, "Failed to process");
                    throw;
                }
            }
        }
    }
}
