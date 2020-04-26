using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Моделирование_информационного_центра
{
    interface ITimeRandomizer
    {
        double NextValue(int minValue, int maxValue);
    }

    class TimeRandomizer:ITimeRandomizer
    {
        private readonly Random rnd = new Random();

        public double NextValue(int minValue, int maxValue)
        {
            return rnd.Next(minValue, maxValue) + rnd.NextDouble();
        }
    }
}
