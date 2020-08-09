using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake
{
    public class GameManager
    {
        public const int BLOCK_WIDTH = 20;
        public const int SCREEN_WIDTH = 400;
        public const int SCREEN_HEIGHT = 400;
        public const int ScoreStep = 10;
        public static float TICK = 0.1f;

        public double TimeSinceLastTick { get; set; } = 0f;


        public delegate void ItemPickup();
        public event ItemPickup Pickup;

        /// <summary>
        /// Determines if the game "ticks" meaning all objects that should move will will move
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public bool Tick(double deltaTime)
        {
            TimeSinceLastTick += deltaTime;
            if(TimeSinceLastTick > TICK)
            {
                TimeSinceLastTick = 0;
                return true;
            }

            return false;
        }

        public bool CheckCollision(Vector2 a, Vector2 b)
        {
            return a == b;
        }

        public void IncreaseSpeed()
        {
            if(TICK > 0.05)
            {
                TICK -= 0.005f;
            }
        }
    }
}
