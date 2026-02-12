using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CricSummit.Domain.Rules
{
    public class CombinationRuleProvider : ICombinationRuleProvider
    {
        private readonly Dictionary<BowlingType, Dictionary<BattingType, Combination>> _rules =
        [
            ];
        public IReadOnlyDictionary<
            BowlingType,
            IReadOnlyDictionary<BattingType, Combination>
        > CombinationRules =>
            _rules.ToDictionary(
                x => x.Key,
                x => (IReadOnlyDictionary<BattingType, Combination>)x.Value
            );
        private readonly ILogger<CombinationRuleProvider> _logger;

        public CombinationRuleProvider(ILogger<CombinationRuleProvider> logger)
        {
            _logger = logger;
            _logger.LogInformation("Start generating combination rule data");
            InitializeCombinations();
            ApplyPerfectCombinations();
            ApplyGoodCombination();
            ApplyPoorCombination();
            _logger.LogInformation("Generated combination rule data");
        }

        private void InitializeCombinations()
        {
            foreach (BowlingType bowl in Enum.GetValues(typeof(BowlingType)))
            {
                _rules[bowl] =  [ ];
                foreach (BattingType bat in Enum.GetValues(typeof(BattingType)))
                {
                    _rules[bowl][bat] = Combination.Average;
                }
            }
        }

        private void ApplyPerfectCombinations()
        {
            // Fast & Bouncer
            MakePerfectCombination(BowlingType.Fast, BattingType.PullShot, BattingType.HookShot);
            MakePerfectCombination(BowlingType.Bouncer, BattingType.PullShot, BattingType.HookShot);

            // Swing & Seam
            MakePerfectCombination(BowlingType.Swing, BattingType.CoverDrive, BattingType.LateCut);
            MakePerfectCombination(
                BowlingType.Seam,
                BattingType.CoverDrive,
                BattingType.StraightDrive
            );

            // Yorker
            MakePerfectCombination(
                BowlingType.Yorker,
                BattingType.DefensiveBlock,
                BattingType.StraightDrive
            );

            // Spin
            MakePerfectCombination(
                BowlingType.OffSpin,
                BattingType.SweepShot,
                BattingType.ReverseSweep
            );
            MakePerfectCombination(
                BowlingType.LegSpin,
                BattingType.SweepShot,
                BattingType.ReverseSweep
            );
            MakePerfectCombination(BowlingType.Doosra, BattingType.SweepShot);

            // Variations
            MakePerfectCombination(BowlingType.SlowerBall, BattingType.StraightDrive);
            MakePerfectCombination(BowlingType.ReverseSwing, BattingType.LateCut);
        }

        private void ApplyGoodCombination()
        {
            // Fast bowling
            MakeGoodCombination(
                BowlingType.Fast,
                BattingType.StraightDrive,
                BattingType.DefensiveBlock
            );

            // Swing / Seam
            MakeGoodCombination(BowlingType.Swing, BattingType.StraightDrive);
            MakeGoodCombination(BowlingType.Seam, BattingType.LateCut);

            // Yorker
            MakeGoodCombination(BowlingType.Yorker, BattingType.DefensiveBlock);

            // Spin
            MakeGoodCombination(BowlingType.OffSpin, BattingType.DefensiveBlock);
            MakeGoodCombination(BowlingType.LegSpin, BattingType.DefensiveBlock);

            // Slower / Reverse
            MakeGoodCombination(BowlingType.SlowerBall, BattingType.DefensiveBlock);
            MakeGoodCombination(BowlingType.ReverseSwing, BattingType.CoverDrive);
        }

        private void ApplyPoorCombination()
        {
            // Short balls + sweep
            MakePoorCombination(BowlingType.Fast, BattingType.SweepShot);
            MakePoorCombination(BowlingType.Bouncer, BattingType.SweepShot);

            // Yorker + attacking shots
            MakePoorCombination(
                BowlingType.Yorker,
                BattingType.PullShot,
                BattingType.HookShot,
                BattingType.LoftedShot
            );

            // Spin + pull/hook
            MakePoorCombination(BowlingType.OffSpin, BattingType.PullShot);
            MakePoorCombination(BowlingType.LegSpin, BattingType.HookShot);

            // Swing + risky shots
            MakePoorCombination(BowlingType.Swing, BattingType.LoftedShot);
            MakePoorCombination(BowlingType.Seam, BattingType.LoftedShot);
        }

        private void MakePerfectCombination(
            BowlingType bowlingType,
            params BattingType[] battingTypes
        )
        {
            foreach (BattingType bat in battingTypes)
            {
                _rules[bowlingType][bat] = Combination.Perfect;
            }
        }

        private void MakeGoodCombination(BowlingType bowlingType, params BattingType[] battingTypes)
        {
            foreach (BattingType bat in battingTypes)
            {
                _rules[bowlingType][bat] = Combination.Good;
            }
        }

        private void MakePoorCombination(BowlingType bowlingType, params BattingType[] battingTypes)
        {
            foreach (BattingType bat in battingTypes)
            {
                _rules[bowlingType][bat] = Combination.Poor;
            }
        }

        public Combination GetCombination(BowlingType bowlingType, BattingType battingType)
        {
            if (
                _rules.TryGetValue(bowlingType, out var value)
                && value.TryGetValue(battingType, out var combination)
            )
            {
                return combination;
            }
            _logger.LogWarning(
                "Combination not found for BowlingType : {bowlingType} BattingType : {battingType}",
                bowlingType,
                battingType
            );
            throw new InvalidDataException();
        }
    }
}
