using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Szabadulo_szoba
{
    public class Main
    {
        static jatekos jatekos = new jatekos();
        static List<targy> targyak = new List<targy>();
        static List<szoba> haz = new List<szoba>();

        public static void Ini()
        {
            foreach (string targyAdat in File.ReadAllLines("targyInit.txt"))
            {
                targyak.Add(new targy(targyAdat));
            }
            foreach (string szobaAdat in File.ReadLines("szobaInit.txt"))
            {
                haz.Add(new szoba(szobaAdat));
            }
        }
    }
}
