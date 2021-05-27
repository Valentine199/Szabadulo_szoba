using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Szabadulo_szoba
{
    class szoba
    {
        // át kell helyezni a "targyba"
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

            //külön methode
            if (data.Length>7)
            {
                TartalomFeltoltes(data);
            }

        }

        private void TartalomFeltoltes(string[] data)
        {
            for (int i = 7; i < data.Length; i++)
            {
                tartalma.Add(Program.targyak.First(x => x.neve == data[i]));
            }
        }

        public override string ToString()
        {
            return String.Format($"{id};{neve};{leiras};{eszak};{kelet};{del};{nyugat};{string.Join(';', Tartalma.Select(x=> x.neve).ToArray())}");
        }
    }
}
