using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Mortuum
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Mortuum : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;
        GameScreen gameScreen;
        TitleScreen titleScreen;

        GameState currentGameState;
        GameState lastGameState;
        GameState nextGameState;

        float FPStime;
        int FPS;

        public Mortuum()
        {
            graphics = new GraphicsDeviceManager(this);
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
            graphics.PreferredBackBufferWidth = Settings.GraphicsWidth;
            graphics.PreferredBackBufferHeight = Settings.GraphicsHeight;
            graphics.PreferredBackBufferFormat = Settings.GraphicsFormat;
            graphics.IsFullScreen = Settings.GraphicsFullScreen;
            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24;
            graphics.SynchronizeWithVerticalRetrace = Settings.SyncWithVTrace;
            graphics.ApplyChanges();

            this.Window.Title = Settings.GameTitle;
            this.IsFixedTimeStep = Settings.FixedTimeStep;

            Camera.Resize(90.0f, graphics.GraphicsDevice.Viewport.AspectRatio, 0.0001f, 100.0f);

            Debug.Start("debug.log");
            Debug.Write("Mortuum starting.");

            FPS = 0;
            FPStime = 0;

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

            player = new Player();
            player.Init(Content, graphics);

            titleScreen = new TitleScreen();
            titleScreen.Load(Content, graphics, player);

            gameScreen = new GameScreen();
            gameScreen.Load(Content, graphics, player);

            currentGameState = GameState.TitleScreen;
            lastGameState = GameState.TitleScreen;
            nextGameState = GameState.TitleScreen;


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            titleScreen.Unload();

            Debug.Write("Mortuum stopping.");
            Debug.Stop();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);

            if (nextGameState != currentGameState)
            {
                if (GameState.Exit == nextGameState)
                    this.Exit();

                lastGameState = currentGameState;
                currentGameState = nextGameState;
            }
            
            switch(currentGameState)
            {
                case GameState.TitleScreen:
                    nextGameState = titleScreen.Update(elapsedTime);
                    break;

                case GameState.GameScreen:
                    nextGameState = gameScreen.Update(elapsedTime);
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            float elapsedTime = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);

            FPS = (int)(1.0f / elapsedTime);

            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            if (FPStime > 1.0f)
            {
                FPStime -= 1.0f;
                FPS = 0;
            }

            switch (currentGameState)
            {
                case GameState.TitleScreen:
                    titleScreen.Draw();
                    break;

                case GameState.GameScreen:
                    gameScreen.Draw();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
