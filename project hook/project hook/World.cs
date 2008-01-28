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

		private static Boolean m_PlayerDead;
		public static Boolean PlayerDead
		{
			get
			{
				return m_PlayerDead;
			}
			set
			{
				m_PlayerDead = value;
			}
		}

		public static Player m_Player;
		public static SimpleScore m_Score;
		Tail tail;
		Sprite crosshairs;
		YScrollingBackground back;

		GameState m_PreviousState;
		GameState m_State = GameState.Nothing;
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

		//public float m_Distance = 0;
		//private int m_Speed = 80;
		//public int Speed
		//{
		//    get
		//    {
		//        return m_Speed;
		//    }
		//    set
		//    {
		//        m_Speed = value;
		//        back.ScrollSpeed = m_Speed;
		//    }
		//}

		private WorldPosition m_Position;
		public WorldPosition Position
		{
			get { return m_Position; }
		}

		LevelReader m_LReader;
		LevelHandler m_LHandler;
		EnvironmentLoader m_ELoader=new EnvironmentLoader();


		Random m_RanX = new Random();

#if DEBUG
		System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
		TextSprite coll = new TextSprite("", new Vector2(100, 50), Color.GreenYellow);
		double colltotal;
		int collcount;
		bool DisplayCollision = false;
#endif

		public World() {}

		//This method will initialize all the objects needed to run the game
		public void initialize(Rectangle p_DrawArea)
		{
			m_SpriteList = new List<Sprite>();
			m_SpriteListA = new List<Sprite>();
			m_Position = new WorldPosition( 80f );
			m_ViewPortSize = p_DrawArea;
			IniDefaults();
			Music.Initialize();
			Sound.Initialize();
			this.m_LReader = new LevelReader("LevelTest.xml");
			this.m_LHandler = new LevelHandler(m_LReader.ReadFile(), this);
			AddSprites(this.m_ELoader.Initialize(m_Position, System.Environment.CurrentDirectory + "\\Content\\Levels\\testBMP.bmp"));
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
				m_Position.Update(p_GameTime);

				m_ELoader.Update(p_GameTime);

				m_LHandler.CheckEvents(m_Position.Distance);

				m_Player.UpdatePlayer(p_GameTime);

				this.CreateBloodCell();

#if DEBUG
				timer.Reset();
				timer.Start();
#endif
				Collision.CheckCollisions(m_SpriteList);
#if DEBUG
				timer.Stop();
				colltotal += timer.Elapsed.TotalMilliseconds;
				collcount++;
				if (collcount >= 50)
				{
					coll.Text = (colltotal / collcount).ToString();
					colltotal = 0;
					collcount = 0;
				}
#endif

				List<Sprite> toAdd = new List<Sprite>();
#if DEBUG
				int count = m_SpriteList.Count;
#endif
				m_SpriteList.RemoveAll(Sprite.isToBeRemoved);
#if DEBUG
				int diff = (count - m_SpriteList.Count);
				if (diff != 0)
				{
					Console.WriteLine("Removed " + diff + " AlphaBlended Sprites");
				}
#endif
				foreach (Sprite s in m_SpriteList)
				{
					s.Update(p_GameTime);

					if (s.SpritesToBeAdded != null)
					{
						toAdd.AddRange(s.SpritesToBeAdded);
						s.SpritesToBeAdded.Clear();
					}
				}
#if DEBUG
				if (toAdd.Count != 0)
				{
					Console.WriteLine("Added " + toAdd.Count + " AlphaBlended Sprites");
				}
#endif
				AddSprites(toAdd);
				
				//Additive:

				toAdd.Clear();
#if DEBUG
				count = m_SpriteListA.Count;
#endif
				m_SpriteListA.RemoveAll(Sprite.isToBeRemoved);
#if DEBUG
				diff = (count - m_SpriteListA.Count);
				if (diff != 0)
				{
					Console.WriteLine("Removed " + diff + " Additive Sprites");
				}
#endif
				foreach (Sprite s in m_SpriteListA)
				{
					s.Update(p_GameTime);

					if (s.SpritesToBeAdded != null)
					{
						toAdd.AddRange(s.SpritesToBeAdded);
						s.SpritesToBeAdded.Clear();
					}
				}
#if DEBUG
				if (toAdd.Count != 0)
				{
					Console.WriteLine("Added " + toAdd.Count + " Additive Sprites");
				}
#endif
				AddSprites(toAdd);

#if DEBUG
				if (DisplayCollision) {
					Collision.DevEnableCollisionDisplay(m_SpriteList);
				}
#endif
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
					tail.TailAttack();
				}
				if (InputHandler.IsActionDown(Actions.TailSecondary))
				{
					if (tail.EnemyCaught != null)
					{
						//tail.EnemyCaught.shoot(InputHandler.MousePostion);
						tail.EnemyShoot();
					}
				}
#if DEBUG
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
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.C))
				{
					if (!DisplayCollision)
					{
						DisplayCollision = true;
					}
					else
					{
						DisplayCollision = false;
						Collision.DevDisableCollisionDisplay(m_SpriteList);
					}
				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.PageUp))
				{
					m_Position.setSpeed(m_Position.Speed * 2f);
				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.PageDown))
				{
					m_Position.setSpeed(m_Position.Speed * 0.5f);
				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.End))
				{
					m_Position.setSpeed(m_Position.Speed * 200f);
				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Home))
				{
					m_Position.setSpeed(0);
				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.B))
				{
					String Breakpoint = this.ToString();
				}
