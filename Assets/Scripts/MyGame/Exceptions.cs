using System;

namespace MyGame
{
    public class MaxLevelException : Exception
    {
        public MaxLevelException() : base("Максимальный уровень достигнут") { }
    }
}
