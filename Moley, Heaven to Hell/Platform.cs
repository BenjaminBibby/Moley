﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Moley_Heaven_to_Hell
{
    class Platform : GameObject, ICollidable
    {
        public RectangleF CollisionBox
        {
            get { return new RectangleF(position.X, position.Y, Sprite.Width * Size.X, Sprite.Height * Size.Y * 0.5f); }
        }

        public Platform(PointF position, PointF velocity, PointF size, string imagePath, float animationSpeed)
            : base(position, velocity, size, imagePath, animationSpeed)
        {
            
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            this.position.Y += velocity.Y;

            if (this.position.Y + this.Sprite.Height < 0)
            {
                Destroy(this);
            }

            CheckCollision();
        }

        public override void Draw(Graphics dc)
        {
#if DEBUG
            dc.DrawRectangle(new Pen(Brushes.Red, 0.1f), CollisionBox.X, CollisionBox.Y, CollisionBox.Width, CollisionBox.Height);
#endif
            base.Draw(dc);
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
            while (((other as ICollidable).CollisionBox.Y + (other as ICollidable).CollisionBox.Height) >= this.CollisionBox.Y)
            {
                other.Position = new PointF(other.Position.X, other.Position.Y - 1);
            }
        }
    }
}
