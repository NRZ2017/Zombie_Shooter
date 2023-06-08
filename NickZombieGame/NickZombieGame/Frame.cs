using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickZombieGame
{
    class Frame
    {
        public Rectangle Bounds;
        public Vector2 Origin;

        public Frame(int x, int y, int width, int height)
        {
            Bounds = new Rectangle(x, y, width, height);
            Origin = new Vector2(width / 2, height / 2);
        }
        public Frame(int x, int y, int width, int height, Vector2 origin)
            : this(x, y, width, height)
        {
            Origin = origin;
        }
        public Frame(Rectangle bounds, Vector2 origin)
            : this(bounds.X, bounds.Y, bounds.Width, bounds.Height, origin) { }

        public Frame(Rectangle bounds)
            : this(bounds.X, bounds.Y, bounds.Width, bounds.Height) { }
    }
}
