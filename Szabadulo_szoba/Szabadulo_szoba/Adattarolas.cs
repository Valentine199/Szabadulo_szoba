using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace Szabadulo_szoba
{
    public enum SzobaID {nappali, fürdőszoba }
    public enum TargyID {szekrény, doboz, kulcs, ajtó, ablak, kád, feszítővas, ágy }

    [Serializable]
    class targy
    {
        public int id { get;}
        public string neve { get; }
        public int kezdoHelye { get; }
        public string leiras { get;} 
        public bool felveheto { get; }
        public bool nyithato { get;}
        public bool huzhato { get; }
        public bool torheto { get;}
        public bool lathato { get; set; }
        private List<int> kapcsolat = new List<int>();
        public List<int> Kapcsolat { get => kapcsolat; set => kapcsolat = value; }

        
        public targy(XmlNode adat)
        {

            this.id = (int)TargyID.Parse(typeof(TargyID), (adat["neve"].InnerText)) ;
            this.neve = adat["neve"].InnerText;
            this.leiras = adat["leiras"].InnerText;

            this.felveheto = bool.Parse(adat["felveheto"].InnerText);
            this.nyithato = bool.Parse(adat["nyihato"].InnerText);
            this.huzhato = bool.Parse(adat["huzhato"].InnerText);
            this.torheto = bool.Parse(adat["torheto"].InnerText);
            this.lathato = bool.Parse(adat["lathato"].InnerText);

            if(SzobaID.IsDefined(typeof(SzobaID), adat["helye"].InnerText))
            {
               this.kezdoHelye = (int)(SzobaID.Parse(typeof(SzobaID), (adat["helye"].InnerText)));
            }
            

            KapcsolatFeltoltese(adat["kapcsolat"].InnerText);
        }

        private void KapcsolatFeltoltese(string adat)
        {
            if (adat != "")
            {
                string[] kapcsolatok = adat.Split(';');
                foreach (string elem in kapcsolatok)
                {
                    Kapcsolat.Add((int) TargyID.Parse(typeof(TargyID), adat));
                }
            }
        }

        public override string ToString()
        {
            return String.Format($"{id};{neve};{kezdoHelye};{leiras};{felveheto};{nyithato};{huzhato};{torheto};{lathato};{string.Join(";", Kapcsolat)}");
        }
    }
    [Serializable]
    class szoba
    {
        public int id { get; }
        public string neve { get; }
        public string leiras { get; }
        public bool eszak { get; set; }
        public bool kelet { get; set; }
        public bool del { get; set; }
        public bool nyugat { get; set; }
        List<targy> tartalma = new List<targy>();
        public List<targy> Tartalma { get => tartalma; set => tartalma = value; }
        public szoba (XmlNode adat)
        {
            this.id = (int)SzobaID.Parse(typeof(SzobaID), (adat["neve"].InnerText));
            this.neve = adat["neve"].InnerText;
            this.leiras = adat["leiras"].InnerText;
            this.eszak = bool.Parse(adat["eszak"].InnerText);
            this.kelet = bool.Parse(adat["kelet"].InnerText);
            this.del = bool.Parse(adat["del"].InnerText);
            this.nyugat = bool.Parse(adat["nyugat"].InnerText);
        }

        public override string ToString()
        {
            return String.Format($"{id};{neve};{leiras};{eszak};{kelet};{del};{nyugat};{string.Join(';', Tartalma.Select(x => x.neve).ToArray())}");
        }
    }
    [Serializable]
    class jatekos
    {
        int helye = 0;
        List<targy> leltar = new List<targy>();

        public jatekos()
        {

        }

        public int Helye { get => helye; set => helye = value; }
        internal List<targy> Leltar { get => leltar; set => leltar = value; }
        public override string ToString()
        {
            return string.Format($"{Helye};{string.Join(';', Leltar.Select(x => x.id).ToArray())}");
        }
    }

    class TaroloEljarasok
    {
        public static void Inicializalas()
        {
            Parancsok.targyak.Clear();
            Parancsok.haz.Clear();
            Parancsok.jatekos.Leltar.Clear();

            XmlDocument doc = new XmlDocument();
            doc.Load("targyInit.xml");

            foreach (XmlNode node in doc.DocumentElement)
            {
                Parancsok.targyak.Add(new targy(node));
            }

            doc.Load("szobaInit.xml");
            foreach (XmlNode node in doc.DocumentElement)
            {
                Parancsok.haz.Add(new szoba(node));
            }

           foreach (szoba szobak in Parancsok.haz)
            {
                var temp = Parancsok.targyak.Select(x => x).Where(x => x.kezdoHelye == szobak.id);
                foreach (targy targy in temp)
                {
                    szobak.Tartalma.Add(targy);
                }
            }
        }

        /// <summary>
        /// Az összes tárgy, szoba és játékos attributomot lementi. 
        /// <para>Először összegyűjti az adatokat majd elválasztva sorokba egymás mellé helyezi az elemeket. Egy sor felépítése: tárgy adatai tab szoba adatai tab játékos adatai.</para>
        /// </summary>
        public static void Mentés()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("mentes.sav", FileMode.Create, FileAccess.Write);

            formatter.Serialize(stream, Parancsok.targyak);
            formatter.Serialize(stream, Parancsok.haz);
            formatter.Serialize(stream, Parancsok.jatekos);
            stream.Close();

            Console.WriteLine("A mentés sikerült a mentes.sav fájlba.");
        }
        /// <summary>
        /// Kitörli az eddigi tárgyakat és helyükre a mentett elemeket helyezi.
        /// </summary>
        public static void Betoltes()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("mentes.sav", FileMode.Open, FileAccess.Read);

            try
            {
                Parancsok.targyak = (List<targy>)formatter.Deserialize(stream);
                Parancsok.haz = (List<szoba>)formatter.Deserialize(stream);
                Parancsok.jatekos = (jatekos)formatter.Deserialize(stream);
                stream.Close();
                Console.WriteLine("A betöltés sikeres.");
            }
            catch (Exception)
            {
                stream.Close();
                Console.WriteLine("Betöltés sikertelen. Hibás fájl. Kezdeti állapot betöltése");
                Inicializalas();
            }

        }
    }
}
