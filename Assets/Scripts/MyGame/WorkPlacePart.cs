using System;

namespace MyGame
{
    public class WorkplacePart : UpgradeablePart
    {
        public const byte workPlacePartTypeLength = 3;

        public enum WorkplacePartType : byte
        {
            TABLE,
            CHAIR,
            COMPUTER,
        }

        private WorkplacePartType type;
        public WorkplacePartType Type { get => type; }

        public WorkplacePart(byte type) : this((WorkplacePartType)type) { }

        public WorkplacePart(WorkplacePartType type) : base()
        {
            this.type = type;
        }

        public WorkplacePart(WorkplacePartType type, ulong level) : base(level)
        {
            this.type = type;
        }

        public override float GetModifier()
        {
            return Config.Base.MODIFIER_BASE + level * type switch
            {
                WorkplacePartType.TABLE => Config.WORKPLACE_PART_TABLE_MODIFIER_PER_LEVEL,
                WorkplacePartType.CHAIR => Config.WORKPLACE_PART_CHAIR_MODIFIER_PER_LEVEL,
                WorkplacePartType.COMPUTER => Config.WORKPLACE_PART_COMPUTER_MODIFIER_PER_LEVEL,
                _ => throw new NotImplementedException(),
            };
        }

        public override ulong GetUpgradeCost()
        {
            return Convert.ToUInt64(Math.Pow(level, Config.WORKPLACE_PART_UPGRADE_MONEY_COST_EXP) * Config.WORKPLACE_PART_UPGRADE_MONEY_COST);
        }

        public override string ToString()
        {
            return type.ToString();
        }
    }
}
