using System;
using System.Collections.Generic;
using System.Text;

namespace AdresLezen
{
    public class Straatnaam : IComparable<Straatnaam>
    {
        public Straatnaam(int iD, string naam, Gemeente gemeente)
        {
            ID = iD;
            Naam = naam;
            Gemeente = gemeente;
        }
        public int ID { get; set; }
        public string Naam { get; set; }
        public Gemeente Gemeente { get; set; }

        public int CompareTo(Straatnaam straatnaam)
        {
            if (!ReferenceEquals(straatnaam, null))
            {
                int compareTo = Naam.CompareTo(straatnaam.Naam);
                if (compareTo == 0) compareTo = ID.CompareTo(straatnaam.ID);
                return compareTo;
            }
            else return +1;
        }

        public override bool Equals(object obj)
        {
            return obj is Straatnaam straatnaam &&
                   ID == straatnaam.ID &&
                   Naam == straatnaam.Naam &&
                   EqualityComparer<Gemeente>.Default.Equals(Gemeente, straatnaam.Gemeente);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Naam, Gemeente);
        }

        public override string ToString()
        {
            return ($"{ID}, {Naam}, {Gemeente}");
        }
    }
}
