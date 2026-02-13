using CricSummit.Domain.ValueObjects;

namespace CricSummit.Application.Interfaces
{
    public interface IPredictScore
    {
        Score EvaluateScore(
            BowlingType bowlingType,
            BattingType battingType,
            ShotTiming shotTiming
        );
    }
}
