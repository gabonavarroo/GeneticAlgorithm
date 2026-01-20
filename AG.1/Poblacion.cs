using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG._1
{
    public class Poblacion
    {
        private GAConfig _config;
        private GAStatistics _stats;
        public Organismo[] poblacion;
        public Organismo Mejor = new Organismo();
        private double _currentMutationStrength;

        public Poblacion() : this(new GAConfig(), new GAStatistics()) { }

        public Poblacion(GAConfig config, GAStatistics stats)
        {
            _config = config;
            _stats = stats;
            poblacion = new Organismo[config.PopulationSize + config.OffspringSize];
            _currentMutationStrength = config.InitialMutationStrength;
        }

        public void PrimerGen(Random r, Double[,] puntos)
        {
            for (int jp = 0; jp < _config.PopulationSize; jp++)
            {
                Organismo org = new Organismo();
                for (int i = 0; i < 20; i++)
                {
                    org.cromosomaA1[i] = r.Next(0, 2);
                    org.cromosomaA2[i] = r.Next(0, 2);
                    org.cromosomaA3[i] = r.Next(0, 2);
                    org.cromosomaA4[i] = r.Next(0, 2);
                }
                poblacion[jp] = org;
            }
            
            for (int i = 0; i < _config.OffspringSize; i++)
            {
                poblacion[_config.PopulationSize + i] = new Organismo();
            }
        }

        public void GuardarArchivo()
        {
            StreamWriter writer = new StreamWriter("Organismos.txt", true);
            writer.WriteLine($"El mejor organismo es {poblacion[0].a1}x + " +
                $"{poblacion[0].a2}*sin({poblacion[0].a3}x) + {poblacion[0].a4}, " +
                $"SSE: {poblacion[0].adecuacion:F6}, R²: {poblacion[0].rSquared:F6}");
            writer.Close();
        }

        public void Ordenar(Double[,] puntos)
        {
            int totalSize = _config.PopulationSize + _config.OffspringSize;
            for (int i = 0; i < totalSize; i++)
            {
                poblacion[i].ConversionCromosoma();
                poblacion[i].Adecuar(puntos);
                poblacion[i].CalculateRSquared(puntos);
                _stats.IncrementFitnessEvaluations();
            }
            
            List<Organismo> segunda = new List<Organismo>();
            for (int i = 0; i < totalSize; i++)
                segunda.Add(poblacion[i]);
            poblacion = segunda.OrderBy(x => x.adecuacion).ToArray();
            Mejor = poblacion[0];

            Console.WriteLine($"Best SSE: {poblacion[0].adecuacion:F6}, R²: {poblacion[0].rSquared:F6}");
        }

        /// <summary>
        /// Tournament selection: randomly select k individuals and return the best.
        /// </summary>
        public Organismo TournamentSelect(Random r)
        {
            int bestIdx = r.Next(0, _config.PopulationSize);
            for (int i = 1; i < _config.TournamentSize; i++)
            {
                int idx = r.Next(0, _config.PopulationSize);
                if (poblacion[idx].adecuacion < poblacion[bestIdx].adecuacion)
                {
                    bestIdx = idx;
                }
            }
            return poblacion[bestIdx];
        }

        /// <summary>
        /// Selects parents using tournament selection.
        /// </summary>
        public List<Organismo> SeleccionarCruzasTorneo(Random r, int count)
        {
            var selected = new List<Organismo>();
            for (int i = 0; i < count; i++)
            {
                selected.Add(TournamentSelect(r));
            }
            return selected;
        }

        /// <summary>
        /// Arithmetic crossover: offspring = alpha * parent1 + (1-alpha) * parent2
        /// </summary>
        public void CruzarAritmetico(Random r)
        {
            var parents = SeleccionarCruzasTorneo(r, _config.OffspringSize * 2);
            
            for (int i = 0; i < _config.OffspringSize; i++)
            {
                var parent1 = parents[i * 2];
                var parent2 = parents[i * 2 + 1];
                
                double alpha = _config.ArithmeticCrossoverAlpha + (r.NextDouble() - 0.5) * 0.2;
                alpha = Math.Max(0, Math.Min(1, alpha));
                
                double newA1 = alpha * parent1.a1 + (1 - alpha) * parent2.a1;
                double newA2 = alpha * parent1.a2 + (1 - alpha) * parent2.a2;
                double newA3 = alpha * parent1.a3 + (1 - alpha) * parent2.a3;
                double newA4 = alpha * parent1.a4 + (1 - alpha) * parent2.a4;
                
                poblacion[_config.PopulationSize + i].SetFromRealValues(newA1, newA2, newA3, newA4);
            }
        }

        /// <summary>
        /// Adaptive Gaussian mutation: adds normally distributed noise that decreases over time.
        /// </summary>
        public void MutarGaussianoAdaptivo(Random r)
        {
            int mutationCount = (int)(_config.PopulationSize * _config.MutationRate);
            
            for (int i = 0; i < mutationCount; i++)
            {
                int targetIdx = r.Next(1, _config.PopulationSize);
                var original = poblacion[targetIdx];
                var mutant = original.Clone();
                
                mutant.a1 += GaussianNoise(r) * _currentMutationStrength;
                mutant.a2 += GaussianNoise(r) * _currentMutationStrength;
                mutant.a3 += GaussianNoise(r) * _currentMutationStrength * 0.1;
                mutant.a4 += GaussianNoise(r) * _currentMutationStrength;
                
                mutant.SetFromRealValues(mutant.a1, mutant.a2, mutant.a3, mutant.a4);
                
                int replaceIdx = _config.PopulationSize + _config.OffspringSize - 1 - i;
                if (replaceIdx >= _config.PopulationSize)
                {
                    poblacion[replaceIdx] = mutant;
                }
            }
            
            _currentMutationStrength *= _config.MutationDecayRate;
            if (_currentMutationStrength < _config.MinMutationStrength)
            {
                _currentMutationStrength = _config.MinMutationStrength;
            }
        }

        private double GaussianNoise(Random r)
        {
            double u1 = 1.0 - r.NextDouble();
            double u2 = 1.0 - r.NextDouble();
            return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        }

        /// <summary>
        /// Calculates population diversity as standard deviation of fitness values.
        /// </summary>
        public double CalculateDiversity()
        {
            double mean = 0;
            for (int i = 0; i < _config.PopulationSize; i++)
            {
                mean += poblacion[i].adecuacion;
            }
            mean /= _config.PopulationSize;

            double variance = 0;
            for (int i = 0; i < _config.PopulationSize; i++)
            {
                variance += Math.Pow(poblacion[i].adecuacion - mean, 2);
            }
            variance /= _config.PopulationSize;

            return Math.Sqrt(variance);
        }

        /// <summary>
        /// Calculates genetic diversity based on parameter variance.
        /// </summary>
        public double CalculateGeneticDiversity()
        {
            double[] a1Values = new double[_config.PopulationSize];
            double[] a2Values = new double[_config.PopulationSize];
            double[] a3Values = new double[_config.PopulationSize];
            double[] a4Values = new double[_config.PopulationSize];

            for (int i = 0; i < _config.PopulationSize; i++)
            {
                a1Values[i] = poblacion[i].a1;
                a2Values[i] = poblacion[i].a2;
                a3Values[i] = poblacion[i].a3;
                a4Values[i] = poblacion[i].a4;
            }

            double div = CalculateStdDev(a1Values) + CalculateStdDev(a2Values) +
                         CalculateStdDev(a3Values) + CalculateStdDev(a4Values);
            return div / 4.0;
        }

        private double CalculateStdDev(double[] values)
        {
            double mean = values.Average();
            double variance = values.Select(v => Math.Pow(v - mean, 2)).Average();
            return Math.Sqrt(variance);
        }

        public double GetCurrentMutationStrength()
        {
            return _currentMutationStrength;
        }

        // Legacy methods for backward compatibility
        public Organismo[] cruzados = new Organismo[30];

        public void SeleccionarCruzas(Random r)
        {
            bool[] seleccionados = new bool[_config.PopulationSize];
            for (int i = 0; i < _config.PopulationSize; i++)
            {
                seleccionados[i] = false;
            }
            int indice = 0;
            int cruzadosSize = Math.Min(30, _config.PopulationSize / 3);
            cruzados = new Organismo[cruzadosSize];
            
            for (int i = 0; i < cruzadosSize;)
            {
                indice = r.Next(0, _config.PopulationSize / 5);
                if (!seleccionados[indice])
                {
                    seleccionados[indice] = true;
                    cruzados[i] = poblacion[indice];
                    i++;
                }
            }
        }

        public void Cruzar(Random r)
        {
            bool[] bul = new bool[30];
            for (int i = 0; i < 30; i++)
                bul[i] = false;

            for (int x = 0; x < 30; x = x + 2)
            {
                int cut = r.Next(1, 20);
                int m = r.Next(0, 30);
                int n = r.Next(0, 30);

                if (!bul[m])
                {
                    bul[m] = true;
                }
                else
                {
                    while (bul[m])
                        m = r.Next(0, 30);
                }

                if (!bul[n])
                {
                    bul[n] = true;
                }
                else
                {
                    while (bul[n])
                        n = r.Next(0, 30);
                }

                for (int i = 0; i < cut; i++)
                    poblacion[100 + x].cromosomaA1[i] = cruzados[m].cromosomaA1[i];
                for (int k = cut; k < 20; k++)
                    poblacion[100 + x].cromosomaA1[k] = poblacion[n].cromosomaA1[k];
                for (int i = 0; i < cut; i++)
                    poblacion[101 + x].cromosomaA1[i] = cruzados[n].cromosomaA1[i];
                for (int k = cut; k < 20; k++)
                    poblacion[101 + x].cromosomaA1[k] = poblacion[m].cromosomaA1[k];

                cut = r.Next(1, 20);
                for (int i = 0; i < cut; i++)
                    poblacion[100 + x].cromosomaA2[i] = cruzados[m].cromosomaA2[i];
                for (int k = cut; k < 20; k++)
                    poblacion[100 + x].cromosomaA2[k] = poblacion[n].cromosomaA2[k];
                for (int i = 0; i < cut; i++)
                    poblacion[101 + x].cromosomaA2[i] = cruzados[n].cromosomaA2[i];
                for (int k = cut; k < 20; k++)
                    poblacion[101 + x].cromosomaA2[k] = poblacion[m].cromosomaA2[k];

                cut = r.Next(1, 20);
                for (int i = 0; i < cut; i++)
                    poblacion[100 + x].cromosomaA3[i] = cruzados[m].cromosomaA3[i];
                for (int k = cut; k < 20; k++)
                    poblacion[100 + x].cromosomaA3[k] = poblacion[n].cromosomaA3[k];
                for (int i = 0; i < cut; i++)
                    poblacion[101 + x].cromosomaA3[i] = cruzados[n].cromosomaA3[i];
                for (int k = cut; k < 20; k++)
                    poblacion[101 + x].cromosomaA3[k] = poblacion[m].cromosomaA3[k];

                cut = r.Next(1, 20);
                for (int i = 0; i < cut; i++)
                    poblacion[100 + x].cromosomaA4[i] = cruzados[m].cromosomaA4[i];
                for (int k = cut; k < 20; k++)
                    poblacion[100 + x].cromosomaA4[k] = poblacion[n].cromosomaA4[k];
                for (int i = 0; i < cut; i++)
                    poblacion[101 + x].cromosomaA4[i] = cruzados[n].cromosomaA4[i];
                for (int k = cut; k < 20; k++)
                    poblacion[101 + x].cromosomaA4[k] = poblacion[m].cromosomaA4[k];
            }
        }

        public void Mutar(Random r)
        {
            int Mutante, CasillaM, rCrom, muta, NumCrom;
            int totalSize = _config.PopulationSize + _config.OffspringSize;
            
            for (int i = totalSize - 1; i >= totalSize - 10; i--)
            {
                Mutante = r.Next(0, _config.PopulationSize);
                CasillaM = r.Next(0, 20);
                muta = r.Next(0, 2);
                NumCrom = r.Next(1, 5);

                var mutant = poblacion[Mutante].Clone();

                if (NumCrom == 1)
                {
                    rCrom = r.Next(1, 5);
                    switch (rCrom)
                    {
                        case 1:
                            mutant.cromosomaA1[CasillaM] = muta;
                            break;
                        case 2:
                            mutant.cromosomaA2[CasillaM] = muta;
                            break;
                        case 3:
                            mutant.cromosomaA3[CasillaM] = muta;
                            break;
                        case 4:
                            mutant.cromosomaA4[CasillaM] = muta;
                            break;
                    }
                }
                else if (NumCrom >= 2)
                {
                    mutant.cromosomaA1[CasillaM] = muta;
                    mutant.cromosomaA2[r.Next(0, 20)] = r.Next(0, 2);
                }

                poblacion[i] = mutant;
            }
        }
    }
}
