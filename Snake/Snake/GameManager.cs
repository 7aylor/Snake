using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class GameManager
    {
        public const int BLOCK_WIDTH = 20;
        public static float TICK = 0.1f;

        public double TimeSinceLastTick { get; set; } = 0f;

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
    }
}
