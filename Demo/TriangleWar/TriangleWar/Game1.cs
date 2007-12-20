#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace TriangleWar
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ContentManager content;
        KeyHandler keyboard;

        Random rand = new Random();
        Texture2D myTexture;
        Vector2 spritePosition;
        Vector2 spriteSpeed = Vector2.Zero;
        SpriteBatch spriteBatch;
        int score = 0;
        Boolean shotExists = false;
        Texture2D shotTexture;
        Vector2 shotPosition = Vector2.Zero;
        Vector2 shotSpeed = new Vector2(0, -200);
        Vector2 shotStart = new Vector2(22, -5);
        Boolean targetExists = false;
        Texture2D targetTexture;
        Vector2 targetPosition = Vector2.Zero;
        Vector2 targetSpeed = new Vector2(75, 0);
        float respawnAt = 1;
        Boolean targetShotExists = false;
        Texture2D targetShotTexture;
        Vector2 targetShotPosition = Vector2.Zero;
        Vector2 targetShotSpeed = new Vector2(0, 200);
        Vector2 targetShotStart = new Vector2(22, 55);
        float fps;
        float updateInterval = 1.0f;
        float timeSinceLastUpdate = 0.0f;
        int framecount = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);
            keyboard = new KeyHandler();

			graphics.SynchronizeWithVerticalRetrace = false;
			IsFixedTimeStep = false;
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spritePosition = new Vector2((graphics.GraphicsDevice.Viewport.Width - 50) / 2,
                graphics.GraphicsDevice.Viewport.Height - 50);

            base.Initialize();
        }


        /// <summary>
        /// Load your graphics content.  If loadAllContent is true, you should
        /// load content from both ResourceManagementMode pools.  Otherwise, just
        /// load ResourceManagementMode.Manual content.
        /// </summary>
        /// <param name="loadAllContent">Which type of content to load.</param>
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                myTexture = content.Load<Texture2D>("spaceship2");
                spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
                shotTexture = content.Load<Texture2D>("OjTri");
                targetTexture = content.Load<Texture2D>("GreenTri");
                targetShotTexture = content.Load<Texture2D>("PurTri");
            }

        }


        /// <summary>
        /// Unload your graphics content.  If unloadAllContent is true, you should
        /// unload content from both ResourceManagementMode pools.  Otherwise, just
        /// unload ResourceManagementMode.Manual content.  Manual content will get
        /// Disposed by the GraphicsDevice during a Reset.
        /// </summary>
        /// <param name="unloadAllContent">Which type of content to unload.</param>
        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent)
            {
                // TODO: Unload any ResourceManagementMode.Automatic content
                content.Unload();
            }

            // TODO: Unload any ResourceManagementMode.Manual content
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            CheckKeyboardInput();
            UpdateSprite(gameTime);
            UpdateFPS(gameTime);

            base.Update(gameTime);
        }

        private void CheckKeyboardInput()
        {
            keyboard.Update();

            if (keyboard.IsActionPressed(KeyHandler.Actions.MenuBack))
            {
                this.Exit();
            }

            if (keyboard.IsActionDown(KeyHandler.Actions.Left) && keyboard.IsActionDown(KeyHandler.Actions.Right))
            {
                spriteSpeed.X = 0;
            }
            else if (keyboard.IsActionDown(KeyHandler.Actions.Left))
            {
                spriteSpeed.X = -100;
            }
            else if (keyboard.IsActionDown(KeyHandler.Actions.Right))
            {
                spriteSpeed.X = 100;
            }
            else
            {
                spriteSpeed.X = 0;
            }

            if (keyboard.IsActionDown(KeyHandler.Actions.Up) && keyboard.IsActionDown(KeyHandler.Actions.Down))
            {
                spriteSpeed.Y = 0;
            }
            else if (keyboard.IsActionDown(KeyHandler.Actions.Up))
            {
                spriteSpeed.Y = -100;
            }
            else if (keyboard.IsActionDown(KeyHandler.Actions.Down))
            {
                spriteSpeed.Y = 100;
            }
            else
            {
                spriteSpeed.Y = 0;
            }

            if (keyboard.IsActionPressed(KeyHandler.Actions.PrimaryShoot))
            {
                if (!shotExists)
                {
                    shotExists = true;
                    shotPosition = spritePosition + shotStart;
                }

            }

            if (keyboard.IsKeyPressed(Keys.F))
            {
                graphics.ToggleFullScreen();
            }

        }

        private void UpdateSprite(GameTime gameTime)
        {
            spritePosition += spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (shotExists)
            {
                shotPosition += shotSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (targetExists)
            {
                targetPosition += targetSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (targetShotExists)
            {
                targetShotPosition += targetShotSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            int MaxX = graphics.GraphicsDevice.Viewport.Width - myTexture.Width;
            int MinX = 0;
            int MaxY = graphics.GraphicsDevice.Viewport.Height - myTexture.Height;
            int MinY = 0;

            if (spritePosition.X > MaxX)
            {
                spritePosition.X = MaxX;
            }
            else if (spritePosition.X < MinX)
            {
                spritePosition.X = MinX;
            }

            if (spritePosition.Y > MaxY)
            {

                spritePosition.Y = MaxY;
            }
            else if (spritePosition.Y < MinY)
            {
                spritePosition.Y = MinY;
            }

            if (targetPosition.X > MaxX)
            {
                targetPosition.X = MaxX;
                targetSpeed.X = -75;
            }
            else if (targetPosition.X < MinX)
            {
                targetPosition.X = MinX;
                targetSpeed.X = 75;
            }

            if (shotExists)
            {
                if (shotPosition.X > MaxX || shotPosition.X < MinX || shotPosition.Y > MaxY || shotPosition.Y < MinY)
                {
                    shotExists = false;
                }
            }
            if (targetShotExists)
            {
                if (targetShotPosition.X > MaxX || targetShotPosition.X < MinX || targetShotPosition.Y > MaxY || targetShotPosition.Y < MinY)
                {
                    targetShotExists = false;
                }
            }
            else
            {
                if (targetPosition.X - spritePosition.X > -20 && targetPosition.X - spritePosition.X < 20)
                {
                    targetShotExists = true;
                    targetShotPosition = targetPosition + targetShotStart;
                }
            }


            if (shotExists && targetExists)
            {
                if (shotPosition.X > targetPosition.X && shotPosition.X < targetPosition.X + 50
                    && shotPosition.Y > targetPosition.Y && shotPosition.Y < targetPosition.Y + 45)
                {
                    targetExists = false;
                    shotExists = false;
                    score++;
                    respawnAt = (float)gameTime.TotalGameTime.TotalSeconds + 1;
                }
            }

            if (targetShotExists)
            {
                if (targetShotPosition.X > spritePosition.X && targetShotPosition.X < spritePosition.X + 50
                    && targetShotPosition.Y > spritePosition.Y && targetShotPosition.Y < spritePosition.Y + 45)
                {
                    targetShotExists = false;
                    //Game Over!
                }
            }

            if (!targetExists && respawnAt < (float)gameTime.TotalGameTime.TotalSeconds)
            {
                targetExists = true;
                targetPosition = new Vector2(rand.Next(graphics.GraphicsDevice.Viewport.Width - 50), rand.Next(100));
                targetSpeed.X *= -1;
            }

        }

        private void UpdateFPS(GameTime gameTime)
        {
            framecount++;
            timeSinceLastUpdate += (float)gameTime.ElapsedRealTime.TotalSeconds;
            if (timeSinceLastUpdate > updateInterval)
            {
                fps = framecount / timeSinceLastUpdate;
                Window.Title = "FPS: " + fps.ToString();
                framecount = 0;
                timeSinceLastUpdate = 0.0f;
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            if (targetExists)
            {
                spriteBatch.Draw(targetTexture, targetPosition, Color.White);
            }
            if (shotExists)
            {
                spriteBatch.Draw(shotTexture, shotPosition, Color.White);
            }
            if (targetShotExists)
            {
                spriteBatch.Draw(targetShotTexture, targetShotPosition, Color.White);
            }
            spriteBatch.Draw(myTexture, spritePosition, Color.White);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
