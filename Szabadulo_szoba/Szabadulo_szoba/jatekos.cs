using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Szabadulo_szoba
{
    class jatekos
    {
        string helye = "0";
        List<targy> leltar = new List<targy>();

        public jatekos()
        {

        }
        public jatekos(string helye, List<targy> leltar)
        {
            Helye = helye;
            Leltar = leltar;
        }

        public string Helye { get => helye; set => helye = value; }
        internal List<targy> Leltar { get => leltar; set => leltar = value; }

        /// <summary>
        /// Kiirja a leltár elemeit.
        /// </summary>
        public void Leltaram()
        {

            if(Leltar.Count>0)
            {
                foreach (var item in Leltar)
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
                if (Program.targyak.First(x => x.neve == nev).lathato == true)
                {
                    if (Program.haz.First(x => x.id == Helye).Tartalma.Select(x => x.neve).Contains(nev))
                    {
                        if (nev == "kád")
                        {
                            Program.targyak.First(x => x.neve == "feszítővas").lathato = true;
                        }
                        return Program.targyak.First(x => x.neve == nev).leiras;
                    }
                    return $"A(z) {nev} nem ebben a szobában van.";
                }
                return $"Nem látok {Program.targyak.First(x => x.neve == nev).neve}-(a)t";
            }
            else
            {
                return Program.haz.First(x => x.id == Helye).leiras;
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
                        Program.targyak.First(x => x.neve == "doboz").lathato = true;
                        break;
                    case "doboz":
                        if (Leltar.Count > 0)
                        {
                            if (Leltar.First(x => x.neve == mit).neve.Contains(mit))
                            {
                                Console.WriteLine("kinyitottad a dobozt. Egy kulcsot találsz benne");
                                Program.targyak.First(x => x.neve == "kulcs").lathato = true;
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
                    string id = Program.targyak.First(x => x.neve == mit).id;
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
                        else if (Program.targyak.First(x => x.neve == mivel).Kapcsolat.Contains(id))
                        {
                            if (Leltar.Contains(Program.targyak.First(x => x.neve == mivel)) || Leltar.Contains(Program.targyak.First(x => x.neve == mit)))
                            {
                                Console.WriteLine("Kinyitottad az ajtót");
                                Program.haz.First(x => x.id == Helye).nyugat = true;
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
            if (Program.targyak.First(x => x.neve == nev).felveheto)
            {
                //egyszerüsíhető
                var HazTartalom = Program.haz.Select(x => new { x.Tartalma, x.id }).First(y => y.id == Helye);
                if (irany == "fel" ||irany =="Fel")
                {
                    if (HazTartalom.Tartalma.Select(x => x.neve).Contains(nev))
                    {
                        var szoba = Program.haz.Select(x => x).First(x => x.id == Helye);
                        var kivetendo = szoba.Tartalma.IndexOf(szoba.Tartalma.First(x => x.neve == nev));
                        Leltar.Add(szoba.Tartalma.First(x => x.neve == nev));
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
                    if (Leltar.Count > 0)
                    {
                        if (Leltar.Select(x => x.neve).Contains(nev))
                        {
                            int index = Leltar.IndexOf(Leltar.First(x => x.neve == nev));
                            var szoba = Program.haz.Select(x => x).First(x => x.id == Helye);
                            szoba.Tartalma.Add(Leltar.First(x => x.neve == nev));
                            Leltar.RemoveAt(index);
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
                if (Program.targyak.Select(x => x.neve).Contains(nev))
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
            if(Program.targyak.First(x => x.neve == nev).lathato)
            {
                switch (nev)
                {
                    case "szekrény":
                        Console.WriteLine("Elhúztad a szekrényt. Mögötte egy ablakot látsz.");
                        Program.targyak.First(x => x.neve == "ablak").lathato = true;
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
            if(Program.targyak.First(x => x.neve == mit).lathato || Program.targyak.First(x => x.neve == mivel).lathato)
            {
                switch (mit)
                {
                    case "ablak":
                    case "feszítővas":
                        string id = Program.targyak.First(x => x.neve == mit).id;
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
                        else if (Program.targyak.Find(x => x.neve == mivel).Kapcsolat.Contains(id))
                        {
                            if(Leltar.Contains(Program.targyak.First(x => x.neve == mivel)) || Leltar.Contains(Program.targyak.First(x => x.neve == mit)))
                            {
                                Console.WriteLine("A feszítővassal betöröd az ablakot");
                                Program.haz.First(x => x.id == Helye).eszak = true;
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
                case "Észak":
                    switch (Helye)
                    {
                        case "0":
                            
                            if(Program.haz.First(x=> x.id==Helye).eszak)
                            {
                                Console.WriteLine("Gratulálunk, sikerült megszöknöd.");
                                Program.nyert = true;
                            }
                            else if (Program.targyak.First(x => x.neve == "ablak").lathato)
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
                case "Kelet":
                    switch (Helye)
                    {
                        case "0":
                            Console.WriteLine("Keletre nincs kijárat ");
                            break;
                        case "1":
                            Helye = "0";
                            Console.WriteLine(Program.haz.First(x => x.id == Helye).leiras);
                            break;
                        default:
                            break;
                    }
                    break;
                case "Nyugat":
                    switch (Helye)
                    {
                        case "0":
                            if(Program.haz.Where(x => x.id==Helye).First().nyugat)
                            {
                                Helye = "1";
                                Console.WriteLine(Program.haz.First(x => x.id == Helye).leiras);
                            }
                            else
                            {
                                Console.WriteLine("Nem tudok arra menni, zárva van az ajtó.");
                            }
                            break;
                        case "1":
                            Helye = "0";
                            Console.WriteLine(Program.haz.First(x => x.id == Helye).leiras);
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
    }
}
