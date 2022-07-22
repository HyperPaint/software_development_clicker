namespace MyGame
{
    public abstract class UpgradeablePart : Upgradeable
    {
        public UpgradeablePart() : this(0) { }

        public UpgradeablePart(ulong level) : base(level) { }

        public abstract float GetModifier();
    }
}
