using System;
using System.Collections.Generic;
using System.Text;

namespace Szabadulo_szoba
{
    class szoba
    {

        public string id { get;}
        public string neve { get;}
        public string  leiras { get;}
         List<targy> tartalma = new List<targy>();
        public List<targy> Tartalma { get => tartalma; set => tartalma = value; }

        
        public szoba(string adat)
        {
            string[] data = adat.Split(';');
            this.id = data[0];
            this.neve = data[1];
            this.leiras = data[2];
        }
        


    }
}
