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
        private bool walkLeft;
        private float maxSpeed, speed, gravitySpeed;
        private enum State { idle, walk, fall };
        private State state;

        public Mole(PointF position, PointF velocity, PointF size, string imagePath, float animationSpeed)
            : base(position, velocity, size, imagePath, animationSpeed)
        {
            this.maxSpeed = 10;
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
                /*
                if (speed <= 1)
                {
                    speed += 0.05f;
                }
                position.X -= speed * maxSpeed;*/
                position.X -= Acceleration(0.04f);
                state = State.walk;
            }
            else if (Keyboard.IsKeyDown(Keys.D) && this.position.X + speed < GameWorld.DisplayRectangle.Width - this.sprite.Width * this.size.X)
            {
                if (!walkLeft)
                {
                    FlipSprite();
                    walkLeft = true;
                }
                /*
                if (acceleration <= 1)
                {
                    acceleration += 0.05f;
                }
                position.X += speed * acceleration;*/

                position.X += Acceleration(0.04f);
                state = State.walk;
            }
            else
            {
                speed = 0;

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
        private float Acceleration(float acc)
        {
            if (speed <= 1)
            {
                speed += acc;
            }
            return speed * maxSpeed;
        }
    }
}
