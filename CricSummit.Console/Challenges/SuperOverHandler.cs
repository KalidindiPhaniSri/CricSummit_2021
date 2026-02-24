using CricSummit.Application.DTO;
using CricSummit.Application.Interfaces;
using CricSummit.Console.Interfaces;
using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CricSummit.Console.Challenges
{
    public class SuperOverHandler
    {
        private readonly ILogger<SuperOverHandler> _logger;
        private readonly IInputProvider _inputProvider;
        private readonly ISuperOverCommentaryService _superOverCommentaryService;

        public SuperOverHandler(
            ILogger<SuperOverHandler> logger,
            IInputProvider inputProvider,
            ISuperOverCommentaryService superOverCommentaryService
        )
        {
            _logger = logger;
            _inputProvider = inputProvider;
            _superOverCommentaryService = superOverCommentaryService;
        }

        public void Execute()
        {
            _logger.LogInformation("Start predicting the score and commentary");

            System.Console.WriteLine("\n Format");
            System.Console.WriteLine("BattingType_ShotTiming BattingType_ShotTiming");
            System.Console.WriteLine("\n Example");
            System.Console.WriteLine("PullShot_Perfect CoverDrive_Good \n");

            var input = _inputProvider.ReadAll();
            if (input == null || !input.Any())
            {
                _logger.LogWarning("Input should not be empty");
                throw new InvalidOperationException("Input should not be empty");
            }
            try
            {
                List<SuperOverRequestDto> superOverRequestDtos =  [ ];
                foreach (string[] entry in input)
                {
                    bool validBatType = Enum.TryParse(entry[0], true, out BattingType bat);
                    bool validTimingType = Enum.TryParse(entry[1], true, out ShotTiming timing);
                    superOverRequestDtos.Add(
                        new SuperOverRequestDto { BattingType = bat, ShotTiming = timing }
                    );
                }
                var (commentary, finalScore, resultMessage, ScoreDifference) =
                    _superOverCommentaryService.PlaySuperOver(superOverRequestDtos);
                foreach (SuperOverCommentaryDto over in commentary)
                {
                    System.Console.WriteLine($"\n {over.Bowler} bowled {over.BowlingType} ball");
                    System
                        .Console
                        .WriteLine($"{over.Batter} played {over.ShotTiming} {over.BattingType}");
                    System.Console.WriteLine($"\n {over.Commentary} - {over.Score} ");
                }
                System.Console.WriteLine($"\n AUSTRALIA scored : {finalScore}");
                System.Console.WriteLine($"\n AUSTRALIA {resultMessage} by {ScoreDifference}");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogWarning(ex, "Failed to process");
                throw;
            }
        }
    }
}
