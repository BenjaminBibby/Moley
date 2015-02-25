using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Moley_Heaven_to_Hell
{
    class Background : GameObject
    {
        private PointF startVelocity;

        public PointF StartVelocity
        {
            get { return startVelocity; }
            set { startVelocity = value; }
        }

        public Background(PointF position, PointF velocity, PointF size, string imagePath, int imageAmount, float animationSpeed)
            : base(position, velocity, size, imagePath, imageAmount, animationSpeed)
        {
            this.startVelocity = velocity;  
        }

        public override void Update(float deltaTime)
        {
            position.Y -= velocity.Y;
            if (position.Y + this.Sprite.Height * this.Size.Y <= 2)
            {
                position.Y = GameWorld.DisplayRectangle.Height;
            }
        }
    }
}
