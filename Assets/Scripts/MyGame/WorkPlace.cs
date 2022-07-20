using System;

namespace MyGame
{
    public class WorkPlace : Clickable
    {
        private Worker worker;
        public Worker Worker { get => worker; set => worker = value; }

        private WorkPlacePart[] parts;
        public WorkPlacePart[] Parts { get => parts; }

        public WorkPlace() : this(null, new WorkPlacePart[WorkPlacePart.workPlacePartTypeLength])
        {
            for (byte i = 0; i < WorkPlacePart.workPlacePartTypeLength; i++)
            {
                parts[i] = new WorkPlacePart(i);
            }
        }

        public WorkPlace(Worker worker, WorkPlacePart[] parts)
        {
            this.worker = worker;
            this.parts = parts;
        }

        public Works MakeWork(ref OfficeKitchen kitchen, float modifiers)
        {
            Works works = new Works();
            modifiers = modifiers * GetModifiers() * GetClickModifier();
            if (worker != null)
            {
                switch (worker.Type)
                {
                    case Worker.EmployeeType.FULLSTACK:
                        works.fullstack += worker.MakeWork(ref kitchen, modifiers);
                        break;

                    case Worker.EmployeeType.DESIGNER:
                        works.designing += worker.MakeWork(ref kitchen, modifiers);
                        break;

                    case Worker.EmployeeType.ARTIST:
                        works.art += worker.MakeWork(ref kitchen, modifiers);
                        break;

                    case Worker.EmployeeType.PROGRAMMER:
                        works.programming += worker.MakeWork(ref kitchen, modifiers);
                        break;

                    case Worker.EmployeeType.TESTER:
                        works.testing += worker.MakeWork(ref kitchen, modifiers);
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
            for (byte i = 0; i < WorkPlacePart.workPlacePartTypeLength; i++)
            {
                value *= parts[i].GetModifier();
            }
            return value;
        }
    }
}
