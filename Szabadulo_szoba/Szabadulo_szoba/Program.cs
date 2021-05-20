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
            bool nyert = false;

            /*var tartalom = Program.haz.Select(x => new { x.Tartalma, x.id }).Where(y => y.id == jatekos.Helye).First();

            if(tartalom.Tartalma.Select(x=> x.neve).Contains("szekrény"))
            {
                Console.WriteLine(tartalom.Tartalma.Select(x=> x.leiras).First());
            }*/


            Console.WriteLine("Adjon meg egy parancsot");
            do
            {

            
            string beadott = Console.ReadLine();
           string[] ertelmezett = ertelmezes(beadott);

            switch (ertelmezett[0])
            {
                case "Nézd":
                case "nézd":
                    Console.WriteLine(jatekos.Nezd(ertelmezett[1]));
                    break;
                case "Nyisd":
                case "nyisd":
                    if (ertelmezett[1]=="")
                    {
                        Console.WriteLine("Mit nyissak ki?");
                    }
                    else if(targyak.Where(x => x.neve == ertelmezett[1]).First().nyithato)
                    {
                        jatekos.Nyisd(ertelmezett[1]);
                    }
                    else
                    {
                        Console.WriteLine($"A {targyak.Where(x => x.neve == ertelmezett[1]).First().neve} nem nyitható");
                    }
                    break;
                case "Tedd":
                case "tedd":
                case "Vedd":
                case "vedd":
                        jatekos.TargyMozgatas(ertelmezett[1], ertelmezett[3]);
                        break;
                default:
                        Console.WriteLine("Ilyen parancsot nem ismerek.");
                    break;
            }
            } while (!nyert);


        }

        private static string[] ertelmezes(string beadott)
        {
            string[] ertelmezett = beadott.Split(' ');



            string parancs = "";
            string mit = "";
            string mivel = "";
            string hova = "";

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
                else if(ertelmezett[i]=="fel"||ertelmezett[i]=="le")
                {
                    hova = ertelmezett[i];
                }
                else if(parancs =="")
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
