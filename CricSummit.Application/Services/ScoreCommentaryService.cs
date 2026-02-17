using CricSummit.Application.DTO;
using CricSummit.Application.Interfaces;
using CricSummit.Domain.Rules;
using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CricSummit.Application.Services
{
    public class ScoreCommentaryService : IScoreCommentaryService
    {
        private ILogger<ScoreCommentaryService> _logger;
        private IPredictScoreService _predictScoreService;
        private ICommentaryRuleProvider _commentaryRuleProvider;

        public ScoreCommentaryService(
            ILogger<ScoreCommentaryService> logger,
            IPredictScoreService predictScoreService,
            ICommentaryRuleProvider commentaryRuleProvider
        )
        {
            _logger = logger;
            _predictScoreService = predictScoreService;
            _commentaryRuleProvider = commentaryRuleProvider;
        }

        public ScoreCommentaryDto GetScoreAndCommentary(
            BowlingType bowlingType,
            BattingType battingType,
            ShotTiming shotTiming
        )
        {
            _logger.LogInformation(
                "Evaluating score and commentary for BowlingType {bowlingType}, BattingType {battingType} and ShotTiming {shotTiming}",
                bowlingType,
                battingType,
                shotTiming
            );
            try
            {
                Score score = _predictScoreService.EvaluateScore(
                    bowlingType,
                    battingType,
                    shotTiming
                );
                List<string> commentary = _commentaryRuleProvider.GetCommentary(score);
                string selectedCommentary = "";
                if (commentary.Count > 0)
                {
                    var random = new Random();
                    int index = random.Next(commentary.Count);
                    selectedCommentary = commentary[index];
                }
                return new ScoreCommentaryDto { Score = score, Commentary = selectedCommentary };
            }
            catch (ArgumentException ex)
            {
                _logger.LogInformation("Failed to evaluate the score and commentary");
                throw new ArgumentException($"Unable to evaluate the score and commentary,{ex}");
            }
        }
    }
}
