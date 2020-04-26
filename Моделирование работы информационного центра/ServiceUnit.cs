using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Моделирование_информационного_центра
{
    class ServiceUnit
    {
        /*Статус оператора в данный момент*/
        public bool Active { get; private set; } = false;

        public double ? TimeLeft { get; private set; } = null;

        private readonly int minServiceTime, maxServiceTime;

        public int MinServiceTime { get { return minServiceTime; } }

        public int MaxServiceTime { get { return maxServiceTime; } }

        private readonly ITimeRandomizer timeRandomizer;

        public ServiceUnit (int minServTime, int maxServTime, ITimeRandomizer timeRand = null)
        {
            minServiceTime = minServTime;
            maxServiceTime = maxServTime;
            timeRandomizer = timeRand ?? new TimeRandomizer();
        }

        /*В момент начала обработки заявки оператором, его статус Active становится true, 
         * задается время обработки заявки*/
        public virtual void StartServing()
        {
            Active = true;

            TimeLeft = timeRandomizer.NextValue(minServiceTime, maxServiceTime);
        }

        /*Оператор выполняет обработку заявки, пока не закончилось время*/
        public virtual void ContinueServing(double dt)
        {
            if (Active)
            {
                TimeLeft -= dt;

                Active = TimeLeft > 0;
            }
        }

        /*Когда оператор закончил обработку заявки, его статус Active становится false*/
        public virtual void StopServing()
        {
            Active = false;

            TimeLeft = null;
        }
    }
}
