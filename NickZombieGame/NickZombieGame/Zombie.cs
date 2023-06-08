
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NickZombieGame
{
    class Zombie : Sprite
    {

        float speed = 7;
        
        public Zombie(Vector2 position, Texture2D image, Color tint) : base(position, image, tint, 1)
        {

        }
        public void Update(Survivor survivor)
        {
            float distance = Vector2.Distance(Position, survivor.Position);
            Vector2 diff = survivor.Position - Position;
            Rotation = (float)Math.Atan2(diff.Y, diff.X);
            diff.Normalize();

            if (distance > 100)
            {
                Position += diff * speed;
            }

        }

        public void Summon()
        {


        }
    }
}
