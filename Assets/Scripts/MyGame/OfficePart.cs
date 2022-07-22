using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class OfficePart : UpgradeablePart
    {
        public const byte officePartTypeLength = 3;

        public enum OfficePartType : byte
        {
            INTERNET,
            CLIMATE,
            MUSIC,
        }

        private OfficePartType type;
        public OfficePartType Type { get => type; }

        public OfficePart(byte type) : this((OfficePartType)type) { }

        public OfficePart(OfficePartType type) : base()
        {
            this.type = type;
        }

        public OfficePart(OfficePartType type, ulong level) : base(level)
        {
            this.type = type;
        }

        public override ulong GetUpgradeCost()
        {
            return Convert.ToUInt64(Math.Pow(level, Config.OFFICE_PART_UPGRADE_MONEY_COST_EXP) * Config.OFFICE_PART_UPGRADE_MONEY_COST);
        }

        public override float GetModifier()
        {
            return Config.Base.MODIFIER_BASE + level * type switch
            {
                OfficePartType.INTERNET => Config.OFFICE_PART_INTERNET_MODIFIER_PER_LEVEL,
                OfficePartType.CLIMATE => Config.OFFICE_PART_CLIMATE_MODIFIER_PER_LEVEL,
                OfficePartType.MUSIC => Config.OFFICE_PART_MUSIC_MODIFIER_PER_LEVEL,
                _ => throw new NotImplementedException(),
            };
        }

        public override string ToString()
        {
            return type.ToString();
        }
    }
}
