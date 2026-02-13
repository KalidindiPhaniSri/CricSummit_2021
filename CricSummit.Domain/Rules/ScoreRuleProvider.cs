using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CricSummit.Domain.Rules
{
    public class ScoreRuleProvider : IScoreRuleProvider
    {
        private readonly ILogger<ScoreRuleProvider> _logger;
        private readonly Dictionary<ShotTiming, Dictionary<Combination, Score>> _rules =  [ ];
        public IReadOnlyDictionary<
            ShotTiming,
            IReadOnlyDictionary<Combination, Score>
        > ScoreRules =>
            _rules.ToDictionary(x => x.Key, x => (IReadOnlyDictionary<Combination, Score>)x.Value);

        public ScoreRuleProvider(ILogger<ScoreRuleProvider> logger)
        {
            _logger = logger;
            _logger.LogInformation("start generating score based on combination and shot timing");
            InitializeScore();
            ValidateScoreRules();
            _logger.LogInformation("Generated scores rules data");
        }

        private void InitializeScore()
        {
            _rules[ShotTiming.Late] = new Dictionary<Combination, Score>
            {
                { Combination.Poor, Score.Zero },
                { Combination.Average, Score.One },
                { Combination.Good, Score.Two },
                { Combination.Perfect, Score.Three },
            };

            _rules[ShotTiming.Early] = new Dictionary<Combination, Score>
            {
                { Combination.Poor, Score.One },
                { Combination.Average, Score.Two },
                { Combination.Good, Score.Three },
                { Combination.Perfect, Score.Four },
            };

            _rules[ShotTiming.Good] = new Dictionary<Combination, Score>
            {
                { Combination.Poor, Score.Two },
                { Combination.Average, Score.Three },
                { Combination.Good, Score.Four },
                { Combination.Perfect, Score.Four },
            };

            _rules[ShotTiming.Perfect] = new Dictionary<Combination, Score>
            {
                { Combination.Poor, Score.Three },
                { Combination.Average, Score.Four },
                { Combination.Good, Score.Four },
                { Combination.Perfect, Score.Six },
            };
        }

        public void ValidateScoreRules()
        {
            foreach (ShotTiming shotTiming in Enum.GetValues(typeof(ShotTiming)))
            {
                if (!_rules.TryGetValue(shotTiming, out var ScoreRule))
                {
                    throw new InvalidDataException($"Missing ScoreRule for {shotTiming}");
                }
                foreach (Combination combination in Enum.GetValues(typeof(Combination)))
                {
                    if (!ScoreRule.TryGetValue(combination, out var score))
                    {
                        throw new InvalidDataException(
                            $"Missing ScoreRule for {shotTiming} and {combination}"
                        );
                    }
                }
            }
        }

        public Score GetScore(ShotTiming shotTiming, Combination combination)
        {
            if (_rules.TryGetValue(shotTiming, out var scoreRule))
            {
                if (scoreRule.TryGetValue(combination, out var score))
                {
                    return score;
                }
            }
            _logger.LogWarning(
                "Score not found for {shotTiming} and {combination}",
                shotTiming,
                combination
            );
            throw new InvalidDataException($"Score not found for {shotTiming} and {combination}");
        }

        // private Score AssignScore(ShotTiming shotTiming, Combination combination)
        // {
        //     if (shotTiming == ShotTiming.Perfect && combination == Combination.Perfect)
        //     {
        //         return Score.Six;
        //     }
        //     if (shotTiming >= ShotTiming.Good && combination >= Combination.Good)
        //     {
        //         return Score.Four;
        //     }
        //     if (
        //         shotTiming >= ShotTiming.Good && combination >= Combination.Average
        //         || shotTiming >= ShotTiming.Early && combination >= Combination.Good
        //     )
        //     {
        //         return Score.Three;
        //     }
        //     if (shotTiming >= ShotTiming.Early && combination >= Combination.Average)
        //     {
        //         return Score.Two;
        //     }
        //     if (
        //         shotTiming >= ShotTiming.Perfect && combination >= Combination.Poor
        //         || shotTiming >= ShotTiming.Late && combination >= Combination.Perfect
        //     )
        //     {
        //         return Score.One;
        //     }
        //     return Score.Wicket;
        // }
    }
}
