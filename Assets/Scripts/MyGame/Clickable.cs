using System;

namespace MyGame
{
    public class Clickable
    {
        public enum Power : byte
        {
            NONE,
            EASY,
            NORMAL,
            MEDIUM,
            HARD,
        }

        protected float clicks;
        public int Clicks { get => Convert.ToInt32(clicks); }

        public Clickable() : this(0) { }

        public Clickable(int clicks)
        {
            this.clicks = clicks;
        }

        public virtual void Click()
        {
            const float max = Config.CLICKABLE_CLICKS_MAX;
            if (clicks < max)
            {
                clicks++;
            }
        }

        public Power GetPower()
        {
            if (clicks >= Config.CLICKABLE_POWER_HARD)
            {
                return Power.HARD;
            }
            else if (clicks >= Config.CLICKABLE_POWER_MEDIUM)
            {
                return Power.MEDIUM;
            }
            else if (clicks >= Config.CLICKABLE_POWER_NORMAL)
            {
                return Power.NORMAL;
            }
            else if (clicks >= Config.CLICKABLE_POWER_EASY)
            {
                return Power.EASY;
            }
            else
            {
                return Power.NONE;
            }
        }

        protected float GetClickableModifier()
        {
            if (clicks >= Config.CLICKABLE_POWER_HARD)
            {
                clicks -= Config.CLICKABLE_POWER_HARD_CLICK_CONSUMING_MODIFIER;
                return Config.CLICKABLE_POWER_HARD_MODIFIER;
            }
            else if (clicks >= Config.CLICKABLE_POWER_MEDIUM)
            {
                clicks -= Config.CLICKABLE_POWER_MEDIUM_CLICK_CONSUMING_MODIFIER;
                return Config.CLICKABLE_POWER_MEDIUM_MODIFIER;
            }
            else if (clicks >= Config.CLICKABLE_POWER_NORMAL)
            {
                clicks -= Config.CLICKABLE_POWER_NORMAL_CLICK_CONSUMING_MODIFIER;
                return Config.CLICKABLE_POWER_NORMAL_MODIFIER;
            }
            else if (clicks >= Config.CLICKABLE_POWER_EASY)
            {
                clicks -= Config.CLICKABLE_POWER_EASY_CLICK_CONSUMING_MODIFIER;
                return Config.CLICKABLE_POWER_EASY_MODIFIER;
            }
            else
            {
                if (clicks > 0f)
                {
                    clicks -= Config.CLICKABLE_POWER_NONE_CLICK_CONSUMING_MODIFIER;
                }
                return Config.CLICKABLE_POWER_NONE_MODIFIER;

            }
        }
    }
}
