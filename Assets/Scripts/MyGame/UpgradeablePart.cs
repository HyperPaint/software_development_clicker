using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public abstract class UpgradeablePart : Upgradeable
    {
        public UpgradeablePart() : this(0) { }

        public UpgradeablePart(byte level) : base(level) { }

        public abstract float GetModifier();
    }
}
