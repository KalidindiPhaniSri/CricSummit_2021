using CricSummit.Domain.Rules;
using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging.Abstractions;

namespace CricSummit.Tests.Domain.Rules
{
    public class CombinationRuleProviderTests
    {
        private readonly ICombinationRuleProvider _combinationRuleProvider =
            new CombinationRuleProvider(NullLogger<CombinationRuleProvider>.Instance);

        [Fact]
        public void GetCombination_WhenPerfectPair_ReturnsPerfect()
        {
            Combination combination = _combinationRuleProvider.GetCombination(
                BowlingType.Fast,
                BattingType.PullShot
            );
            Assert.Equal(Combination.Perfect, combination);
        }

        [Fact]
        public void GetCombination_WhenGoodPair_ReturnsGood()
        {
            Combination combination = _combinationRuleProvider.GetCombination(
                BowlingType.Yorker,
                BattingType.DefensiveBlock
            );
            Assert.Equal(Combination.Good, combination);
        }

        [Fact]
        public void GetCombination_WhenAverageCombination_ReturnsAverage()
        {
            Combination combination = _combinationRuleProvider.GetCombination(
                BowlingType.Fast,
                BattingType.PaddleShot
            );
            Assert.Equal(Combination.Average, combination);
        }

        [Fact]
        public void GetCombination_WhenPoorCombination_ReturnsPoor()
        {
            Combination combination = _combinationRuleProvider.GetCombination(
                BowlingType.OffSpin,
                BattingType.PullShot
            );
            Assert.Equal(Combination.Poor, combination);
        }

        [Fact]
        public void GetCombination_InvalidRule_ThrowsException()
        {
            Assert.Throws<InvalidDataException>(
                () => _combinationRuleProvider.GetCombination((BowlingType)99, (BattingType)99)
            );
        }

        [Fact]
        public void CombinationRulesDataCount_MatchWithBowlingBattingTypesCount()
        {
            int BowlingTypesCount = Enum.GetValues(typeof(BowlingType)).Length;
            int BattingTypesCount = Enum.GetValues(typeof(BattingType)).Length;
            int combinationCount = 0;
            foreach (var bowlingRule in _combinationRuleProvider.CombinationRules)
            {
                combinationCount += bowlingRule.Value.Count;
            }
            Assert.Equal(BowlingTypesCount * BattingTypesCount, combinationCount);
        }
    }
}
