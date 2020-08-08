using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Linq;

namespace Snake
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        GameManager gameManager;
        InputManager inputManager;

        SpriteBatch spriteBatch;
        Snake snake;
        Apple apple;
        Texture2D whitePixel;
        Direction newDirection; //caches the new direction so it is maintained if a button isn't pressed


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameManager.SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = GameManager.SCREEN_HEIGHT;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            snake = new Snake();
            gameManager = new GameManager();
            inputManager = new InputManager();
            newDirection = Direction.None;
            apple = new Apple(snake.Body.Select(x => x.Position).ToArray(), false); 
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            whitePixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            whitePixel.SetData<Color>(new Color[] { Color.White });

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            //handle key press
            if(inputManager.WasKeyPressed())
                newDirection = inputManager.GetDirectionFromValidInput();

            if(gameManager.Tick(gameTime.ElapsedGameTime.TotalSeconds))
            {
                snake.Move(newDirection);
                if(gameManager.CheckCollision(snake.Body[0].Position, apple.Position))
                {
                    apple.DropApple(snake.Body.Select(x => x.Position).ToArray());
                    snake.Grow();
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            apple.Draw(spriteBatch, whitePixel);
            snake.Draw(spriteBatch, whitePixel);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
