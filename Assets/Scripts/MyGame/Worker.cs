using System;

namespace MyGame
{
    public class Worker
    {
        public const byte employeeTypeLength = 5;

        public enum EmployeeType : byte
        {
            FULLSTACK,
            DESIGNER,
            ARTIST,
            PROGRAMMER,
            TESTER,
        }

        public const byte employeeBoostLength = 5;

        public enum EmployeeFood : byte
        {
            WATER,
            COFFEE,
            SUSHI,
            PIZZA,
            CAKE,
        }

        public const byte employeeFeatureLength = 5;

        public enum EmployeeFeature : byte
        {
            NONE,
            STUDENT,
            SPECIALIST,
            LAZY,
            CREATIVE,
        }

        private EmployeeType type;
        public EmployeeType Type { get => type; }
        private EmployeeFood food;
        public EmployeeFood Food { get => food; }
        private EmployeeFeature feature;
        public EmployeeFeature Feature { get => feature; }
        private string firstName;
        public string FirstName { get => firstName; }
        private string lastName;
        public string LastName { get => lastName; }
        private string nickName;
        public string NickName { get => nickName; set => nickName = value; }
        private byte skill;
        public byte Skill { get => skill; }
        private string summary;
        public string Summary { get => summary; }

        public Worker() : this(EmployeeType.FULLSTACK, EmployeeFood.WATER, EmployeeFeature.NONE, "", "", "", 5, "") { }

        public Worker(EmployeeType type, EmployeeFood food, EmployeeFeature feature, string firstName, string lastName, string nickName, byte skill, string summary)
        {
            this.type = type;
            this.food = food;
            this.feature = feature;
            this.firstName = firstName;
            this.lastName = lastName;
            this.nickName = nickName;
            this.skill = skill;
            this.summary = summary;
        }

        public ulong MakeWork(Kitchen kitchen, float modifiers)
        {
            float workValue = skill * modifiers * Config.WORKER_WORK_PER_SKILL;
            workValue *= type switch
            {
                EmployeeType.FULLSTACK => Config.WORKER_FULLSTACK_WORK_MODIFIER,
                EmployeeType.DESIGNER => Config.WORKER_DESIGNER_WORK_MODIFIER,
                EmployeeType.ARTIST => Config.WORKER_ARTIST_WORK_MODIFIER,
                EmployeeType.PROGRAMMER => Config.WORKER_PROGRAMMER_WORK_MODIFIER,
                EmployeeType.TESTER => Config.WORKER_TESTER_WORK_MODIFIER,
                _ => throw new NotImplementedException(),
            };
            workValue *= food switch
            {
                EmployeeFood.WATER => kitchen.GetWaterModifier(),
                EmployeeFood.COFFEE => kitchen.GetCoffeeModifier(),
                EmployeeFood.SUSHI => kitchen.GetSushiModifier(),
                EmployeeFood.PIZZA => kitchen.GetPizzaModifier(),
                EmployeeFood.CAKE => kitchen.GetCakeModifier(),
                _ => throw new NotImplementedException(),
            };
            switch (feature)
            {
                case EmployeeFeature.STUDENT:
                    workValue *= Config.WORKER_STUDENT_WORK_MODIFIER;
                    break;
                case EmployeeFeature.SPECIALIST:
                    workValue *= Config.WORKER_SPECIALIST_WORK_MODIFIER;
                    break;
                case EmployeeFeature.LAZY:
                    if (GameModel.Random.Next(1, 100) <= Config.WORKER_LAZY_WORK_CHANCE)
                        workValue *= Config.WORKER_LAZY_WORK_MODIFIER;
                    break;
                case EmployeeFeature.CREATIVE:
                    if (GameModel.Random.Next(1, 100) <= Config.WORKER_CREATIVE_WORK_CHANCE)
                        workValue *= Config.WORKER_CREATIVE_WORK_MODIFIER;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return Convert.ToUInt64(Math.Ceiling(workValue));
        }

        public ulong GetCost()
        {
            float skillValue = Convert.ToSingle(Math.Pow(skill, Config.WORKER_SKILL_COST_EXP) * Config.WORKER_SKILL_COST);
            switch (feature)
            {
                case EmployeeFeature.STUDENT:
                    skillValue *= Config.WORKER_STUDENT_COST_MODIFIER;
                    break;
                case EmployeeFeature.SPECIALIST:
                    skillValue *= Config.WORKER_SPECIALIST_COST_MODIFIER;
                    break;
                case EmployeeFeature.LAZY:
                    skillValue *= Config.WORKER_LAZY_COST_MODIFIER;
                    break;
                case EmployeeFeature.CREATIVE:
                    skillValue *= Config.WORKER_CREATIVE_COST_MODIFIER;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return Convert.ToUInt64(skillValue);
        }



        public ulong GetUpgradeCost()
        {
            return (ulong)(Math.Pow(skill, Config.WORKER_UPGRADE_EXP) * Config.WORKER_UPGRADE_COST);
        }

#nullable enable
    public event Event<Worker>? Upgraded;
#nullable disable

    public void Upgrade()
    {
        if (skill == byte.MaxValue)
            throw new MaxLevelException();
        GameModel.Get().TakeMoney(GetUpgradeCost());
        skill++;
        Upgraded?.Invoke(this);
    }

    public override string ToString()
    {
        if (string.IsNullOrEmpty(nickName))
        {
            return firstName + " " + lastName;
        }
        else
        {
            return firstName + " '" + nickName + "' " + lastName;
        }
    }
    }
}

