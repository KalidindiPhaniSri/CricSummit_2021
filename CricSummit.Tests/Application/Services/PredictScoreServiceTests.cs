using CricSummit.Application.Interfaces;
using CricSummit.Application.Services;
using CricSummit.Domain.Rules;
using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CricSummit.Tests.Application.Services
{
    public class PredictScoreServiceTests
    {
        private readonly ICombinationRuleProvider comboProvider =
            Substitute.For<ICombinationRuleProvider>();
        private readonly IScoreRuleProvider scoreProvider = Substitute.For<IScoreRuleProvider>();

        [Fact]
        public void EvaluateScore_ReturnsExpectedScore()
        {
            //Arrange
            IPredictScoreService service = new PredictScoreService(
                NullLogger<PredictScoreService>.Instance,
                comboProvider,
                scoreProvider
            );

            comboProvider
                .GetCombination(BowlingType.Fast, BattingType.HookShot)
                .Returns(Combination.Perfect);

            scoreProvider.GetScore(ShotTiming.Perfect, Combination.Perfect).Returns(Score.Six);
            //Act
            var result = service.EvaluateScore(
                BowlingType.Fast,
                BattingType.HookShot,
                ShotTiming.Perfect
            );
            //Assert
            Assert.Equal(Score.Six, result);
        }

        [Fact]
        public void EvaluateScore_WhenWrongInput_ThrowsError()
        {
            //Arrange
            IPredictScoreService service = new PredictScoreService(
                NullLogger<PredictScoreService>.Instance,
                comboProvider,
                scoreProvider
            );
            comboProvider
                .GetCombination((BowlingType)99, BattingType.CoverDrive)
                .Throws<ArgumentException>();
            //Act
            Assert.Throws<ArgumentException>(
                () =>
                    service.EvaluateScore((BowlingType)99, BattingType.CoverDrive, ShotTiming.Early)
            );
        }
    }
}
