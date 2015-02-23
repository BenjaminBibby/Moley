using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;

namespace Moley_Heaven_to_Hell
{
    class GameWorld
    {
        #region Field
        private int maxRandom = 100;
        private Random rand = new Random();
        private static List<GameObject> tmpObjects, objects;
        private Stopwatch timer = new Stopwatch();
        private Graphics dc;
        private DateTime endTime;
        private float deltaTime;
        private BufferedGraphics backBuffer;
        private static Rectangle displayRectangle;
        #endregion
        #region Properties
        public static Rectangle DisplayRectangle
        {
            get { return GameWorld.displayRectangle; }
        }
        public static List<GameObject> Objects
        {
            get { return GameWorld.objects; }
        }
        public static List<GameObject> TmpObjects
        {
            get { return GameWorld.tmpObjects; }
        }
        #endregion
        #region Constructor
        public GameWorld(Graphics dc, Rectangle displayRectangle)
        {
            tmpObjects = new List<GameObject>();
            objects = new List<GameObject>();

            GameWorld.displayRectangle = displayRectangle;

            this.backBuffer = BufferedGraphicsManager.Current.Allocate(dc, displayRectangle);
            this.dc = backBuffer.Graphics;

            SetupWorld();
        }
        #endregion

        public void SetupWorld()
        {
            timer.Start();
            endTime = DateTime.Now;

            // All the GameObjects that is in the world to start with, should be added here:
            Mole player = new Mole(new PointF(displayRectangle.Width/2, displayRectangle.Height/2 ), new PointF(0, 0), new PointF(0.5f, 0.5f), @"Sprites\Mole\Walk_Left_1.png;Sprites\Mole\Walk_Left_2.png", 1, 5f);
            objects.Add(player);
        }

        public void GameLoop()
        {

            TimeSpan deltaTimeSpan = DateTime.Now - endTime;
            int milliSeconds = deltaTimeSpan.Milliseconds > 0 ? deltaTimeSpan.Milliseconds : 1;
            deltaTime = 1 / ((float)1000 / milliSeconds);
            endTime = DateTime.Now;

            if (timer.ElapsedMilliseconds >= 1500)
            {
<<<<<<< HEAD
                objects.Add(new Platform(new PointF(rand.Next(0, displayRectangle.Width - 128), displayRectangle.Height), new PointF(0, -3), new PointF(1, 1), @"Sprites\Platforms\ground0.png", 1, 0));
=======
                int random = rand.Next(0, maxRandom);
                maxRandom++;
                if (random < maxRandom * 0.25f)
                {
                    objects.Add(new Platform(new PointF(rand.Next(-50, displayRectangle.Width - 128), displayRectangle.Height), new PointF(0, -3), new PointF(1, 1), @"Sprites\Platforms\ground0.png", 0));
                }
                else if (random > maxRandom * 0.25f && random < maxRandom * 0.75f )
                {
                    objects.Add(new Platform(new PointF(rand.Next(-50, displayRectangle.Width - 128), displayRectangle.Height), new PointF(0, -3), new PointF(1, 1), @"Sprites\Platforms\ground10.png", 0));
                }
                else if(random > maxRandom * 0.75f)
                {
                    objects.Add(new Platform(new PointF(rand.Next(-50, displayRectangle.Width - 128), displayRectangle.Height), new PointF(0, -3), new PointF(1, 1), @"Sprites\Platforms\ground11.png", 0));
                }

>>>>>>> 84c0d94daf635b623e7c89cef2a35db60797890f
                timer.Restart();
            }

            Update(deltaTime);
            CallAnimationFrames(deltaTime);
            Draw();
        }

        public void Draw()
        {
            dc.Clear(Color.White);
            Font f = new Font("Arial", 16);

            foreach(GameObject obj in tmpObjects)
            {
                obj.Draw(dc);
            }
#if DEBUG
            dc.DrawString("Fps: " + 1 / deltaTime, f, Brushes.Black, 0, 0);
            dc.DrawString("Items: " + objects.Count, f, Brushes.Black, 0, 32);

#endif
            backBuffer.Render();
        }
        public void Update(float deltaTime)
        {
            tmpObjects = objects.ToList();

            foreach (GameObject obj in tmpObjects)
            {
                obj.Update(deltaTime);
            }
        }
        public void CallAnimationFrames(float deltaTime)
        {
            foreach (GameObject obj in tmpObjects)
            {
                obj.UpdateAnimation(deltaTime);
            }
        }
    }
}
