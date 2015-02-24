﻿using System;
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
                    break;
                case State.walk:
                    break;
                case State.idle:
                    break;
            }

            foreach (GameObject obj in GameWorld.Objects)
            {
                if (obj is ICollidable)//If the gameobject is Collidable
                {
                    if (this.IsCollidingWith(obj) == false) //If the mole is not colliding with an object
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
                if (PlaceFree_x(-15))
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
                if (PlaceFree_x(15))
                {
                    position.X += Acceleration(0.04f);
                    state = State.walk;
                }
            }
            else
            {
                speed = 0;
                if(state != State.fall)
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

        private bool PlaceFree_y(int y)
        {
            RectangleF checkBox = new RectangleF(position.X, position.Y, 0, 0);

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
        private void AvoidStuck()
        {
            float[] directions = new float[4];
            //directions[0] = directions[1] = directions[2] = directions[3] = 0;
        }
    }
}
