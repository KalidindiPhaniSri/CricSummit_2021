using CricSummit.Domain.ValueObjects;

namespace CricSummit.Domain.Rules
{
    public interface ICombinationRuleProvider
    {
        IReadOnlyDictionary<
            BowlingType,
            IReadOnlyDictionary<BattingType, Combination>
        > CombinationRules { get; }
        Combination GetCombination(BowlingType bowlingType, BattingType battingType);
    }
}
