using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace NickZombieGame
{
    /// <summary>
    /// Sprite: basic componets to draw an image
    /// 
    /// Frame: rectangle, origin
    /// Animation (no inheritance): list of frames 
    /// AnimatedSprite (inherates sprite): current state, Enum animation states, Dictionary(state, animation), image that comes from sprite is the full spritesheet
    /// 
    /// Surivor (inherates AnimatedSprite): health, ammoCount, List(bullets), functions to control the survivor/change animations
    /// Bullet (inherates Sprite): speed, damage
    /// Zombie (inherates Sprite, AnimatedSprite): helath, speed, damage
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Survivor survivor;
        Sprite AimReticle;
        MouseState ms;
        KeyboardState ks;
        Texture2D Background;
        List<Zombie> zombie;
        Random random;
        int ZombieCount;
        SpriteFont Font1;
        Texture2D Heart;
        int HeartCount = 3;

        TimeSpan spawnTimer = TimeSpan.Zero;
        TimeSpan spawnTime = TimeSpan.FromMilliseconds(1000);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1900;
            graphics.PreferredBackBufferHeight = 1000;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = false;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ms = Mouse.GetState();
            random = new Random();
            survivor = new Survivor(new Vector2(500, 500), Content.Load<Texture2D>("pistol"), Content.Load<Texture2D>("BulletMegaE433"), Color.White);
            AimReticle = new Sprite(new Vector2(ms.X, ms.Y), Content.Load<Texture2D>("Aim"), Color.White, 0.5f);
            Background = Content.Load<Texture2D>("Background");
            zombie = new List<Zombie>();
            Font1 = Content.Load<SpriteFont>("Font");
            Heart = Content.Load<Texture2D>("McHeart");

            for (int i = 0; i < 10; i++)
            {
                SpawnZombie();
            }




            // TODO: use this.Content to load your game content here
        }

        public void SpawnZombie()
        {
            Vector2 spawnPoint = Vector2.One;
            while (GraphicsDevice.Viewport.Bounds.Contains(spawnPoint))
            {
                spawnPoint = new Vector2(random.Next(-2000, GraphicsDevice.Viewport.Width + 2000), random.Next(-2000, GraphicsDevice.Viewport.Height + 2000));
            }
            zombie.Add(new Zombie(spawnPoint, Content.Load<Texture2D>("zombie"), Color.White));
            

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            ms = Mouse.GetState();
            ks = Keyboard.GetState();


            spawnTimer += gameTime.ElapsedGameTime;
            if (spawnTimer > spawnTime)
            {
                spawnTimer = TimeSpan.Zero;
                SpawnZombie();
            }

            AimReticle.Position = new Vector2(ms.X, ms.Y);

            survivor.Update(ks, ms, gameTime);

            for (int i = 0; i < zombie.Count; i++)
            {
                zombie[i].Update(survivor);
            }
            for (int i = 0; i < zombie.Count; i++)
            {
                for (int k = 0; k < survivor.bullets.Count; k++)
                {
                    if (survivor.bullets[k].Hitbox.Intersects(zombie[i].Hitbox))
                    {
                        zombie.RemoveAt(i);
                        survivor.bullets.RemoveAt(k);
                        ZombieCount++;
                    }
                }
            }
            
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(Background, GraphicsDevice.Viewport.Bounds, Color.White);
            survivor.Draw(spriteBatch);
            for (int i = 0; i < zombie.Count; i++)
            {
                zombie[i].Draw(spriteBatch);
            }
            AimReticle.Draw(spriteBatch);
           
            spriteBatch.DrawString(Font1, $"Score:{ZombieCount}", new Vector2(GraphicsDevice.Viewport.Bounds.Width -Font1.MeasureString($"Score{ZombieCount}").X - 30, 0) , Color.Black);
            for (int i = 0; i < zombie.Count; i++)
            {
                if (survivor.Hitbox.Intersects(zombie[i].Hitbox))
                {
                    HeartCount--;
                    zombie.Remove(zombie[i]);
                }
            }
            switch(HeartCount)
            {
                case 1:
                    spriteBatch.Draw(Heart, new Rectangle(20, 20, 100, 100), Color.White);
                    break;

                case 2:
                    spriteBatch.Draw(Heart, new Rectangle(20, 20, 100, 100), Color.White);
                    spriteBatch.Draw(Heart, new Rectangle(100, 20, 100, 100), Color.White);
                    break;

                case 3:
                    spriteBatch.Draw(Heart, new Rectangle(20, 20, 100, 100), Color.White);
                    spriteBatch.Draw(Heart, new Rectangle(100, 20, 100, 100), Color.White);
                    spriteBatch.Draw(Heart, new Rectangle(180, 20, 100, 100), Color.White);
                    break;
            }
            if (HeartCount == 0)
            {
                Exit();
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
