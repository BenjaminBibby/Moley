﻿using System;
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
        private Stopwatch scoreTime = new Stopwatch();
        private Graphics dc;
        private DateTime endTime;
        private float deltaTime;
        private BufferedGraphics backBuffer;
        private static Rectangle displayRectangle;
        private static bool gameRunning;


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
        public static bool GameRunning
        {
            get { return GameWorld.gameRunning; }
            set { GameWorld.gameRunning = value; }
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
            scoreTime.Start();
            endTime = DateTime.Now;
            gameRunning = true;

<<<<<<< HEAD
            Objects.Add(new Background(new PointF(0,0), new PointF(0,2), new PointF(1.5f,1.5f), @"Sprites\Backgrounds\Background1.png", 1, 0));
            Objects.Add(new Background(new PointF(0, displayRectangle.Height), new PointF(0, 2), new PointF(1.5f, 1.5f), @"Sprites\Backgrounds\Background1.png", 1, 0));
=======
            Objects.Add(new Background(new PointF(0, 0), new PointF(0, 2), new PointF(1f, 1f), @"Sprites\Backgrounds\Background_sky.png;Sprites\Backgrounds\Background_transition.png;Sprites\Backgrounds\Background1.png;Sprites\Backgrounds\lava.png", 0));
            Objects.Add(new Background(new PointF(0, displayRectangle.Height), new PointF(0, 2), new PointF(1f, 1f), @"Sprites\Backgrounds\Background_sky.png;Sprites\Backgrounds\Background1.png;Sprites\Backgrounds\lava.png;Sprites\Backgrounds\lava.png", 0));
>>>>>>> 95fdf89c38ff85c00d203da8a059847873832c45

            // All the GameObjects that is in the world to start with, should be added here:
            Mole player = new Mole(new PointF(displayRectangle.Width / 2, displayRectangle.Height / 2), new PointF(0, 0), new PointF(0.5f, 0.5f), @"Sprites\Mole\Walk_Left_1.png;Sprites\Mole\Walk_Left_2.png;Sprites\Mole\Fall.png;Sprites\Mole\Idle_Left.png;Sprites\Mole\dig_1.png;Sprites\Mole\dig_2.png;Sprites\Mole\dig_3.png;Sprites\Mole\dig_4.png;Sprites\Mole\dig_5.png;Sprites\Mole\dig_6.png;Sprites\Mole\dig_7.png", 5);
            objects.Add(player);
        }

        public void GameLoop()
        {
            TimeSpan deltaTimeSpan = DateTime.Now - endTime;
            int milliSeconds = deltaTimeSpan.Milliseconds > 0 ? deltaTimeSpan.Milliseconds : 1;
            deltaTime = 1 / ((float)1000 / milliSeconds);
            endTime = DateTime.Now;

            if (timer.ElapsedMilliseconds >= 1000 && gameRunning)
            {
                int random = rand.Next(0, maxRandom);
                maxRandom++;
                if (random < maxRandom * 0.25f)
                {
                    objects.Add(new Platform(new PointF(rand.Next(-50, displayRectangle.Width - 128), displayRectangle.Height), new PointF(0, -3), new PointF(1, 1), @"Sprites\Platforms\ground10.png", 0));
                }
                else if (random > maxRandom * 0.25f && random < maxRandom * 0.75f )
                {
                    objects.Add(new Platform(new PointF(rand.Next(-50, displayRectangle.Width - 128), displayRectangle.Height), new PointF(0, -3), new PointF(1, 1), @"Sprites\Platforms\ground11.png", 0));
                }
                else if(random > maxRandom * 0.75f && random < maxRandom * 0.85f)
                {
                    objects.Add(new Platform(new PointF(15, displayRectangle.Height), new PointF(0, -3), new PointF(1, 1), @"Sprites\Platforms\ground13.png", 0));
                }
                else if(random > maxRandom * 0.85f)
                {
                    objects.Add(new Platform(new PointF(rand.Next(-50, displayRectangle.Width - 128), displayRectangle.Height), new PointF(0, -3), new PointF(1, 1), @"Sprites\Platforms\ground14.png", 0));
                }
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

            if (gameRunning)
            {
                if (!scoreTime.IsRunning)
                {
                    scoreTime.Restart();
                }
                f = new Font("Arial", 16);
                dc.DrawString("Score: " + scoreTime.ElapsedMilliseconds / 1000, f, Brushes.White, new PointF(0, 0));
            }
#if DEBUG
            dc.DrawString("Fps: " + 1 / deltaTime, f, Brushes.White, 0, 0);
            dc.DrawString("Items: " + objects.Count, f, Brushes.White, 0, 32);
#endif
            if (!gameRunning)
            {
                scoreTime.Stop();
                f = new Font("Comic Sans MS", 32);
                dc.DrawString("You lose!", f, Brushes.White, new PointF(displayRectangle.Width * 0.5f - 100, 125));
                f = new Font("Comic Sans MS", 16);
                dc.DrawString("Press spacebar to restart!", f, Brushes.White, new PointF(displayRectangle.Width * 0.5f - 135, 175));
                f = new Font("Arial", 16);
                dc.DrawString("You survived for " + scoreTime.ElapsedMilliseconds / 1000 + " seconds!", f, Brushes.White, new PointF(displayRectangle.Width * 0.5f - 140, 210));
            }

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
