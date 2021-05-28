using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Szabadulo_szoba
{
    class Ellenorzo
    {
        //Ellenőrzi, hogy az adott tárgy látható-e.
        public static bool Lathato(string nev)
        {
            return Parancsok.targyak.First(x => x.neve == nev).lathato;
        }
        //Ellenőrzi, hogy az adott elem ugyan abban a szobában van mint a játékos, vagy a játékos leltárában van-e?
        public static bool Elerheto(string nev)
        {
            return Parancsok.haz.First(x => x.id == Parancsok.jatekos.Helye).Tartalma.Select(x => x.neve).Contains(nev) || LeltarambanVan(nev);
        
        }
        //Ellenőrzi, hogy az adott tárgy a játékos leltárában van-e.
        public static bool LeltarambanVan(string nev)
        {
            return Parancsok.jatekos.Leltar.Select(x => x.neve).Contains(nev);
        }
        //Ellenőrzi, hogy az adott két tárgy egymással interakcióba léphet-e.
        public static bool KapcsolatbanVannak(string mit, string mivel)
        {
            return Parancsok.targyak.First(x => x.neve == mivel).Kapcsolat.Contains(Parancsok.targyak.First(x => x.neve == mit).id);
        }
        //Ellenőrzi, hogy a felhasználó egy olyan tárgyat adott meg amely létezik és a program tud dolgozni vele.
        public static bool Letezik(string[] ertelmezett)
        {
            return Parancsok.targyak.Select(x => x.neve).Contains(ertelmezett[1]);
        }
    }
}
