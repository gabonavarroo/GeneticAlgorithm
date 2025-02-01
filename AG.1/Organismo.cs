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

        public int[] cromosomaA1 = new int[20];
        public int[] cromosomaA2 = new int[20];
        public int[] cromosomaA3 = new int[20];
        public int[] cromosomaA4 = new int[20];

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
            //Console.WriteLine("Cromosomas: "+ a1 + " " + a2 + " " + a3 + " " + a4);
        }
        public void Adecuar(Double[,] puntos)
        {
            adecuacion = 0;
            double y;
            int t = 0;
            for (t = 0; t < 20; t++) 
            { 
                y = a1*t + a2*Math.Sin(a3*t) + a4;
                adecuacion += Math.Abs(puntos[1, t] - y);
            }
            //Console.WriteLine($"Adecuación del organismo: {adecuacion}");
        }
        
    }
}