#endif
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
            TextureLibrary.LoadTexture("energy");
            TextureLibrary.LoadTexture("Shot");
            TextureLibrary.LoadTexture("shot_electric");
            TextureLibrary.LoadTexture("plaque");
			TextureLibrary.LoadTexture("wall_flat");
            TextureLibrary.LoadTexture("Explosion2");
			TextureLibrary.LoadTexture("poisonsplat");
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
		}

		private void IniDefaults()
		{
			GameTexture cloudTexture = TextureLibrary.getGameTexture("Cloud", "");
			
			//test scrolling background
			back = new YScrollingBackground(TextureLibrary.getGameTexture("veinbg", ""), m_Position);

			m_Player = new Player("Ship", new Vector2(400.0f, 500.0f), 100, 100, TextureLibrary.getGameTexture("Ship2", "1"), 255f, true, 0.0f, Depth.GameLayer.PlayerShip, m_ViewPortSize);
			// Sprite back2 = new Sprite("back", new Vector2(100.0f, 100.0f), 500, 600, TextureLibrary.getGameTexture("Back", ""), 100, true, 0.0f, Depth.MidGround.Bottom);
			Sprite cloud = new Sprite("Cloud", new Vector2(0f, 0f), cloudTexture.Height, cloudTexture.Width, cloudTexture, 0.8f, true, 0, Depth.BackGroundLayer.Upper);
			
			ICollection<Sprite> m_TailBodySprites = new List<Sprite>();

			crosshairs = new Sprite("crosshair", new Vector2(100f, 100f), TextureLibrary.getGameTexture("crosshairs", "").Height, TextureLibrary.getGameTexture("crosshairs", "").Width, TextureLibrary.getGameTexture("crosshairs", ""), 100f, true, 0f, Depth.GameLayer.Cursor);
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
			tail = new Tail("Tail", m_Player.PlayerShip.Position, TextureLibrary.getGameTexture("temptail", "").Height, TextureLibrary.getGameTexture("temptail", "").Width, TextureLibrary.getGameTexture("temptail", ""), 100f, true, 0f, Depth.GameLayer.Tail, Collidable.Factions.Player, float.NaN, null, 25, m_Player.PlayerShip, 1, m_TailBodySprites);
			tail.m_TargetObject = crosshairs;

			AddSprite(back);
			AddSprite(cloud);
			AddSprite(tail);
			AddSprite(m_Player.PlayerShip);
			AddSprite(crosshairs);
			AddSprites(m_TailBodySprites);

			Sprite TextFpsExample = new FPSSprite(new Vector2(100, 20), Color.Pink);
			AddSprite(TextFpsExample);

			AddSprite(coll);

			m_Score = new SimpleScore();
			Sprite score = new TextSprite(m_Score.ToString, new Vector2(800, 0), Color.LightBlue);
			AddSprite(score);

			//SpawnPoint sp = new SpawnPoint(3,1000,"ss",new Vector2(100,100),100,100,TextureLibrary.getGameTexture("virus",""),100,true,0,Depth.GameLayer.Ships,Collidable.Factions.Enemy,10000,null,50);
			//sp.setShips("bloodcell", new Vector2(100f, 200f), 50, 50, TextureLibrary.getGameTexture("bloodcell", "1"), 255f, true, 0f, Depth.GameLayer.Ships, Collidable.Factions.Enemy, 100, 0, TextureLibrary.getGameTexture("Explosion", "3"), 50);
			//sp.Target= m_Player.PlayerShip;
			//AddSprite(sp);

		}

		public void ChangeFile(String p_FileName)
		{
			m_LReader = new LevelReader(p_FileName);
			m_Position = new WorldPosition();
			m_LHandler = new LevelHandler(m_LReader.ReadFile(), this);
		}

		public void AddSprite(Sprite p_Sprite)
		{
			if (p_Sprite.BlendMode == SpriteBlendMode.AlphaBlend)
			{
				m_SpriteList.Add(p_Sprite);
			}
			else if (p_Sprite.BlendMode == SpriteBlendMode.Additive)
			{
				m_SpriteListA.Add(p_Sprite);
			}
		}
		public void AddSprites(IEnumerable<Sprite> p_Sprites)
		{
			foreach (Sprite s in p_Sprites)
			{
				AddSprite(s);
			}
		}

		public void CreateBloodCell()
		{
			if (m_RanX.Next(0, 500) == 5)
			{
				//"bloodcell", new Vector2(100f, 200f), 50, 50, TextureLibrary.getGameTexture("bloodcell", "1"), 255f, true, 0f, Depth.GameLayer.Ships, Collidable.Factions.Enemy, 100, 0, TextureLibrary.getGameTexture("Explosion", "3"), 50);
				//new Collidable(
				Collidable t_Blood = new Collidable("BloodCell", new Vector2(m_RanX.Next(100, 800), 0), 50, 50,
										TextureLibrary.getGameTexture("bloodcell", "1"), 1f, true, 0f, Depth.BackGroundLayer.Upper,
										Collidable.Factions.Blood, 45, TextureLibrary.getGameTexture("Explosion", "3"), 25);
				t_Blood.Task = new TaskStraightVelocity(new Vector2(0, 100));
				t_Blood.setAnimation("bloodcell", 60);
				t_Blood.Animation.StartAnimation();
				m_SpriteList.Add(t_Blood);
			}
		}
	}
}
