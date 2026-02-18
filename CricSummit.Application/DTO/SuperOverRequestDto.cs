using CricSummit.Domain.ValueObjects;

namespace CricSummit.Application.DTO
{
    public class SuperOverRequestDto
    {
        public BattingType BattingType { get; set; }
        public ShotTiming ShotTiming { get; set; }
    }
}
