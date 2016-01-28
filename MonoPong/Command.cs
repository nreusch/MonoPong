using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoPong
{
    abstract class Command
    {
        protected Player _actor;
        public Command(Player actor) { _actor = actor; }
        public abstract void execute();

    }
}
