using System;
using System.Collections.Generic;
using System.Text;

namespace AdresLezen
{
    public class Adres
    {
        public Adres(int iD, Straatnaam straatnaam, string appartementnummer, string busnummer, string huisnummer, string huisnummerLabel, AdresLocatie locatie)
        {
            ID = iD;
            Straatnaam = straatnaam;
            Appartementnummer = appartementnummer;
            Busnummer = busnummer;
            Huisnummer = huisnummer;
            HuisnummerLabel = huisnummerLabel;
            this.Locatie = locatie;
        }
        public int ID { get; set; }
        public Straatnaam Straatnaam { get; set; }
        public string Appartementnummer { get; set; }
        public string Busnummer { get; set; }
        public string Huisnummer { get; set; }
        public string HuisnummerLabel { get; set; }
        public AdresLocatie Locatie { get; set; }
        public int Postcode { get; set; }
        public override string ToString()
        {
            return ($"{ID}, {Appartementnummer}, {Busnummer}, {Huisnummer}, {HuisnummerLabel}, {Locatie.ID}, {Straatnaam.Gemeente}, {Straatnaam.Naam}");
        }
    }
}
