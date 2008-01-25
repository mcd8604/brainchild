using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class World
	{
		// Alpha sprites
		private List<Sprite> m_SpriteList;
		// Additive Sprites;
		private List<Sprite> m_SpriteListA;

		private static Boolean m_CreateWorld = false;
		public static Boolean CreateWorld
		{
			get
			{
				return m_CreateWorld;
			}
			set
			{
				m_CreateWorld = value;
			}
		}

		private static Boolean m_destroyWorld;
		public static Boolean DestroyWorld
		{
			get
			{
				return m_destroyWorld;
			}
			set
			{
				m_destroyWorld = value;
			}
		}

		private static Boolean m_ResumeWorld;
		public static Boolean ResumeWorld
		{
			get
			{
				return m_ResumeWorld;
			}
			set
			{
				m_ResumeWorld = value;
			}
		}

		Player m_Player;
		Tail tail;
		Sprite crosshairs;

		GameState m_PreviousState;
		GameState m_State;
		public GameState State
		{
			get
			{
				return m_State;
			}

		}

		public enum GameState
		{
			Nothing,
			DoNotRender,
			Loading,
			Ready,
			Running,
			Paused,
			Dead,
			Won,
			Finished,
			Exiting
		}

		public static Rectangle m_ViewPortSize;

		public float m_Distance = 0;
		public int m_Speed = 80;
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

		LevelReader m_LReader;
		LevelHandler m_LHandler;
		EnvironmentLoader m_ELoader=new EnvironmentLoader();

		public World()
		{

			m_SpriteList = new List<Sprite>();
			m_SpriteListA = new List<Sprite>();

			m_State = GameState.Nothing;

		}

		//This method will initialize all the objects needed to run the game
		public void initialize(Rectangle p_DrawArea)
		{
			m_ViewPortSize = p_DrawArea;
			IniDefaults();
			Sprite.DrawWithRot();
			Music.Initialize();
			Sound.Initialize();
			this.m_LReader = new LevelReader("LevelTest.xml");
			this.m_LHandler = new LevelHandler(m_LReader.ReadFile(), this);
			m_SpriteList.AddRange( this.m_ELoader.Initialize(this.m_Speed) );
			this.m_ELoader.ReadLevelBmp(System.Environment.CurrentDirectory + "\\Content\\Levels\\testBMP.bmp");
		}

		//This method will load the level
		//This will load the level defintion into memory
		//and will also load all the textures/sounds needed to start the level.  
		public void loadLevel(ContentManager p_Content)
		{
			LoadDefaults(p_Content);

		}

		//This will deallocate any variables that need de allocation
		public void unload()
		{


		}

		//This will update the game world.  
		//Different update methdos can be run based on the game state.
		public void update(GameTime p_GameTime)
		{

			//This will be for normal everyday update operations.  
			if (m_State == GameState.Running)
			{
				m_LHandler.CheckEvents(Convert.ToInt32(m_Distance));

				m_Player.UpdatePlayer(p_GameTime);

				Collision.CheckCollisions(m_SpriteList);

				List<Sprite> toAdd = new List<Sprite>();

				m_SpriteList.RemoveAll(Sprite.isToBeRemoved);
				foreach (Sprite s in m_SpriteList)
				{
					s.Update(p_GameTime);

					if (s.SpritesToBeAdded != null)
					{

						toAdd.AddRange(s.SpritesToBeAdded);
						s.SpritesToBeAdded.Clear();
					}
				}
				m_SpriteList.AddRange(toAdd);

				m_Distance += m_Speed * (float)(p_GameTime.ElapsedGameTime.TotalSeconds);
				this.m_ELoader.Update(p_GameTime);
			}

		}

		public void checkKeys(GameTime p_GameTime)
		{

			// Allows the game to exit
			if (InputHandler.IsActionPressed(Actions.Pause))
			{
				if (m_State == GameState.Paused)
				{
					changeState(m_PreviousState);
					Menus.setCurrentMenu(Menus.MenuScreens.None);
				}
				else
				{
					changeState(GameState.Paused);
					Menus.setCurrentMenu(Menus.MenuScreens.Pause);
				}
			}

			if (m_State != GameState.Paused)
			{
				if (InputHandler.IsActionDown(Actions.ShipPrimary))
				{
					m_Player.Shoot();
				}

				if (InputHandler.IsActionPressed(Actions.ShipPrimary))
				{

				}

				if (InputHandler.IsActionDown(Actions.Right))
				{
					m_Player.MoveRight();
				}
				if (InputHandler.IsActionDown(Actions.Left))
				{
					m_Player.MoveLeft();
				}
				if (InputHandler.IsActionDown(Actions.Up))
				{
					m_Player.MoveUp();
				}
				if (InputHandler.IsActionDown(Actions.Down))
				{
					m_Player.MoveDown();
				}
				if (InputHandler.HasMouseMoved())
				{
					crosshairs.Center = InputHandler.MousePostion;
				}
				if (InputHandler.IsActionPressed(Actions.TailPrimary))
				{
					tail.TailAttack(InputHandler.MousePostion);
				}
				if (InputHandler.IsActionDown(Actions.TailSecondary))
				{
					if (tail.EnemyCaught != null)
					{
						//tail.EnemyCaught.shoot(InputHandler.MousePostion);
						tail.EnemyCaught.shoot();
					}
				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.M))
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
			}
			if (InputHandler.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.C))
			{
				Collision.DevEnableCollisionDisplay(m_SpriteList);
			}
			if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.X))
			{
				Sprite.DrawWithRot();
			}

			update(p_GameTime);

		}


		//This method will be called to change the games state.
		//This method will determine what actions need to be executed
		//to change to the new state.
		public void changeState(GameState p_State)
		{
			m_PreviousState = m_State;
			m_State = p_State;

		}

		//This will draw the 
		public void draw(SpriteBatch p_SpriteBatch)
		{
			if (!(m_State == GameState.DoNotRender))
			{
				p_SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);

				foreach (Sprite s in m_SpriteList)
				{
					if (s.Enabled == true)
					{
						s.Draw(p_SpriteBatch);
					}
				}

				p_SpriteBatch.End();

				p_SpriteBatch.Begin(SpriteBlendMode.Additive, SpriteSortMode.BackToFront, SaveStateMode.None);

				foreach (Sprite s in m_SpriteListA)
				{
					if (s.Enabled == true)
					{
						s.Draw(p_SpriteBatch);
					}
				}

				p_SpriteBatch.End();
			}
		}



		//This method will load some default values for the game
		public void LoadDefaults(ContentManager p_Content)
		{
			TextureLibrary.LoadTexture("Ship2");
			TextureLibrary.LoadTexture("Back");
			TextureLibrary.LoadTexture("veinbg");
			TextureLibrary.LoadTexture("RedShot");
			TextureLibrary.LoadTexture("Cloud");
			TextureLibrary.LoadTexture("Enemy1");
			TextureLibrary.LoadTexture("Explosion");
			TextureLibrary.LoadTexture("Shield");
			TextureLibrary.LoadTexture("FireBall");
			TextureLibrary.LoadTexture("temptail");
			TextureLibrary.LoadTexture("poisonsplat");
			TextureLibrary.LoadTexture("blood");
			TextureLibrary.LoadTexture("crosshairs");
            TextureLibrary.LoadTexture("tailbody");
            TextureLibrary.LoadTexture("tailbody");
            TextureLibrary.LoadTexture("tail_segment");
            TextureLibrary.LoadTexture("shot_energy");
			TextureLibrary.LoadTexture("virus1");
			TextureLibrary.LoadTexture("bloodcell");
			TextureLibrary.LoadTexture("wall1");
			TextureLibrary.LoadTexture("wall_flat");
			TextureLibrary.LoadTexture("wall_rand1");
			TextureLibrary.LoadTexture("wall_rand2");
			TextureLibrary.LoadTexture("wall_rand3");
		}

		private void IniDefaults()
		{
			GameTexture cloudTexture = TextureLibrary.getGameTexture("Cloud", "");

			//test scrolling background
			YScrollingBackground back = new YScrollingBackground(TextureLibrary.getGameTexture("veinbg", ""), m_Speed);

			m_Player = new Player("Ship", new Vector2(400.0f, 500.0f), 100, 100, TextureLibrary.getGameTexture("Ship2", "1"), 255f, true, 0.0f, Depth.ForeGround.Bottom, m_ViewPortSize);
			// Sprite back2 = new Sprite("back", new Vector2(100.0f, 100.0f), 500, 600, TextureLibrary.getGameTexture("Back", ""), 100, true, 0.0f, Depth.MidGround.Bottom);
			Sprite cloud = new Sprite("Cloud", new Vector2(0f, 0f), cloudTexture.Height, cloudTexture.Width, cloudTexture, 255f, true, 0, Depth.BackGround.Top);
			Ship enemy = new Ship("bloodcell", new Vector2(100f, 200f), 96, 128, TextureLibrary.getGameTexture("bloodcell", "1"), 255f, true, 0f, Depth.MidGround.Bottom, Collidable.Factions.Enemy, 100, 0, TextureLibrary.getGameTexture("Explosion", "3"), 50);
			enemy.setAnimation("bloodcell", 60);
			enemy.Animation.StartAnimation();
			Ship enemy2 = new Ship("Enemy", new Vector2(800f, 150f), 100, 100, TextureLibrary.getGameTexture("Enemy1", ""), 255f, true, MathHelper.PiOver2, Depth.MidGround.Bottom, Collidable.Factions.Enemy, 100, 0, TextureLibrary.getGameTexture("Explosion", "3"), 50);
			ICollection<Sprite> m_TailBodySprites = new List<Sprite>();

			crosshairs = new Sprite("crosshair", new Vector2(100f, 100f), TextureLibrary.getGameTexture("crosshairs", "").Height, TextureLibrary.getGameTexture("crosshairs", "").Width, TextureLibrary.getGameTexture("crosshairs", ""), 100f, true, 0f, Depth.MidGround.Mid);
			for (int i = 0; i < 60; i++)
			{
                if (i % 5 == 0)
                {
                    TailBodySprite tailBodySprite = new TailBodySprite("tail_segment", new Vector2(100f, 100f), 20, 20, TextureLibrary.getGameTexture("tail_segment", ""), 64, true, 0.0f, Depth.MidGround.Bottom);
                    tailBodySprite.Transparency = 0.5f;
                    tailBodySprite.BlendMode = SpriteBlendMode.AlphaBlend;
                    m_TailBodySprites.Add(tailBodySprite);
                }
                else
                {
                    TailBodySprite tailBodySprite = new TailBodySprite("shot_energy", new Vector2(100f, 100f), 10, 10, TextureLibrary.getGameTexture("shot_energy", ""), 64, true, 0.0f, Depth.MidGround.Bottom);
                    tailBodySprite.Transparency = 0.2f;
                    tailBodySprite.BlendMode = SpriteBlendMode.Additive;
                    m_TailBodySprites.Add(tailBodySprite);
                }

			}
			tail = new Tail("Tail", m_Player.PlayerShip.Position, TextureLibrary.getGameTexture("temptail", "").Height, TextureLibrary.getGameTexture("temptail", "").Width, TextureLibrary.getGameTexture("temptail", ""), 100f, true, 0f, Depth.ForeGround.Bottom, Collidable.Factions.Player, -1, null, 30, m_Player.PlayerShip, 700, m_TailBodySprites);
			tail.Health = int.MinValue;


			//Dictionary<PathStrategy.ValueKeys, Object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
			//dic.Add(PathStrategy.ValueKeys.Start, enemy.Center);
			//dic.Add(PathStrategy.ValueKeys.End, new Vector2(700, 200));
			//dic.Add(PathStrategy.ValueKeys.Duration, 5000.0f);
			//dic.Add(PathStrategy.ValueKeys.Base, enemy);
			//enemy.Path = new Path(Paths.Line, dic);
			//enemy.Update(new GameTime());

			enemy.Task = new TaskFire();

			//enemy pathing:

			//PathGroup group1a = new PathGroup();
			//Dictionary<PathStrategy.ValueKeys, Object> dicS = new Dictionary<PathStrategy.ValueKeys, object>();
			//dicS.Add(PathStrategy.ValueKeys.Base, enemy);
			//group1a.AddPath(new Path(Paths.Shoot, dicS));

			//Dictionary<PathStrategy.ValueKeys, Object> dic1a = new Dictionary<PathStrategy.ValueKeys, object>();
			//dic1a.Add(PathStrategy.ValueKeys.Base, enemy);
			//dic1a.Add(PathStrategy.ValueKeys.Speed, 100f);
			//dic1a.Add(PathStrategy.ValueKeys.End, new Vector2(500, 200));
			//dic1a.Add(PathStrategy.ValueKeys.Duration, 4f);
			//dic1a.Add(PathStrategy.ValueKeys.Rotation, false);
			//group1a.AddPath(new Path(Paths.Straight, dic1a));

			//enemy.PathList.AddPath(group1a);

			//PathGroup group1b = new PathGroup();

			//group1b.AddPath(new Path(Paths.Shoot, dicS));

			//Dictionary<PathStrategy.ValueKeys, Object> dic1b = new Dictionary<PathStrategy.ValueKeys, object>();
			//dic1b.Add(PathStrategy.ValueKeys.Base, enemy);
			//dic1b.Add(PathStrategy.ValueKeys.Speed, 100f);
			//dic1b.Add(PathStrategy.ValueKeys.End, new Vector2(100, 200));
			//dic1b.Add(PathStrategy.ValueKeys.Duration, 4f);
			//dic1b.Add(PathStrategy.ValueKeys.Rotation, false);
			//group1b.AddPath(new Path(Paths.Straight, dic1b));

			//enemy.PathList.AddPath(group1b);

			//enemy.PathList.Mode = ListModes.Repeat;


			enemy2.Task = new TaskFire();


			//PathGroup group2a = new PathGroup();
			//Dictionary<PathStrategy.ValueKeys, Object> dicS2 = new Dictionary<PathStrategy.ValueKeys, object>();
			//dicS2.Add(PathStrategy.ValueKeys.Base, enemy2);
			//group2a.AddPath(new Path(Paths.Shoot, dicS2));

			//Dictionary<PathStrategy.ValueKeys, Object> dic2a = new Dictionary<PathStrategy.ValueKeys, object>();
			//dic2a.Add(PathStrategy.ValueKeys.Base, enemy2);
			//dic2a.Add(PathStrategy.ValueKeys.Speed, 100f);
			//dic2a.Add(PathStrategy.ValueKeys.End, new Vector2(300, 150));
			//dic2a.Add(PathStrategy.ValueKeys.Duration, 3f);
			//dic2a.Add(PathStrategy.ValueKeys.Rotation, false);
			//group2a.AddPath(new Path(Paths.Straight, dic2a));

			//enemy2.PathList.AddPath(group2a);

			//PathGroup group2b = new PathGroup();

			//group2b.AddPath(new Path(Paths.Shoot, dicS2));

			//Dictionary<PathStrategy.ValueKeys, Object> dic2b = new Dictionary<PathStrategy.ValueKeys, object>();
			//dic2b.Add(PathStrategy.ValueKeys.Base, enemy2);
			//dic2b.Add(PathStrategy.ValueKeys.Speed, 100f);
			//dic2b.Add(PathStrategy.ValueKeys.End, new Vector2(800, 150));
			//dic2b.Add(PathStrategy.ValueKeys.Duration, 3f);
			//dic2b.Add(PathStrategy.ValueKeys.Rotation, false);
			//group2b.AddPath(new Path(Paths.Straight, dic2b));

			//enemy2.PathList.AddPath(group2b);

			//enemy2.PathList.Mode = ListModes.Repeat;

			// end enemy pathing



			m_SpriteList.Add(back);
			//s  m_SpriteList.Add(back2);
			m_SpriteList.Add(cloud);
			m_SpriteList.Add(enemy);
			m_SpriteList.Add(enemy2);
			m_SpriteList.Add(tail);
			m_SpriteList.Add(m_Player.PlayerShip);
			m_SpriteList.Add(crosshairs);
			foreach (Sprite s in m_TailBodySprites)
				m_SpriteListA.Add(s);


			Sprite TextFpsExample = new FPSSprite(new Vector2(100, 20), Color.Pink);
			m_SpriteList.Add(TextFpsExample);

			SpawnPoint sp = new SpawnPoint(3,1000,"ss",new Vector2(100,100),100,100,TextureLibrary.getGameTexture("virus",""),100,true,0,Depth.MidGround.Mid,Collidable.Factions.Enemy,10000,null,50);
			sp.setShips("bloodcell", new Vector2(100f, 200f), 50, 50, TextureLibrary.getGameTexture("bloodcell", "1"), 255f, true, 0f, Depth.MidGround.Bottom, Collidable.Factions.Enemy, 100, 0, TextureLibrary.getGameTexture("Explosion", "3"), 50);
			sp.Target= m_Player.PlayerShip;
			m_SpriteList.Add(sp);


		}

		public void ChangeFile(String p_FileName)
		{
			m_LReader = new LevelReader(p_FileName);
			m_Distance = 0;
			m_LHandler = new LevelHandler(m_LReader.ReadFile(), this);
		}

		public void AddSprite(Collidable p_Sprite)
		{
			this.m_SpriteList.Add(p_Sprite);
		}

		public void AddAdditiveSprite(Collidable p_Sprite)
		{
			this.m_SpriteListA.Add(p_Sprite);
		}

	}
}
