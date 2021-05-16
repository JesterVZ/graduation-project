using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_FOR_MAP.ViewModel
{
    public class Result
    {
        public string Address { get; set; }
        public double[] coords { get; set; }
        public decimal Rating { get; set; }
        public Result(string address, float x, float y, decimal rating)
        {
            coords = new double[2];
            if (string.IsNullOrEmpty(address)){
                throw new ArgumentException();
            }
            Address = address;
            coords[0] = x;
            coords[1] = y;
            Rating = rating;
        }
    }
}
