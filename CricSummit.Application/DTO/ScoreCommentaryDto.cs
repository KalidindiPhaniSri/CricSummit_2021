using CricSummit.Domain.ValueObjects;

namespace CricSummit.Application.DTO
{
    public class ScoreCommentaryDto
    {
        public Score Score { get; set; }
        public required string Commentary { get; set; }

        // Deconstruct method for tuple assignment
        public void Deconstruct(out Score score, out string commentary)
        {
            score = this.Score;
            commentary = this.Commentary;
        }
    }
}
