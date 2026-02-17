using CricSummit.Domain.ValueObjects;

namespace CricSummit.Application.DTO
{
    public class ScoreCommentaryDto
    {
        public Score Score { get; set; }
        public required string Commentary { get; set; }
    }
}
