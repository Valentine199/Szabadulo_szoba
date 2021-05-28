using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            try
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
                this.lathato = (data[8] == "1" || data[8] == "True");
                for (int i = 9; i < data.Length; i++)
                {
                    kapcsolat.Add(data[i]);
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Hiba a tárgyak betöltésekor. Hibás mentés, a kezdeti állapot betöltése");
                Parancsok.zavartalanBetoltes = false;
                Program.parancsok.Inicializalas();
            }
        }
        public override string ToString()
        {
            return String.Format($"{id};{neve};{kezdoHelye};{leiras};{felveheto};{nyithato};{huzhato};{torheto};{lathato};{string.Join(";", Kapcsolat)}");
        }
    }

    class szoba
    {
        public string id { get; }
        public string neve { get; }
        public string leiras { get; }
        public bool eszak { get; set; }
        public bool kelet { get; set; }
        public bool del { get; set; }
        public bool nyugat { get; set; }
        List<targy> tartalma = new List<targy>();
        public List<targy> Tartalma { get => tartalma; set => tartalma = value; }

        public szoba(string adat)
        {
            try
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
                if (data.Length > 7)
                {
                    TartalomFeltoltes(data);
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Hiba a szobák betöltésekor. Hibás mentés, a kezdeti állapot betöltése");
                Parancsok.zavartalanBetoltes = false;
                Program.parancsok.Inicializalas();
            }

        }

        private void TartalomFeltoltes(string[] data)
        {
            for (int i = 7; i < data.Length; i++)
            {
                tartalma.Add(Parancsok.targyak.First(x => x.neve == data[i]));
            }
        }

        public override string ToString()
        {
            return String.Format($"{id};{neve};{leiras};{eszak};{kelet};{del};{nyugat};{string.Join(';', Tartalma.Select(x => x.neve).ToArray())}");
        }
    }

    class jatekos
    {
        string helye = "0";
        List<targy> leltar = new List<targy>();

        public jatekos()
        {

        }
        public void jatekosBetoltes(string adat)
        {
            try
            {
                string[] data = adat.Split(';');
                Helye = data[0];
                if (data.Length > 1)
                {
                    for (int i = 1; i < data.Length; i++)
                    {
                        Leltar.Add(Parancsok.targyak.First(x => x.id == data[i]));
                    }
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Hiba a tárgyak betöltésekor. Hibás mentés, a kezdeti állapot betöltése");
                jatekosBetoltes("0");
                Parancsok.zavartalanBetoltes = false;
                Program.parancsok.Inicializalas();
            }

        }

        public string Helye { get => helye; set => helye = value; }
        internal List<targy> Leltar { get => leltar; set => leltar = value; }
        public override string ToString()
        {
            return string.Format($"{Helye};{string.Join(';', Leltar.Select(x => x.id).ToArray())}");
        }
    }
}
