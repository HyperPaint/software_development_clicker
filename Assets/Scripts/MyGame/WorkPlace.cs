using System;

namespace MyGame
{
    public class Workplace : Clickable
    {
        private Worker worker;
        public Worker Worker { get => worker; internal set => worker = value; }

        private WorkplacePart[] parts;
        public WorkplacePart[] Parts { get => parts; }
        public WorkplacePart PartTable { get => parts[(byte)WorkplacePart.WorkplacePartType.TABLE]; }
        public WorkplacePart PartChair { get => parts[(byte)WorkplacePart.WorkplacePartType.CHAIR]; }
        public WorkplacePart PartComputer { get => parts[(byte)WorkplacePart.WorkplacePartType.COMPUTER]; }

        public Workplace() : this(null, new WorkplacePart[WorkplacePart.workPlacePartTypeLength])
        {
            for (byte i = 0; i < WorkplacePart.workPlacePartTypeLength; i++)
            {
                parts[i] = new WorkplacePart(i);
            }
        }

        public Workplace(Worker worker, WorkplacePart[] parts)
        {
            this.worker = worker;
            this.parts = parts;
        }

        public override void Click()
        {
            if (worker != null)
            {
                base.Click();
            }
        }

#nullable enable
        public event EventWith1Object<Workplace, Worker>? OnWorkerBought;
#nullable disable

        public void BuyWorker(Worker worker, bool gameStart = false)
        {
            if (gameStart)
            {
                this.worker = worker;
                OnWorkerBought?.Invoke(this, worker);
            }
            else if (GameModel.Get().TakeMoney(worker.GetCost()))
            {
                this.worker = worker;
                OnWorkerBought?.Invoke(this, worker);
            }
        }

#nullable enable
        public event EventWith1Object<Workplace, Worker>? OnWorkerDismiss;
#nullable disable

        public void DismissWorker()
        {
            Worker buff = worker;
            worker = null;
            OnWorkerDismiss?.Invoke(this, worker);

            GameModel.Get().DecreaseReputation(Config.WORKER_REPUTATION_CHANGE_ON_DISMISS);
        }

        public Works MakeWork(Kitchen kitchen, float modifiers)
        {
            Works works = new Works();
            modifiers = modifiers * GetModifiers() * GetClickableModifier();
            if (worker != null)
            {
                switch (worker.Type)
                {
                    case Worker.EmployeeType.FULLSTACK:
                        works.fullstack += worker.MakeWork(kitchen, modifiers);
                        break;

                    case Worker.EmployeeType.DESIGNER:
                        works.designing += worker.MakeWork(kitchen, modifiers);
                        break;

                    case Worker.EmployeeType.ARTIST:
                        works.art += worker.MakeWork(kitchen, modifiers);
                        break;

                    case Worker.EmployeeType.PROGRAMMER:
                        works.programming += worker.MakeWork(kitchen, modifiers);
                        break;

                    case Worker.EmployeeType.TESTER:
                        works.testing += worker.MakeWork(kitchen, modifiers);
                        break;

                    default:
                        throw new NotImplementedException();
                }
                
            }
            return works;
        }

        public float GetModifiers()
        {
            float value = 1f;
            for (byte i = 0; i < WorkplacePart.workPlacePartTypeLength; i++)
            {
                value *= parts[i].GetModifier();
            }
            return value;
        }

        public override string ToString()
        {
            return "Workplace";
        }
    }
}
