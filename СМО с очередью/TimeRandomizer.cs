using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace СМО_с_очередью
{
    interface TimeGenerator
    {
        double NextValue();
    }

/*Моделирование генератора заявок (равномерное распределение)*/
    class UniformTimeRandomizer: TimeGenerator
    {
        private double a, b;

        private Random rnd = new Random();

        public UniformTimeRandomizer(double leftValue, double rightValue)
        {
            a = leftValue;
            b = rightValue;
        }


        public double NextValue()
        {
            double T = 0;

            T = a + (b - a) * rnd.NextDouble();

            return T;
        }
    }

    /*Моделирование обслуживающего аппарата (нормальное распределение)*/
    class NormalTimeRandomizer : TimeGenerator
    {
        private double M, sigma;

        private Random rnd = new Random();

        public NormalTimeRandomizer(double m, double s)
        {
            M = m;
            sigma = s;
        }

        public double NextValue()
        {
            double s = 0, T = 0;

            for(int i = 1; i < 12; i++)
            {
                s += rnd.NextDouble();
            }

            T = Math.Abs(M + (s - 6) * sigma);

            return T;
        }
    }
}
