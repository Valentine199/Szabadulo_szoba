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
                return Program.targyak.Where(x => x.neve == nev).First().leiras;
            }
            else
            {
                return Program.haz.Where(x => x.id == Helye).First().leiras;
            }
        }
        

    }
}
