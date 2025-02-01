using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AG._1
{
    
    internal class AlgoritmoGenetico
    {
        Poblacion p = new Poblacion();
        List<Poblacion> generaciones = new List<Poblacion>();
        double stop = 10000;
        int j = 0;
        
        public void Algoritmo(Random r, Double[,] puntos)
        {
            p.PrimerGen(r, puntos);
            for (int i = 0; i < 40; )
            {
                double comparar = stop - p.Mejor.adecuacion;
                generaciones.Add(p);
                j++;
                if (j % 3 == 0)
                    p.Mutar(r);
                p.Ordenar(puntos);
                p.SeleccionarCruzas(r);
                p.Cruzar(r);
                p.Ordenar(puntos);

                if (comparar >= 10 )
                {
                    i = 0;
                    stop = p.Mejor.adecuacion;
                }
                if (comparar < 0)
                    i = 0;
                if (comparar <10 && comparar >= 0)
                {
                    stop = p.Mejor.adecuacion;
                    i++;
                }
                /*if (p.Mejor.adecuacion < stop )
                {
                    stop = p.Mejor.adecuacion;
                    i = 0;
                }
                if (p.Mejor.adecuacion == stop)
                {
                    i++;
                }
                if (p.Mejor.adecuacion > stop)
                {
                    i = 0;
                }*/

                Console.Write($"Generación {i} = " + p.poblacion[0].a1 + " " 
                    + p.poblacion[0].a2 + " " + p.poblacion[0].a3 + " " + p.poblacion[0].a4);
                Console.WriteLine();
                p.GuardarArchivo();
            }
            Console.Write($"Cromosomas = " + p.poblacion[0].a1 + " " 
            + p.poblacion[0].a2 + " " + p.poblacion[0].a3 + " " + p.poblacion[0].a4);
            Console.WriteLine();
            p.Ordenar(puntos);
        }
        


    }
}
