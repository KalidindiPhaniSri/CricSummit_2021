using CricSummit.Domain.ValueObjects;

namespace CricSummit.Domain.DomainServices
{
    public interface ISuperOverService
    {
        List<BowlingType> GenerateBowlingTypes();
        BowlingType GenerateBowler(List<BowlingType> bowlings);
        List<BattingType> GenerateBatters(List<BattingType> batters);
        int GenerateTarget();
        bool IsInningsOver(int wicketsCount);
        bool IsTargetChased(int score, int target);
    }
}
