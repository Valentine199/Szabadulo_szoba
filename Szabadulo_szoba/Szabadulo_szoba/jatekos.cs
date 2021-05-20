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

        public string Nezd(string nev)
        {
            
            if(nev !="")
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
            var HazTartalom = Program.haz.Select(x => new { x.Tartalma, x.id}).Where(y=> y.id==Helye).First();
            if (irany == "fel")
            {
                if (HazTartalom.Tartalma.Select(x => x.neve).Contains(nev))
                {
                    for (int i = 0; i < HazTartalom.Tartalma.Count; i++)
                    {
                        if(HazTartalom.Tartalma[i].neve==nev)
                        {
                            HazTartalom.Tartalma.Remove(HazTartalom.Tartalma[i]);
                        }
                    }
                    Leltar.Add(Program.targyak.Select(x => x = x).Where(x => x.neve == nev).First());
                    int a = 12;
                }
            }
            else if (Leltar.Select(x => x.neve).Contains(nev))
            {

            }
            
           
        }
        

    }
}
