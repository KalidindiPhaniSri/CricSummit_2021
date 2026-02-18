using CricSummit.Domain.ValueObjects;

namespace CricSummit.Application.DTO
{
    public class SuperOverResultDto
    {
        public required List<SuperOverCommentaryDto> Commentary { get; set; }
        public int FinalScore { get; set; }
        public required MatchResult ResultMessage { get; set; }
    }
}
