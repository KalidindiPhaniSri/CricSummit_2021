using CricSummit.Domain.ValueObjects;

namespace CricSummit.Application.Interfaces
{
    public interface IPredictScoreService
    {
        Score EvaluateScore(
            BowlingType bowlingType,
            BattingType battingType,
            ShotTiming shotTiming
        );
    }
}
