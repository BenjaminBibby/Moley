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
            get { return new RectangleF(position.X, position.Y, Sprite.Width * Size.X, Sprite.Height * Size.Y); }
        }

        public Platform(PointF position, PointF velocity, PointF size, string imagePath, int imageAmount, float animationSpeed)
            : base(position, velocity, size, imagePath, imageAmount, animationSpeed)
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
            base.Draw(dc);

            dc.DrawRectangle(new Pen(Color.Black, 0.1f), position.X, position.Y, CollisionBox.Width, CollisionBox.Height);
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
            if (other is Mole && other.Position.Y + other.Sprite.Height * other.Size.Y < this.position.Y + this.Sprite.Height * this.Size.Y)
                other.SetPosition(new PointF(other.Position.X, this.position.Y - other.Sprite.Height * other.Size.Y));
        }
    }
}
