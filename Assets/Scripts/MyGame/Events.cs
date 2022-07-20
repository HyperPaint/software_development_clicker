using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public delegate void Event<SENDER>(SENDER sender);
    public delegate void EventObject<SENDER, OBJ>(SENDER sender, OBJ obj);
}
