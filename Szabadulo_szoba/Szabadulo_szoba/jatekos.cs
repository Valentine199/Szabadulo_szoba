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
        public void Nyisd(string nev)
        {
            switch (nev)
            {
                case "szekrény":
                    Console.WriteLine("Kinyitottad a szekrényt. Egy dobozt látsz.");
                    Program.targyak.Where(x => x.neve == "doboz").First().lathato = true;
                    break;
                case "doboz":
                    if (Leltar.Count > 0)
                    {
                        if (Leltar.Where(x => x.neve == nev).First().neve.Contains(nev))
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
                default:
                    Console.WriteLine();
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
    }
}
