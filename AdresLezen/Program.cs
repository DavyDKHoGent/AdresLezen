using System;
using System.Collections.Generic;

namespace AdresLezen
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=LAPTOP-1U6AQSEQ\SQLEXPRESS;Initial Catalog=AdresLezen;Integrated Security=True";
            AdresBeheer adb = new AdresBeheer(connectionString);
            //adb.LeesGML();
            adb.TestGML();

            Bevraging bvg = new Bevraging(connectionString);
            //Adres adres = bvg.GetAdres(2000000001);
            //Console.WriteLine(adres);

            //SortedSet<Straatnaam> straten = bvg.GetStratenPerGemeente("Wetteren");
            //foreach (var s in straten)
            //    Console.WriteLine(s);

            //List<Adres> adressen = bvg.GetAdressenPerStraatnaamId(5);
            //foreach (var a in adressen)
            //    Console.WriteLine(a);
        }
    }
}
