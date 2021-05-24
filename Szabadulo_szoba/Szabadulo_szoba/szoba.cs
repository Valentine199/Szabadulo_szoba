using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Szabadulo_szoba
{
    class szoba
    {

        public string id { get;}
        public string neve { get;}
        public string  leiras { get;}
        public bool eszak { get; set; }
        public bool kelet { get; set; }
        public bool del { get; set; }
        public bool nyugat { get; set; }
        List<targy> tartalma = new List<targy>();
        public List<targy> Tartalma { get => tartalma; set => tartalma = value; }

        public szoba(string adat)
        {
            string[] data = adat.Split(';');
            this.id = data[0];
            this.neve = data[1];
            this.leiras = data[2];
            this.eszak = (data[3] == "1" || data[3] == "True");
            this.kelet = (data[4] == "1" || data[4] == "True");
            this.del = (data[5] == "1" || data[5] == "True");
            this.nyugat = (data[6] == "1" || data[6] == "True");

            if(data.Length>7)
            {
                string[] tartalom = data[7].Split(" ");
                for (int i = 0; i < tartalom.Length-1; i++)
                {
                    Tartalma.Add(Program.targyak.First(x => x.id == tartalom[i]));
                }
            }
        }
    }
}
