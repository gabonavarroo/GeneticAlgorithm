using Xunit;
using AG._1;

namespace AG._1.Tests
{
    public class GAStatisticsTests
    {
        [Fact]
        public void IncrementFitnessEvaluations_TracksCount()
        {
            var stats = new GAStatistics();

            stats.IncrementFitnessEvaluations(10);
            stats.IncrementFitnessEvaluations(5);

            Assert.Equal(15, stats.FitnessEvaluations);
        }

        [Fact]
        public void RecordGeneration_TracksHistory()
        {
            var stats = new GAStatistics();

            stats.RecordGeneration(100.0, 0.5, 10.0, 1.0);
            stats.RecordGeneration(50.0, 0.8, 5.0, 0.9);

            Assert.Equal(2, stats.Generations);
            Assert.Equal(2, stats.FitnessHistory.Count);
            Assert.Equal(2, stats.RSquaredHistory.Count);
            Assert.Equal(50.0, stats.BestFitness);
            Assert.Equal(0.8, stats.BestRSquared);
        }

        [Fact]
        public void MarkConverged_SetsConvergenceMetrics()
        {
            var stats = new GAStatistics();
            stats.IncrementFitnessEvaluations(100);
            stats.RecordGeneration(10.0, 0.96, 1.0, 0.5);

            stats.MarkConverged(0.95);

            Assert.True(stats.Converged);
            Assert.Equal(100, stats.EvaluationsToConvergence);
        }

        [Fact]
        public void MarkConverged_DoesNotConvergeIfBelowThreshold()
        {
            var stats = new GAStatistics();
            stats.RecordGeneration(10.0, 0.90, 1.0, 0.5);

            stats.MarkConverged(0.95);

            Assert.False(stats.Converged);
            Assert.Equal(-1, stats.EvaluationsToConvergence);
        }

        [Fact]
        public void CalculateSuccessProbability_ReturnsCorrectValue()
        {
            var stats = new GAStatistics();
            stats.RecordGeneration(10.0, 0.96, 1.0, 0.5);

            double successAbove = stats.CalculateSuccessProbability(0.95);
            double successBelow = stats.CalculateSuccessProbability(0.99);

            Assert.Equal(1.0, successAbove);
            Assert.Equal(0.0, successBelow);
        }
    }
}
