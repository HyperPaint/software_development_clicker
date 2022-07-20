namespace MyGame
{
    public struct OfficeKitchen
    {
        public ulong water;
        public ulong coffee;
        public ulong sushi;
        public ulong pizza;
        public ulong cake;

        public OfficeKitchen(ulong water, ulong coffee, ulong sushi, ulong pizza, ulong cake)
        {
            this.water = water;
            this.coffee = coffee;
            this.sushi = sushi;
            this.pizza = pizza;
            this.cake = cake;
        }
    }

    public struct UnitModifiers
    {
        public float unitInternet;
        public float unitLight;
        public float unitClimate;
        public float unitMusic;

        public UnitModifiers(float unitInternet, float unitLight, float unitClimate, float unitMusic)
        {
            this.unitInternet = unitInternet;
            this.unitLight = unitLight;
            this.unitClimate = unitClimate;
            this.unitMusic = unitMusic;
        }

        public float Get()
        {
            return unitInternet + unitLight + unitClimate + unitMusic;
        }
    }

    public struct WorkPlaceModifiers
    {
        public float workPlaceTable;
        public float workPlaceChair;
        public float workPlaceComputer;

        public WorkPlaceModifiers(float workPlaceTable, float workPlaceChair, float workPlaceComputer)
        {
            this.workPlaceTable = workPlaceTable;
            this.workPlaceChair = workPlaceChair;
            this.workPlaceComputer = workPlaceComputer;
        }

        public float Get()
        {
            return workPlaceTable + workPlaceChair + workPlaceComputer;
        }
    }

    public struct Works
    {
        public ulong fullstack;
        public ulong designing;
        public ulong art;
        public ulong programming;
        public ulong testing;

        public Works(ulong fullstack, ulong designing, ulong art, ulong programming, ulong testing)
        {
            this.fullstack = fullstack;
            this.designing = designing;
            this.art = art;
            this.programming = programming;
            this.testing = testing;
        }

        public static Works operator +(Works a, Works b) => new Works(a.fullstack + b.fullstack, a.designing + b.designing, a.art + b.art, a.programming + b.programming, a.testing + b.testing);

        public static Works operator -(Works a, Works b) => new Works(a.fullstack - b.fullstack, a.designing - b.designing, a.art - b.art, a.programming - b.programming, a.testing - b.testing);
    }

    public struct OrderPart
    {
        public ulong current;
        public ulong needed;
        public bool completed;

        public OrderPart(ulong current, ulong needed, bool completed)
        {
            this.current = current;
            this.needed = needed;
            this.completed = completed;
        }
    }
}
