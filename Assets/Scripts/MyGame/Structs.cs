namespace MyGame
{
    public struct Works
    {
        public ulong fullstack;
        public ulong designing;
        public ulong art;
        public ulong programming;
        public ulong testing;

        public Works(ulong fullstack, ulong designing, ulong art, ulong programming, ulong testing)
        {
            this.fullstack = fullstack;
            this.designing = designing;
            this.art = art;
            this.programming = programming;
            this.testing = testing;
        }

        public static Works operator +(Works a, Works b) => new Works(a.fullstack + b.fullstack, a.designing + b.designing, a.art + b.art, a.programming + b.programming, a.testing + b.testing);

        public static Works operator -(Works a, Works b) => new Works(a.fullstack - b.fullstack, a.designing - b.designing, a.art - b.art, a.programming - b.programming, a.testing - b.testing);
    }
}
