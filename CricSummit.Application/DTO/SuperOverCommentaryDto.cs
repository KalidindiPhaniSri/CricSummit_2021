using CricSummit.Domain.ValueObjects;

namespace CricSummit.Application.DTO
{
    public class SuperOverCommentaryDto
    {
        public required string Bowler { get; set; }
        public BowlingType BowlingType { get; set; }
        public required string Batter { get; set; }
        public BattingType BattingType { get; set; }
        public ShotTiming ShotTiming { get; set; }
        public required string Commentary { get; set; }
        public required string Score { get; set; }
    }
}
