namespace MyGame
{
    public class Unit
    {
        private Workplace[] workplaces;
        public Workplace[] Workplaces { get => workplaces; }

        public Unit() : this(new Workplace[Config.UNIT_WORKPLACES_COUNT])
        {
            for(int i = 0; i < workplaces.Length; i++)
            {
                workplaces[i] = new Workplace();
            }
        }

        public Unit(Workplace[] workplaces)
        {
            this.workplaces = workplaces;
        }

        public Works MakeWork(Kitchen kitchen, float modifiers)
        {
            Works works = new Works();
            foreach (Workplace workPlace in Workplaces)
            {
                works += workPlace.MakeWork(kitchen, modifiers);
            }
            return works;
        }
    }
}
