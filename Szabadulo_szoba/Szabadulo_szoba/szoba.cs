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
            this.eszak = (data[3] == "1");
            this.kelet = (data[4] == "1");
            this.del = (data[5] == "1");
            this.nyugat = (data[6] == "1");

            if(data.Length>7)
            {
                string[] tartalom = data[5].Split(" ");
                for (int i = 0; i < tartalom.Length; i++)
                {
                    Tartalma.Add(Program.targyak.First(x => x.neve == tartalom[i]));
                }
            }
        }
    }
}
