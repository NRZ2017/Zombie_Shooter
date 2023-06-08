using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace NickZombieGame
{
    class Survivor : Sprite
    {
        public enum State
        {
            Idle,
            Melee,
            Move,
            Reload,
            Shoot
        }

        public override Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Animations[CurrentState].CurrentFrame.Bounds.Width, Animations[CurrentState].CurrentFrame.Bounds.Height); 
            }
        }

        public State CurrentState;
        public Dictionary<State, Animation> Animations;

        int health;
        int ammoCount;
        int speed = 5;
        public List<Bullet> bullets;
        Texture2D bulletImage;
        Vector2 gunPort;

        TimeSpan timer = TimeSpan.Zero;

        private float radius = 100;
        private float extraAngle = MathHelper.ToRadians(28);



        public Survivor(Vector2 position, Texture2D image, Texture2D bulletImage, Color tint) : base(position, image, tint, 1)
        {
            bullets = new List<Bullet>();
            this.bulletImage = bulletImage;
            gunPort = Vector2.Zero;

            CurrentState = State.Idle;
            Animations = new Dictionary<State, Animation>();

            //5 for loops, each representing a different animation
            //0-name
            //1-isRotated (this can be ignored)
            //2-5 rectangle
            //6-7 size
            //8-9 pivotPoint/origin

            string[] dataFileLines = File.ReadAllLines("./pistol.txt");
            List<Frame> idleFrames = new List<Frame>();
            List<Frame> moveFrames = new List<Frame>();
            List<Frame> shootFrames = new List<Frame>();
            //create all lists here
            for (int i = 0; i < dataFileLines.Length; i++)
            {
                string[] cols = dataFileLines[i].Split(';');
                var sourceRectangle = new Rectangle(
                       int.Parse(cols[2]),
                       int.Parse(cols[3]),
                       int.Parse(cols[4]),
                       int.Parse(cols[5]));
                var pivotPoint = new Vector2(
                    float.Parse(cols[8]),
                    float.Parse(cols[9]));
                if (cols[0].Substring(0, 13) == "survivor-idle")
                {
                    idleFrames.Add(new Frame(sourceRectangle));
                }
                if (cols[0].Substring(0, 13) == "survivor-move")
                {
                    moveFrames.Add(new Frame(sourceRectangle));
                }
                if (cols[0].Substring(0, 14) == "survivor-shoot")
                {
                    shootFrames.Add(new Frame(sourceRectangle));
                }
            }

            Animation idle = new Animation(idleFrames, TimeSpan.FromMilliseconds(50));
            Animations.Add(State.Idle, idle);
            Animation move = new Animation(moveFrames, TimeSpan.FromMilliseconds(16));
            Animations.Add(State.Move, move);
            Animation shoot = new Animation(shootFrames, TimeSpan.FromMilliseconds(5));
            Animations.Add(State.Shoot, shoot);
        }

        public void Update(KeyboardState ks, MouseState ms, GameTime gameTime)
        {
            Animations[CurrentState].Animate(gameTime);


            gunPort = new Vector2((float)(radius * Math.Cos(Rotation + extraAngle)),
                    (float)(radius * Math.Sin(Rotation + extraAngle)))
                    + Position;

            timer += gameTime.ElapsedGameTime;

            CurrentState = State.Idle;
            

            //Move
            if (ks.IsKeyDown(Keys.A))
            {
                Position.X -= speed;
                CurrentState = State.Move;
            }
            if (ks.IsKeyDown(Keys.D))
            {
                Position.X += speed;
                CurrentState = State.Move;
            }
            if (ks.IsKeyDown(Keys.W))
            {
                Position.Y -= speed;
                CurrentState = State.Move;
            }
            if (ks.IsKeyDown(Keys.S))
            {
                Position.Y += speed;
                CurrentState = State.Move;
            }

            //Rotate to Mouse
            Vector2 diff = new Vector2(ms.X - Position.X, ms.Y - Position.Y);
            Rotation = (float)Math.Atan2(diff.Y, diff.X);


            // count timer
            if (ms.LeftButton == ButtonState.Pressed && timer > TimeSpan.FromMilliseconds(250)) // && timer > timeToShoot)
            {
                timer = TimeSpan.Zero;
                //timer back to zero
                CurrentState = State.Shoot;
                bullets.Add(new Bullet(gunPort, bulletImage, Color.White, 0.05f, Rotation, 100));
            }
            foreach (Bullet b in bullets)
            {
                b.Update();
            }


        }

        public override void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(sb);
            }

            SourceRectangle = Animations[CurrentState].CurrentFrame.Bounds;
            Origin = Animations[CurrentState].CurrentFrame.Origin;
            base.Draw(sb);
        }


    }
}
