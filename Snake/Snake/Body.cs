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
    class BodyBlock
    {
        public Direction Direction { get; set; }
        public Vector2 Position { get; set; }

        public BodyBlock(Direction direction, Vector2 position)
        {
            Direction = direction;
            Position = position;
        }
    }
}
