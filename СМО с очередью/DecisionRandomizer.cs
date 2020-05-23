using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace СМО_с_очередью
{
    class DecisionRandomizer
    {
        private double probability; //Вероятность возврата заявки

        private Random rnd = new Random();

        public DecisionRandomizer(double prob)
        {
            probability = prob;
        }

        public bool ShouldPerformAction()
        {
            return rnd.NextDouble() <= probability;
        }
    }
}
