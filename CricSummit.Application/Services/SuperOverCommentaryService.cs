using System.Text.RegularExpressions;
using CricSummit.Application.DTO;
using CricSummit.Application.Interfaces;
using CricSummit.Domain.DomainServices;
using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CricSummit.Application.Services
{
    public class SuperOverCommentaryService : ISuperOverCommentaryService
    {
        private readonly ILogger<SuperOverCommentaryService> _logger;
        private readonly IScoreCommentaryService _scoreCommentaryService;
        private readonly ISuperOverService _superOverService;

        public SuperOverCommentaryService(
            ILogger<SuperOverCommentaryService> logger,
            IScoreCommentaryService scoreCommentaryService,
            ISuperOverService superOverService
        )
        {
            _logger = logger;
            _scoreCommentaryService = scoreCommentaryService;
            _superOverService = superOverService;
        }

        private void ValidateInput(List<SuperOverRequestDto> input)
        {
            if (input.Count != 6)
            {
                _logger.LogWarning("Super over request data contain less than 6 deliveries");
                throw new ArgumentException("Super over requires exactly 6 deliveries");
            }
        }

        internal class SuperOverMatch
        {
            public required string Bowler { get; set; }
            public required List<string> Batters { get; set; }
            public required List<BowlingType> BowlingTypes { get; set; }
            public int TargetScore { get; set; }

            public int BallIndex { get; set; } = 0;
            public int Score { get; set; } = 0;
            public int Wickets { get; set; } = 0;

            public required string Striker { get; set; }
            public required string NonStriker { get; set; }

            public required List<SuperOverCommentaryDto> Commentary { get; set; }
        }

        private SuperOverMatch InitializeMatch()
        {
            List<string> batters = _superOverService.GenerateBatters();
            return new SuperOverMatch
            {
                Bowler = _superOverService.GenerateBowler(),
                Batters = batters,
                BowlingTypes = _superOverService.GenerateBowlingTypes(),
                TargetScore = _superOverService.GenerateTarget(),
                Commentary =  [ ],
                Striker = batters[0],
                NonStriker = batters[1]
            };
        }

        private void UpdateMatchState(SuperOverMatch match, int? runs, Score score)
        {
            if (!runs.HasValue || ScoreExtensions.IsWicket(score))
            {
                match.Wickets++;
                if (match.Wickets < 2)
                    match.Striker = match.Batters[match.Wickets];

                return;
            }

            if (runs % 2 != 0)
            {
                (match.NonStriker, match.Striker) = (match.Striker, match.NonStriker);
            }
            match.Score += runs.Value;
        }

        private void AddCommentary(
            SuperOverMatch match,
            BowlingType bowlingType,
            SuperOverRequestDto input,
            string comment,
            int? runs
        )
        {
            SuperOverCommentaryDto superOverCommentaryDto =
                new()
                {
                    Bowler = match.Bowler,
                    BowlingType = bowlingType,
                    Batter = match.Striker,
                    BattingType = input.BattingType,
                    ShotTiming = input.ShotTiming,
                    Commentary = comment,
                    Score = runs == null ? "Wicket" : $"{runs} runs"
                };
            match.Commentary.Add(superOverCommentaryDto);
        }

        private void ProcessDelivery(
            SuperOverMatch match,
            BowlingType bowlingType,
            SuperOverRequestDto input
        )
        {
            var (score, comment) = _scoreCommentaryService.GetScoreAndCommentary(
                bowlingType,
                input.BattingType,
                input.ShotTiming
            );
            int? runs = ScoreExtensions.ToRuns(score);
            UpdateMatchState(match, runs, score);
            AddCommentary(match, bowlingType, input, comment, runs);
            match.BallIndex++;
        }

        private bool IsMatchFinished(SuperOverMatch match)
        {
            return _superOverService.IsTargetChased(match.Score, match.TargetScore)
                || _superOverService.IsInningsOver(match.Wickets);
        }

        private SuperOverResultDto BuildMatchResult(SuperOverMatch match)
        {
            if (_superOverService.IsTargetChased(match.Score, match.TargetScore))
            {
                _logger.LogInformation("Australia won the match");
                return CreateResult(match, MatchResult.Won);
            }

            _logger.LogInformation("Australia lost the match");
            return CreateResult(match, MatchResult.Lost);
        }

        private SuperOverResultDto CreateResult(SuperOverMatch match, MatchResult matchResult)
        {
            return new SuperOverResultDto
            {
                Commentary = match.Commentary,
                FinalScore = match.Score,
                ResultMessage = matchResult,
            };
        }

        private SuperOverResultDto BuildFinalResult(SuperOverMatch match)
        {
            if (match.Score == match.TargetScore)
            {
                _logger.LogInformation(
                    "Match tied again with score : {score} target score : {target}",
                    match.Score,
                    match.TargetScore
                );
                return CreateResult(match, MatchResult.Tie);
            }
            _logger.LogInformation("Australia lost the match");
            return CreateResult(match, MatchResult.Lost);
        }

        public SuperOverResultDto PlaySuperOver(List<SuperOverRequestDto> input)
        {
            _logger.LogInformation("Started super over match");
            ValidateInput(input);
            try
            {
                SuperOverMatch match = InitializeMatch();

                foreach (BowlingType bowlingType in match.BowlingTypes)
                {
                    ProcessDelivery(match, bowlingType, input[match.BallIndex]);
                    if (IsMatchFinished(match))
                        return BuildMatchResult(match);
                }

                return BuildFinalResult(match);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Invalid super over request data");
                throw new ArgumentException("Invalid super over request data ", ex);
            }
        }
    }
}
