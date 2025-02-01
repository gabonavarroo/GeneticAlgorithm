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
         public Organismo[] poblacion = new Organismo[140];
         public Organismo[] cruzados = new Organismo [30];
         public Organismo Mejor = new Organismo();

        public void PrimerGen(Random r, Double[,] puntos)
        {
            for (int jp = 0; jp < 100; jp++)
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
                Organismo NuevoO = new Organismo();
                for (int i = 0; i < 40; i++)
                    poblacion[100 + i] = NuevoO;
        }

        public void GuardarArchivo()
        {
            StreamWriter writer = new StreamWriter("Organismos.txt", true);
            writer.WriteLine($"El mejor organismo es {poblacion[0].a1}x + " +
                $"{poblacion[0].a2}*sin({poblacion[0].a3}x) + {poblacion[0].a4}, y tiene una adecuación de {poblacion[0].adecuacion}");
            writer.Close();
        }

        public void Ordenar ( Double[,] puntos)
        {
            
            for (int i = 0; i < 140; i++)
            {
                poblacion[i].ConversionCromosoma();
                poblacion[i].Adecuar(puntos);
                /*Console.Write("Organismo " + (i + 1) + ": " + poblacion[i].a1 + ", " + poblacion[i].a2 +
                    ", " + poblacion[i].a3 + ", " + poblacion[i].a4);
                Console.WriteLine();*/
            }
            List<Organismo> segunda = new List<Organismo>();
            for (int i = 0; i < 140; i++)
                segunda.Add(poblacion[i]);
            poblacion = segunda.OrderBy(x => x.adecuacion).ToArray();
            Mejor = poblacion[0];

            Console.WriteLine("Adecuación del mejor organismo: " + poblacion[0].adecuacion);
        }

        public void SeleccionarCruzas(Random r)
        {
            bool[] seleccionados = new bool[200];
            for(int i = 0; i<100; i++)
            {
                seleccionados[i] = false;
            }
            int indice = 0;
            for( int i = 0; i < 14; )
            {
                indice = r.Next(0, 20);

                if (!seleccionados[indice])
                {
                    seleccionados[indice] = true;
                    cruzados[i] = poblacion[indice];
                    i++;
                }
            }
            for (int i = 0; i <9;)
            {
                indice = r.Next(20, 40);

                if (!seleccionados[indice])
                {
                    seleccionados[indice] = true;
                    cruzados[i+14] = poblacion[indice];
                    i++;
                }
            }
            for (int i = 0; i < 4;)
            {
                indice = r.Next(40, 60);

                if (!seleccionados[indice])
                {
                    seleccionados[indice] = true;
                    cruzados[i + 23] = poblacion[indice];
                    i++;
                }
            }
            for (int i = 0; i < 2;)
            {
                indice = r.Next(60, 80);

                if (!seleccionados[indice])
                {
                    seleccionados[indice] = true;
                    cruzados[i + 27] = poblacion[indice];
                    i++;
                }
            }

            for (int i = 0; i < 1;)
            {
                indice = r.Next(80, 100);

                if (!seleccionados[indice])
                {
                    seleccionados[indice] = true;
                    cruzados[i + 29] = poblacion[indice];
                    i++;
                }
            }
        }

        public void Cruzar(Random r)
        {
            bool[] bul = new bool[30];
            for (int i = 0; i < 30;i++)
                bul[i] = false;

            for(int x = 0; x < 30; x=x+2)
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
                    n= r.Next(0, 30);
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
            for(int i = 139; i >= 130; i--)
            {
                Mutante = r.Next(0, 100);
                CasillaM = r.Next(0, 20);
                muta = r.Next(0, 2);
                NumCrom = r.Next(1, 5);

                if(NumCrom == 1)
                {
                    rCrom = r.Next(1, 5);
                    switch (rCrom)
                    {
                        case 1:
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA1[CasillaM] = muta;
                            break;

                        case 2:
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA2[CasillaM] = muta;
                            break; 

                        case 3:
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA3[CasillaM] = muta;
                            break;

                        case 4:
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA4[CasillaM] = muta;
                            break;

                    }
                }
                
                if (NumCrom == 2)
                {
                    rCrom = r.Next(1, 7);
                    switch (rCrom)
                    {
                        case 1:
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA1[CasillaM] = muta;
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA2[CasillaM] = muta;
                            break;

                        case 2:
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA1[CasillaM] = muta;
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA3[CasillaM] = muta;
                            break;

                        case 3:
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA1[CasillaM] = muta;
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA4[CasillaM] = muta;
                            break;

                        case 4:
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA2[CasillaM] = muta;
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA3[CasillaM] = muta;
                            break;

                        case 5:
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA2[CasillaM] = muta;
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA4[CasillaM] = muta;
                            break;

                        case 6:
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA3[CasillaM] = muta;
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA4[CasillaM] = muta;
                            break;

                    }
                }

                if (NumCrom == 3)
                {
                    rCrom = r.Next(1, 5);
                    switch (rCrom)
                    {
                        case 1:
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA1[CasillaM] = muta;
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA2[CasillaM] = muta;
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA3[CasillaM] = muta;
                            break;

                        case 2:
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA1[CasillaM] = muta;
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA2[CasillaM] = muta;
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA4[CasillaM] = muta;
                            break;

                        case 3:
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA1[CasillaM] = muta;
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA3[CasillaM] = muta;
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA4[CasillaM] = muta;
                            break;

                        case 4:
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA2[CasillaM] = muta;
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA3[CasillaM] = muta;
                            poblacion[i] = poblacion[Mutante];
                            poblacion[i].cromosomaA4[CasillaM] = muta;
                            break;
                    }
                }

                if (NumCrom == 4)
                {
                    poblacion[i] = poblacion[Mutante];
                    poblacion[i].cromosomaA1[CasillaM] = muta;
                    poblacion[i] = poblacion[Mutante];
                    poblacion[i].cromosomaA2[CasillaM] = muta;
                    poblacion[i] = poblacion[Mutante];
                    poblacion[i].cromosomaA3[CasillaM] = muta;
                    poblacion[i] = poblacion[Mutante];
                    poblacion[i].cromosomaA4[CasillaM] = muta;
                }
            }
        }
    }
}
