﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace Szabadulo_szoba
{
   
    class Parancsok
    {
        public static jatekos jatekos = new jatekos();
        public static List<targy> targyak = new List<targy>();
        public static List<szoba> haz = new List<szoba>();

        /// <summary>
        /// Kiirja a leltár elemeit.
        /// </summary>
        public void Leltaram()
        {

            if(jatekos.Leltar.Count>0)
            {
                foreach (var item in jatekos.Leltar)
                {
                    Console.WriteLine("--> " + item.neve);
                }
            }
            else
            {
                Console.WriteLine("Nincs semmi a leltáramban.");
            }
        }

        /// <summary>
        /// Paraméter nélkül megadja a szoba leírását.
        /// Ha megadunk egy tárgyat mögötte, akkor annak a leírását adja vissza ha látjuk és a szobában van.
        /// </summary>
        /// <param name="nev"></param>
        /// <returns></returns>
        public string Nezd(string nev)
        {
            if (nev != "")
            {
                if (targyak.First(x => x.neve == nev).lathato == true)
                {
                    if (haz.First(x => x.id == jatekos.Helye).Tartalma.Select(x => x.neve).Contains(nev) || jatekos.Leltar.Select(x=> x.neve).Contains(nev))
                    {
                        if (nev == "kád")
                        {
                            targyak.First(x => x.neve == "feszítővas").lathato = true;
                        }
                        return targyak.First(x => x.neve == nev).leiras;
                    }
                    return $"A(z) {nev} nem ebben a szobában van.";
                }
                return $"Nem látok {targyak.First(x => x.neve == nev).neve}-(a)t";
            }
            else
            {
                return haz.First(x => x.id == jatekos.Helye).leiras;
            }
        }
        /// <summary>
        /// Kinyitja a paraméterben megadott tárgyat. 
        /// Ha két tárgy kell valaminek a kinyitásához akkor ellenőrzi mind a kettő meglétét
        /// </summary>
        /// <param name="mit"></param>
        /// <param name="mivel"></param>
        public void Nyisd(string mit, string mivel)
        {
            
                switch (mit)
                {
                    case "szekrény":
                        Console.WriteLine("Kinyitottad a szekrényt. Egy dobozt látsz.");
                        targyak.First(x => x.neve == "doboz").lathato = true;
                        break;
                    case "doboz":
                        if (jatekos.Leltar.Count > 0)
                        {
                            if (jatekos.Leltar.First(x => x.neve == mit).neve.Contains(mit))
                            {
                                Console.WriteLine("kinyitottad a dobozt. Egy kulcsot találsz benne");
                                targyak.First(x => x.neve == "kulcs").lathato = true;
                            }
                            else
                            {
                                Console.WriteLine("Könnyebb lenne ha felvenném és úgy nyitnám ki.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Könnyebb lenne ha felvenném és úgy nyitnám ki.");
                        }
                        break;
                    case "kulcs":
                    case "ajtó":
                    string id = targyak.First(x => x.neve == mit).id;
                    if (mivel == "")
                        {
                            switch (mit)
                            {
                                case "kulcs":
                                    Console.WriteLine("Mivel használjam a kulcsot?");
                                    break;
                                case "ajtó":
                                    Console.WriteLine("Az ajtó kulcsra van zárva");
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (targyak.First(x => x.neve == mivel).Kapcsolat.Contains(id))
                        {
                            if (jatekos.Leltar.Contains(targyak.First(x => x.neve == mivel)) || jatekos.Leltar.Contains(targyak.First(x => x.neve == mit)))
                            {
                                Console.WriteLine("Kinyitottad az ajtót");
                                haz.First(x => x.id == jatekos.Helye).nyugat = true;
                            }
                            else
                            {
                                Console.WriteLine("Ehhez egy olyan tárgy kell ami nincs a leltáramban.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Ez a két tárgy {mit} és {mivel} nem nyitják egymást.");
                        }


                        break;
                case "ablak":
                    Console.WriteLine("Az ablak zárva van");
                    break;
                    default:
                        Console.WriteLine($"Az {mit} nem nyitható");
                        break;
                }
            
        }

        /// <summary>
        /// A név alapján tudja, hogy melyik tárgyat kell mozgatni.Az irány azt adja meg, hogy le kell-e tenni vagy felvenni.
        /// Ha felvenni akkor a tárgyat hozzáadjuk a játékos leltár listájához és a szobáéból elvesszük.
        /// Ha letnni szeretnénk akkor a tárgyat hozzáadjuk a szoba leltárához majd elvesszük a játékos leltárából.
        /// </summary>
        /// <param name="nev"></param>
        /// <param name="irany"></param>
        public void TargyMozgatas(string nev, string irany)
        {
            if (targyak.First(x => x.neve == nev).felveheto)
            {
                //egyszerüsíhető
                var HazTartalom = haz.Select(x => new { x.Tartalma, x.id }).First(y => y.id == jatekos.Helye);
                if (irany == "fel" ||irany =="Fel")
                {
                    if (HazTartalom.Tartalma.Select(x => x.neve).Contains(nev))
                    {
                        var szoba = haz.Select(x => x).First(x => x.id == jatekos.Helye);
                        var kivetendo = szoba.Tartalma.IndexOf(szoba.Tartalma.First(x => x.neve == nev));
                        jatekos.Leltar.Add(szoba.Tartalma.First(x => x.neve == nev));
                        szoba.Tartalma.RemoveAt(kivetendo);
                        Console.WriteLine($"Felvettem a(z) {nev}-t");
                    }
                    else
                    {
                        Console.WriteLine($"A(z) {nev} nem itt van.");
                    }
                }
                else if (irany == "le" || irany =="Le")
                {
                    if (jatekos.Leltar.Count > 0)
                    {
                        if (jatekos.Leltar.Select(x => x.neve).Contains(nev))
                        {
                            int index = jatekos.Leltar.IndexOf(jatekos.Leltar.First(x => x.neve == nev));
                            var szoba = haz.Select(x => x).First(x => x.id == jatekos.Helye);
                            szoba.Tartalma.Add(jatekos.Leltar.First(x => x.neve == nev));
                            jatekos.Leltar.RemoveAt(index);
                            Console.WriteLine($"Letettem a(z) {nev}-t");
                        }
                        else
                        {
                            Console.WriteLine("Ez nincs a leltáramban");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Nincs semmi a Leltáramban");
                    }
                }


            }
            else
            {
                if (targyak.Select(x => x.neve).Contains(nev))
                {
                    Console.WriteLine($"A(z) {nev} nem mozgatható");
                }
                else
                {
                    Console.WriteLine($"A(z) {nev} nem található");
                }
            }


        }
        /// <summary>
        /// A tárgy neve alapján eldönti, hogy el lehet-e húzni és végrehajtja a változtatásokat amiket ez okozott.
        /// </summary>
        /// <param name="nev"></param>
        public void Huzas(string nev)
        {
            if(targyak.First(x => x.neve == nev).lathato)
            {
                switch (nev)
                {
                    case "szekrény":
                        Console.WriteLine("Elhúztad a szekrényt. Mögötte egy ablakot látsz.");
                        targyak.First(x => x.neve == "ablak").lathato = true;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Console.WriteLine("Nem látom a(z) {0}-t", nev);
            }
        }
        /// <summary>
        /// Elenőrzi hogy a két tárgy közül az egyikkel el lehet-e törni a másikat. Ha mind a kettő elérhető (Leltárban van és látható) akkor törhető a tárgy.
        /// </summary>
        /// <param name="mit"></param>
        /// <param name="mivel"></param>
        public void Tores(string mit, string mivel)
        {
            if(targyak.First(x => x.neve == mit).lathato || targyak.First(x => x.neve == mivel).lathato)
            {
                switch (mit)
                {
                    case "ablak":
                    case "feszítővas":
                        string id = targyak.First(x => x.neve == mit).id;
                        if (mivel == "")
                        {
                            switch (mit)
                            {
                                case "feszítővas":
                                    Console.WriteLine("Mit törjek össze?");
                                    break;
                                case "ablak":
                                    Console.WriteLine("A kezeddel nem tudod összetörni, mert megvágnád magad.");
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (targyak.Find(x => x.neve == mivel).Kapcsolat.Contains(id))
                        {
                            if(jatekos.Leltar.Contains(targyak.First(x => x.neve == mivel)) || jatekos.Leltar.Contains(targyak.First(x => x.neve == mit)))
                            {
                                Console.WriteLine("A feszítővassal betöröd az ablakot");
                                haz.First(x => x.id == jatekos.Helye).eszak = true;
                            }
                            else
                            {
                                Console.WriteLine("Ehhez egy olyan tárgy kell ami nincs a leltáramban.");
                            }
                            
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                Console.WriteLine("Nem látom ezeket a tárgyakat.");
            }
        }
        /// <summary>
        /// A játékos helyéhez képest eldönti, hogy az elmozdulás lehetséges vagy változtat-e bármin. Ha igen végrehajtja a változtatásokat.
        /// </summary>
        /// <param name="irany"></param>
        public void Menni(string irany)
        {
            switch (irany)
            {
                case "észak":
                    switch (jatekos.Helye)
                    {
                        case "0":
                            
                            if(haz.First(x=> x.id== jatekos.Helye).eszak)
                            {
                                Console.WriteLine("Gratulálunk, sikerült megszöknöd.");
                                Program.nyert = true;
                            }
                            else if (targyak.First(x => x.neve == "ablak").lathato)
                            {
                                Console.WriteLine("Északra nem mehetek, útban van az ablak");
                            }
                            else
                            {
                                Console.WriteLine("Északra csak a szekrény van");
                            }
                            break;
                        case "1":
                            Console.WriteLine("Északra nincs kijárat.");
                            break;
                        default:
                            break;
                    }
                    break;
                case "kelet":
                    switch (jatekos.Helye)
                    {
                        case "0":
                            Console.WriteLine("Keletre nincs kijárat ");
                            break;
                        case "1":
                            jatekos.Helye = "0";
                            Console.WriteLine(haz.First(x => x.id == jatekos.Helye).leiras);
                            break;
                        default:
                            break;
                    }
                    break;
                case "nyugat":
                    switch (jatekos.Helye)
                    {
                        case "0":
                            if(haz.Where(x => x.id== jatekos.Helye).First().nyugat)
                            {
                                jatekos.Helye = "1";
                                Console.WriteLine(haz.First(x => x.id == jatekos.Helye).leiras);
                            }
                            else
                            {
                                Console.WriteLine("Nem tudok arra menni, zárva van az ajtó.");
                            }
                            break;
                        case "1":
                            Console.WriteLine("Arra nincs kijárat");
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    Console.WriteLine($"Arra nincs kijárat");
                    break;
            }
        }

        /// <summary>
        /// Kitörli az eddigi tárgyakat és helyükre a mentett elemeket helyezi.
        /// </summary>
        public void Betoltes()
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
        public void Mentés()
        {
            List<string> mentes = new List<string>();
            mentes.Add(string.Join('\t', targyak));
            mentes.Add(string.Join('\t', haz));
            mentes.Add(string.Join('\t', jatekos));
            File.WriteAllLines("mentes.sav", mentes);
            Console.WriteLine("A mentés sikerült a mentes.sav fájlba.");
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