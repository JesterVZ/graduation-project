using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetingGetter.Model
{
    public class Result
    {
        public string Address { get; set; }
        public decimal rating { get; set; }
        public string error { get; set; }
        public double[] coords { get; set; }
    }
}
