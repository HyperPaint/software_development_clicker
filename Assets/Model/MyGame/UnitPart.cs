using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class UnitPart : UpgradeablePart
    {
        public const byte unitPartTypeLength = 4;

        public enum UnitPartType : byte
        {
            INTERNET,
            LIGHT,
            CLIMATE,
            MUSIC,
        }

        private UnitPartType type;
        public UnitPartType Type { get => type; }

        public UnitPart(byte type) : this((UnitPartType)type) { }

        public UnitPart(UnitPartType type) : base()
        {
            this.type = type;
        }

        public UnitPart(UnitPartType type, byte level) : base(level)
        {
            this.type = type;
        }

        private static readonly float INTERNET_WORK_MODIFIER_PER_LEVEL = 0.05f;
        private static readonly float LIGHT_WORK_MODIFIER_PER_LEVEL = 0.05f;
        private static readonly float CLIMATE_WORK_MODIFIER_PER_LEVEL = 0.05f;
        private static readonly float MUSIC_WORK_MODIFIER_PER_LEVEL = 0.05f;

        public override float GetModifier()
        {
            return 1f + level * type switch
            {
                UnitPartType.INTERNET => INTERNET_WORK_MODIFIER_PER_LEVEL,
                UnitPartType.LIGHT => LIGHT_WORK_MODIFIER_PER_LEVEL,
                UnitPartType.CLIMATE => CLIMATE_WORK_MODIFIER_PER_LEVEL,
                UnitPartType.MUSIC => MUSIC_WORK_MODIFIER_PER_LEVEL,
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
