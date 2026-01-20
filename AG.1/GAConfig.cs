using System;

namespace AG._1
{
    /// <summary>
    /// Configuration for the genetic algorithm with deterministic seeding support.
    /// </summary>
    public class GAConfig
    {
        public int PopulationSize { get; set; } = 100;
        public int OffspringSize { get; set; } = 40;
        public double MutationRate { get; set; } = 0.1;
        public double InitialMutationStrength { get; set; } = 1.0;
        public double MutationDecayRate { get; set; } = 0.995;
        public double MinMutationStrength { get; set; } = 0.01;
        public int TournamentSize { get; set; } = 3;
        public double ArithmeticCrossoverAlpha { get; set; } = 0.5;
        public double ConvergenceThreshold { get; set; } = 0.001;
        public int MaxGenerationsWithoutImprovement { get; set; } = 40;
        public int? Seed { get; set; } = null;

        public Random CreateRandom()
        {
            return Seed.HasValue ? new Random(Seed.Value) : new Random();
        }
    }
}
