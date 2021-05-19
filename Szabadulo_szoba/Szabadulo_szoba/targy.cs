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
        private List<string> kapcsolat = new List<string>();
        public List<string> Kapcsolat { get => kapcsolat; set => kapcsolat = value; }

        

        public targy(string sor)
        {
            string[] data = sor.Split(';');
            this.id = data[0];
            this.neve = data[1];
            this.kezdoHelye = data[2];
            this.leiras = data[3];

            this.felveheto = (data[4] == "1");
            this.nyithato = (data[5] == "1");
            this.huzhato = (data[6] == "1");
            this.torheto = (data[7] == "1");

            for (int i = 8; i < data.Length; i++)
            {
                kapcsolat.Add(data[i]);
            }
        }
    }
}
