using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickZombieGame
{
    class Animation
    {
        public Frame CurrentFrame
        {
            get
            {
                return currentFrameCounter < Frames.Count ? Frames[currentFrameCounter] : null;
            }
        }
        public List<Frame> Frames;

        private int currentFrameCounter;
        private TimeSpan FrameRate;
        private TimeSpan timer;

        public Animation(TimeSpan frameRate)
        {
            timer = TimeSpan.Zero;
            FrameRate = frameRate;
            currentFrameCounter = 0;
            Frames = new List<Frame>();
        }

        public Animation(List<Frame> frames, TimeSpan frameRate)
            : this(frameRate)
        {
            Frames = frames;
        }


        public void Animate(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime;
            if (timer > FrameRate)
            {
                timer = TimeSpan.Zero;
                currentFrameCounter++;
                if (currentFrameCounter >= Frames.Count)
                {
                    currentFrameCounter = 0;
                }
            }
        }
    }
}
