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

        public string Helye { get => helye; set => helye = value; }
        internal List<targy> Leltar { get => leltar; set => leltar = value; }

        public void Leltaram()
        {
            if(Leltar.Count>0)
            {
                foreach (var item in Leltar)
                {
                    Console.WriteLine(item.neve);
                }
            }
            else
            {
                Console.WriteLine("Nincs semmi a leltáramban.");
            }
        }

        public string Nezd(string nev)
        {

            if (nev != "")
            {
                if (Program.targyak.Where(x => x.neve == nev).First().lathato == true)
                {
                    return Program.targyak.Where(x => x.neve == nev).First().leiras;
                }
                return $"Nem látok {Program.targyak.Where(x => x.neve == nev).First().neve}-(a)t";
            }
            else
            {
                return Program.haz.Where(x => x.id == Helye).First().leiras;
            }
        }
        public void Nyisd(string mit, string mivel)
        {
            switch (mit)
            {
                case "szekrény":
                    Console.WriteLine("Kinyitottad a szekrényt. Egy dobozt látsz.");
                    Program.targyak.Where(x => x.neve == "doboz").First().lathato = true;
                    break;
                case "doboz":
                    if (Leltar.Count > 0)
                    {
                        if (Leltar.Where(x => x.neve == mit).First().neve.Contains(mit))
                        {
                            Console.WriteLine("kinyitottad a dobozt. Egy kulcsot találsz benne");
                            Program.targyak.Where(x => x.neve == "kulcs").First().lathato = true;
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
                   string id = Program.targyak.Where(x => x.neve == mit).First().id;
                    if(mivel == "")
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
                    else if(Program.targyak.Where(x=> x.neve==mivel).First().Kapcsolat.Contains(id))
                    {
                        Console.WriteLine("Kinyitottad az ajtót");
                        Program.haz.Where(x => x.id == Helye).First().nyugat = true;
                    }
                    else
                    {
                        Console.WriteLine($"Ez a két tárgy {mit} és {mivel} nem nyitják egymást.");
                    }


                    break;
                default:
                    Console.WriteLine($"Az {mit} nem nyitható");
                    break;
            }
        }
        public void TargyMozgatas(string nev, string irany)
        {
            if (Program.targyak.Where(x => x.neve == nev).First().felveheto)
            {
                //egyszerüsíhető
                var HazTartalom = Program.haz.Select(x => new { x.Tartalma, x.id }).Where(y => y.id == Helye).First();
                if (irany == "fel")
                {
                    if (HazTartalom.Tartalma.Select(x => x.neve).Contains(nev))
                    {
                        var szoba = Program.haz.Select(x => x).Where(x => x.id == Helye).First();
                        var kivetendo = szoba.Tartalma.IndexOf(szoba.Tartalma.Where(x => x.neve == nev).First());
                        Leltar.Add(szoba.Tartalma.Where(x => x.neve == nev).First());
                        szoba.Tartalma.RemoveAt(kivetendo);
                        Console.WriteLine($"Felvettem a(z) {nev}-t");
                    }
                    else
                    {
                        Console.WriteLine($"A(z) {nev} nem itt van.");
                    }
                }
                else if (irany == "le")
                {
                    if (Leltar.Count > 0)
                    {
                        if (Leltar.Select(x => x.neve).Contains(nev))
                        {
                            int index = Leltar.IndexOf(Leltar.Where(x => x.neve == nev).First());
                            var szoba = Program.haz.Select(x => x).Where(x => x.id == Helye).First();
                            szoba.Tartalma.Add(Leltar.Where(x => x.neve == nev).First());
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
        public void Huzas(string nev)
        {
            if(Program.targyak.Where(x=> x.neve == nev).First().lathato)
            {
                switch (nev)
                {
                    case "szekrény":
                        Console.WriteLine("Elhúztad a szekrényt. Mögötte egy ablakot látsz.");
                        Program.targyak.Where(x => x.neve == "ablak").First().lathato = true;
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
        public void Tores(string mit, string mivel)
        {
            if(Program.targyak.Where(x=> x.neve == mit).First().lathato && Program.targyak.Where(x => x.neve == mivel).First().lathato)
            {
                switch (mit)
                {
                    case "ablak":
                    case "feszítővas":
                        string id = Program.targyak.Where(x => x.neve == mit).First().id;
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
                        else if (Program.targyak.Where(x => x.neve == mivel).First().Kapcsolat.Contains(id))
                        {
                            Console.WriteLine("A feszítővassal betöröd az ablakot");
                            Program.haz.Where(x => x.id == Helye).First().eszak = true;
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
    }
}
