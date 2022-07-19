using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Unit
    {
        public const byte unitTypeLength = Worker.employeeTypeLength - 1;

        private List<WorkPlace> workPlaces;
        public List<WorkPlace> WorkPlaces { get => workPlaces; }
        private UnitPart[] parts;
        public UnitPart[] Parts { get => parts; }
        public UnitPart PartInternet { get => parts[(byte)UnitPart.UnitPartType.INTERNET]; }
        public UnitPart PartLight { get => parts[(byte)UnitPart.UnitPartType.LIGHT]; }
        public UnitPart PartClimate { get => parts[(byte)UnitPart.UnitPartType.CLIMATE]; }
        public UnitPart PartMusic { get => parts[(byte)UnitPart.UnitPartType.MUSIC]; }

        public Unit() : this(new List<WorkPlace>(), new UnitPart[UnitPart.unitPartTypeLength])
        {
            for (byte i = 0; i < UnitPart.unitPartTypeLength; i++)
            {
                parts[i] = new UnitPart(i);
            }
            AddNewWorkPlace();
        }

        public Unit(List<WorkPlace> workPlaces, UnitPart[] parts)
        {
            this.workPlaces = workPlaces;
            this.parts = parts;
        }

        public Works MakeWork(ref OfficeKitchen kitchen, float modifiers)
        {
            Works works = new Works();
            foreach (WorkPlace workPlace in WorkPlaces)
            {
                works += workPlace.MakeWork(ref kitchen, modifiers * GetModifiers());
            }
            return works;
        }

        public float GetModifiers()
        {
            float value = 1f;
            for (byte i = 0; i < UnitPart.unitPartTypeLength; i++)
            {
                value *= parts[i].GetModifier();
            }
            return value;
        }

        public void AddNewWorkPlace()
        {
            workPlaces.Add(new WorkPlace());
        }
    }
}
