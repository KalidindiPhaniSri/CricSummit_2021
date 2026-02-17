using CricSummit.Domain.ValueObjects;

namespace CricSummit.Domain.Rules
{
    public interface ICommentaryRuleProvider
    {
        List<string> GetCommentary(Score score);
    }
}
