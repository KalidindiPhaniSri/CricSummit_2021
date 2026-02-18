using CricSummit.Domain.ValueObjects;

namespace CricSummit.Domain.DomainServices
{
    public interface ISuperOverService
    {
        List<BowlingType> GenerateBowlingTypes();
        string GenerateBowler(List<string> bowlers);
        List<string> GenerateBatters(List<string> batters);
        int GenerateTarget();
        bool IsInningsOver(int wicketsCount);
        bool IsTargetChased(int score, int target);
    }
}
