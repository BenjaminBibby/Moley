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
            //If the player has reached the bottom of the screen
            if (this.position.Y + this.Sprite.Height * this.Size.Y >= GameWorld.DisplayRectangle.Height)
            {
                this.position.Y = GameWorld.DisplayRectangle.Height - this.Sprite.Height * this.Size.Y; //Keep the player at the bottom of the screen
            }

            switch (state)
            {
                case State.fall:
                    this.Sprite = this.animationFrames[2];//Change the sprite to the falling sprite
                    break;
                case State.walk:
                    this.Sprite = this.animationFrames[1];//Change the sprite to the walking sprites
                    break;
                default:
                    this.Sprite = this.animationFrames[0];//Change the sprite to the idle sprite
                    break;
            }

            foreach (GameObject obj in GameWorld.Objects)
            {
                if (obj is ICollidable)//If the gameobject is Collidable
                {
                    if (!this.IsCollidingWith(obj) && (obj != this)) //If the mole is not colliding with an object
                    {
                        state = State.fall;//Sets the state to falling
                    }
                }
            }

            this.position.Y += velocity.Y;//Makes the mole fall downwards

            if (Keyboard.IsKeyDown(Keys.A) && this.position.X - speed > 0)
            {
                if (walkLeft)
                {
                    FlipSprite();
                    walkLeft = false;
                }
                if (PlaceFree_x(-3))
                {
                    position.X -= Acceleration(0.04f);
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
                if (PlaceFree_x(3))
                {
                    position.X += Acceleration(0.04f);
                    state = State.walk;
                }
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
            throw new NotImplementedException();
        }

        public void OnCollision(GameObject other)
        {
            throw new NotImplementedException();
        }
        private bool PlaceFree_x(int x)
        {
            RectangleF checkBox = new RectangleF(position.X, position.Y, 0, 0);

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
    }
}
