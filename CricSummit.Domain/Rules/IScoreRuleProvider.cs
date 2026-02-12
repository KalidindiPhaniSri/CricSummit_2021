using CricSummit.Domain.ValueObjects;

namespace CricSummit.Domain.Rules
{
    public interface IScoreRuleProvider
    {
        IReadOnlyDictionary<ShotTiming, IReadOnlyDictionary<Combination, Score>> ScoreRules { get; }
        Score GetScore(ShotTiming shotTiming, Combination combination);
    }
}
