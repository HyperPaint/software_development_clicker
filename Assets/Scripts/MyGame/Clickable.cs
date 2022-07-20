using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Clickable
    {
        public enum Tired : byte
        {
            NONE,
            EASY,
            NORMAL,
            MEDIUM,
            HARD,
        }

        protected byte clicks;
        public byte Clicks { get => clicks; }

        public Clickable() : this(0) { }

        public Clickable(byte clicks)
        {
            this.clicks = clicks;
        }

        public void Click()
        {
            if (clicks < byte.MaxValue)
            {
                clicks++;
            }
        }

        public static readonly byte CLICKABLE_CLICKS_TIRED_NONE = 0;
        public static readonly byte CLICKABLE_CLICKS_TIRED_EASY = 1;
        public static readonly byte CLICKABLE_CLICKS_TIRED_NORMAL = 3;
        public static readonly byte CLICKABLE_CLICKS_TIRED_MEDIUM = 5;
        public static readonly byte CLICKABLE_CLICKS_TIRED_HARD = 7;

        public Tired GetTired()
        {
            if (clicks >= CLICKABLE_CLICKS_TIRED_HARD)
            {
                return Tired.HARD;
            }
            else if (clicks >= CLICKABLE_CLICKS_TIRED_MEDIUM)
            {
                return Tired.MEDIUM;
            }
            else if (clicks >= CLICKABLE_CLICKS_TIRED_NORMAL)
            {
                return Tired.NORMAL;
            }
            else if (clicks >= CLICKABLE_CLICKS_TIRED_EASY)
            {
                return Tired.EASY;
            }
            else
            {
                return Tired.NONE;
            }
        }

        public static readonly float CLICKABLE_PER_CLICK_MODIDIER = 0.25f;

        public static readonly float CLICKABLE_CLICKS_TIRED_NONE_MODIFIER = 1f;
        public static readonly float CLICKABLE_CLICKS_TIRED_EASY_MODIFIER = 1.5f;
        public static readonly float CLICKABLE_CLICKS_TIRED_NORMAL_MODIFIER = 2f;
        public static readonly float CLICKABLE_CLICKS_TIRED_MEDIUM_MODIFIER = 2.5f;
        public static readonly float CLICKABLE_CLICKS_TIRED_HARD_MODIFIER = 3f;

        protected float GetClickModifier()
        {
            if (clicks > CLICKABLE_CLICKS_TIRED_HARD)
            {
                clicks -= Convert.ToByte(CLICKABLE_CLICKS_TIRED_HARD_MODIFIER);
                return 1f + CLICKABLE_CLICKS_TIRED_HARD_MODIFIER * CLICKABLE_PER_CLICK_MODIDIER;
            }
            else if (clicks > CLICKABLE_CLICKS_TIRED_MEDIUM)
            {
                clicks -= Convert.ToByte(CLICKABLE_CLICKS_TIRED_MEDIUM_MODIFIER);
                return 1f + CLICKABLE_CLICKS_TIRED_MEDIUM_MODIFIER * CLICKABLE_PER_CLICK_MODIDIER;
            }
            else if (clicks > CLICKABLE_CLICKS_TIRED_NORMAL)
            {
                clicks -= Convert.ToByte(CLICKABLE_CLICKS_TIRED_NORMAL_MODIFIER);
                return 1f + CLICKABLE_CLICKS_TIRED_NORMAL_MODIFIER * CLICKABLE_PER_CLICK_MODIDIER;
            }
            else if (clicks > CLICKABLE_CLICKS_TIRED_EASY)
            {
                clicks -= Convert.ToByte(CLICKABLE_CLICKS_TIRED_EASY_MODIFIER);
                return 1f + CLICKABLE_CLICKS_TIRED_EASY_MODIFIER * CLICKABLE_PER_CLICK_MODIDIER;
            }
            else if (clicks > CLICKABLE_CLICKS_TIRED_NONE)
            {
                clicks -= Convert.ToByte(CLICKABLE_CLICKS_TIRED_NONE_MODIFIER);
                return 1f + CLICKABLE_CLICKS_TIRED_NONE_MODIFIER * CLICKABLE_PER_CLICK_MODIDIER;
            }
            else
            {
                return 1f;
            }
        }
    }
}
