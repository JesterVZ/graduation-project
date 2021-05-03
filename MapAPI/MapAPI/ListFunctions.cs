using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapAPI
{
    public class ListFunctions
    {
        public List<Home> Home;
        public List<Meter> Meters;
        public ListFunctions()
        {
            Home = new List<Home>();
            Meters = new List<Meter>();
        }
        public void FillList(Home home)
        {
            Home.Add(home);
        }
        public void FillList(Meter meters)
        {
            Meters.Add(meters);
        }
    }
}
