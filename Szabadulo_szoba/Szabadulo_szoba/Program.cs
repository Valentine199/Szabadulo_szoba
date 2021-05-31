using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Szabadulo_szoba
{
    class Program
    {
       public static Parancsok parancsok = new Parancsok();
        public static bool nyert = false;

        static void Main()
        {

            //A játék addig tart amíg a nyert nem lesz true. addig folyamatosan kér új parancsokat.

            TaroloEljarasok.Inicializalas();

            Console.WriteLine("Adjon meg egy parancsot");
            do
            {
                string beadott = Console.ReadLine();
                string[] ertelmezett = ertelmezes(beadott);

                switch (ertelmezett[0])
                {
                    case "leltár":
                        parancsok.Leltaram();
                        break;
                    case "nézd":
                        Console.WriteLine(parancsok.Nezd(ertelmezett[1]));
                        break;
                    case "nyisd":
                        if (ertelmezett[1] == "")
                        {
                            Console.WriteLine("Mit nyissak ki?");
                        }
                        else if (Ellenorzo.Elerheto(ertelmezett[1]))
                        {

                            parancsok.Nyisd(ertelmezett[1], ertelmezett[2]);
                        }
                        else
                        {
                            Console.WriteLine($"A(z) {ertelmezett[1]} nincs itt.");
                        }

                        break;
                    case "tedd":
                    case "vedd":
                        if (Parancsok.jatekos.Leltar.Count == 0 && ertelmezett[3] == "le")
                        {
                            Console.WriteLine("Nincs a leltáramban semmi.");
                        }
                        else if (Ellenorzo.Letezik(ertelmezett))
                        {
                            if (Ellenorzo.Lathato(ertelmezett[1]))
                            {
                                parancsok.TargyMozgatas(ertelmezett[1], ertelmezett[3]);
                            }
                            else
                            {
                                Console.WriteLine($"Nem látom a(z) {ertelmezett[1]}-t");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Nincs ilyen tárgy.");
                        }
                        break;
                    case "húzd":
                        if (Ellenorzo.Elerheto(ertelmezett[1]))
                        {
                            if (Ellenorzo.Lathato(ertelmezett[1]))
                            {

                                parancsok.Huzas(ertelmezett[1]);
                            }
                            else
                            {
                                Console.WriteLine($"A(z) {ertelmezett[1]} nem látható.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ez a tárgy nem ebben a szobában van.");
                        }
                        break;
                    case "törd":
                        if (Ellenorzo.Elerheto(ertelmezett[1]))
                        {
                            if(Ellenorzo.Lathato(ertelmezett[1]) || Ellenorzo.Lathato(ertelmezett[2]))
                            {
                                parancsok.Tores(ertelmezett[1], ertelmezett[2]);
                            }
                            else
                            {
                                Console.WriteLine($"A(z) {ertelmezett[1]} nem látható");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ezek a tárgyak nincsenek ebben a szobában.");
                        }
                        break;
                    case "menj":
                        parancsok.Menni(ertelmezett[4]);
                        break;
                    case "mentés":
                    case "ments":
                        TaroloEljarasok.Mentés();
                        break;
                    case "betöltés":
                        if (File.Exists("mentes.sav"))
                        {
                            Console.WriteLine("Biztosan betöltöd egy korábbi mentésed? Jelenlegi állásod elveszhet. (y/n)");
                            string valasz = Console.ReadLine().ToLower();
                            if (valasz == "y" || valasz == "yes" || valasz == "igen")
                            {
                                TaroloEljarasok.Betoltes();
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nem található mentés.");
                        }
                        break;
                    default:
                        Console.WriteLine("Ilyen parancsot nem ismerek.");
                        break;
                }
            } while (!nyert);

            Console.ReadKey();
        }

        




        /// <summary>
        /// feldolgozza a felhasználó áálltal megadott sort és elrendezi úgy hogy értelmezhető legyen a programnak.
        /// 5 eleme van melyek mindig ugyan úgy kvetik egymást
        /// 1. maga parancs amit végre kell hajtani.
        /// 2. mit mi az a tárgy amivel a parancsopt hajtsa végre.
        /// 3. mivel ha valamihez (pl ajtó nyitás) két tárgy kell akkor itt adható meg a másik.
        /// 4. hova eldönti hogy fel venni vagy letenni szeretnénk valamit.
        /// 5. irany az az irányamerre menni szeretnénk.
        /// </summary>
        /// <param name="beadott"></param>
        /// <returns></returns>
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
                string eldontendo = ertelmezett[i].ToLower();
                if (Parancsok.targyak.Select(x => x.neve).Contains(eldontendo))
                {
                    if (mit.Length > 0)
                    {
                        mivel = eldontendo;
                    }
                    else
                    {
                        mit = eldontendo;
                    }
                }
                else if(eldontendo=="fel"||eldontendo=="le")
                {
                    hova = eldontendo;
                }
                else if(eldontendo == "észak" || eldontendo == "dél" || eldontendo == "kelet" || eldontendo == "nyugat")
                {
                    irany = eldontendo;
                }
                else if(parancs =="")
                {
                    parancs = eldontendo;
                }

            }
            string[] vegrehajtas = { parancs, mit, mivel, hova, irany};
            return vegrehajtas;
        }
    }
}
