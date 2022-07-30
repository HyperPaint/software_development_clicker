using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class MaxLevelException : Exception
    {
        public MaxLevelException() : base("Максимальный уровень достигнут") { }
    }
}
