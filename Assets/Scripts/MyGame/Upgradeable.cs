namespace MyGame
{
    public abstract class Upgradeable
    {
        protected ulong level;
        public ulong Level { get => level; }

        public Upgradeable() : this(1)
        {
        }

        public Upgradeable(ulong level)
        {
            this.level = level;
        }

#nullable enable
        public event EventWith1Object<Upgradeable, ulong>? OnUpgraded;
#nullable disable

        public abstract ulong GetUpgradeCost();

        public virtual void BuyUpgrade()
        {
            if (level < ulong.MaxValue)
            {
                if (GameModel.Get().TakeMoney(GetUpgradeCost()))
                {
                    level++;
                    OnUpgraded?.Invoke(this, level);
                }
            }
        }
    }
}
