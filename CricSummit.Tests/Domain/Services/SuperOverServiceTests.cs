using CricSummit.Domain.DomainServices;
using CricSummit.Domain.ValueObjects;
using Microsoft.Extensions.Logging.Abstractions;

namespace CricSummit.Tests.Domain.Services
{
    public class SuperOverServiceTests
    {
        private readonly NullLogger<SuperOverService> _logger =
            NullLogger<SuperOverService>.Instance;

        [Fact]
        public void GenerateBowlingTypes_ReturnsSixBowlingTypes()
        {
            ISuperOverService superOverService = new SuperOverService(_logger);
            Assert.Equal(6, superOverService.GenerateBowlingTypes().Count);
        }

        [Fact]
        public void GenerateBowlingTypes_DefinedInEnumBowlingType()
        {
            ISuperOverService superOverService = new SuperOverService(_logger);

            foreach (var bowl in superOverService.GenerateBowlingTypes())
            {
                Assert.False(!Enum.IsDefined(typeof(BowlingType), bowl));
            }
        }

        [Fact]
        public void GenerateBowler_WhenValidBowlersList_RetursBowler()
        {
            ISuperOverService superOverService = new SuperOverService(_logger);
            string selectedBowler = superOverService.GenerateBowler();
            Assert.NotEmpty(selectedBowler);
        }

        [Fact]
        public void GenerateBatters_WhenValidBattersList_ReturnsThreeBattersLlist()
        {
            ISuperOverService superOverService = new SuperOverService(_logger);
            List<string> selectedBatters = superOverService.GenerateBatters();
            Assert.Equal(3, selectedBatters.Count);
            Assert.All(selectedBatters, item => Assert.NotEmpty(item));
        }

        [Fact]
        public void GenerateTarget_ReturnsValidTargetScore()
        {
            ISuperOverService superOverService = new SuperOverService(_logger);
            int targetScore = superOverService.GenerateTarget();
            Assert.InRange(targetScore, 15, 21);
        }

        [Fact]
        public void IsInningsOver_ReturnsFalse()
        {
            ISuperOverService superOverService = new SuperOverService(_logger);
            bool over = superOverService.IsInningsOver(1);
            Assert.False(over);
        }

        [Fact]
        public void IsInningsOver_ReturnsTrue()
        {
            ISuperOverService superOverService = new SuperOverService(_logger);
            bool over = superOverService.IsInningsOver(2);
            Assert.True(over);
        }

        [Fact]
        public void IsTargetChased_WhenScoreLessThanTarget_ReturnsFalse()
        {
            ISuperOverService superOverService = new SuperOverService(_logger);
            bool chased = superOverService.IsTargetChased(16, 21);
            Assert.False(chased);
        }

        [Fact]
        public void IsTargetChased_WhenScoreEqualToTarget_ReturnsTrue()
        {
            ISuperOverService superOverService = new SuperOverService(_logger);
            bool chased = superOverService.IsTargetChased(21, 21);
            Assert.True(chased);
        }
    }
}
