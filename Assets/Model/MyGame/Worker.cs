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

        public enum EmployeeBoost : byte
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
        private EmployeeBoost boost;
        public EmployeeBoost Boost { get => boost; }
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

        public Worker() : this(EmployeeType.FULLSTACK, EmployeeBoost.WATER, EmployeeFeature.NONE, "", "", "", 255, "") { }

        public Worker(EmployeeType type, EmployeeBoost boost, EmployeeFeature feature, string firstName, string lastName, string nickName, byte skill, string summary)
        {
            this.type = type;
            this.boost = boost;
            this.feature = feature;
            this.firstName = firstName;
            this.lastName = lastName;
            this.nickName = nickName;
            this.skill = skill;
            this.summary = summary;

            Upgraded += (sender) =>
            {
                Logger.Get().Log("Навык работника " + ToString() + " улучшен до " + skill.ToString());
            };
        }

        private static readonly float WORK_PER_SKILL = 0.25f;
        private static readonly float WORK_OFFICE_BOOST_MODIFIER = 1.1f;

        private static readonly float FULLSTACK_WORK_MODIFIER = 0.5f;

        private static readonly float STUDENT_WORK_MODIFIER = 0.5f;
        private static readonly float SPECIALIST_WORK_MODIFIER = 2f;
        private static readonly byte LAZY_WORK_CHANCE = 25;
        private static readonly float LAZY_WORK_MODIFIER = 2f;
        private static readonly byte CREATIVE_WORK_CHANCE = 25;
        private static readonly float CREATIVE_WORK_MODIFIER = 2f;

        public ulong MakeWork(ref OfficeKitchen kitchen, float modifiers)
        {
            float workValue = skill * modifiers * WORK_PER_SKILL;
            if (Type == EmployeeType.FULLSTACK)
            {
                workValue *= FULLSTACK_WORK_MODIFIER;
            }
            switch (boost)
            {
                case EmployeeBoost.WATER:
                    workValue *= GetOfficeBoost(ref kitchen.water);
                    break;

                case EmployeeBoost.COFFEE:
                    workValue *= GetOfficeBoost(ref kitchen.coffee);
                    break;

                case EmployeeBoost.SUSHI:
                    workValue *= GetOfficeBoost(ref kitchen.sushi);
                    break;

                case EmployeeBoost.PIZZA:
                    workValue *= GetOfficeBoost(ref kitchen.pizza);
                    break;

                case EmployeeBoost.CAKE:
                    workValue *= GetOfficeBoost(ref kitchen.cake);
                    break;

                default:
                    throw new NotImplementedException();
            }
            return Feature switch
            {
                EmployeeFeature.NONE => (ulong)(workValue),
                EmployeeFeature.STUDENT => (ulong)(workValue * STUDENT_WORK_MODIFIER),
                EmployeeFeature.SPECIALIST => (ulong)(workValue * SPECIALIST_WORK_MODIFIER),
                EmployeeFeature.LAZY => (ulong)(GameModel.Random.Next(1, 100) <= LAZY_WORK_CHANCE ? workValue * LAZY_WORK_MODIFIER : Skill * WORK_PER_SKILL),
                EmployeeFeature.CREATIVE => (ulong)(GameModel.Random.Next(1, 100) <= CREATIVE_WORK_CHANCE ? workValue / CREATIVE_WORK_MODIFIER : Skill * WORK_PER_SKILL),
                _ => throw new NotImplementedException(),
            };
        }

        private float GetOfficeBoost(ref ulong boost)
        {
            if (boost > 0)
            {
                boost--;
                return WORK_OFFICE_BOOST_MODIFIER;
            }
            else
            {
                return 1f;
            }
        }

        private static readonly float STUDENT_COST_MODIFIER = 0.5f;
        private static readonly float SPECIALIST_COST_MODIFIER = 2f;
        private static readonly float LAZY_COST_MODIFIER = 0.66f;
        private static readonly float CREATIVE_COST_MODIFIER = 1.33f;

        private static readonly float SKILL_COST = 5;
        private static readonly float SKILL_COST_EXP = 3;

        public ulong GetCost()
        {
            float skillValue = (float)Math.Pow(Skill, SKILL_COST_EXP) * SKILL_COST;
            return feature switch
            {
                EmployeeFeature.NONE => (ulong)skillValue,
                EmployeeFeature.STUDENT => (ulong)(skillValue * STUDENT_COST_MODIFIER),
                EmployeeFeature.SPECIALIST => (ulong)(skillValue * SPECIALIST_COST_MODIFIER),
                EmployeeFeature.LAZY => (ulong)(skillValue * LAZY_COST_MODIFIER),
                EmployeeFeature.CREATIVE => (ulong)(skillValue * CREATIVE_COST_MODIFIER),
                _ => throw new NotImplementedException(),
            };
        }

        private static readonly float SKILL_COST_PREMIUM = 5;
        private static readonly float SKILL_COST_EXP_PREMIUM = 3;

        public ulong GetPremiumCost()
        {
            float skillValue = (float)Math.Pow(Skill, SKILL_COST_EXP_PREMIUM) * SKILL_COST_PREMIUM;
            return feature switch
            {
                EmployeeFeature.NONE => (ulong)skillValue,
                EmployeeFeature.STUDENT => (ulong)(skillValue * STUDENT_COST_MODIFIER),
                EmployeeFeature.SPECIALIST => (ulong)(skillValue * SPECIALIST_COST_MODIFIER),
                EmployeeFeature.LAZY => (ulong)(skillValue * LAZY_COST_MODIFIER),
                EmployeeFeature.CREATIVE => (ulong)(skillValue * CREATIVE_COST_MODIFIER),
                _ => throw new NotImplementedException(),
            };
        }

        private static readonly float UPGRADE_COST = 5;
        private static readonly float UPGRADE_EXP = 3;

        public ulong GetUpgradeCost()
        {
            return (ulong)(Math.Pow(skill, UPGRADE_EXP) * UPGRADE_COST);
        }

        private static readonly float UPGRADE_COST_PREMIUM = 5;
        private static readonly float UPGRADE_EXP_PREMIUM = 3;

        public ulong GetUpgradePremiumCost()
        {
            return (ulong)(Math.Pow(skill, UPGRADE_EXP_PREMIUM) * UPGRADE_COST_PREMIUM);
        }

#nullable enable
        public event Event? Upgraded;
#nullable disable

        public void Upgrade()
        {
            if (skill == byte.MaxValue)
                throw new MaxLevelException();
            GameModel.Get().TakeMoney(GetUpgradeCost());
            skill++;
            Upgraded?.Invoke(this);
        }

        public void UpgradePremium()
        {
            if (skill == byte.MaxValue)
                throw new MaxLevelException();
            GameModel.Get().TakePremiumMoney(GetUpgradePremiumCost());
            skill++;
            Upgraded?.Invoke(this);
        }

        public override string ToString()
        {
            if (String.IsNullOrEmpty(nickName))
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

