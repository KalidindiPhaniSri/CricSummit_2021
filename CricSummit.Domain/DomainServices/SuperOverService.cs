using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CricSummit.Domain.DomainServices
{
    public class SuperOverService : ISuperOverService
    {
        private readonly ILogger<ISuperOverService> _logger;

        public SuperOverService(ILogger<ISuperOverService> logger)
        {
            _logger = logger;
        }

        private readonly Random _random = new();

        public List<BowlingType> GenerateBowlingTypes()
        {
            List<BowlingType> over = Enumerable
                .Range(0, 6)
                .Select(_ => (BowlingType)_random.Next(Enum.GetValues(typeof(BowlingType)).Length))
                .ToList();
            _logger.LogInformation("Generated super over bowling types");
            return over;
        }

        public string GenerateBowler()
        {
            List<string> bowlers =
            [
                "James",
                "William",
                "Oliver",
                "Henry",
                "Thomas",
                "George",
                "Edward",
                "Charles",
                "Arthur",
                "Harry"
            ];

            _logger.LogInformation("Generated super over selected bowler ");

            return bowlers[_random.Next(bowlers.Count)];
        }

        public List<string> GenerateBatters()
        {
            List<string> batters =
            [
                "Jack",
                "Samuel",
                "Daniel",
                "Joseph",
                "Benjamin",
                "Alexander",
                "Leo",
                "Oscar",
                "Jacob",
                "Frederick"
            ];

            _logger.LogInformation("Generated super over batters participant list");
            // orderby is assigning each key with a _random number and sort it with the key
            return batters.OrderBy(_ => _random.Next()).Take(3).ToList();
        }

        public int GenerateTarget()
        {
            int target = _random.Next(15, 21);
            _logger.LogInformation("Generated super over target {target}", target);
            return target;
        }

        public bool IsInningsOver(int wicketsCount)
        {
            bool over = wicketsCount >= 2;
            _logger.LogInformation(
                "IsInningsOver? Wicket Count : {wicketcount}, Result : {result}",
                wicketsCount,
                over
            );
            return over;
        }

        public bool IsTargetChased(int score, int target)
        {
            bool chased = score >= target;
            _logger.LogInformation(
                "IsTargetChased? Score : {score}, Result : {result}",
                score,
                chased
            );
            return chased;
        }
    }
}
