using CricSummit.Application.Interfaces;
using CricSummit.Application.Services;
using CricSummit.Domain.Rules;
using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CricSummit.Tests.Application.Services
{
    public class ScoreCommentaryServiceTests
    {
        private readonly NullLogger<ScoreCommentaryService> _logger =
            NullLogger<ScoreCommentaryService>.Instance;
        private readonly IPredictScoreService _predictScore =
            Substitute.For<IPredictScoreService>();
        private readonly ICommentaryRuleProvider _commentaryRule =
            Substitute.For<ICommentaryRuleProvider>();

        [Fact]
        public void GetScoreAndCommentary_WhenValidInput_ReturnsData()
        {
            //Arrange
            IScoreCommentaryService scoreCommentary = new ScoreCommentaryService(
                _logger,
                _predictScore,
                _commentaryRule
            );
            _predictScore
                .EvaluateScore(BowlingType.Bouncer, BattingType.CoverDrive, ShotTiming.Good)
                .Returns(Score.Three);
            _commentaryRule
                .GetCommentary(Score.Three)
                .Returns([ "Brilliant placement for three.", "They come back for the third!" ]);
            //Act
            var result = scoreCommentary.GetScoreAndCommentary(
                BowlingType.Bouncer,
                BattingType.CoverDrive,
                ShotTiming.Good
            );
            //Assert
            Assert.Equal(Score.Three, result.Score);
            Assert.False(string.IsNullOrWhiteSpace(result.Commentary));
        }

        [Fact]
        public void GetScore_WhenInvalidInput_ThrowsError()
        {
            //Arrange
            IScoreCommentaryService scoreCommentary = new ScoreCommentaryService(
                _logger,
                _predictScore,
                _commentaryRule
            );
            _predictScore
                .EvaluateScore(BowlingType.Bouncer, BattingType.CoverDrive, (ShotTiming)999)
                .Throws<ArgumentException>();
            //Act
            Assert.Throws<ArgumentException>(
                () =>
                    scoreCommentary.GetScoreAndCommentary(
                        BowlingType.Bouncer,
                        BattingType.CoverDrive,
                        (ShotTiming)999
                    )
            );
        }
    }
}
