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
    public enum Direction { Up, Down, Left, Right, None }

    class Snake
    {
        public List<BodyBlock> Body { get; set; }
        public int Score { get; set; }
        public Direction PrevDirection { get; set; }
        public bool IsAlive { get; set; }

        public Snake()
        {
            Body = new List<BodyBlock>();
            PrevDirection = Direction.Right;
            IsAlive = true;
            Score = 0;

            for (int i = 8; i > 0; i--)
            {
                //TODO: Make initialization vary with random start direction and axis
                Body.Add(new BodyBlock(Direction.Left, new Vector2(0 + (float)(i * GameManager.BLOCK_WIDTH), 300)));
            }
        }

        /// <summary>
        /// Move the snake, block by block
        /// </summary>
        /// <param name="newDirection"></param>
        public void Move(Direction newDirection)
        {
            if (newDirection != Direction.None && IsValidDirection(newDirection)) PrevDirection = newDirection;

            //TODO: handle bug where snake is in a corner and a button is pressed, making it go off the screen

            for (int i = Body.Count - 1; i > 0; i--)
            {
                Body[i].Direction = Body[i - 1].Direction;
                Body[i].Position = Body[i - 1].Position;
            }

            Body[0].Direction = PrevDirection;

            switch (PrevDirection)
            {
                case (Direction.Left):
                    if(Body[0].Position.X - GameManager.BLOCK_WIDTH < 0)
                    {
                        Body[0].Position = new Vector2(GameManager.SCREEN_WIDTH - GameManager.BLOCK_WIDTH, Body[0].Position.Y);
                    }
                    else
                    {
                        Body[0].Position = Body[0].Position + (Vector2.UnitX * -GameManager.BLOCK_WIDTH);
                    }
                    break;
                case (Direction.Up):
                    if (Body[0].Position.Y - GameManager.BLOCK_WIDTH < 0)
                    {
                        Body[0].Position = new Vector2(Body[0].Position.X, GameManager.SCREEN_HEIGHT - GameManager.BLOCK_WIDTH);
                    }
                    else
                    {
                        Body[0].Position = Body[0].Position + (Vector2.UnitY * -GameManager.BLOCK_WIDTH);
                    }
                    break;
                case (Direction.Right):
                    if (Body[0].Position.X + GameManager.BLOCK_WIDTH > GameManager.SCREEN_WIDTH)
                    {
                        Body[0].Position = new Vector2(0, Body[0].Position.Y);
                    }
                    else
                    {
                        Body[0].Position = Body[0].Position + (Vector2.UnitX * GameManager.BLOCK_WIDTH);
                    }
                    break;
                case (Direction.Down):
                    if (Body[0].Position.Y + GameManager.BLOCK_WIDTH > GameManager.SCREEN_HEIGHT)
                    {
                        Body[0].Position = new Vector2(Body[0].Position.X, 0);
                    }
                    else
                    {
                        Body[0].Position = Body[0].Position + (Vector2.UnitY * GameManager.BLOCK_WIDTH);
                    }
                    break;
            }

            if(Body.Select(x => x.Position).Where(x => x == Body[0].Position)?.Count() > 1)
            {
                IsAlive = false;
                Body.Clear();
            }
        }

        public void Grow()
        {
            Score += GameManager.ScoreStep;
            Vector2 newBlockPos = Vector2.One;
            BodyBlock prevBlock = Body[Body.Count - 1];
            switch (prevBlock.Direction)
            {
                case (Direction.Left):
                    newBlockPos = new Vector2(prevBlock.Position.X - GameManager.BLOCK_WIDTH, prevBlock.Position.Y);
                    break;
                case (Direction.Up):
                    newBlockPos = new Vector2(prevBlock.Position.X, prevBlock.Position.Y - GameManager.BLOCK_WIDTH);
                    break;
                case (Direction.Right):
                    newBlockPos = new Vector2(prevBlock.Position.X + GameManager.BLOCK_WIDTH, prevBlock.Position.Y);
                    break;
                case (Direction.Down):
                    newBlockPos = new Vector2(prevBlock.Position.X, prevBlock.Position.Y + GameManager.BLOCK_WIDTH);
                    break;
            }


            BodyBlock newBlock = new BodyBlock(prevBlock.Direction, newBlockPos);

            //if the new block is in the same position as the head, game over
            if(newBlock.Position == Body[0].Position)
            {
                IsAlive = false;
            }

            Body.Add(newBlock);

        }

        /// <summary>
        /// ensures that the snake can go in the same or opposite direction as the previous direction
        /// </summary>
        /// <param name="newDirection"></param>
        /// <returns></returns>
        private bool IsValidDirection(Direction newDirection)
        {
            if(PrevDirection == Direction.Left || PrevDirection == Direction.Right)
            {
                return newDirection == Direction.Up || newDirection == Direction.Down;
            }
            else if (PrevDirection == Direction.Up || PrevDirection == Direction.Down)
            {
                return newDirection == Direction.Left || newDirection == Direction.Right;
            }

            return false;
        }

        /// <summary>
        /// draw the snake, block by block
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="sprite"></param>
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
