using CricSummit.Application.DTO;
using CricSummit.Application.Interfaces;
using CricSummit.Application.Services;
using CricSummit.Domain.DomainServices;
using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace CricSummit.Tests.Application.Services
{
    public class SuperOverCommentaryServiceTests
    {
        private readonly IScoreCommentaryService _scoreCommentaryService;
        private readonly ISuperOverService _superOverService;
        private readonly ISuperOverCommentaryService _superOverCommentaryService;

        public SuperOverCommentaryServiceTests()
        {
            _scoreCommentaryService = Substitute.For<IScoreCommentaryService>();
            _superOverService = Substitute.For<ISuperOverService>();
            _superOverCommentaryService = new SuperOverCommentaryService(
                NullLogger<SuperOverCommentaryService>.Instance,
                _scoreCommentaryService,
                _superOverService
            );
            SetupDefaults();
        }

        private readonly List<SuperOverRequestDto> _requestData =
        [
            .. Enumerable
            .Range(0, 6)
            .Select(
                _ =>
                    new SuperOverRequestDto
                    {
                        BattingType = BattingType.CoverDrive,
                        ShotTiming = ShotTiming.Early
                    }
            )
        ];

        private void SetupDefaults()
        {
            _superOverService.GenerateBowler().Returns("Ani");
            _superOverService.GenerateBatters().Returns([ "A", "B", "C" ]);
            _superOverService.GenerateTarget().Returns(18);
            _superOverService
                .GenerateBowlingTypes()
                .Returns([ .. Enumerable.Range(0, 6).Select(_ => BowlingType.Fast) ]);
        }

        [Fact]
        public void PlaySuperOver_InvalidInput_ThrowsError()
        {
            List<SuperOverRequestDto> input =  [ new SuperOverRequestDto() ];
            Assert.Throws<ArgumentException>(
                () => _superOverCommentaryService.PlaySuperOver(input)
            );
        }

        [Fact]
        public void PlaySuperOver_TargetChased_ReturnsWon()
        {
            _scoreCommentaryService
                .GetScoreAndCommentary(
                    Arg.Any<BowlingType>(),
                    Arg.Any<BattingType>(),
                    Arg.Any<ShotTiming>()
                )
                .Returns(new ScoreCommentaryDto { Score = Score.Four, Commentary = "Great Shot" });
            var result = _superOverCommentaryService.PlaySuperOver(_requestData);
            Assert.Equal(MatchResult.Won, result.ResultMessage);
            Assert.True(result.FinalScore > 18);
        }

        [Fact]
        public void PlaySuperOver_TargetNotChased_ReturnsLost()
        {
            _scoreCommentaryService
                .GetScoreAndCommentary(
                    Arg.Any<BowlingType>(),
                    Arg.Any<BattingType>(),
                    Arg.Any<ShotTiming>()
                )
                .Returns(
                    new ScoreCommentaryDto
                    {
                        Score = Score.Two,
                        Commentary = "Great effort for two"
                    }
                );
            var result = _superOverCommentaryService.PlaySuperOver(_requestData);
            Assert.Equal(MatchResult.Lost, result.ResultMessage);
            Assert.True(result.FinalScore == 12);
        }

        [Fact]
        public void PlaySuperOver_TwoWicketsFall_InningsEnd()
        {
            _scoreCommentaryService
                .GetScoreAndCommentary(
                    Arg.Any<BowlingType>(),
                    Arg.Any<BattingType>(),
                    Arg.Any<ShotTiming>()
                )
                .Returns(new ScoreCommentaryDto { Score = Score.Wicket, Commentary = "Out" });
            var result = _superOverCommentaryService.PlaySuperOver(_requestData);
            Assert.Equal(MatchResult.Lost, result.ResultMessage);
            Assert.True(result.FinalScore == 0);
        }

        [Fact]
        public void PlaySuperOver_ScoreEqualsTarget_ReturnsTie()
        {
            _scoreCommentaryService
                .GetScoreAndCommentary(
                    Arg.Any<BowlingType>(),
                    Arg.Any<BattingType>(),
                    Arg.Any<ShotTiming>()
                )
                .Returns(
                    new ScoreCommentaryDto { Score = Score.Three, Commentary = "Brilliant play" }
                );
            var result = _superOverCommentaryService.PlaySuperOver(_requestData);
            Assert.Equal(MatchResult.Tie, result.ResultMessage);
            Assert.True(result.FinalScore == 18);
        }

        [Fact]
        public void PlaySuperOver_ValidInput_ReturnsCommentary()
        {
            _scoreCommentaryService
                .GetScoreAndCommentary(
                    Arg.Any<BowlingType>(),
                    Arg.Any<BattingType>(),
                    Arg.Any<ShotTiming>()
                )
                .Returns(new ScoreCommentaryDto { Score = Score.Zero, Commentary = "No run" });
            var result = _superOverCommentaryService.PlaySuperOver(_requestData);
            Assert.NotEmpty(result.Commentary);
        }
    }
}
