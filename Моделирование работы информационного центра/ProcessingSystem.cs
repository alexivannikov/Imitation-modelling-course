using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Моделирование_информационного_центра
{
    class ProcessingSystem
    {
        public bool Active { get; private set; } = false;

        /*Длина очереди*/
        public int QueueLength { get; private set; } = 0;

        private readonly ServiceUnit computer;

        public ProcessingSystem(int minProcTime, int maxProcTime)
        {
            computer = new ServiceUnit(minProcTime, maxProcTime);
        }

        /*Поставить заявку в очередь*/
        public void EnqueueRequest()
        {
            Active = true;

            /*Пока компьютер занят, заявка остается в накопителе*/
            if (computer.Active)
            {
                QueueLength++;
            }
            /*В противном случае компьютер начинает обработку заявки*/
            else
            {
                computer.StartServing();
            }
        }

        public bool ContinueServing(double dt)
        {
            if (computer.Active)
            {
                computer.ContinueServing(dt);

                if (!computer.Active)
                {
                    return true;
                }
            }
            else if (QueueLength > 0)
            {
                computer.StartServing();
            }
            else
            {
                Active = false;
            }

            return false;
        }

        public void StopServing()
        {
            Active = false;

            computer.StopServing();
        }
    }
}
