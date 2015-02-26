using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Moley_Heaven_to_Hell
{
    class Background : GameObject
    {
        private PointF startVelocity;
        private Stopwatch time = new Stopwatch();

        public PointF StartVelocity
        {
            get { return startVelocity; }
            set { startVelocity = value; }
        }

        public Background(PointF position, PointF velocity, PointF size, string imagePath, float animationSpeed)
            : base(position, velocity, size, imagePath, animationSpeed)
        {
            time.Start();
            this.startVelocity = velocity;  
        }

        public override void Update(float deltaTime)
        {
            if(Keyboard.IsKeyDown(Keys.Space) && !GameWorld.GameRunning)
            {
                base.currentFrameIndex = 0;
            }
            
            if (time.ElapsedMilliseconds >= 5000)
            {
                if (this.position.Y >= GameWorld.DisplayRectangle.Height)
                {
                    time.Reset();
                    base.currentFrameIndex++;
                }
            }

            position.Y -= velocity.Y;
            if (position.Y + this.Sprite.Height * this.Size.Y <= 2)
            {
                position.Y = GameWorld.DisplayRectangle.Height;
            }
        }
    }
}
