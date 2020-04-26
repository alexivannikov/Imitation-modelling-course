using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Моделирование_информационного_центра
{
    class Client
    {
        /*Интервал времени, через который будет подана следующая заявка*/
        public double NextRequestInterval { get; private set; } = 0;

        private readonly int minRequestTimeInterval, maxRequestTimeInterval;

        private readonly ITimeRandomizer nextRequestTimeRandomizer;

        public Client(int minReqTimeInt, int maxReqTimeInt,
            ITimeRandomizer timeRand = null)
        {
            minRequestTimeInterval = minReqTimeInt;
            maxRequestTimeInterval = maxReqTimeInt;
            nextRequestTimeRandomizer = timeRand ?? new TimeRandomizer();
        }

        public bool MoveOn(double dt)
        {
            /*Счетчик времени до следующей заявки*/
            NextRequestInterval -= dt;

            if (NextRequestInterval <= 0)
            {
                NextRequestInterval = nextRequestTimeRandomizer
                    .NextValue(minRequestTimeInterval, maxRequestTimeInterval);
                return true;
            }
            return false;
        }
    }
}
