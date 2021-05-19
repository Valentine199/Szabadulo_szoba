using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Szabadulo_szoba
{
    class Program
    {
       public static jatekos jatekos = new jatekos();
       public static List<targy> targyak = new List<targy>();
       public static List<szoba> haz = new List<szoba>();
        static void Main()
        {
            Inicializalas();

            Console.WriteLine("Adjon meg egy parancsot");
            string beadott = Console.ReadLine();
           string[] ertelmezett = ertelmezes(beadott);

            switch (ertelmezett[0])
            {
                case "Nézd":
                case "nézd":
                    Console.WriteLine(jatekos.Nezd(ertelmezett[1]));
                    break;
                default:
                    break;
            }


        }

        private static string[] ertelmezes(string beadott)
        {
            string[] ertelmezett = beadott.Split(' ');



            string parancs = "";
            string mit = "";
            string mivel = "";

            for (int i = 0; i < ertelmezett.Length; i++)
            {
                if (targyak.Select(x => x.neve).Contains(ertelmezett[i]))
                {
                    if (mit.Length > 0)
                    {
                        mivel = ertelmezett[i];
                    }
                    else
                    {
                        mit = ertelmezett[i];
                    }
                }
                else
                {
                    parancs = ertelmezett[i];
                }

            }
            string[] vegrehajtas = { parancs, mit, mivel, };
            return vegrehajtas;
        }

        private static void Inicializalas()
        {
            foreach (string targyAdat in File.ReadAllLines("targyInit.txt").Skip(1))
            {
                targyak.Add(new targy(targyAdat));
            }
            foreach (string szobaAdat in File.ReadLines("szobaInit.txt").Skip(1))
            {
                haz.Add(new szoba(szobaAdat));
            }

            foreach (szoba szobak in haz)
            {
                var temp = targyak.Select(x => x).Where(x => x.kezdoHelye == szobak.id);
                foreach (targy targy in temp)
                {
                    szobak.Tartalma.Add(targy);
                }
            }
        }
    }
}
