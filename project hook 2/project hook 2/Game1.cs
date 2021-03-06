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
		private List<Sprite> spritelist = new List<Sprite>();
		List<Shot> shots = new List<Shot>();

		//level stuff
		LevelReader m_LReader;
		LevelHandler m_LHandler;
		float m_Distance = 0;
		int m_Speed = 100;
		public int Speed
		{
			get
			{
				return m_Speed;
			}
			set
			{
				m_Speed = value;
			}
		}

		public static GraphicsDeviceManager graphics;
		Sprite cloud;
		SpriteBatch m_spriteBatch;
		Sprite back;
		Player player;
		Sprite back2;
		Sprite crosshair;
		ICollection<Sprite> m_TailBodySprites = new List<Sprite>();
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

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);

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


			InputHandler.LoadDefaultBindings();

			TextureLibrary.iniTextures(Content);

			graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;

			base.Initialize();

			Sound.Initialize();

			Music.Initialize();

			//test
			//System.Diagnostics.Stopwatch t_Time = new System.Diagnostics.Stopwatch();
			//t_Time.Start();
			//m_LReader = new LevelReader("LevelTest.xml");
			//m_LHandler = new LevelHandler(m_LReader.ReadFile(), this);
			//t_Time.Stop();
			//Console.WriteLine(t_Time.ElapsedMilliseconds);
			//EnvironmentLoader.Initialize(m_Speed);
			//EnvironmentLoader.ReadLevelBmp(System.Environment.CurrentDirectory + "\\Content\\Levels\\testBMP.bmp");

		}

		protected override void LoadContent()
		{
			TextureLibrary.reloadAll();



			TextureLibrary.LoadTexture("Cloud");
			TextureLibrary.LoadTexture("Ship2");
			TextureLibrary.LoadTexture("veinbg");
			TextureLibrary.LoadTexture("RedShot");
			TextureLibrary.LoadTexture("Enemy1");
			TextureLibrary.LoadTexture("virus1");
			TextureLibrary.LoadTexture("Explosion");
			TextureLibrary.LoadTexture("Shield");
			TextureLibrary.LoadTexture("FireBall");
			TextureLibrary.LoadTexture("temptail");
			TextureLibrary.LoadTexture("blood");
			TextureLibrary.LoadTexture("crosshairs");
			TextureLibrary.LoadTexture("tailbody");
			TextureLibrary.LoadTexture("tail_segment");
			TextureLibrary.LoadTexture("shot_energy");
			TextureLibrary.LoadTexture("bloodcell");
			TextureLibrary.LoadTexture("plaque");
			TextureLibrary.LoadTexture("wall_flat");
			TextureLibrary.LoadTexture("Explosion2");
			TextureLibrary.LoadTexture("walls\\wall_left");
			TextureLibrary.LoadTexture("wall_rand1");
			TextureLibrary.LoadTexture("wall_rand2");
			TextureLibrary.LoadTexture("wall_rand3");
			TextureLibrary.LoadTexture("walls\\plaque");
			TextureLibrary.LoadTexture("walls\\plaque2");
			TextureLibrary.LoadTexture("walls\\plaque3");
			TextureLibrary.LoadTexture("walls\\plaque4");
			TextureLibrary.LoadTexture("walls\\plaque_left");
			TextureLibrary.LoadTexture("walls\\plaque_right");
			TextureLibrary.LoadTexture("walls\\plaque_top");
			TextureLibrary.LoadTexture("walls\\plaque_top_left");
			TextureLibrary.LoadTexture("walls\\plaque_top_left_invert");
			TextureLibrary.LoadTexture("walls\\plaque_top_right");
			TextureLibrary.LoadTexture("walls\\plaque_top_right_invert");
			TextureLibrary.LoadTexture("walls\\plaque_btm");
			TextureLibrary.LoadTexture("walls\\plaque_btm_left");
			TextureLibrary.LoadTexture("walls\\plaque_btm_left_invert");
			TextureLibrary.LoadTexture("walls\\plaque_btm_right");
			TextureLibrary.LoadTexture("walls\\plaque_btm_right_invert");

			drawtext.Load(Content);




			Rectangle PlayerBounds = new Rectangle(graphics.GraphicsDevice.Viewport.X,
													graphics.GraphicsDevice.Viewport.Y,
													 graphics.GraphicsDevice.Viewport.Width,
													   graphics.GraphicsDevice.Viewport.Height);
			m_spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
			GameTexture cloudTexture = TextureLibrary.getGameTexture("Cloud", "");
			GameTexture crosshairs = TextureLibrary.getGameTexture("crosshairs", "");
			//back = new YScrollingBackground(TextureLibrary.getGameTexture("Back", ""), );
			player = new Player("Ship", new Vector2(300.0f, 400.0f), 100, 100, TextureLibrary.getGameTexture("Ship2", "1"), 100, true, 0.0f, Depth.GameLayer.Ships, PlayerBounds);
			back2 = new Sprite("back", new Vector2(100.0f, 100.0f), 500, 600, TextureLibrary.getGameTexture("Back", ""), 100, true, 0.0f, Depth.BackGroundLayer.Background);
			cloud = new Sprite("Cloud", new Vector2(0f, 0f), cloudTexture.Height, cloudTexture.Width, cloudTexture, 100f, true, 0, Depth.BackGroundLayer.Upper);
			//spawnEnemy();
			crosshair = new Sprite("crosshair", new Vector2(100f, 100f), crosshairs.Height, crosshairs.Width, crosshairs, 100f, true, 0f, Depth.GameLayer.Cursor);
			for (int i = 0; i < 60; i++)
			{
				if (i % 5 == 0)
				{
					Sprite tailBodySprite = new Sprite("tail_segment", new Vector2(100f, 100f), 20, 20, TextureLibrary.getGameTexture("tail_segment", ""), 64, true, 0.0f, Depth.GameLayer.TailBody);
					tailBodySprite.Transparency = 0.5f;
					tailBodySprite.BlendMode = SpriteBlendMode.AlphaBlend;
					m_TailBodySprites.Add(tailBodySprite);
				}
				else
				{
					Sprite tailBodySprite = new Sprite("shot_energy", new Vector2(100f, 100f), 10, 10, TextureLibrary.getGameTexture("shot_energy", ""), 64, true, 0.0f, Depth.GameLayer.TailBody);
					tailBodySprite.Transparency = 0.2f;
					tailBodySprite.BlendMode = SpriteBlendMode.Additive;
					m_TailBodySprites.Add(tailBodySprite);
				}

			}
			tail = new Tail("Tail", player.PlayerShip.Position, TextureLibrary.getGameTexture("temptail", "").Height, TextureLibrary.getGameTexture("temptail", "").Width, TextureLibrary.getGameTexture("temptail", ""), 100f, true, 0f, Depth.GameLayer.Tail, Collidable.Factions.Player, float.NaN, null, 30, player.PlayerShip, 700, m_TailBodySprites);

			spritelist.Add(back);

			spritelist.Add(tail);

			spritelist.Add(player.PlayerShip);

			m_spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
		}

		private static Random rand = new Random();


		private void spawnEnemy()
		{
			//enemy = new Ship("Enemy", new Vector2(100f, -100f), 100, 100, TextureLibrary.getGameTexture("Enemy1", ""), 100f, true, MathHelper.PiOver2, Depth.MidGround.Bottom, Collidable.Factions.Enemy, 100, 0, TextureLibrary.getGameTexture("Explosion", "1"), 100, "OneShot");

			//PathGroup group1 = new PathGroup();
			//Dictionary<PathStrategy.ValueKeys, Object> dicS = new Dictionary<PathStrategy.ValueKeys, object>();
			//dicS.Add(PathStrategy.ValueKeys.Base, (Ship)enemy);
			//group1.AddPath(new Path(Paths.Shoot, dicS));

			//Dictionary<PathStrategy.ValueKeys, Object> dic1 = new Dictionary<PathStrategy.ValueKeys, object>();
			//dic1.Add(PathStrategy.ValueKeys.Base, enemy);
			//dic1.Add(PathStrategy.ValueKeys.Speed, 100f);
			//dic1.Add(PathStrategy.ValueKeys.End, new Vector2(600, 200));
			//dic1.Add(PathStrategy.ValueKeys.Duration, (float)rand.Next(1, 6));
			//dic1.Add(PathStrategy.ValueKeys.Rotation, false);
			//group1.AddPath(new Path(Paths.Straight, dic1));

			//enemy.PathList.AddPath(group1);

			//PathGroup group2 = new PathGroup();

			//group2.AddPath(new Path(Paths.Shoot, dicS));

			//Dictionary<PathStrategy.ValueKeys, Object> dic2 = new Dictionary<PathStrategy.ValueKeys, object>();
			//dic2.Add(PathStrategy.ValueKeys.Base, enemy);
			//dic2.Add(PathStrategy.ValueKeys.Speed, 100f);
			//dic2.Add(PathStrategy.ValueKeys.End, new Vector2(100, 200));
			//dic2.Add(PathStrategy.ValueKeys.Duration, (float)rand.Next(1, 6));
			//dic2.Add(PathStrategy.ValueKeys.Rotation, false);
			//group2.AddPath(new Path(Paths.Straight, dic2));

			//enemy.PathList.AddPath(group2);

			//enemy.PathList.Mode = ListModes.Repeat;

			//spritelist.Add(enemy);
		}

		protected override void UnloadContent()
		{
			Content.Unload();
		}


		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{

			if (!IsActive)
				return;

			m_LHandler.CheckEvents(m_Distance);
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
				player.Shoot();
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
				tail.TailAttack();
			}


			if (InputHandler.IsActionDown(Actions.TailSecondary))
			{
				if (tail.EnemyCaught != null)
				{
					tail.EnemyShoot();
				}
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

			if (InputHandler.IsKeyPressed(Keys.M))
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
				player.MoveRight();
			}
			if (InputHandler.IsActionDown(Actions.Left))
			{
				player.MoveLeft();
			}
			if (InputHandler.IsActionDown(Actions.Up))
			{
				player.MoveUp();
			}
			if (InputHandler.IsActionDown(Actions.Down))
			{
				player.MoveDown();
			}
			if (InputHandler.IsKeyPressed(Keys.F))
			{
				graphics.ToggleFullScreen();
			}

			if (InputHandler.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.C))
			{
				Collision.DevEnableCollisionDisplay(spritelist);
			}


			player.UpdatePlayer(gameTime);
			// lazy fps code
			UpdateFPS(gameTime);
			// adn
			//enemy.Update(gameTime);
			tail.Update(gameTime);

			Collision.CheckCollisions(spritelist);

			List<Sprite> toAdd = new List<Sprite>();
			if (player.PlayerShip.SpritesToBeAdded != null)
			{
				toAdd.AddRange(player.PlayerShip.SpritesToBeAdded);
				player.PlayerShip.SpritesToBeAdded.Clear();
			}
			List<Sprite> toBeRemoved = new List<Sprite>();

			foreach (Sprite s in spritelist)
			{
				if (!s.Enabled)
				{
					toBeRemoved.Add(s);
				}
				else
				{
					s.Update(gameTime);
				}
				if (s.SpritesToBeAdded != null)
				{

					toAdd.AddRange(s.SpritesToBeAdded);
					s.SpritesToBeAdded.Clear();
				}
			}

			spritelist.AddRange(toAdd);

			foreach (Sprite s in toBeRemoved)
			{
				spritelist.Remove(s);
			}
			//    enemy.RegisterCollision(null, null);

			//if (enemy.AmIDead())
			//{
			//    spritelist.Remove(enemy);
			//    spawnEnemy();
			//}

			//if (enemy.Faction == Collidable.Factions.Player) // hack check if it's been tail-grabbed
			//{
			//    spawnEnemy();
			//}
			//((Ship)enemy).shoot(gameTime);

			//change the distance
			//m_Delta = m_Speed * p_GameTime.ElapsedGameTime.TotalSeconds;
			m_Distance += m_Speed * (float)(gameTime.ElapsedGameTime.TotalSeconds);
			//EnvironmentLoader.Update(gameTime);
			//Console.WriteLine(m_Distance);

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

			player.DrawPlayer(gameTime, m_spriteBatch);

			crosshair.Draw(m_spriteBatch);

			foreach (Sprite s in spritelist)
			{
				s.Draw(m_spriteBatch);
			}

			//EnvironmentLoader.Draw(m_spriteBatch);
			//back2.Draw(m_spriteBatch);
			//shotEffect.Draw(m_spriteBatch);
			//shot2Effect.Draw(m_spriteBatch);
			drawtext.DrawString(m_spriteBatch, "Press Space!!!!", new Vector2(100, 100), Color.Yellow, Depth.HUDLayer.Midground);
			drawtext.DrawString(m_spriteBatch, "Score: " + player.Score.ScoreTotal, new Vector2(0, 50), Color.White);
			drawtext.DrawString(m_spriteBatch, "FPS: " + fps.ToString(), Vector2.Zero, Color.White);

			//enemy.Draw(m_spriteBatch);
			//explosion.Draw(m_spriteBatch);
			tail.Draw(m_spriteBatch);

			m_spriteBatch.End();

			framecount++;

			base.Draw(gameTime);
		}

		public void ChangeFile(String p_FileName)
		{
			m_LReader = new LevelReader(p_FileName);
			m_Distance = 0;
			//m_LHandler = new LevelHandler(m_LReader.ReadFile(), this);
		}

		public void AddSprite(Sprite p_Sprite)
		{
			spritelist.Add(p_Sprite);
		}
	}
}
