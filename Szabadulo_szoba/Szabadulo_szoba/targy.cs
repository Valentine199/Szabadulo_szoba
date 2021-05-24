using System;
using System.Collections.Generic;
using System.Text;

namespace Szabadulo_szoba
{
    class targy
    {
        public string id { get;}
        public string neve { get; }
        public string kezdoHelye { get; }
        //public bool felVanVeve { get; set; } Nem biztos hogy szükséges.
        public string leiras { get;} 
        public bool felveheto { get; }
        public bool nyithato { get;}
        public bool huzhato { get; }
        public bool torheto { get;}
        public bool lathato { get; set; }
        private List<string> kapcsolat = new List<string>();
        public List<string> Kapcsolat { get => kapcsolat; set => kapcsolat = value; }

        

        public targy(string sor)
        {
            string[] data = sor.Split(';');
            this.id = data[0];
            this.neve = data[1];
            this.kezdoHelye = data[2];
            this.leiras = data[3];

            this.felveheto = (data[4] == "1" || data[4] == "True");
            this.nyithato = (data[5] == "1" || data[5] == "True");
            this.huzhato = (data[6] == "1" || data[6] == "True");
            this.torheto = (data[7] == "1" || data[7] == "True");
            this.lathato= (data[8] == "1" || data[8] == "True");
            for (int i = 9; i < data.Length; i++)
            {
                kapcsolat.Add(data[i]);
            }
        }
    }
}
