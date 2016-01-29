using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoPong
{
    class MoveDownCommand : Command
    {
        public MoveDownCommand(Player actor) : base(actor)
        { }

        public override void execute()
        {
            Vector2 newpos = new Vector2(_actor.Position.X, _actor.Position.Y + _actor.Speed);

            if (!Game1.boundingBoxBottom.Intersects(_actor.BoundingBox))
            {
                _actor.Position = newpos;
            }
            //TODO: set to nearest postion otherwise there is a gap
        }
    }
}
