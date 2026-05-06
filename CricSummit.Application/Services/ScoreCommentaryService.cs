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
                //EvaluateScore and GetCommentary may throw error
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
            //we didn't configure the rules properly. user is giving valid data.
            catch (InvalidOperationException ex)
            {
                _logger.LogInformation("Failed to evaluate the score and commentary");
                throw new InvalidOperationException(
                    $"Unable to evaluate the score and commentary,{ex}"
                );
            }
        }
    }
}
