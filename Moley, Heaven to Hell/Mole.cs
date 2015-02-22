using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Moley_Heaven_to_Hell
{
    class Mole : GameObject
    {
        private int speed;
        public float acceleration;
        private bool walkLeft;
        private float maxSpeed;
        private enum State { idle, walk, fall };
        private State state;

        public Mole(PointF position, PointF velocity, PointF size, string imagePath, float animationSpeed)
            : base(position, velocity, size, imagePath, animationSpeed)
        {
            this.speed = 10;
        }

        public override void Update(float deltaTime)
        {
            if (Keyboard.IsKeyDown(Keys.A) && this.position.X - speed > 0)
            {
                if (walkLeft)
                {
                    FlipSprite();
                    walkLeft = false;
                }

                if (acceleration <= 1)
                {
                    acceleration += 0.05f;
                }

                state = State.walk;
                position.X -= speed * acceleration;
            }
            else if (Keyboard.IsKeyDown(Keys.D) && this.position.X + speed < GameWorld.DisplayRectangle.Width - this.sprite.Width * this.size.X)
            {
                if (!walkLeft)
                {
                    FlipSprite();
                    walkLeft = true;
                }

                if (acceleration <= 1)
                {
                    acceleration += 0.05f;
                }

                state = State.walk;
                position.X += speed * acceleration;
            }
            else
            {
                acceleration = 0;
                state = State.idle;
            }

            base.Update(deltaTime);
        }
        private void FlipSprite()
        {
            foreach (Image spr in animationFrames)
            {
                spr.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
        }
    }
}
