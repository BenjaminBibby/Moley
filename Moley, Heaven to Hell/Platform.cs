using System;
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
            get { return new RectangleF(position.X, position.Y, sprite.Width * size.X, sprite.Height * size.Y); }
        }
        public Platform(PointF position, PointF velocity, PointF size, string imagePath, float animationSpeed)
            : base(position, velocity, size, imagePath, animationSpeed)
        {
            
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            this.position.Y += velocity.Y;

            if (this.position.Y + this.sprite.Height < 0)
            {
                Destroy(this);
            }

            CheckCollision();
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
            
        }
    }
}
