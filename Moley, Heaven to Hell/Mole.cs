using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Moley_Heaven_to_Hell
{
    class Mole : GameObject, ICollidable
    {
        private bool walkLeft;
        private float maxSpeed, speed, gravitySpeed;
        private enum State { idle, walk, fall };
        private State state;

        public Mole(PointF position, PointF velocity, PointF size, string imagePath, int imageAmount, float animationSpeed)
            : base(position, velocity, size, imagePath, imageAmount, animationSpeed)
        {
            this.velocity.Y = 2;
            this.maxSpeed = 10;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            CheckCollision();

            if (!PlaceFree_y((int)(this.Sprite.Height * this.Size.Y + 15)))
            {
                state = State.fall;
            }
            else if(state != State.walk)
            {
                state = State.idle;
            }

            //If the player has reached the bottom of the screen
            if (this.position.Y + this.Sprite.Height * this.Size.Y >= GameWorld.DisplayRectangle.Height)
            {
                this.position.Y = GameWorld.DisplayRectangle.Height - this.Sprite.Height * this.Size.Y; //Keep the player at the bottom of the screen
            }

            // Adjust animation to current state of movement
            if ((base.currentFrameIndex >= 2) && state == State.walk)//Walk
            {
                base.currentFrameIndex = 0;
            }
            else if ((base.currentFrameIndex >= 2 || base.currentFrameIndex <= 1) && state == State.fall)   //Fall
            {
                base.currentFrameIndex = 2;
            }
            else if ((base.currentFrameIndex >= 3 || base.currentFrameIndex <= 2) && state == State.idle)  //Idle
            {
                base.currentFrameIndex = 3;
            }

            this.position.Y += velocity.Y;//Makes the mole fall downwards

            if (Keyboard.IsKeyDown(Keys.A) && this.position.X - speed > 0)
            {
                if (walkLeft)
                {
                    FlipSprite();
                    walkLeft = false;
                }
                if (PlaceFree_x(-15))
                {
                    position.X -= Acceleration(0.04f);
                    if (!PlaceFree_y(1));
                    state = State.walk;
                }
            }
            else if (Keyboard.IsKeyDown(Keys.D) && this.position.X + speed < GameWorld.DisplayRectangle.Width - this.Sprite.Width * this.Size.X)
            {
                if (!walkLeft)
                {
                    FlipSprite();
                    walkLeft = true;
                }
                if (PlaceFree_x(15))
                {
                    position.X += Acceleration(0.04f);
                    if (!PlaceFree_y(1))
                    state = State.walk;
                }
            }
            else
            {
                speed = 0;
                if (state != State.fall) ;
                state = State.idle;
            }
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

        public RectangleF CollisionBox
        {
            get { return new RectangleF(position.X,position.Y, this.Sprite.Width * Size.X, this.Sprite.Height * Size.Y); }
        }

        public bool IsCollidingWith(GameObject other)
        {
            return CollisionBox.IntersectsWith((other as ICollidable).CollisionBox);
        }

        public void CheckCollision()
        {
            foreach (GameObject obj in GameWorld.Objects)
            {
                if (obj is ICollidable)
                {
                    if (this.IsCollidingWith(obj) && (obj != this))
                    {
                        OnCollision(obj);
                    }
                }
            }
        }

        public void OnCollision(GameObject other)
        {
            while ((position.Y + CollisionBox.Height) >= other.Position.Y)
            {
                position.Y --;
            }
        }
        private bool PlaceFree_x(int x)
        {
            RectangleF checkBox = new RectangleF(position.X, position.Y, 0, CollisionBox.Height - 2);   // the reducing of 2, ensures it doesn't mess up with collision

            // Setting the offset of the checkbox
            if (x > 0)
            {
                checkBox.X += CollisionBox.Width;
                checkBox.Width = x;
            }
            else
            {
                checkBox.X += x;
                checkBox.Width = -x;
            }
            foreach (GameObject obj in GameWorld.Objects)
            {
                if (obj is ICollidable && obj != this)
                {
                    if (checkBox.IntersectsWith((obj as ICollidable).CollisionBox))
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }

        private bool PlaceFree_y(int y)
        {
            RectangleF checkBox = new RectangleF(position.X, position.Y, CollisionBox.Width, 0);

            // Setting the offset of the checkbox
            if (y > 0)
            {
                checkBox.Y += CollisionBox.Height;
                checkBox.Height = y;
            }
            else
            {
                checkBox.Y += y;
                checkBox.Height = -y;
            }
            foreach (GameObject obj in GameWorld.Objects)
            {
                if (obj is ICollidable && obj != this)
                {
                    if (checkBox.IntersectsWith((obj as ICollidable).CollisionBox))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
