using CricSummit.Domain.Rules;
using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging.Abstractions;

namespace CricSummit.Tests.Domain.Services
{
    public class ScoreRuleProviderTestss
    {
        IScoreRuleProvider scoreRuleProvider = new ScoreRuleProvider(
            NullLogger<ScoreRuleProvider>.Instance
        );

        [Fact]
        public void GetScore_WhenLateTimingPoorCombination_ReturnsWicket()
        {
            Score score = scoreRuleProvider.GetScore(ShotTiming.Late, Combination.Poor);
            Assert.Equal(Score.Wicket, score);
        }

        [Fact]
        public void GetScore_WhenEarlyTimingAverageCombination_ReturnsTwo()
        {
            Score score = scoreRuleProvider.GetScore(ShotTiming.Early, Combination.Average);
            Assert.Equal(Score.Two, score);
        }

        [Fact]
        public void GetScore_WhenGoodTimingGoodCombination_ReturnsFour()
        {
            Score score = scoreRuleProvider.GetScore(ShotTiming.Good, Combination.Good);
            Assert.Equal(Score.Four, score);
        }

        [Fact]
        public void GetScore_WhenPerfectTimingPerfectCombination_ReturnsSix()
        {
            Score score = scoreRuleProvider.GetScore(ShotTiming.Perfect, Combination.Perfect);
            Assert.Equal(Score.Six, score);
        }

        [Fact]
        public void GetScore_WhenIncorrectTimingCombination_ThrowsError()
        {
            Assert.Throws<InvalidDataException>(
                () => scoreRuleProvider.GetScore((ShotTiming)99, (Combination)99)
            );
        }

        [Fact]
        public void ScoreRuleDataCount_MatchWithShotTimingCombinationcount()
        {
            int shotTimingCount = Enum.GetValues(typeof(ShotTiming)).Length;
            int combinationCount = Enum.GetValues(typeof(Combination)).Length;
            int scoreCount = 0;
            foreach (var shotTimingObj in scoreRuleProvider.ScoreRules)
            {
                scoreCount += shotTimingObj.Value.Count;
            }
            Assert.Equal(shotTimingCount * combinationCount, scoreCount);
        }
    }
}
