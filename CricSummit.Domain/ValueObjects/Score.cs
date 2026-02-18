namespace CricSummit.Domain.ValueObjects
{
    public enum Score
    {
        Wicket,
        Zero,
        One,
        Two,
        Three,
        Four,
        Six
    }

    public static class ScoreExtensions
    {
        public static int? ToRuns(this Score score)
        {
            return score switch
            {
                Score.Zero => 0,
                Score.One => 1,
                Score.Two => 2,
                Score.Three => 3,
                Score.Four => 4,
                Score.Six => 6,
                Score.Wicket => null,
                _ => throw new ArgumentOutOfRangeException(nameof(score), "Unknown score")
            };
        }

        public static bool IsWicket(this Score score)
        {
            return Score.Wicket == score;
        }
    }
}
