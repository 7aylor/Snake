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
    public enum Direction { Up, Down, Left, Right }

    class Snake
    {

        public List<BodyBlock> Body { get; set; }
        public int Score { get; set; }

        public Snake()
        {
            Body = new List<BodyBlock>();
            for (int i = 0; i < 4; i++)
            {
                //TODO: Make initialization vary with random start direction and axis
                Body.Add(new BodyBlock(Direction.Left, new Vector2(0 + (float)(i * GameManager.BLOCK_WIDTH), 300)));
            }
        }

        public void Move()
        {
            foreach(var bodyBlock in Body)
            {
                var prevPos = bodyBlock.Position;
                bodyBlock.Position = new Vector2(prevPos.X + GameManager.BLOCK_WIDTH, prevPos.Y);
            }
        }

        public void Draw(SpriteBatch sb, Texture2D sprite)
        {
            foreach (var bodyBlock in Body)
            {
                sb.Draw(sprite, new Rectangle((int)bodyBlock.Position.X, (int)bodyBlock.Position.Y, GameManager.BLOCK_WIDTH, GameManager.BLOCK_WIDTH ), Color.Black);

                sprite.SetData<Color>(new Color[] { Color.White });

                sb.Draw(sprite, new Rectangle((int)bodyBlock.Position.X + 1, (int)bodyBlock.Position.Y + 1, GameManager.BLOCK_WIDTH - 2, GameManager.BLOCK_WIDTH - 2), Color.White);
            }
        }
    }
}
