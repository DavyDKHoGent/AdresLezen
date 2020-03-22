using System;
using System.Collections.Generic;
using System.Text;

namespace AdresLezen
{
    public class AdresLocatie
    {
        public AdresLocatie(double x, double y)
        {
            X = x;
            Y = y;
        }
        public AdresLocatie(int iD, double x, double y)
        {
            ID = iD;
            X = x;
            Y = y;
        }
        public int ID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
