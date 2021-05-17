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
        public decimal DebetorIndex { get; set; }
        public decimal MeterIndex { get; set; }
        public decimal RepairIndex { get; set; }
        public Result(string address, float x, float y, decimal rating, decimal debetorIndex, decimal meterIndex, decimal requestIndex)
        {
            coords = new double[2];
            if (string.IsNullOrEmpty(address)){
                throw new ArgumentException();
            }
            Address = address;
            coords[0] = x;
            coords[1] = y;
            Rating = rating;
            DebetorIndex = debetorIndex;
            MeterIndex = meterIndex;
            RepairIndex = requestIndex;
        }
    }
}
