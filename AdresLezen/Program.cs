using System;
using System.Collections.Generic;

namespace AdresLezen
{
    class Program
    {
        static void Main()
        {
            string connectionString = @"Data Source=LAPTOP-1U6AQSEQ\SQLEXPRESS;Initial Catalog=AdresLezen;Integrated Security=True";
            AdresBeheer adb = new AdresBeheer(connectionString);
            //adb.LeesGML();

            Bevraging bvg = new Bevraging(connectionString);
            //Adres adres = bvg.GetAdres(2000000001);
            //Console.WriteLine(adres);

            //SortedSet<Straatnaam> straten = bvg.GetStratenPerGemeente("Wetteren");
            //foreach (var s in straten)
            //    Console.WriteLine(s);

            //List<Adres> adressen = bvg.GetAdressenPerStraatnaamId(5);
            //foreach (var a in adressen)
            //    Console.WriteLine(a);

            Dictionary<string, int> stratenPerGem = bvg.GetAantalStratenPerGemeente();
            foreach (var x in stratenPerGem)
                Console.WriteLine($"[{x.Key};{x.Value}]");
        }
    }
}
