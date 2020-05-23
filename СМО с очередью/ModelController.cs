using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace СМО_с_очередью
{
    abstract class ModelController
    {
        protected int MAX_QUEUE_LENGTH; //Максимальная длина очереди

        protected int numRequests; //Число заявок

        private UniformTimeRandomizer recvTimeGenerator; //Генератор заявок
        private NormalTimeRandomizer handlTimeGenerator; //ОА

        private DecisionRandomizer loopBackDecisionRandomizer; //Вовзращение заявки

        protected double[] recvTime; //Время получения заявки

        protected bool requestIsBeingHandled; //Заявка обрабатывается

        protected double handlingFinishTime; //Время обработки заявки

        public double CurrentTime { get; protected set; } //Текущее время
        public int QueueLength { get; protected set; } //Длина очереди
        public int MaxQueueLength { get; protected set; } //Максимальная длина очереди

        public int NumSentRequests { get; protected set; } //Количество отправленных заявок
        public int NumReceivedRequests { get; protected set; } //Количество полученных заявок
        public int NumDeclinedRequests { get; protected set; } //Количество отклоненных заявок
        public int NumHandledRequests { get; protected set; } //Количество обработанных заявок
        public int NumLoopedBackRequests { get; protected set; } //Количество возвращенных заявок

        public ModelController(int nReq, UniformTimeRandomizer recvTimeGen,
            NormalTimeRandomizer handlTimeGen, DecisionRandomizer lbDecisionRand)
        {
            numRequests = nReq;

            recvTime = new double[numRequests];

            recvTimeGenerator = recvTimeGen;
            handlTimeGenerator = handlTimeGen;
            loopBackDecisionRandomizer = lbDecisionRand;

            MAX_QUEUE_LENGTH = nReq;
        }

        /*Инициализация*/
        public void Initialize()
        {
            recvTime[0] = recvTimeGenerator.NextValue();

            /*Время работы генератора заявок*/
            for (int i = 1; i < numRequests; i++)
            {
                recvTime[i] = recvTime[i - 1] + recvTimeGenerator.NextValue();
            }

            requestIsBeingHandled = false;

            QueueLength = 0;
            MaxQueueLength = 0;
            CurrentTime = 0;

            NumSentRequests = 0;
            NumReceivedRequests = 0;
            NumDeclinedRequests = 0;
            NumHandledRequests = 0;
            NumLoopedBackRequests = 0;
        }

        public void MoveOn()
        {
            PassTime();

            MaxQueueLength = Math.Max(MaxQueueLength, QueueLength);

            if (requestIsBeingHandled) //Если заявка в обоаботке
            {
                if (handlingFinishTime <= CurrentTime)
                {
                    NumHandledRequests++;

                    if (loopBackDecisionRandomizer.ShouldPerformAction())
                    {
                        NumLoopedBackRequests++;
                        handlingFinishTime = CurrentTime + handlTimeGenerator.NextValue();
                    }
                    else
                    {
                        requestIsBeingHandled = false;
                    }
                }
            }
            else if (QueueLength > 0) //Если в буфере есть заявка
            {
                QueueLength--;
                handlingFinishTime = CurrentTime + handlTimeGenerator.NextValue();
                requestIsBeingHandled = true;
            }

            if (NumSentRequests < numRequests &&
                recvTime[NumSentRequests] <= CurrentTime)
            {
                NumSentRequests++;
                if (QueueLength <= MAX_QUEUE_LENGTH)
                {
                    NumReceivedRequests++;
                    QueueLength++;
                }
                else
                {
                    NumDeclinedRequests++;
                }
            }
        }

        public abstract void PassTime();

        public bool Finished()
        {
            return NumSentRequests == numRequests &&
                NumHandledRequests == NumReceivedRequests + NumLoopedBackRequests;
        }
    }

    class TimeModelController : ModelController
    {
        public double TimeStep { get; private set; }

        public TimeModelController(int nReq, UniformTimeRandomizer recvTimeGen,
        NormalTimeRandomizer handlTimeGen, DecisionRandomizer lbDecisionRand, double timeStep):
        base(nReq, recvTimeGen, handlTimeGen, lbDecisionRand)
        {
            TimeStep = timeStep;
        }

        public override void PassTime()
        {
        CurrentTime += TimeStep;
        }
    }

    class EventModelController : ModelController
    {
        private const double ERROR = 1e-7;

        public EventModelController(int nReq, UniformTimeRandomizer recvTimeGen,
            NormalTimeRandomizer handlTimeGen, DecisionRandomizer lbDecisionRand) :
            base(nReq, recvTimeGen, handlTimeGen, lbDecisionRand)
        { }

        public override void PassTime()
        {
            if (NumReceivedRequests == numRequests)
            {
                CurrentTime = handlingFinishTime + ERROR;
            }
            else
            {
                if (requestIsBeingHandled)
                {
                    CurrentTime = Math.Min(handlingFinishTime,
                        recvTime[NumSentRequests]) + ERROR;
                }
                else
                {
                    CurrentTime = recvTime[NumSentRequests] + ERROR;
                }
            }
        }
    }

}
