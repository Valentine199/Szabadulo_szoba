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
            //A játék addig tart amíg a nyert nem lesz true. addig folyamatosan kér új parancsokat.

            Inicializalas();

            Console.WriteLine("Adjon meg egy parancsot");
            do
            {


                string beadott = Console.ReadLine();
                string[] ertelmezett = ertelmezes(beadott);

                switch (ertelmezett[0])
                {
                    case "leltar":
                    case "leltár":
                        jatekos.Leltaram();
                        break;
                    case "nézd":
                        Console.WriteLine(jatekos.Nezd(ertelmezett[1]));
                        break;
                    case "nyisd":
                        if (ertelmezett[1] == "")
                        {
                            Console.WriteLine("Mit nyissak ki?");
                        }
                        else if(Program.haz.First(x=> x.id==jatekos.Helye).Tartalma.Contains(targyak.First(x=>x.neve==ertelmezett[1])) ||jatekos.Leltar.Contains(targyak.First(x => x.neve == ertelmezett[1])))
                        {
                             
                                jatekos.Nyisd(ertelmezett[1], ertelmezett[2]);
                        }
                        else
                        {
                            Console.WriteLine($"A(z) {ertelmezett[1]} nincs itt.");
                        }
                        
                        break;
                    case "tedd":
                    case "vedd":
                        if(jatekos.Leltar.Count==0 && ertelmezett[3]=="le")
                        {
                            Console.WriteLine("Nincs a leltáramban semmi.");
                        }
                       else if (targyak.Select(x => x.neve).Contains(ertelmezett[1]))
                        {
                            if (targyak.First(x => x.neve == ertelmezett[1]).lathato)
                            {
                                jatekos.TargyMozgatas(ertelmezett[1], ertelmezett[3]);
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
                        if (Program.haz.First(x => x.id == jatekos.Helye).Tartalma.Contains(targyak.First(x => x.neve == ertelmezett[1])))
                            {


                            if (targyak.First(x => x.neve == ertelmezett[1]).huzhato)
                            {
                                jatekos.Huzas(ertelmezett[1]);
                            }
                            else
                            {
                                Console.WriteLine($"A(z) {ertelmezett[1]} nem húzható.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ez a tárgy nem ebben a szobában van.");
                        }
                        break;
                    case "törd":
                        if ((Program.haz.First(x => x.id == jatekos.Helye).Tartalma.Contains(targyak.First(x => x.neve == ertelmezett[1])) || jatekos.Leltar.Contains(targyak.First(x => x.neve == ertelmezett[1]))))
                        {
                            if (targyak.First(x => x.neve == ertelmezett[1]).torheto || targyak.First(x => x.neve == ertelmezett[2]).torheto)
                            {
                                jatekos.Tores(ertelmezett[1], ertelmezett[2]);
                            }
                            else
                            {
                                Console.WriteLine($"A(z) {ertelmezett[1]} nem törhető");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ezek a tárgyak nincsenek ebben a szobában.");
                        }
                        break;
                    case "menj":
                        jatekos.Menni(ertelmezett[4]);
                        break;
                    case "mentés":
                    case "ments":
                        Mentés();
                        break;
                    case "betöltés":
                        if(File.Exists("mentes.sav"))
                        {
                            Console.WriteLine("Biztosan betöltöd egy korábbi mentésed? Jelenlegi állásod elveszhet. (y/n)");
                            string valasz = Console.ReadLine().ToLower();
                            if(valasz=="y" ||valasz =="yes" ||valasz == "igen")
                            {
                                Betoltes();
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
        /// Kitörli az eddigi tárgyakat és helyükre a mentett elemeket helyezi.
        /// </summary>
        private static void Betoltes()
        {
            targyak.Clear();
            haz.Clear();
            jatekos.Leltar.Clear();

            string[] betoltottAdat = File.ReadAllLines("mentes.sav");

            foreach (string betoltottTargyak in betoltottAdat[0].Split('\t'))
            {
                targyak.Add(new targy(betoltottTargyak));
            }
            foreach (string betoltottSzobak in betoltottAdat[1].Split('\t'))
            {
                haz.Add(new szoba(betoltottSzobak));
            }
            foreach (string betoltottJatekos in betoltottAdat[2].Split('\t'))
            {
                jatekos.jatekosBetoltes(betoltottJatekos);
            }

            Console.WriteLine("Betöltés sikeres");
        }
        /// <summary>
        /// Az összes tárgy, szoba és játékos attributomot lementi. 
        /// <para>Először összegyűjti az adatokat majd elválasztva sorokba egymás mellé helyezi az elemeket. Egy sor felépítése: tárgy adatai tab szoba adatai tab játékos adatai.</para>
        /// </summary>
        private static void Mentés()
        {
            List<string> mentes = new List<string>();
            mentes.Add(string.Join('\t', targyak));
            mentes.Add(string.Join('\t', haz));
            mentes.Add(string.Join('\t', jatekos));
            File.WriteAllLines("mentes.sav", mentes);
            Console.WriteLine("A mentés sikerült a mentes.sav fájlba.");
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
                if (targyak.Select(x => x.neve).Contains(eldontendo))
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
