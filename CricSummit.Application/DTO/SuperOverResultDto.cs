using CricSummit.Domain.ValueObjects;

namespace CricSummit.Application.DTO
{
    public class SuperOverResultDto
    {
        public required List<SuperOverCommentaryDto> Commentary { get; set; }
        public int FinalScore { get; set; }
        public required MatchResult ResultMessage { get; set; }
        public int ScoreDifference { get; set; }

        public void Deconstruct(
            out List<SuperOverCommentaryDto> commentary,
            out int finalScore,
            out MatchResult resultMessage,
            out int scoreDifference
        )
        {
            commentary = this.Commentary;
            finalScore = this.FinalScore;
            resultMessage = this.ResultMessage;
            scoreDifference = this.ScoreDifference;
        }
    }
}
