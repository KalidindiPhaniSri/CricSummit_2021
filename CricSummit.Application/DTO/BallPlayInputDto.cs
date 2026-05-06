using CricSummit.Domain.ValueObjects;

namespace CricSummit.Console.DTO
{
    public class BallPlayInputDto
    {
        public BowlingType Bowl { get; set; }
        public BattingType Bat { get; set; }
        public ShotTiming Timing { get; set; }
    }
}
