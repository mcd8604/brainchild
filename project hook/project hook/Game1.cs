#region Using Statements
using System;
using System.Collections.Generic;
using System.Collections;
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
    /// This class is for texting purposes
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        List<Sprite> spritelist = new List<Sprite>();
        List<Shot> shots = new List<Shot>();

        public static GraphicsDeviceManager graphics;
        ContentManager content;
        Sprite cloud;
        SpriteBatch m_spriteBatch;
        Sprite back;
        Player back1;
        Sprite back2;
        Sprite crosshair;
        ArrayList m_TailBodySprites = new ArrayList();
        Tail tail;
        //ButtonState lastMouseButton = ButtonState.Released;
        //ButtonState lastRightMouseButton = ButtonState.Released;

        //Sprite shotEffect;
        //Sprite shot2Effect;

        Collidable enemy;

        //Sprite explosion;

        // lazy fps code
        DrawText drawtext;
        float fps;
        float updateInterval = 1.0f;
        float timeSinceLastUpdate = 0.0f;
        int framecount = 0;
        int path = 0;
        // adn

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);

            // lazy fps code
            drawtext = new DrawText();
            //graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;
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
			//test
			LevelReader t_LR = new LevelReader("LevelTest.xml");
			t_LR.ReadFile();

            InputHandler.LoadDefaultBindings();

            TextureLibrary.iniTextures(content);

            graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;

            base.Initialize();

            Sprite.DrawWithRot();

            Sound.Initialize();

            Music.Initialize();
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
                TextureLibrary.reloadAll();

                TextureLibrary.LoadTexture("Ship2");
                TextureLibrary.LoadTexture("Back");
                TextureLibrary.LoadTexture("RedShot");
                TextureLibrary.LoadTexture("Cloud");
                TextureLibrary.LoadTexture("Enemy1");
                TextureLibrary.LoadTexture("Explosion");
                TextureLibrary.LoadTexture("Shield");
                TextureLibrary.LoadTexture("FireBall");
                TextureLibrary.LoadTexture("temptail");
                TextureLibrary.LoadTexture("crosshairs");
                TextureLibrary.LoadTexture("poisonsplat");
                drawtext.Load(content);

                Rectangle PlayerBounds = new Rectangle(graphics.GraphicsDevice.Viewport.X,
                                                        graphics.GraphicsDevice.Viewport.Y,
                                                         graphics.GraphicsDevice.Viewport.Width,
                                                           graphics.GraphicsDevice.Viewport.Height);
                m_spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
                GameTexture cloudTexture = TextureLibrary.getGameTexture("Cloud", "");
                GameTexture crosshairs = TextureLibrary.getGameTexture("crosshairs", "");
                back = new YScrollingBackground(TextureLibrary.getGameTexture("Back", ""));
                back1 = new Player("Ship", new Vector2(100.0f, 100.0f), 100, 100, TextureLibrary.getGameTexture("Ship2", "1"), 100, true, 0.0f, Depth.ForeGround.Bottom, PlayerBounds);
                back2 = new Sprite("back", new Vector2(100.0f, 100.0f), 500, 600, TextureLibrary.getGameTexture("Back", ""), 100, true, 0.0f, Depth.MidGround.Bottom);
                cloud = new Sprite("Cloud", new Vector2(0f, 0f), cloudTexture.Height, cloudTexture.Width, cloudTexture, 100f, true, 0, Depth.BackGround.Top);
                enemy = new Ship("Enemy", new Vector2(100f, 200f), 100, 100, TextureLibrary.getGameTexture("Enemy1", ""), 100f, true, 0f, Depth.MidGround.Bottom, Collidable.Factions.Enemy, 100, 0, null, 100, TextureLibrary.getGameTexture("Explosion", "1"), 100);
                crosshair = new Sprite("crosshair", new Vector2(100f, 100f), crosshairs.Height, crosshairs.Width, crosshairs, 100f, true, 0f, Depth.MidGround.Mid);
                for (int i = 0; i < 6; i++)
                {
                    Sprite tailBodySprite = new Sprite("poisonsplat", new Vector2(100f, 100f), 20, 20, TextureLibrary.getGameTexture("poisonsplat", ""), 255, true, 0.0f, Depth.MidGround.Bottom);
                    m_TailBodySprites.Add(tailBodySprite);
                }
                tail = new Tail("Tail", back1.PlayerShip.Position, TextureLibrary.getGameTexture("temptail", "").Height, TextureLibrary.getGameTexture("temptail", "").Width, TextureLibrary.getGameTexture("temptail", ""), 100f, true, 0f, Depth.ForeGround.Bottom, Collidable.Factions.Player, -1, 0, null, 30, back1.PlayerShip, 700, m_TailBodySprites);


                Dictionary<PathStrategy.ValueKeys, Object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
				dic.Add(PathStrategy.ValueKeys.Base, enemy);
				dic.Add(PathStrategy.ValueKeys.Speed, 100f);
                dic.Add(PathStrategy.ValueKeys.End, new Vector2(700, 200));
                dic.Add(PathStrategy.ValueKeys.Duration, 5f);
                enemy.PathList.AddPath(new Path(Paths.Straight, dic));

				Dictionary<PathStrategy.ValueKeys, Object> dic2 = new Dictionary<PathStrategy.ValueKeys, object>();
				dic2.Add(PathStrategy.ValueKeys.Base, enemy);
				dic2.Add(PathStrategy.ValueKeys.Speed, 100f);
				dic2.Add(PathStrategy.ValueKeys.End, new Vector2(100, 200));
				dic2.Add(PathStrategy.ValueKeys.Duration, 5f);
				enemy.PathList.AddPath(new Path(Paths.Straight, dic2));

				enemy.PathList.Mode = ListModes.Repeat;



                /*
                shot2Effect = new Sprite("RedShot2", new Vector2(-400.0f, 100.0f), 100, 50, TextureLibrary.getGameTexture("RedShot", "1"), 100, true, 0, Depth.MidGround.Top);
                shot2Effect.setAnimation("RedShot", 10);
                shotEffect = new Sprite("RedShot", new Vector2(-100.0f, 100.0f), 100, 50, TextureLibrary.getGameTexture("RedShot", "1"), 100, true, 0, Depth.MidGround.Top);
                shotEffect.setAnimation("RedShot", 10);
                shotEffect.Animation.StartAnimation();
                shot2Effect.Animation.StartAnimation();
                */
                //explosion = new Sprite("Explosion", new Vector2(-100f, -100f), 100, 100, TextureLibrary.getGameTexture("Explosion", ""), 50f, true, 0, Depth.ForeGround.Mid);

                spritelist.Add(back);
                spritelist.Add(enemy);
                spritelist.Add(tail);
                /*
				spritelist.Add(shotEffect);
				spritelist.Add(shot2Effect);
                 * */

                m_spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

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
            InputHandler.Update();
            Sound.Update();
            Music.Update();

            // Allows the game to exit
            if (InputHandler.IsActionPressed(Actions.Pause))
            {
                this.Exit();
            }

            if (InputHandler.IsActionDown(Actions.ShipPrimary))
            {
                /*
                 Vector2 shot = shotEffect.Position;
                 shot.X = back1.PlayerShip.Position.X;
                 shot.Y = back1.PlayerShip.Position.Y;
                 shotEffect.Position = shot;

                 shot = shot2Effect.Position;
                 shot.X = back1.PlayerShip.Position.X + 100;
                 shot.Y = back1.PlayerShip.Position.Y;
                 shot2Effect.Position = shot;	
                */
                List<Shot> t_Shots = back1.Shoot(gameTime);

                foreach (Sprite s in t_Shots)
                {
                    if (s.Name.Equals("no_Shot"))
                    {
                        //used so when the weapon can't shoot
                    }
                    else
                    {
                        spritelist.Add(s);
                    }
                }
            }

            if (InputHandler.IsActionPressed(Actions.ShipPrimary))
            {
                //Shot shot2 = new Shot("RedShot2", new Vector2(100, 100.0f), 75, 30, TextureLibrary.getGameTexture("RedShot", "1"), 100, true, 0, Depth.MidGround.Top
                //                        , Collidable.Factions.Player, -1, null, 2, null, 5, 10);
                //Vector2 shot = shot2.Position;
                //shot.X = back1.PlayerShip.Position.X;
                //shot.Y = back1.PlayerShip.Position.Y;
                //shot2.Position = shot;

                //shot2.setAnimation("RedShot", 10);

                //Dictionary<PathStrategy.ValueKeys, Object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
                //dic.Add(PathStrategy.ValueKeys.Start, shot2.Center);
                //dic.Add(PathStrategy.ValueKeys.End, new Vector2(shot2.Center.X, -100));
                //dic.Add(PathStrategy.ValueKeys.Duration, -1.0f);
                //dic.Add(PathStrategy.ValueKeys.Base, shot2);
                //shot2.Path = new Path(Path.Paths.Shot, dic);


                //Shot shot1 = new Shot("RedShot", new Vector2(100.0f, 100.0f), 75, 30, TextureLibrary.getGameTexture("RedShot", "1"), 100, true, 0, Depth.MidGround.Top
                //                      , Collidable.Factions.Player, -1, null, 2, null, 5, 10);

                //shot = shot1.Position;
                //shot.X = back1.PlayerShip.Position.X + 50;
                //shot.Y = back1.PlayerShip.Position.Y;
                //shot1.Position = shot;

                //shot1.setAnimation("RedShot", 10);

                //dic = new Dictionary<PathStrategy.ValueKeys, object>();
                //dic.Add(PathStrategy.ValueKeys.Start, shot1.Center);
                //dic.Add(PathStrategy.ValueKeys.End, new Vector2(shot1.Center.X, -100));
                //dic.Add(PathStrategy.ValueKeys.Duration, -1.0f);
                //dic.Add(PathStrategy.ValueKeys.Base, shot1);
                //shot1.Path = new Path(Path.Paths.Shot, dic);

                //shot1.Animation.StartAnimation();
                //shot2.Animation.StartAnimation();

                //spritelist.Add(back1.Shoot(gameTime));
                //spritelist.Add(shot2);
                //Shot t_Shot = back1.Shoot(gameTime);
                /*
                                List<Shot> t_Shots = back1.Shoot(gameTime);

                                foreach (Sprite s in t_Shots)
                                {
                                    if (s.Name.Equals("no_Shot"))
                                    {
                                        //used so when the weapon can't shoot
                                    }
                                    else
                                    {
                                        spritelist.Add(s);
                                    }
                                }
                * */

            }






            //Vector2 temp = new Vector2(Mouse.GetState().X,Mouse.GetState().Y);
            //crosshair.Center = temp;

            if (InputHandler.HasMouseMoved())
            {
                crosshair.Center = InputHandler.MousePostion;
            }






            //if(Mouse.GetState().LeftButton != lastMouseButton)
            //{
            //    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            //    {
            //        tail.TailAttack(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), gameTime);
            //    }
            //    lastMouseButton = Mouse.GetState().LeftButton;

            //}


            if (InputHandler.IsActionPressed(Actions.TailPrimary))
            {
                tail.TailAttack(InputHandler.MousePostion, gameTime);
            }






            //if (Mouse.GetState().RightButton != lastRightMouseButton)
            //{
            //    if (Mouse.GetState().RightButton == ButtonState.Pressed)
            //    {
            //        if (Music.IsPlaying("bg1"))
            //        {
            //            Music.Stop("bg1");
            //        }
            //        else
            //        {
            //            Music.Play("bg1");
            //        }
            //    }
            //    lastRightMouseButton = Mouse.GetState().RightButton;
            //}

            if (InputHandler.IsActionPressed(Actions.TailSecondary))
            {
                if (Music.IsPlaying("bg1"))
                {
                    Music.Stop("bg1");
                }
                else
                {
                    Music.Play("bg1");
                }
            }






            if (InputHandler.IsActionDown(Actions.Right))
            {
                back1.MoveRight();
            }
            if (InputHandler.IsActionDown(Actions.Left))
            {
                back1.MoveLeft();
            }
            if (InputHandler.IsActionDown(Actions.Up))
            {
                back1.MoveUp();
            }
            if (InputHandler.IsActionDown(Actions.Down))
            {
                back1.MoveDown();
            }
            if (InputHandler.IsKeyPressed(Keys.F))
            {
                graphics.ToggleFullScreen();
            }

            /*
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
            */
            back1.UpdatePlayer(gameTime);
            // lazy fps code
            UpdateFPS(gameTime);
            // adn
            enemy.Update(gameTime);
            tail.Update(gameTime);
            ((Sprite)m_TailBodySprites[0]).Update(gameTime);

            Collision.CheckCollisions(spritelist);

            List<Sprite> toBeRemoved = new List<Sprite>();

            foreach (Sprite s in spritelist)
            {
                if (!s.Visible)
                {
                    toBeRemoved.Add(s);
                }
                else
                {
                    s.Update(gameTime);
                }
            }

            foreach (Sprite s in toBeRemoved)
            {
                spritelist.Remove(s);
            }
            //    enemy.RegisterCollision(null, null);

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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            m_spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);

            back.Draw(m_spriteBatch);
            foreach (Sprite s in m_TailBodySprites)
                s.Draw(m_spriteBatch);

            back1.DrawPlayer(gameTime, m_spriteBatch);

            crosshair.Draw(m_spriteBatch);

            foreach (Sprite s in spritelist)
            {
                s.Draw(m_spriteBatch);
            }
            //back2.Draw(m_spriteBatch);
            //shotEffect.Draw(m_spriteBatch);
            //shot2Effect.Draw(m_spriteBatch);
            drawtext.DrawString(m_spriteBatch, "Press Space!!!!", new Vector2(100, 100), Color.Yellow, Depth.ForeGround.Top);
            drawtext.DrawString(m_spriteBatch, "Score: " + back1.Score.ScoreTotal, new Vector2(0, 50), Color.White);
            drawtext.DrawString(m_spriteBatch, "FPS: " + fps.ToString(), Vector2.Zero, Color.White);

            enemy.Draw(m_spriteBatch);
            //explosion.Draw(m_spriteBatch);
            tail.Draw(m_spriteBatch);

            m_spriteBatch.End();

            framecount++;

            base.Draw(gameTime);
        }
    }


}
