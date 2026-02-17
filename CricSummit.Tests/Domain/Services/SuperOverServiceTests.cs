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
    }
}
