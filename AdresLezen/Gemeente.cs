using System;
using System.Collections.Generic;
using System.Text;

namespace AdresLezen
{
    public class Gemeente
    {
        public Gemeente(int NISCode, string gemeentenaam)
        {
            this.NISCode = NISCode;
            Gemeentenaam = gemeentenaam;
        }

        public int NISCode { get; set; }
        public string Gemeentenaam { get; set; }
        public override string ToString()
        {
            return ($"{NISCode}, {Gemeentenaam}");
        }
    }
}
