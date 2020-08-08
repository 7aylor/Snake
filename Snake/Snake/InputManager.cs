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
    class InputManager
    {
        Keys[] _validInputs = new Keys[]
        {
            Keys.A, Keys.Left,
            Keys.W, Keys.Up,
            Keys.D, Keys.Right,
            Keys.S, Keys.Down
        };

        /// <summary>
        /// checks if key was pressed
        /// </summary>
        /// <returns></returns>
        public bool WasKeyPressed()
        {
            return Keyboard.GetState().GetPressedKeys().Length > 0;
        }

        /// <summary>
        /// Checks if the user pressed a valid input and returns the direction
        /// </summary>
        /// <returns></returns>
        public Direction GetDirectionFromValidInput()
        {
            var keysPressed = Keyboard.GetState().GetPressedKeys();

            if (keysPressed.Length > 0 && keysPressed.Intersect(_validInputs).Any())
            {
                var key = keysPressed[keysPressed.Length - 1];

                if(key == Keys.A || key == Keys.Left)
                {
                    return Direction.Left;
                }
                if(key == Keys.W || key == Keys.Up)
                {
                    return Direction.Up;
                }
                if (key == Keys.D || key == Keys.Right)
                {
                    return Direction.Right;
                }
                if (key == Keys.S || key == Keys.Down)
                {
                    return Direction.Down;
                }
            }

            return Direction.None;
        }
    }
}
