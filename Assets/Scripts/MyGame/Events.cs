using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public delegate void Event(object sender);
    public delegate void EventFactory(object sender, object created);
}
