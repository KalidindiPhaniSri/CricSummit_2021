using CricSummit.Domain.Rules;
using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging.Abstractions;

namespace CricSummit.Tests.Domain.Rules
{
    public class CommentaryRuleProviderTests
    {
        private readonly ICommentaryRuleProvider _commentaryRuleProvider =
            new CommentaryRuleProvider(NullLogger<CommentaryRuleProvider>.Instance);

        [Fact]
        public void GetCommentary_WhenValidScore_ReturnsData()
        {
            List<string> commentary = _commentaryRuleProvider.GetCommentary(Score.One);
            Assert.True(commentary.Count > 0);
        }

        [Fact]
        public void GetCommentary_WhenInvalidScore_ThrowsError()
        {
            Assert.Throws<KeyNotFoundException>(
                () => _commentaryRuleProvider.GetCommentary((Score)10)
            );
        }
    }
}
