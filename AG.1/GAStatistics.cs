using System;
using System.Collections.Generic;
using System.Linq;

namespace AG._1
{
    /// <summary>
    /// Tracks runtime statistics and convergence metrics for the GA.
    /// </summary>
    public class GAStatistics
    {
        public int FitnessEvaluations { get; private set; } = 0;
        public int Generations { get; private set; } = 0;
        public double BestFitness { get; private set; } = double.MaxValue;
        public double BestRSquared { get; private set; } = 0;
        public List<double> FitnessHistory { get; } = new List<double>();
        public List<double> RSquaredHistory { get; } = new List<double>();
        public List<double> DiversityHistory { get; } = new List<double>();
        public List<double> MutationStrengthHistory { get; } = new List<double>();
        public bool Converged { get; private set; } = false;
        public int EvaluationsToConvergence { get; private set; } = -1;

        public void IncrementFitnessEvaluations(int count = 1)
        {
            FitnessEvaluations += count;
        }

        public void RecordGeneration(double bestFitness, double rSquared, double diversity, double mutationStrength)
        {
            Generations++;
            FitnessHistory.Add(bestFitness);
            RSquaredHistory.Add(rSquared);
            DiversityHistory.Add(diversity);
            MutationStrengthHistory.Add(mutationStrength);

            if (bestFitness < BestFitness)
            {
                BestFitness = bestFitness;
            }
            if (rSquared > BestRSquared)
            {
                BestRSquared = rSquared;
            }
        }

        public void MarkConverged(double rSquaredThreshold)
        {
            if (!Converged && BestRSquared >= rSquaredThreshold)
            {
                Converged = true;
                EvaluationsToConvergence = FitnessEvaluations;
            }
        }

        public double CalculateSuccessProbability(double rSquaredThreshold)
        {
            return BestRSquared >= rSquaredThreshold ? 1.0 : 0.0;
        }

        public void PrintSummary()
        {
            Console.WriteLine("\n=== GA Runtime Analysis ===");
            Console.WriteLine($"Total Generations: {Generations}");
            Console.WriteLine($"Total Fitness Evaluations: {FitnessEvaluations}");
            Console.WriteLine($"Best Fitness (SSE): {BestFitness:F6}");
            Console.WriteLine($"Best R²: {BestRSquared:F6}");
            Console.WriteLine($"Converged: {Converged}");
            if (Converged)
            {
                Console.WriteLine($"Evaluations to Convergence: {EvaluationsToConvergence}");
            }
            Console.WriteLine($"Final Population Diversity: {(DiversityHistory.Count > 0 ? DiversityHistory.Last() : 0):F4}");
        }
    }
}
