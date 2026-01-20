using Xunit;
using AG._1;

namespace AG._1.Tests
{
    public class OrganismoTests
    {
        [Fact]
        public void Clone_CreatesIndependentCopy()
        {
            var original = new Organismo();
            original.a1 = 1.5;
            original.a2 = 2.5;
            original.a3 = 0.5;
            original.a4 = 3.0;
            original.cromosomaA1[0] = 1;

            var clone = original.Clone();
            clone.a1 = 10.0;
            clone.cromosomaA1[0] = 0;

            Assert.Equal(1.5, original.a1);
            Assert.Equal(1, original.cromosomaA1[0]);
            Assert.Equal(10.0, clone.a1);
            Assert.Equal(0, clone.cromosomaA1[0]);
        }

        [Fact]
        public void Predict_CalculatesCorrectValue()
        {
            var org = new Organismo();
            org.a1 = 1.0;
            org.a2 = 0.0;
            org.a3 = 0.0;
            org.a4 = 5.0;

            double result = org.Predict(10.0);

            Assert.Equal(15.0, result, 6);
        }

        [Fact]
        public void CalculateRSquared_PerfectFit_ReturnsOne()
        {
            var org = new Organismo();
            org.a1 = 1.0;
            org.a2 = 0.0;
            org.a3 = 0.0;
            org.a4 = 0.0;

            Double[,] puntos = new Double[2, 3];
            puntos[0, 0] = 1; puntos[1, 0] = 1;
            puntos[0, 1] = 2; puntos[1, 1] = 2;
            puntos[0, 2] = 3; puntos[1, 2] = 3;

            org.CalculateRSquared(puntos);

            Assert.Equal(1.0, org.rSquared, 6);
        }

        [Fact]
        public void SetFromRealValues_UpdatesChromosomes()
        {
            var org = new Organismo();
            org.SetFromRealValues(5.0, 3.0, 1.0, 2.0);

            Assert.Equal(5.0, org.a1);
            Assert.Equal(3.0, org.a2);
            Assert.Equal(1.0, org.a3);
            Assert.Equal(2.0, org.a4);
        }

        [Fact]
        public void Adecuar_CalculatesSumOfSquaredErrors()
        {
            var org = new Organismo();
            org.a1 = 1.0;
            org.a2 = 0.0;
            org.a3 = 0.0;
            org.a4 = 0.0;

            Double[,] puntos = new Double[2, 2];
            puntos[0, 0] = 1; puntos[1, 0] = 2;  // predicted: 1, actual: 2, error: 1
            puntos[0, 1] = 2; puntos[1, 1] = 4;  // predicted: 2, actual: 4, error: 2

            org.Adecuar(puntos);

            Assert.Equal(5.0, org.adecuacion, 6);  // 1^2 + 2^2 = 5
        }
    }
}
