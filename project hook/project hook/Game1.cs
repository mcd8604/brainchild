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

namespace project_hook
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

		List<Sprite> spritelist = new List<Sprite>();

        GraphicsDeviceManager graphics;
        ContentManager content;
        KeyHandler keyhandler;
		Sprite cloud;
        SpriteBatch m_spriteBatch;
        Sprite back;
        Player back1;
        Sprite back2;

        Sprite shotEffect;
		Sprite shot2Effect;

		Collidable enemy;

		Sprite explosion;

        // lazy fps code
        DrawText drawtext;
        float fps;
        float updateInterval = 1.0f;
        float timeSinceLastUpdate = 0.0f;
        int framecount = 0;
        // adn

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);
            keyhandler = new KeyHandler();

            // lazy fps code
            drawtext = new DrawText();
            graphics.SynchronizeWithVerticalRetrace = false;
			IsFixedTimeStep = false;
            // adn
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            TextureLibrary.iniTextures(content);

            graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;

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
                TextureLibrary.LoadTexture("Ship2");
                TextureLibrary.LoadTexture("Back");
                TextureLibrary.LoadTexture("RedShot");
				TextureLibrary.LoadTexture("Cloud");
				TextureLibrary.LoadTexture("Enemy1");
				TextureLibrary.LoadTexture("Explosion");
                drawtext.Load(content);

                m_spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
				GameTexture cloudTexture = TextureLibrary.getGameTexture("Cloud","");
                back = new Sprite("back", new Vector2(800.0f, 600.0f), -graphics.PreferredBackBufferHeight, -graphics.PreferredBackBufferWidth, TextureLibrary.getGameTexture("Back", ""), 100, true, 0, Depth.BackGround.Bottom);
                back1 = new Player("Ship", new Vector2(100.0f, 100.0f), 100, 100, TextureLibrary.getGameTexture("Ship2", "1"), 100, true, 0.0f,Depth.ForeGround.Bottom);
                back2 = new Sprite("back", new Vector2(100.0f, 100.0f), 500, 600, TextureLibrary.getGameTexture("Back", ""), 100, true, 0.0f,Depth.MidGround.Bottom);
				cloud = new Sprite("Cloud", new Vector2(0f, 0f), cloudTexture.Height, cloudTexture.Width, cloudTexture, 100f, true, 0, Depth.ForeGround.Top);
				enemy = new Collidable("Enemy", new Vector2(400f, 100f), 100, 100, TextureLibrary.getGameTexture("Enemy1", ""), 100f, true, 0f, Depth.ForeGround.Bottom, Collidable.Factions.Enemy, 100, null, 100, null, 100);
				shot2Effect = new Sprite("RedShot2", new Vector2(-400.0f, 100.0f), 100, 50, TextureLibrary.getGameTexture("RedShot", "1"), 100, true, 0, Depth.MidGround.Top);
				shot2Effect.setAnimation("RedShot", 10);
				shotEffect = new Sprite("RedShot", new Vector2(-100.0f, 100.0f), 100, 50, TextureLibrary.getGameTexture("RedShot", "1"), 100, true, 0, Depth.MidGround.Top);
				shotEffect.setAnimation("RedShot", 10);
				shotEffect.Animation.StartAnimation();
				shot2Effect.Animation.StartAnimation();

				explosion = new Sprite("Explosion", new Vector2(-100f, -100f), 100, 100, TextureLibrary.getGameTexture("Explosion", ""), 50f, true, 0, Depth.ForeGround.Mid);


				spritelist.Add(enemy);
				spritelist.Add(shotEffect);
				spritelist.Add(shot2Effect);

            }

            // TODO: Load any ResourceManagementMode.Manual content
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
            keyhandler.Update();
            // Allows the game to exit
            if (keyhandler.IsActionDown(KeyHandler.Actions.Pause))
            {
                this.Exit();
            }

            if (keyhandler.IsActionDown(KeyHandler.Actions.PrimaryShoot))
             {
				
                
				//back1.Shoot();
				

            }

			if (keyhandler.IsActionPressed(KeyHandler.Actions.PrimaryShoot))
			{
				Vector2 shot = shotEffect.Position;
				shot.X = back1.PlayerShip.Position.X;
				shot.Y = back1.PlayerShip.Position.Y;
				shotEffect.Position = shot;

				shot = shot2Effect.Position;
				shot.X = back1.PlayerShip.Position.X + 100;
				shot.Y = back1.PlayerShip.Position.Y;
				shot2Effect.Position = shot;
				back1.Shoot();
			}

            if (keyhandler.IsActionDown(KeyHandler.Actions.Right))
            { 
                back1.MoveRight();
            }
            if (keyhandler.IsActionDown(KeyHandler.Actions.Left))
            {
                back1.MoveLeft();
            }
            if (keyhandler.IsActionDown(KeyHandler.Actions.Up))
            {
                back1.MoveUp();
            }
            if (keyhandler.IsActionDown(KeyHandler.Actions.Down))
            {
                back1.MoveDown();
            }
			if (keyhandler.IsKeyPressed(Keys.F))
			{
				graphics.ToggleFullScreen();
			}


            Vector2 shotV = shot2Effect.Position;
			shotV.Y += -(float)(gameTime.ElapsedGameTime.TotalSeconds) * 200;
            shot2Effect.Position = shotV;
            shot2Effect.Update(gameTime);


			shotV = shotEffect.Position;
			shotV.Y += -(float)(gameTime.ElapsedGameTime.TotalSeconds)*200;
			shotEffect.Position = shotV;
			shotEffect.Update(gameTime);

			shotEffect.Degree = shotEffect.Degree + (float)(gameTime.ElapsedGameTime.TotalSeconds)*4;



			//back1.Shoot();
			shot2Effect.Degree = shot2Effect.Degree - (float)(gameTime.ElapsedGameTime.TotalSeconds)*4;

            // lazy fps code
            UpdateFPS(gameTime);
            // adn

			QuickCheckCollision();

            base.Update(gameTime);
        }


        // lazy fps code
        private void UpdateFPS(GameTime gameTime)
        {
            timeSinceLastUpdate += (float)gameTime.ElapsedRealTime.TotalSeconds;
            if (timeSinceLastUpdate > updateInterval)
            {
                fps = framecount / timeSinceLastUpdate;
                framecount = 0;
                timeSinceLastUpdate = 0.0f;
            }
        }
        // adn

		private void QuickCheckCollision()
		{

			List<Sprite> temp = new List<Sprite>(spritelist);

			foreach( Sprite item in temp ) {
				//temp.Remove( item ); // Todo: fix this later
				foreach ( Sprite item2 in temp ) {
					if ( item != item2 && Intersection.DoesIntersectDiamond(item.Position + item.Center, item.Height/2.5f, item2.Position + item2.Center, item2.Height/2.5f) ) {
						explosion.Position = (item.Position + item2.Position) / 2;
					}
				}
			}

		}

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            m_spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);

            back.Draw(m_spriteBatch);

            back1.DrawPlayer(gameTime, m_spriteBatch);
         //   back2.Draw(m_spriteBatch);
            shotEffect.Draw(m_spriteBatch);
			shot2Effect.Draw(m_spriteBatch);
            drawtext.DrawString(m_spriteBatch, "Press Space!!!!", new Vector2(100, 100), Color.Yellow, Depth.ForeGround.Top);
			drawtext.DrawString(m_spriteBatch, "Score: " + back1.Score.ScoreTotal, new Vector2(0,50));
            drawtext.DrawString(m_spriteBatch, "FPS: " + fps.ToString());
			cloud.Draw(m_spriteBatch);

			enemy.Draw(m_spriteBatch);
			explosion.Draw(m_spriteBatch);

            m_spriteBatch.End();

            framecount++;

            

            base.Draw(gameTime);
        }
    }
}
