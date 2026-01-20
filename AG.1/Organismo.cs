using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG._1
{
    //Gabriel Navarro

    public class Organismo
    {
        public double a1, a2, a3, a4;
        public double adecuacion;
        public double rSquared;

        public int[] cromosomaA1 = new int[20];
        public int[] cromosomaA2 = new int[20];
        public int[] cromosomaA3 = new int[20];
        public int[] cromosomaA4 = new int[20];

        public Organismo Clone()
        {
            var clone = new Organismo
            {
                a1 = this.a1,
                a2 = this.a2,
                a3 = this.a3,
                a4 = this.a4,
                adecuacion = this.adecuacion,
                rSquared = this.rSquared
            };
            Array.Copy(this.cromosomaA1, clone.cromosomaA1, 20);
            Array.Copy(this.cromosomaA2, clone.cromosomaA2, 20);
            Array.Copy(this.cromosomaA3, clone.cromosomaA3, 20);
            Array.Copy(this.cromosomaA4, clone.cromosomaA4, 20);
            return clone;
        }

        public void ConvABin(double num, int[] crom)
        {
            if (num < 0)
                crom[0] = 0;
            else
                crom[0] = 1;
            num = Math.Abs(num);
            for (int i=0; i<19; i++)
            {
                if (num >= Math.Pow(2, 9 - i))
                {
                    crom[i + 1] = 1;
                    num -= Math.Pow(2, 9 - i);
                }
                else
                    crom[i + 1] = 0;
            }           
        }
        public double ConvDecA1()
        {
            double num = 0;

            for (int i = 1; i < 20; i++)
            {
                if (cromosomaA1[i] == 1)
                    num += Math.Pow(2, 10 - i);
            }
            if (cromosomaA1[0] == 0)
                num = num * -1;
            return num;
        }
        public double ConvDecA2()
        {
            double num = 0;

            for (int i = 1; i < 20; i++)
            {
                if (cromosomaA2[i] == 1)
                    num += Math.Pow(2, 10 - i);
            }
            if (cromosomaA2[0] == 0)
                num = num * -1;
            return num;
        }

        public double ConvDecA3()
        {
            double num = 0;

            for (int i = 1; i < 20; i++)
            {
                if (cromosomaA3[i] == 1)
                    num += Math.Pow(2, 10 - i);
            }
            if (cromosomaA3[0] == 0)
                num = num * -1;
            return num;
        }
        public double ConvDecA4()
        {
            double num = 0;

            for (int i = 1; i < 20; i++)
            {
                if (cromosomaA4[i] == 1)
                    num += Math.Pow(2, 10 - i);
            }
            if (cromosomaA4[0] == 0)
                num = num * -1;
            return num;
        }

        public void ConversionCromosoma()
        {
            a1 = ConvDecA1();
            a2 = ConvDecA2();
            a3 = ConvDecA3();
            a4 = ConvDecA4();
        }

        public void SetFromRealValues(double val1, double val2, double val3, double val4)
        {
            a1 = val1;
            a2 = val2;
            a3 = val3;
            a4 = val4;
            ConvABin(a1, cromosomaA1);
            ConvABin(a2, cromosomaA2);
            ConvABin(a3, cromosomaA3);
            ConvABin(a4, cromosomaA4);
        }

        public double Predict(double t)
        {
            return a1 * t + a2 * Math.Sin(a3 * t) + a4;
        }

        public void Adecuar(Double[,] puntos)
        {
            adecuacion = 0;
            double y;
            int numPoints = puntos.GetLength(1);
            
            for (int t = 0; t < numPoints; t++) 
            { 
                double x = puntos[0, t];
                y = a1 * x + a2 * Math.Sin(a3 * x) + a4;
                adecuacion += Math.Pow(puntos[1, t] - y, 2);
            }
        }

        public void CalculateRSquared(Double[,] puntos)
        {
            int numPoints = puntos.GetLength(1);
            
            double yMean = 0;
            for (int t = 0; t < numPoints; t++)
            {
                yMean += puntos[1, t];
            }
            yMean /= numPoints;

            double ssTot = 0;
            double ssRes = 0;
            
            for (int t = 0; t < numPoints; t++)
            {
                double x = puntos[0, t];
                double yActual = puntos[1, t];
                double yPred = a1 * x + a2 * Math.Sin(a3 * x) + a4;
                
                ssTot += Math.Pow(yActual - yMean, 2);
                ssRes += Math.Pow(yActual - yPred, 2);
            }

            rSquared = ssTot > 0 ? 1 - (ssRes / ssTot) : 0;
            if (rSquared < 0) rSquared = 0;
        }
    }
}
