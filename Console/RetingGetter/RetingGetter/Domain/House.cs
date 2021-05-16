using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetingGetter.Domain
{
    public class House
    {
        public int Id { get; }
        public string Street { get; }
        public int Number { get; }
        public string Address { get; }
        public decimal Rating { get; set; }
        public double[] Coordinates { get; set; }
        public House(int id, string street, int number)
        {
            if (string.IsNullOrEmpty(street))
            {
                throw new ArgumentException($"'{nameof(street)}' cannot be null or empty.", nameof(street));
            }

            Id = id;
            Street = street;
            Number = number;

            Address = $"Пермь, улица {Street}, дом {Number}";
        }
    }
}
