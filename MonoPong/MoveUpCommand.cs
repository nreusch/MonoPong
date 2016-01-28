using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoPong
{
    class MoveUpCommand : Command
    {
        public MoveUpCommand(Player actor) : base(actor)
        { }

        public override void execute()
        {
            _actor.setPosition(new Vector2(_actor.getPosition().X, _actor.getPosition().Y - 1));
        }
    }
}
