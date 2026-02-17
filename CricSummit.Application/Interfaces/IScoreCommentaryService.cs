using CricSummit.Application.DTO;
using CricSummit.Domain.ValueObjects;

namespace CricSummit.Application.Interfaces
{
    public interface IScoreCommentaryService
    {
        ScoreCommentaryDto GetScoreAndCommentary(
            BowlingType bowlingType,
            BattingType battingType,
            ShotTiming shotTiming
        );
    }
}
