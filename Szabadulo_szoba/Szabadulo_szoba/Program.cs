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
       public static bool nyert = false;
        static void Main()
        {

            Inicializalas();

            Console.WriteLine("Adjon meg egy parancsot");
            do
            {


                string beadott = Console.ReadLine();
                string[] ertelmezett = ertelmezes(beadott);

                switch (ertelmezett[0])
                {
                    case "leltar":
                    case "Leltár":
                    case "leltár":
                        jatekos.Leltaram();
                        break;

                    case "Nézd":
                    case "nézd":
                        Console.WriteLine(jatekos.Nezd(ertelmezett[1]));
                        break;
                    case "Nyisd":
                    case "nyisd":
                        if (ertelmezett[1] == "")
                        {
                            Console.WriteLine("Mit nyissak ki?");
                        }
                        else if (targyak.First(x => x.neve == ertelmezett[1]).nyithato)
                        {
                            jatekos.Nyisd(ertelmezett[1], ertelmezett[2]);
                        }
                        else
                        {
                            Console.WriteLine($"A {targyak.First(x => x.neve == ertelmezett[1]).neve} nem nyitható");
                        }
                        break;
                    case "Tedd":
                    case "tedd":
                    case "Vedd":
                    case "vedd":
                        if (targyak.First(x => x.neve == ertelmezett[1]).lathato)
                        {
                            jatekos.TargyMozgatas(ertelmezett[1], ertelmezett[3]);
                        }
                        else
                        {
                            Console.WriteLine($"Nem látom a(z) {ertelmezett[1]}-t");
                        }
                        break;
                    case "Húzd":
                    case "húzd":
                        if (targyak.First(x => x.neve == ertelmezett[1]).huzhato)
                        {
                            jatekos.Huzas(ertelmezett[1]);
                        }
                        else
                        {
                            Console.WriteLine($"A(z) {ertelmezett[1]} nem húzható.");
                        }
                        break;
                    case "törd":
                    case "Törd":
                        if (targyak.First(x => x.neve == ertelmezett[1]).torheto)
                        {
                            jatekos.Tores(ertelmezett[1], ertelmezett[2]);
                        }
                        else
                        {
                            Console.WriteLine($"A(z) {ertelmezett[1]} nem törhető");
                        }
                        break;
                    case "Menj":
                    case "menj":
                        jatekos.Menni(ertelmezett[4]);
                        break;
                    case "Mentés":
                    case "mentés":
                    case "Ments":
                    case "ments":
                        Mentés();
                        break;
                    case "betöltés":
                    case "Betöltés":
                        Betoltes();
                        break;
                    default:
                        Console.WriteLine("Ilyen parancsot nem ismerek.");
                        break;
                }
            } while (!nyert);

            Console.ReadKey();
        }
        
        private static void Betoltes()
        {
            /// <summary>
            /// Betölti a a játékot egy korábbi állásból.
            /// <para>Csak akkor működik ha van mentett állás.</para>
            /// </summary>
            targyak.Clear();
            haz.Clear();
            string jatekosLoad = "";
            foreach (var elem in File.ReadAllLines("mentes.sav"))
            {
                string[] sor = elem.Split('\t');
                if (sor.Length == 1)
                {
                    targyak.Add(new targy(sor[0]));
                }
                else if (sor.Length == 2)
                {
                    targyak.Add(new targy(sor[0]));
                    haz.Add(new szoba(sor[1]));
                }
                else if (sor.Length == 3)
                {
                    targyak.Add(new targy(sor[0]));

                    haz.Add(new szoba(sor[1]));

                    jatekosLoad = sor[2];
                }
            }



            jatekos.Helye = jatekosLoad.Split(";")[0];
            string[] jatekosTargyak = jatekosLoad.Split(";")[1].Split(" ");
            for (int i = 0; i < jatekosTargyak.Length; i++)
            {
                jatekos.Leltar.Add(targyak.First(x => x.neve == jatekosTargyak[i]));
            }
        }

        private static void Mentés()
        {
            List<string> targyMentes = new List<string>();
            List<string> SzobaMentes = new List<string>();
            string jatekosMentes = "";
           

            foreach (targy targy in targyak)
            {
                string kapcsolat = "";
                for (int i = 0; i < targy.Kapcsolat.Count; i++)
                {
                    kapcsolat += targy.Kapcsolat[i] + " ";
                }
                string sor = targy.id + ";" + targy.neve + ";" + targy.kezdoHelye + ";" + targy.leiras + ";" + targy.felveheto + ";" + targy.nyithato + ";" + targy.huzhato + ";" + targy.torheto + ";" + targy.lathato + ";" + kapcsolat;
                targyMentes.Add(sor);
            }

            foreach (szoba szoba in haz)
            {
                string tartalom = "";
                for (int i = 0; i < szoba.Tartalma.Count; i++)
                {
                    tartalom += szoba.Tartalma[i].id + " ";
                }
                string sor = szoba.id + ";" + szoba.neve + ";" + szoba.leiras + ";" + szoba.eszak + ";" + szoba.kelet + ";" + szoba.del + ";" + szoba.nyugat + ";" + tartalom;
                SzobaMentes.Add(sor);
            }

            string leltar = "";
            for (int i = 0; i < jatekos.Leltar.Count; i++)
            {
                leltar += jatekos.Leltar[i].neve + " ";
            }
            jatekosMentes = jatekos.Helye + ";" + leltar;


            List<string> kiiratas = new List<string>();

            int max = 0;
            if (targyMentes.Count>SzobaMentes.Count)
            {
                max = targyMentes.Count;
            }
            else
            {
                max = SzobaMentes.Count;
            }
            

            for (int i = 0; i < max; i++)
            {
                string sor = "";
                if (i<1)
                {
                    sor = targyMentes[i] + "\t" + SzobaMentes[i] + "\t" + jatekosMentes;
                }
                else if (SzobaMentes.Count>i)
                {
                    sor = targyMentes[i] + "\t" + SzobaMentes[i];
                }
                else
                {
                    sor = targyMentes[i];
                }
                kiiratas.Add(sor);
            }

            File.WriteAllLines("mentes.sav", kiiratas);
            Console.WriteLine("A mentés sikerült a mentes.sav fájlba.");

        }

        private static string[] ertelmezes(string beadott)
        {
            string[] ertelmezett = beadott.Split(' ');



            string parancs = "";
            string mit = "";
            string mivel = "";
            string hova = "";
            string irany = "";

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
                else if(ertelmezett[i] == "Észak" || ertelmezett[i] == "Dél" || ertelmezett[i] == "Kelet" || ertelmezett[i] == "Nyugat")
                {
                    irany = ertelmezett[i];
                }
                else if(parancs =="")
                {
                    parancs = ertelmezett[i];
                }

            }
            string[] vegrehajtas = { parancs, mit, mivel, hova, irany};
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
