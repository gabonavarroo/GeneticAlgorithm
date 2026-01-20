using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG._1
{
    public class AlgoritmoGenetico
    {
        private GAConfig _config;
        private GAStatistics _stats;
        private Poblacion _poblacion;
        private List<Poblacion> _generaciones = new List<Poblacion>();
        private double _bestFitness = double.MaxValue;
        private int _generationsWithoutImprovement = 0;

        public AlgoritmoGenetico() : this(new GAConfig()) { }

        public AlgoritmoGenetico(GAConfig config)
        {
            _config = config;
            _stats = new GAStatistics();
            _poblacion = new Poblacion(config, _stats);
        }

        public GAStatistics GetStatistics() => _stats;

        /// <summary>
        /// Enhanced GA with tournament selection, arithmetic crossover, and adaptive Gaussian mutation.
        /// Tracks fitness evaluations for runtime analysis.
        /// </summary>
        public void Algoritmo(Random r, Double[,] puntos)
        {
            _poblacion.PrimerGen(r, puntos);
            _poblacion.Ordenar(puntos);

            Console.WriteLine("\n=== Starting Enhanced Genetic Algorithm ===");
            Console.WriteLine($"Population Size: {_config.PopulationSize}");
            Console.WriteLine($"Tournament Size: {_config.TournamentSize}");
            Console.WriteLine($"Initial Mutation Strength: {_config.InitialMutationStrength}");
            Console.WriteLine($"Mutation Rate: {_config.MutationRate}");
            Console.WriteLine();

            while (_generationsWithoutImprovement < _config.MaxGenerationsWithoutImprovement)
            {
                _poblacion.CruzarAritmetico(r);
                _poblacion.MutarGaussianoAdaptivo(r);
                _poblacion.Ordenar(puntos);

                double currentBest = _poblacion.Mejor.adecuacion;
                double currentRSquared = _poblacion.Mejor.rSquared;
                double diversity = _poblacion.CalculateGeneticDiversity();
                double mutationStrength = _poblacion.GetCurrentMutationStrength();

                _stats.RecordGeneration(currentBest, currentRSquared, diversity, mutationStrength);

                if (currentBest < _bestFitness - _config.ConvergenceThreshold)
                {
                    _bestFitness = currentBest;
                    _generationsWithoutImprovement = 0;
                }
                else
                {
                    _generationsWithoutImprovement++;
                }

                if (currentRSquared >= 0.95)
                {
                    _stats.MarkConverged(0.95);
                }

                Console.WriteLine($"Gen {_stats.Generations}: SSE={currentBest:F4}, R²={currentRSquared:F4}, " +
                    $"Diversity={diversity:F2}, MutStr={mutationStrength:F4}, NoImprove={_generationsWithoutImprovement}");

                _poblacion.GuardarArchivo();
            }

            PrintFinalResults(puntos);
            _stats.PrintSummary();
        }

        /// <summary>
        /// Legacy algorithm for backward compatibility.
        /// </summary>
        public void AlgoritmoLegacy(Random r, Double[,] puntos)
        {
            double stop = 10000;
            int j = 0;
            
            _poblacion.PrimerGen(r, puntos);
            for (int i = 0; i < 40;)
            {
                double comparar = stop - _poblacion.Mejor.adecuacion;
                _generaciones.Add(_poblacion);
                j++;
                if (j % 3 == 0)
                    _poblacion.Mutar(r);
                _poblacion.Ordenar(puntos);
                _poblacion.SeleccionarCruzas(r);
                _poblacion.Cruzar(r);
                _poblacion.Ordenar(puntos);

                if (comparar >= 10)
                {
                    i = 0;
                    stop = _poblacion.Mejor.adecuacion;
                }
                if (comparar < 0)
                    i = 0;
                if (comparar < 10 && comparar >= 0)
                {
                    stop = _poblacion.Mejor.adecuacion;
                    i++;
                }

                Console.Write($"Generación {i} = " + _poblacion.poblacion[0].a1 + " "
                    + _poblacion.poblacion[0].a2 + " " + _poblacion.poblacion[0].a3 + " " + _poblacion.poblacion[0].a4);
                Console.WriteLine();
                _poblacion.GuardarArchivo();
            }
            Console.Write($"Cromosomas = " + _poblacion.poblacion[0].a1 + " "
            + _poblacion.poblacion[0].a2 + " " + _poblacion.poblacion[0].a3 + " " + _poblacion.poblacion[0].a4);
            Console.WriteLine();
            _poblacion.Ordenar(puntos);
        }

        private void PrintFinalResults(Double[,] puntos)
        {
            var best = _poblacion.Mejor;
            Console.WriteLine("\n=== Final Results ===");
            Console.WriteLine($"Best Solution: y = {best.a1:F4}*x + {best.a2:F4}*sin({best.a3:F4}*x) + {best.a4:F4}");
            Console.WriteLine($"Sum of Squared Errors: {best.adecuacion:F6}");
            Console.WriteLine($"R² (Coefficient of Determination): {best.rSquared:F6}");
            
            if (best.rSquared >= 0.95)
            {
                Console.WriteLine("✓ Achieved R² > 0.95 target");
            }
            else
            {
                Console.WriteLine($"✗ R² below 0.95 target (achieved {best.rSquared:F4})");
            }
        }

        /// <summary>
        /// Runs benchmarking across different configurations.
        /// </summary>
        public static void RunBenchmark(Double[,] puntos, int[] populationSizes, double[] mutationRates, int runsPerConfig, int? baseSeed = null)
        {
            Console.WriteLine("\n=== GA Benchmarking Suite ===\n");
            Console.WriteLine("PopSize\tMutRate\tAvgEvals\tAvgR²\tSuccessRate\tAvgGens");
            Console.WriteLine(new string('-', 70));

            foreach (int popSize in populationSizes)
            {
                foreach (double mutRate in mutationRates)
                {
                    var results = new List<(int evals, double rSquared, int gens, bool success)>();

                    for (int run = 0; run < runsPerConfig; run++)
                    {
                        var config = new GAConfig
                        {
                            PopulationSize = popSize,
                            OffspringSize = popSize / 3,
                            MutationRate = mutRate,
                            Seed = baseSeed.HasValue ? baseSeed.Value + run : null,
                            MaxGenerationsWithoutImprovement = 30
                        };

                        var ga = new AlgoritmoGenetico(config);
                        var random = config.CreateRandom();
                        
                        Console.SetOut(TextWriter.Null);
                        ga.Algoritmo(random, puntos);
                        Console.SetOut(Console.Out);

                        var stats = ga.GetStatistics();
                        results.Add((stats.FitnessEvaluations, stats.BestRSquared, stats.Generations, stats.BestRSquared >= 0.95));
                    }

                    double avgEvals = results.Average(r => r.evals);
                    double avgRSquared = results.Average(r => r.rSquared);
                    double successRate = results.Count(r => r.success) / (double)runsPerConfig;
                    double avgGens = results.Average(r => r.gens);

                    Console.WriteLine($"{popSize}\t{mutRate:F2}\t{avgEvals:F0}\t\t{avgRSquared:F4}\t{successRate:P0}\t\t{avgGens:F1}");
                }
            }
        }
    }
}
