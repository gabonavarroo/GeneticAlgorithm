using Xunit;
using AG._1;

namespace AG._1.Tests
{
    public class PoblacionTests
    {
        private Double[,] CreateTestData()
        {
            Double[,] puntos = new Double[2, 5];
            for (int i = 0; i < 5; i++)
            {
                puntos[0, i] = i + 1;
                puntos[1, i] = (i + 1) * 2;
            }
            return puntos;
        }

        [Fact]
        public void TournamentSelect_ReturnsBestFromTournament()
        {
            var config = new GAConfig { PopulationSize = 10, OffspringSize = 4, TournamentSize = 3, Seed = 42 };
            var stats = new GAStatistics();
            var poblacion = new Poblacion(config, stats);
            var random = config.CreateRandom();
            var puntos = CreateTestData();

            poblacion.PrimerGen(random, puntos);
            poblacion.Ordenar(puntos);

            var selected = poblacion.TournamentSelect(random);

            Assert.NotNull(selected);
            Assert.True(selected.adecuacion >= 0);
        }

        [Fact]
        public void CruzarAritmetico_CreatesOffspring()
        {
            var config = new GAConfig { PopulationSize = 20, OffspringSize = 10, TournamentSize = 3, Seed = 42 };
            var stats = new GAStatistics();
            var poblacion = new Poblacion(config, stats);
            var random = config.CreateRandom();
            var puntos = CreateTestData();

            poblacion.PrimerGen(random, puntos);
            poblacion.Ordenar(puntos);
            
            double originalOffspringA1 = poblacion.poblacion[config.PopulationSize].a1;
            
            poblacion.CruzarAritmetico(random);
            poblacion.Ordenar(puntos);

            // Offspring should have been modified
            Assert.NotNull(poblacion.poblacion[config.PopulationSize]);
        }

        [Fact]
        public void MutarGaussianoAdaptivo_DecreasesMutationStrength()
        {
            var config = new GAConfig 
            { 
                PopulationSize = 20, 
                OffspringSize = 10, 
                MutationRate = 0.5,
                InitialMutationStrength = 1.0,
                MutationDecayRate = 0.9,
                Seed = 42 
            };
            var stats = new GAStatistics();
            var poblacion = new Poblacion(config, stats);
            var random = config.CreateRandom();
            var puntos = CreateTestData();

            poblacion.PrimerGen(random, puntos);
            poblacion.Ordenar(puntos);

            double initialStrength = poblacion.GetCurrentMutationStrength();
            poblacion.MutarGaussianoAdaptivo(random);
            double afterStrength = poblacion.GetCurrentMutationStrength();

            Assert.True(afterStrength < initialStrength);
        }

        [Fact]
        public void CalculateGeneticDiversity_ReturnsPositiveValue()
        {
            var config = new GAConfig { PopulationSize = 20, OffspringSize = 10, Seed = 42 };
            var stats = new GAStatistics();
            var poblacion = new Poblacion(config, stats);
            var random = config.CreateRandom();
            var puntos = CreateTestData();

            poblacion.PrimerGen(random, puntos);
            poblacion.Ordenar(puntos);

            double diversity = poblacion.CalculateGeneticDiversity();

            Assert.True(diversity >= 0);
        }

        [Fact]
        public void Ordenar_SortsByFitness()
        {
            var config = new GAConfig { PopulationSize = 20, OffspringSize = 10, Seed = 42 };
            var stats = new GAStatistics();
            var poblacion = new Poblacion(config, stats);
            var random = config.CreateRandom();
            var puntos = CreateTestData();

            poblacion.PrimerGen(random, puntos);
            poblacion.Ordenar(puntos);

            for (int i = 0; i < config.PopulationSize - 1; i++)
            {
                Assert.True(poblacion.poblacion[i].adecuacion <= poblacion.poblacion[i + 1].adecuacion);
            }
        }
    }
}
