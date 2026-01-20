using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG._1
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Historical CO2 data points (x = time index, y = CO2 concentration)
            Double[,] puntos = new Double[2, 20];
            puntos[0, 0] = 1; puntos[1, 0] = 7.999119866;
            puntos[0, 1] = 2; puntos[1, 1] = 9.308087435;
            puntos[0, 2] = 3; puntos[1, 2] = 10.18280511;
            puntos[0, 3] = 4; puntos[1, 3] = 10.45198396;
            puntos[0, 4] = 5; puntos[1, 4] = 11.25039827;
            puntos[0, 5] = 6; puntos[1, 5] = 12.88309079;
            puntos[0, 6] = 7; puntos[1, 6] = 13.90438446;
            puntos[0, 7] = 8; puntos[1, 7] = 14.22900887;
            puntos[0, 8] = 9; puntos[1, 8] = 15.49424006;
            puntos[0, 9] = 10; puntos[1, 9] = 15.93669547;
            puntos[0, 10] = 11; puntos[1, 10] = 16.43849754;
            puntos[0, 11] = 12; puntos[1, 11] = 17.37546461;
            puntos[0, 12] = 13; puntos[1, 12] = 18.43417649;
            puntos[0, 13] = 14; puntos[1, 13] = 19.58648863;
            puntos[0, 14] = 15; puntos[1, 14] = 20.27823061;
            puntos[0, 15] = 16; puntos[1, 15] = 21.06313031;
            puntos[0, 16] = 17; puntos[1, 16] = 21.88591744;
            puntos[0, 17] = 18; puntos[1, 17] = 22.80549753;
            puntos[0, 18] = 19; puntos[1, 18] = 23.39046461;
            puntos[0, 19] = 20; puntos[1, 19] = 24.22478715;

            Console.WriteLine("=== Genetic Algorithm for CO2 Curve Fitting ===\n");
            Console.WriteLine("Model: y = a1*x + a2*sin(a3*x) + a4\n");

            // Parse command line arguments
            bool runBenchmark = args.Contains("--benchmark");
            bool useLegacy = args.Contains("--legacy");
            int? seed = null;
            
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == "--seed" && int.TryParse(args[i + 1], out int parsedSeed))
                {
                    seed = parsedSeed;
                }
            }

            if (runBenchmark)
            {
                // Run benchmarking suite with deterministic seeding
                int[] populationSizes = { 50, 100, 200 };
                double[] mutationRates = { 0.05, 0.1, 0.2 };
                int runsPerConfig = 5;
                
                AlgoritmoGenetico.RunBenchmark(puntos, populationSizes, mutationRates, runsPerConfig, seed ?? 42);
            }
            else
            {
                // Single run with configurable parameters
                var config = new GAConfig
                {
                    PopulationSize = 100,
                    OffspringSize = 40,
                    MutationRate = 0.1,
                    TournamentSize = 3,
                    ArithmeticCrossoverAlpha = 0.5,
                    InitialMutationStrength = 1.0,
                    MutationDecayRate = 0.995,
                    MinMutationStrength = 0.01,
                    MaxGenerationsWithoutImprovement = 40,
                    Seed = seed
                };

                if (seed.HasValue)
                {
                    Console.WriteLine($"Using deterministic seed: {seed.Value}");
                }

                var random = config.CreateRandom();
                var alg = new AlgoritmoGenetico(config);

                if (useLegacy)
                {
                    Console.WriteLine("Running legacy algorithm...\n");
                    alg.AlgoritmoLegacy(random, puntos);
                }
                else
                {
                    Console.WriteLine("Running enhanced algorithm with:");
                    Console.WriteLine("  - Tournament selection");
                    Console.WriteLine("  - Arithmetic crossover");
                    Console.WriteLine("  - Adaptive Gaussian mutation");
                    Console.WriteLine("  - Fitness evaluation tracking\n");
                    alg.Algoritmo(random, puntos);
                }
            }

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}
