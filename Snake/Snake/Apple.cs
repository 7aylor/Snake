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
    class Apple
    {
        public Vector2 Position { get; set; }
        public bool IsGood { get; set; } //if false, has bad side effect

        public Color Color {

            get
            {
                return IsGood ? Color.Red : Color.Green;
            }
        }

        public Apple(Vector2[] pos, bool isGood)
        {
            DropApple(pos);
            IsGood = isGood;
        }

        public void Draw(SpriteBatch sb, Texture2D sprite)
        {
            sprite.SetData<Color>(new Color[] { Color.White });
            sb.Draw(sprite, new Rectangle((int)Position.X + GameManager.BLOCK_WIDTH / 4, (int)Position.Y + GameManager.BLOCK_WIDTH / 4, GameManager.BLOCK_WIDTH / 2, GameManager.BLOCK_WIDTH / 2), this.Color);

        }

        /// <summary>
        /// Get the apple position that isn't already taken
        /// </summary>
        /// <param name="forbiddenCells"></param>
        /// <returns></returns>
        public void DropApple(Vector2[] forbiddenCells)
        {
            Vector2 applePosition;
            do
            {
                applePosition = GetRandomPosition();
            } while (forbiddenCells.Contains(applePosition));

            Position = applePosition;
        }

        /// <summary>
        /// Generates a random positoin that will fall into a grid space
        /// </summary>
        /// <returns></returns>
        private Vector2 GetRandomPosition()
        {
            Random random = new Random();

            double randX = random.NextDouble();
            double randY = random.NextDouble();

            int newPosX = (int)(randX * GameManager.SCREEN_WIDTH) - ((int)(randX * GameManager.SCREEN_WIDTH) % GameManager.BLOCK_WIDTH);
            int newPosY = (int)(randY * GameManager.SCREEN_HEIGHT) - ((int)(randY * GameManager.SCREEN_HEIGHT) % GameManager.BLOCK_WIDTH);

            return new Vector2(newPosX, newPosY);
        }
    }
}
