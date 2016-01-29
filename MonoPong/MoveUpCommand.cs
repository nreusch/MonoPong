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
            Vector2 newpos = new Vector2(_actor.Position.X, _actor.Position.Y - _actor.Speed);

            if (!Game1.boundingBoxTop.Intersects(_actor.BoundingBox))
            {
                _actor.Position = newpos;
            }
        }
    }
}
