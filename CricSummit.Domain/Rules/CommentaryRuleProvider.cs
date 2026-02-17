using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CricSummit.Domain.Rules
{
    public class CommentaryRuleProvider : ICommentaryRuleProvider
    {
        private ILogger<CommentaryRuleProvider> _logger;
        private Dictionary<Score, List<string>> _commentaryRules =  [ ];

        public CommentaryRuleProvider(ILogger<CommentaryRuleProvider> logger)
        {
            _logger = logger;
            _logger.LogInformation("Start Generating Commentary rules");
            InitializeCommentary();
            _logger.LogInformation("Generated commentary rules data");
        }

        private void InitializeCommentary()
        {
            _commentaryRules[Score.Wicket] =
            [
                "It's a wicket!",
                "Edged and taken!",
                "Clean bowled!",
                "Huge breakthrough!"
            ];
            _commentaryRules[Score.Zero] =
            [
                "Excellent line and length.",
                "Dot ball pressure builds.",
                "Defended well.",
                "No run."
            ];
            _commentaryRules[Score.One] =
            [
                "Quick single taken.",
                "Good running between the wickets.",
                "Rotates the strike."
            ];

            _commentaryRules[Score.Two] =
            [
                "Excellent running between the wickets.",
                "Convert ones into twos.",
                "Great effort for two."
            ];

            _commentaryRules[Score.Three] =
            [
                "Brilliant placement for three.",
                "They come back for the third!"
            ];

            _commentaryRules[Score.Four] =
            [
                "Just over the fielder.",
                "Excellent effort on the boundary but it's four.",
                "Cracked through the covers!"
            ];

            _commentaryRules[Score.Six] =
            [
                "That's massive and out of the ground!",
                "It's a huge hit!",
                "Into the crowd!",
                "What a six!"
            ];
        }

        public List<string> GetCommentary(Score score)
        {
            if (_commentaryRules.TryGetValue(score, out var commentaryVals))
            {
                return commentaryVals;
            }
            _logger.LogWarning("Commentary not found for given score {score}", score);
            throw new KeyNotFoundException("Invalid score");
        }
    }
}
