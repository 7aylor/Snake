using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Snake
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        GameManager gameManager;
        InputManager inputManager;

        Texture2D whitePixel;
        SpriteFont defaultFont;

        SpriteBatch spriteBatch;
        Snake snake;
        Apple apple;
        double timeToSpawnBadApple;
        double timeSinceLastBadAppleSpawned;
        bool startOfGame;

        List<Apple> badApples;

        Direction newDirection; //caches the new direction so it is maintained if a button isn't pressed

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameManager.SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = GameManager.SCREEN_HEIGHT;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            snake = new Snake();
            gameManager = new GameManager();
            inputManager = new InputManager();
            newDirection = Direction.None;
            apple = new Apple(snake.Body.Select(x => x.Position).ToArray(), false);
            badApples = new List<Apple>();
            timeToSpawnBadApple = new Random().Next(3, 8);
            timeSinceLastBadAppleSpawned = 0f;
            startOfGame = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            whitePixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            whitePixel.SetData<Color>(new Color[] { Color.White });
            defaultFont = Content.Load<SpriteFont>("default");

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            //handle key press
            if(snake.IsAlive)
            {
                timeSinceLastBadAppleSpawned += gameTime.ElapsedGameTime.TotalSeconds;
                if(inputManager.WasAnyKeyPressed())
                    newDirection = inputManager.GetDirectionFromValidInput();

                if(gameManager.Tick(gameTime.ElapsedGameTime.TotalSeconds))
                {
                    snake.Move(newDirection);

                    if(timeSinceLastBadAppleSpawned > timeToSpawnBadApple)
                    {
                        Apple newBadApple = new Apple(snake.Body.Select(x => x.Position).ToArray(), true);
                        badApples.Add(newBadApple);
                        timeToSpawnBadApple = new Random().Next(3, 8);
                        timeSinceLastBadAppleSpawned = 0;
                    }

                    if (snake.IsAlive)
                    {
                        if(gameManager.CheckCollision(snake.Body[0].Position, apple.Position))
                        {
                            //concat the bad apple positions to the snake positions so good apple doesn't spawn on existing bad apple
                            apple.DropApple(snake.Body.Select(x => x.Position).Concat(badApples.Select(x => x.Position)).ToArray()); 
                            snake.Grow();
                        }
                        foreach(var badApple in badApples)
                        {
                            if (gameManager.CheckCollision(snake.Body[0].Position, badApple.Position))
                            {
                                gameManager.IncreaseSpeed();
                                badApples.Remove(badApple);
                                snake.Score -= 5;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if(inputManager.WasKeyPressed(Keys.Enter))
                {
                    GameManager.TICK = 0.1f;
                    gameManager.Speed = 1;
                    snake = new Snake();
                    badApples.Clear();
                    timeSinceLastBadAppleSpawned = 0;
                    snake.IsAlive = true;
                    startOfGame = false;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();


            if(snake.IsAlive)
            {
                spriteBatch.DrawString(defaultFont, snake.Score.ToString(), Vector2.Zero, Color.White);
                spriteBatch.DrawString(defaultFont, gameManager.Speed.ToString(), new Vector2(GameManager.SCREEN_WIDTH - (10 * (gameManager.Speed.ToString().Length)), 0), Color.White);

                foreach(var badApple in badApples)
                {
                    badApple.Draw(spriteBatch, whitePixel);
                }
                apple.Draw(spriteBatch, whitePixel);

                snake.Draw(spriteBatch, whitePixel);

            }
            else if(startOfGame)
            {
                spriteBatch.DrawString(defaultFont, "Snake and the Bad Apples", new Vector2((GameManager.SCREEN_WIDTH / 2) - 80, (GameManager.SCREEN_HEIGHT / 3)), Color.White);
                spriteBatch.Draw(whitePixel, new Rectangle(140 + GameManager.BLOCK_WIDTH / 4, (GameManager.SCREEN_HEIGHT / 3) + 40 + GameManager.BLOCK_WIDTH / 4, GameManager.BLOCK_WIDTH / 2, GameManager.BLOCK_WIDTH / 2), Color.Red);
                spriteBatch.Draw(whitePixel, new Rectangle(160 + GameManager.BLOCK_WIDTH / 4, (GameManager.SCREEN_HEIGHT / 3) + 40 + GameManager.BLOCK_WIDTH / 4, GameManager.BLOCK_WIDTH / 2, GameManager.BLOCK_WIDTH / 2), Color.Red);
                spriteBatch.Draw(whitePixel, new Rectangle(180 + GameManager.BLOCK_WIDTH / 4, (GameManager.SCREEN_HEIGHT / 3) + 40 + GameManager.BLOCK_WIDTH / 4, GameManager.BLOCK_WIDTH / 2, GameManager.BLOCK_WIDTH / 2), Color.Red);
                spriteBatch.Draw(whitePixel, new Rectangle(200 + GameManager.BLOCK_WIDTH / 4, (GameManager.SCREEN_HEIGHT / 3) + 40 + GameManager.BLOCK_WIDTH / 4, GameManager.BLOCK_WIDTH / 2, GameManager.BLOCK_WIDTH / 2), Color.Green);
                spriteBatch.Draw(whitePixel, new Rectangle(220 + GameManager.BLOCK_WIDTH / 4, (GameManager.SCREEN_HEIGHT / 3) + 40 + GameManager.BLOCK_WIDTH / 4, GameManager.BLOCK_WIDTH / 2, GameManager.BLOCK_WIDTH / 2), Color.Red);
                spriteBatch.Draw(whitePixel, new Rectangle(240 + GameManager.BLOCK_WIDTH / 4, (GameManager.SCREEN_HEIGHT / 3) + 40 + GameManager.BLOCK_WIDTH / 4, GameManager.BLOCK_WIDTH / 2, GameManager.BLOCK_WIDTH / 2), Color.Red);
                spriteBatch.Draw(whitePixel, new Rectangle(260 + GameManager.BLOCK_WIDTH / 4, (GameManager.SCREEN_HEIGHT / 3) + 40 + GameManager.BLOCK_WIDTH / 4, GameManager.BLOCK_WIDTH / 2, GameManager.BLOCK_WIDTH / 2), Color.Red);
                spriteBatch.DrawString(defaultFont, "Press enter to start", new Vector2((GameManager.SCREEN_WIDTH / 4) + 45, (GameManager.SCREEN_HEIGHT / 2) + 10), Color.White);
            }
            else
            {
                spriteBatch.DrawString(defaultFont, "Game Over", new Vector2((GameManager.SCREEN_WIDTH / 2) - 30, (GameManager.SCREEN_HEIGHT / 3) - 10), Color.White);
                spriteBatch.DrawString(defaultFont, snake.Score.ToString(), new Vector2((GameManager.SCREEN_WIDTH / 2) + 5, (GameManager.SCREEN_HEIGHT / 3) + 10), Color.White);
                spriteBatch.DrawString(defaultFont, "Press enter to play again", new Vector2((GameManager.SCREEN_WIDTH / 4) + 20, (GameManager.SCREEN_HEIGHT / 2) + 10), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
