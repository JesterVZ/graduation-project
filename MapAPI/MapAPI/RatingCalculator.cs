using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapAPI
{
    public class RatingCalculator
    {
        public int CalculateIndex(Home home, Meter meter)
        {
            double debCount = home.Numberofdebetors / home.ApartmentCount;
            int meterIndex = 0;
            if (meter.Delta.TotalDays < 30)
            {
                meterIndex = 100;
            }
            if(meter.Delta.TotalDays > 30 && meter.Delta.TotalDays < 60)
            {
                meterIndex = 80;
            }
            if (meter.Delta.TotalDays > 60 && meter.Delta.TotalDays < 80)
            {
                meterIndex = 60;
            }
            if (meter.Delta.TotalDays > 80 && meter.Delta.TotalDays < 100)
            {
                meterIndex = 40;
            }
            if (meter.Delta.TotalDays > 100)
            {
                meterIndex = 0;
            }

            double repairIndex = 100 / home.RepairCount;
            return Calculate(debCount, meterIndex, repairIndex);
        }
        private int Calculate(double debCount, int meterIndex, double repairIndex)
        {
            double p = 0.33333333333;
            int result = (int)Math.Round(p * debCount + p * meterIndex + p * repairIndex );
            return result;
        }
    }
}
