using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class WorkPlacePart : UpgradeablePart
    {
        public const byte workPlacePartTypeLength = 3;

        public enum WorkPlacePartType : byte
        {
            TABLE,
            CHAIR,
            COMPUTER,
        }

        private WorkPlacePartType type;
        public WorkPlacePartType Type { get => type; }

        public WorkPlacePart(byte type) : this((WorkPlacePartType)type) { }

        public WorkPlacePart(WorkPlacePartType type) : base()
        {
            this.type = type;
        }

        public WorkPlacePart(WorkPlacePartType type, byte level) : base(level)
        {
            this.type = type;
        }

        private static readonly float TABLE_WORK_MODIFIER_PER_LEVEL = 0.05f;
        private static readonly float CHAIR_WORK_MODIFIER_PER_LEVEL = 0.10f;
        private static readonly float COMPUTER_WORK_MODIFIER_PER_LEVEL = 0.15f;

        public override float GetModifier()
        {
            return 1f + level * type switch
            {
                WorkPlacePartType.TABLE => TABLE_WORK_MODIFIER_PER_LEVEL,
                WorkPlacePartType.CHAIR => CHAIR_WORK_MODIFIER_PER_LEVEL,
                WorkPlacePartType.COMPUTER => COMPUTER_WORK_MODIFIER_PER_LEVEL,
                _ => throw new NotImplementedException(),
            };
        }

        private static readonly float UPGRADE_COST = 5;
        private static readonly float UPGRADE_EXP = 3;

        public override ulong GetUpgradeCost()
        {
            return (ulong)(Math.Pow(level, UPGRADE_EXP) * UPGRADE_COST);
        }

        private static readonly float UPGRADE_COST_PREMIUM = 5;
        private static readonly float UPGRADE_EXP_PREMIUM = 3;

        public override ulong GetUpgradePremiumCost()
        {
            return (ulong)(Math.Pow(level, UPGRADE_EXP_PREMIUM) * UPGRADE_COST_PREMIUM);
        }
    }
}
