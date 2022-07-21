namespace MyGame
{
    public abstract class Upgradeable
    {
        protected byte level;
        public byte Level { get => level; }

        public Upgradeable() : this(1)
        {
        }

        public Upgradeable(byte level)
        {
            this.level = level;
        }

#nullable enable
        public event Event<Upgradeable>? Upgraded;
#nullable disable

        public abstract ulong GetUpgradeCost();

        public virtual void BuyUpgrade()
        {
            GameModel.Get().TakeMoney(GetUpgradeCost());
            level++;
            Upgraded?.Invoke(this);
        }
    }
}
