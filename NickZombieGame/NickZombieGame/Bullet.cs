using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NickZombieGame
{
    class Bullet : Sprite
    {
        int damage;
        float speed;
        private Vector2 delta;


        public Bullet(Vector2 position, Texture2D image, Color tint, float scale, float rotation, float speed)
            : base(position, image, tint, scale, rotation)
        {
            this.damage = damage;
            this.Rotation = rotation;
            this.speed = speed;
            this.delta = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * speed;

        }

        public void Update()
        {
            Position += delta;
        }
    }
}
