using Xunit;
using AG._1;

namespace AG._1.Tests
{
    public class GAConfigTests
    {
        [Fact]
        public void CreateRandom_WithSeed_IsDeterministic()
        {
            var config = new GAConfig { Seed = 42 };

            var random1 = config.CreateRandom();
            var random2 = config.CreateRandom();

            Assert.Equal(random1.Next(1000), random2.Next(1000));
        }

        [Fact]
        public void CreateRandom_WithoutSeed_CreatesRandom()
        {
            var config = new GAConfig { Seed = null };

            var random = config.CreateRandom();

            Assert.NotNull(random);
        }

        [Fact]
        public void DefaultValues_AreReasonable()
        {
            var config = new GAConfig();

            Assert.Equal(100, config.PopulationSize);
            Assert.Equal(40, config.OffspringSize);
            Assert.Equal(0.1, config.MutationRate);
            Assert.Equal(3, config.TournamentSize);
            Assert.Equal(0.5, config.ArithmeticCrossoverAlpha);
        }
    }
}
