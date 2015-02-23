using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Moley_Heaven_to_Hell
{
    abstract class GameObject
    {
        #region field
        protected PointF position;
        private PointF size;
        protected PointF velocity;
        protected float animationSpeed;
        private float currentFrameIndex;
        private Image sprite;
        protected List<Image> animationFrames;
        #endregion
        #region properties

        public PointF Size
        {
            get { return size; }
        }
        public Image Sprite
        {
            get { return sprite; }
            set { sprite = value;  }
        }
        public PointF Position
        {
          get { return position; }
          set { position = value; }
        }
        
        public PointF Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        #endregion

        public GameObject(PointF position, PointF velocity, PointF size, string imagePath, int imageAmount, float animationSpeed)
        {
            //string[] images = new string[imageAmount];
            this.position = position;
            this.velocity = velocity;
            this.size = size;
            string[] imagePaths = imagePath.Split(';');
            this.animationFrames = new List<Image>();

            foreach (string path in imagePaths)
            {
                animationFrames.Add(Image.FromFile(path));
            }

            this.sprite = this.animationFrames[0];
            this.animationSpeed = animationSpeed;
        }

        public virtual void Update(float deltaTime)
        { 
            
        }

        public virtual void Draw(Graphics dc)
        {
            dc.DrawImage(sprite, position.X, position.Y, sprite.Width * size.X, sprite.Height * size.Y);
        }

        public virtual void UpdateAnimation(float deltaTime)
        {
            // The frame index is based on the animation speed over fps
            //float factor = (1 / deltaTime);
            currentFrameIndex += deltaTime * animationSpeed;
            // Reseting the current frame index, if it exeeds the amount of images in the animation
            if (currentFrameIndex >= animationFrames.Count)
            {
                currentFrameIndex = 0;
            }
            // Replaces the sprite to the current frame index
            sprite = animationFrames[(int)currentFrameIndex];
        }

        public void Destroy(GameObject go)
        {
            GameWorld.Objects.Remove(go);
        }

        public void SetPosition(PointF position)
        {
            this.position = position;
        }
    }
}
