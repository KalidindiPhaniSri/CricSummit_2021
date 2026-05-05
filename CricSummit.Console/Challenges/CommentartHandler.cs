using CricSummit.Application.Interfaces;
using CricSummit.Console.DTO;
using CricSummit.Console.Interfaces;
using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CricSummit.Console.Challenges
{
    public class CommentaryHandler
    {
        private readonly ILogger<CommentaryHandler> _logger;
        private readonly IInputProvider _inputProvider;
        private readonly IScoreCommentaryService _scoreCommentaryService;

        public CommentaryHandler(
            ILogger<CommentaryHandler> logger,
            IInputProvider inputProvider,
            IScoreCommentaryService scoreCommentaryService
        )
        {
            _logger = logger;
            _inputProvider = inputProvider;
            _scoreCommentaryService = scoreCommentaryService;
        }

        public void Execute()
        {
            while (true)
            {
                _logger.LogInformation("Start predicting the score and commentary");
                System.Console.WriteLine("\n Format");
                System.Console.WriteLine("BowlingType_BattingType_ShotTiming");
                System.Console.WriteLine("\n Example");
                System.Console.WriteLine("Fast_PullShot_Perfect \n");
                System.Console.WriteLine();
                var input = _inputProvider.ReadAll();
                if (input == null || !input.Any())
                {
                    _logger.LogWarning("Input should not be empty");
                    continue;
                }
                bool isValid = true;
                List<BallPlayInputDto> enums =  [ ];

                foreach (string[] entry in input)
                {
                    if (entry == null || entry.Length < 3)
                    {
                        isValid = false;
                        _logger.LogWarning(
                            "Invalid format: {entry}",
                            string.Join(",", entry ?? [ ])
                        );
                        break;
                    }
                    bool validBowlType = Enum.TryParse(entry[0], true, out BowlingType bowl);
                    bool validBatType = Enum.TryParse(entry[1], true, out BattingType bat);
                    bool validTimingType = Enum.TryParse(entry[2], true, out ShotTiming timing);
                    if (!validBatType || !validBowlType || !validTimingType)
                    {
                        isValid = false;
                        _logger.LogWarning(
                            "Invalid bowling/batting/shot timing values: {entry}",
                            string.Join(",", entry ?? [ ])
                        );
                        break;
                    }
                    enums.Add(
                        new BallPlayInputDto
                        {
                            Bowl = bowl,
                            Bat = bat,
                            Timing = timing
                        }
                    );
                }
                if (!isValid)
                {
                    System.Console.WriteLine("Invalid input. Please try again");
                    continue;
                }
                foreach (BallPlayInputDto inp in enums)
                {
                    var (score, commentary) = _scoreCommentaryService.GetScoreAndCommentary(
                        inp.Bowl,
                        inp.Bat,
                        inp.Timing
                    );
                    string runs = ScoreExtensions.Runs(score);
                    _logger.LogInformation(
                        "Score and commentary for the given bowling type : {bowl} batting type : {bat} shot timing : {timing} is {runs}, {comment}",
                        inp.Bowl,
                        inp.Bat,
                        inp.Timing,
                        runs,
                        commentary
                    );
                    System.Console.WriteLine($"\n {commentary} - {runs}");
                }
                break;
            }
        }
    }
}
