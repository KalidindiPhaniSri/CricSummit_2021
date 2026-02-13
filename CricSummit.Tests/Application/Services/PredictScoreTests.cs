using CricSummit.Application.Interfaces;
using CricSummit.Application.Services;
using CricSummit.Domain.Rules;
using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace CricSummit.Tests.Application.Services
{
    public class PredictScoreTests
    {
        [Fact]
        public void EvaluateScore_ReturnsExpectedScore()
        {
            var comboProvider = Substitute.For<ICombinationRuleProvider>();
            var scoreProvider = Substitute.For<IScoreRuleProvider>();

            // comboProvider
            //     .GetCombination(BowlingType.Fast, BattingType.HookShot)
            //     .Returns(Combination.Perfect);

            // scoreProvider.GetScore(ShotTiming.Perfect, Combination.Perfect).Returns(Score.Six);

            IPredictScore service = new PredictScore(
                NullLogger<PredictScore>.Instance,
                comboProvider,
                scoreProvider
            );

            var result = service.EvaluateScore(
                BowlingType.Fast,
                BattingType.HookShot,
                ShotTiming.Perfect
            );

            Assert.Equal(Score.Six, result);
        }

        [Fact]
        public void EvaluateScore_WhenWrongInput_ThrowsError()
        {
            var comboProvider = Substitute.For<ICombinationRuleProvider>();
            var scoreProvider = Substitute.For<IScoreRuleProvider>();
            IPredictScore service = new PredictScore(
                NullLogger<PredictScore>.Instance,
                comboProvider,
                scoreProvider
            );
            Assert.Throws<InvalidDataException>(
                () =>
                    service.EvaluateScore((BowlingType)99, BattingType.CoverDrive, ShotTiming.Early)
            );
        }
    }
}
