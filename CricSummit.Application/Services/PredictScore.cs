using CricSummit.Application.Interfaces;
using CricSummit.Domain.Rules;
using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CricSummit.Application.Services
{
    public class PredictScore : IPredictScore
    {
        private ILogger<PredictScore> _logger;
        private ICombinationRuleProvider _combinationRuleProvider;
        private IScoreRuleProvider _scoreRuleProvider;

        public PredictScore(
            ILogger<PredictScore> logger,
            ICombinationRuleProvider combinationRuleProvider,
            IScoreRuleProvider scoreRuleProvider
        )
        {
            _logger = logger;
            _combinationRuleProvider = combinationRuleProvider;
            _scoreRuleProvider = scoreRuleProvider;
        }

        public Score EvaluateScore(
            BowlingType bowlingType,
            BattingType battingType,
            ShotTiming shotTiming
        )
        {
            _logger.LogInformation(
                "Evaluating score for BowlingType {bowlingType}, BattingType {battingType} and ShotTiming {shotTiming}",
                bowlingType,
                battingType,
                shotTiming
            );
            try
            {
                Combination combination = _combinationRuleProvider.GetCombination(
                    bowlingType,
                    battingType
                );
                return _scoreRuleProvider.GetScore(shotTiming, combination);
            }
            catch (InvalidDataException ex)
            {
                _logger.LogInformation("Failed to evaluate the score");
                throw new InvalidDataException($"Unable to evaluate the score,{ex}");
            }
        }
    }
}
