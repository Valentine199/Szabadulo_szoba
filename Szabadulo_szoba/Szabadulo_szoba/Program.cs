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
                        else if(Program.haz.First(x=> x.id==jatekos.Helye).Tartalma.Contains(targyak.First(x=>x.neve==ertelmezett[1])) ||jatekos.Leltar.Contains(targyak.First(x => x.neve == ertelmezett[1])))
                        {
                             
                                jatekos.Nyisd(ertelmezett[1], ertelmezett[2]);
                        }
                        else
                        {
                            Console.WriteLine($"A(z) {ertelmezett[1]} nincs itt.");
                        }
                        
                        break;
                    case "Tedd":
                    case "tedd":
                    case "Vedd":
                    case "vedd":
                        if(jatekos.Leltar.Count==0 && ertelmezett[3]=="le")
                        {
                            Console.WriteLine("Nincs a leltáramban semmi.");
                        }
                       else if (targyak.First(x => x.neve == ertelmezett[1]).lathato)
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
                        if ((Program.haz.First(x => x.id == jatekos.Helye).Tartalma.Contains(targyak.First(x => x.neve == ertelmezett[1])) || jatekos.Leltar.Contains(targyak.First(x => x.neve == ertelmezett[1]))))
                        {
                            if (targyak.First(x => x.neve == ertelmezett[1]).torheto)
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
                        if(File.Exists("mentes.sav"))
                        {
                            Console.WriteLine("Biztosan betöltöd egy korábbi mentésed? Jelenlegi állásod elveszhet. (y/n)");
                            string valasz = Console.ReadLine();
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
            string jatekosLoad = "";
            string szobaLoad = "";
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
                    szobaLoad += sor[1] + "-";
                }
                else if (sor.Length == 3)
                {
                    targyak.Add(new targy(sor[0]));

                    szobaLoad += sor[1] + "-";
                    //haz.Add(new szoba(sor[1]));

                    jatekosLoad = sor[2];
                }
            }

            string[] szobaTargyak = szobaLoad.Split("-");

            foreach (var targyak in szobaTargyak)
            {
                if(targyak != "")
                {
                  haz.Add(new szoba(targyak));
                }
                
            }

            jatekos.Helye = jatekosLoad.Split(";")[0];
            string[] jatekosTargyak = jatekosLoad.Split(";")[1].Split(" ");
            for (int i = 0; i < jatekosTargyak.Length-1; i++)
            {
                jatekos.Leltar.Add(targyak.First(x => x.neve == jatekosTargyak[i]));
            }
            Console.WriteLine("Betöltés sikeres");
        }
        /// <summary>
        /// Az összes tárgy, szoba és játékos attributomot lementi. 
        /// <para>Először összegyűjti az adatokat majd elválasztva sorokba egymás mellé helyezi az elemeket. Egy sor felépítése: tárgy adatai tab szoba adatai tab játékos adatai.</para>
        /// </summary>
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
                    kapcsolat += targy.Kapcsolat[i] + ";";
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
