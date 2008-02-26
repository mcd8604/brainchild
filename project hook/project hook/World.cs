using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal sealed class World
	{
		private readonly List<Sprite> m_SpriteList = new List<Sprite>();  // Alpha sprites
		private readonly List<Sprite> m_SpriteListA = new List<Sprite>(); // Additive Sprites;
		private static Boolean primaryRight = true;

		internal static List<Sprite> getAllSprites()
		{
			List<Sprite> l = new List<Sprite>();
			l.AddRange(Instance.m_SpriteList);
			l.AddRange(Instance.m_SpriteListA);
			return l;
		}

		private YScrollingBackground m_Background;

		internal static Player m_Player;
		internal static SimpleScore m_Score;
		private Tail tail;

		private GameState m_PreviousState;
		private GameState m_State = GameState.Nothing;
		internal GameState State
		{
			get
			{
				return m_State;
			}
		}

		internal enum GameState
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

		#region World States
		private static Boolean m_CreateWorld = false;
		internal static Boolean CreateWorld
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

		private static Boolean m_RestartLevel = false;
		internal static Boolean RestartLevel
		{
			get
			{
				return m_RestartLevel;
			}
			set
			{
				m_RestartLevel = value;
			}
		}

		private static Boolean m_destroyWorld;
		internal static Boolean DestroyWorld
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
		internal static Boolean ResumeWorld
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

		internal static Boolean PlayerDead
		{
			get
			{
				if (m_Player != null && m_Player.PlayerShip != null)
				{
					return m_Player.PlayerShip.IsDead();
				}
				else
				{
					return false;
				}
			}
		}
		#endregion

		private static Rectangle m_ViewPortSize;
		internal static readonly WorldPosition Position = new WorldPosition(80f);

		private LevelReader m_LReader;
		private LevelHandler m_LHandler;
		private EnvironmentLoader m_ELoader;

		private static World Instance;

		private static Vector2 target;
		internal static Vector2 getTarget()
		{
			return target;
		}

#if DEBUG
		TextSprite listsize = new TextSprite("", new Vector2(100, 50), Color.LightCyan, Depth.MenuLayer.Text);
		bool DisplayCollision = false;
		int RemovedWhileVisibleCount = 0;
#endif
#if CHEAT
		Shot kill;
#endif
#if TIME
		System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
		TextSprite coll = new TextSprite("", new Vector2(300, 20), Color.GreenYellow, Depth.MenuLayer.Text);
		TextSprite updt = new TextSprite("", new Vector2(300, 50), Color.SeaShell, Depth.MenuLayer.Text);
		double colltotal;
		int collcount;
		double updttotal;
		int updtcount;
#endif

		internal World(Rectangle p_DrawArea)
		{
			Instance = this;
			m_ViewPortSize = p_DrawArea;

			LoadTextures();
			IniDefaults();

			Music.Initialize();
			Sound.Initialize();
			Music.Play("bg2");

			Position.resetDistance();

			m_ELoader = new EnvironmentLoader();
			AddSprite(m_ELoader.Environment);

			AddSprite(new BloodCellGenerator(4));
		}

		//This method will load the level
		//This will load the level defintion into memory
		internal void loadLevel()
		{
#if DEBUG
			string[] levels = System.IO.Directory.GetFiles(System.Environment.CurrentDirectory + "\\Content\\Levels", "*.xml");
			for (int i = 0; i < levels.Length; i++)
			{
				levels[i] = levels[i].Substring(levels[i].LastIndexOf("\\") + 1);
			}
			LevelForm f = new LevelForm(levels);
			System.Windows.Forms.DialogResult result = f.ShowDialog();
			if (result != System.Windows.Forms.DialogResult.OK)
			{
				Environment.Exit(Environment.ExitCode);
				
			}
			m_LReader = new LevelReader(f.getLevel());
#else
			m_LReader = new LevelReader("Level1Normal.xml");
#endif
			m_LHandler = new LevelHandler(m_LReader.ReadFile(), this);
		}

		internal void restartLevel()
		{
			m_SpriteList.Clear();
			m_SpriteListA.Clear();
			Position.resetDistance();
			Position.setSpeed(1);
			m_ELoader.resetLevel();
			AddSprite(m_ELoader.Environment);
			m_LReader = new LevelReader(m_LReader.FileName);
			m_LHandler = new LevelHandler(m_LReader.ReadFile(), this);
			IniDefaults();
			AddSprite(new BloodCellGenerator(4));
		}

		internal static void togglePrimaryRight()
		{
			primaryRight = !primaryRight;
		}

		internal static Boolean getPrimaryRight()
		{
			return primaryRight;
		}
		//This will deallocate any variables that need de allocation
		internal void unload()
		{
			//m_World = null;
		}

		// private to update method
		private List<Sprite> toAdd = new List<Sprite>();
		private List<Sprite> toAddA = new List<Sprite>();

		//This will update the game world.  
		//Different update methdos can be run based on the game state.
		internal void update(GameTime p_GameTime)
		{
			//This will be for normal everyday update operations.  
			if (m_State == GameState.Running)
			{
				m_LHandler.CheckEvents(Position.Distance);

				Position.Update(p_GameTime);

				m_Player.UpdatePlayer(p_GameTime);

#if TIME
				timer.Reset();
				timer.Start();
#endif
				Collision.CheckCollisions(m_SpriteList, m_SpriteListA);
#if TIME
				timer.Stop();
				colltotal += timer.Elapsed.TotalMilliseconds;
				collcount++;
				if (collcount >= 50)
				{
					coll.Text = "C: " + (colltotal / collcount).ToString();
					colltotal = 0;
					collcount = 0;
				}

				timer.Reset();
				timer.Start();
#endif

#if DEBUG
				int count = m_SpriteList.Count;

				List<Sprite> removed = m_SpriteList.FindAll(Sprite.isToBeRemoved);
				if (removed.Count > 0)
				{
					foreach (Sprite s in removed)
					{
#if VERBOSE
						Game.Out.WriteLine("Removing: " + s);
#endif
						if (isSpriteVisible(s))
						{
							Game.Out.WriteLine("WARNING! " + s.Name + " was removed while it was still active and visible! This is #" + ++RemovedWhileVisibleCount);
						}
					}
				}
#endif
				m_SpriteList.RemoveAll(Sprite.isToBeRemoved);
#if DEBUG && VERBOSE
				int diff = (count - m_SpriteList.Count);
				if (diff != 0)
				{
					Game.Out.WriteLine("Removed " + diff + " AlphaBlended Sprites");
				}
#endif
				foreach (Sprite s in m_SpriteList)
				{
					s.Update(p_GameTime);

					checkToBeAdded(s);
				}

				//Additive:

#if DEBUG
				count = m_SpriteListA.Count;
				List<Sprite> removedA = m_SpriteListA.FindAll(Sprite.isToBeRemoved);
				if (removedA.Count > 0)
				{
					foreach (Sprite s in removedA)
					{
#if VERBOSE
						Game.Out.WriteLine("Removing: " + s);
#endif
						if (isSpriteVisible(s))
						{
							Game.Out.WriteLine("WARNING! " + s.Name + " was removed while it was still active and visible! This is #" + ++RemovedWhileVisibleCount);
						}
					}
				}
#endif
				m_SpriteListA.RemoveAll(Sprite.isToBeRemoved);
#if VERBOSE
				diff = (count - m_SpriteListA.Count);
				if (diff != 0)
				{
					Game.Out.WriteLine("Removed " + diff + " Additive Sprites");
				}
#endif
				foreach (Sprite s in m_SpriteListA)
				{
					s.Update(p_GameTime);

					checkToBeAdded(s);
				}

#if VERBOSE
				if (toAdd.Count != 0)
				{
					Game.Out.WriteLine("Added " + toAdd.Count + " AlphaBlended Sprites");
				}
#endif
				AddSprites(toAdd);
				toAdd.Clear();


#if VERBOSE
				if (toAddA.Count != 0)
				{
					Game.Out.WriteLine("Added " + toAddA.Count + " Additive Sprites");
				}
#endif
				AddSprites(toAddA);
				toAddA.Clear();


#if TIME
				timer.Stop();
				updttotal += timer.Elapsed.TotalMilliseconds;
				updtcount++;
				if (updtcount >= 50)
				{
					updt.Text = "U: " + (updttotal / updtcount).ToString();
					updttotal = 0;
					updtcount = 0;
				}
#endif


#if DEBUG
				if (DisplayCollision)
				{
					Collision.DevEnableCollisionDisplay(m_SpriteList, m_SpriteListA);
				}
#endif

#if DEBUG
				listsize.Text = "S: " + m_SpriteList.Count + " A: " + m_SpriteListA.Count.ToString();
#endif

				if (m_Player.PlayerShip.Center.X < m_ViewPortSize.X ||
					m_Player.PlayerShip.Center.Y < m_ViewPortSize.Y ||
					m_Player.PlayerShip.Center.X > m_ViewPortSize.Width ||
					m_Player.PlayerShip.Center.Y > m_ViewPortSize.Height)
				{
					m_Player.PlayerShip.Health = 0;
				}

			}
		}

		private void checkToBeAdded(Sprite s)
		{
			if (s.SpritesToBeAdded != null && s.SpritesToBeAdded.Count > 0)
			{
				foreach (Sprite z in s.SpritesToBeAdded)
				{
					if (z.BlendMode == SpriteBlendMode.AlphaBlend)
					{
						toAdd.Add(z);
					}
					else if (z.BlendMode == SpriteBlendMode.Additive)
					{
						toAddA.Add(z);
					}
				}
				s.SpritesToBeAdded.Clear();
			}

			if (s.Parts != null && s.Parts.Count > 0)
			{
				foreach (Sprite z in s.Parts)
				{
					checkToBeAdded(z);
				}
			}
		}

		internal void checkKeys(GameTime p_GameTime)
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
				if (InputHandler.IsActionPressed(Actions.TailPrimary))
				{
					tail.TailAttack();
				}
				if (InputHandler.IsActionDown(Actions.TailSecondary))
				{
					if (primaryRight)
					{
						m_Player.Shoot();
					}
					if (tail.EnemyCaught != null)
					{
						tail.EnemyShoot();
					}
				}
				if (InputHandler.HasMouseMoved())
				{
					target = InputHandler.MousePosition;
				}
#if XBOX360
				else if (InputHandler.HasRightStickMoved())
				{
					target = tail.Center + Vector2.Multiply(InputHandler.RightStickPosition, 400);
				}
#endif
#if DEBUG
				if(InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.J))
				{
					tail.Position = new Vector2(tail.Position.X,/*Game.graphics.GraphicsDevice.Viewport.Y + 10*/ 1000);
				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.M))
				{
					if (Music.IsPlaying("bg2"))
					{
						Music.Stop("bg2");
					}
					else
					{
						Music.Play("bg2");
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
						Collision.DevDisableCollisionDisplay(m_SpriteList, m_SpriteListA);
					}
				}

				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.End))
				{
					int highest = 0;
					foreach ( int i in m_LHandler.Events.Keys ) {
						if (i > highest) {
							highest = i;
						}
					}
					Position.Distance = highest;
				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Home))
				{
					Position.setSpeed(80);
				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.PageUp))
				{
					Position.setSpeed(Position.Speed * 2f);
				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.PageDown))
				{
					Position.setSpeed(Position.Speed * 0.5f);
				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Scroll))
				{
					Position.setSpeed(0);
				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.P))
				{
					Game.Out.WriteLine(Position.Distance.ToString() + ", " + (Position.Distance / (float)EnvironmentLoader.TileDimension));
				}
#if CHEAT
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.U))
				{
					//PowerUp p = new PowerUp(50, 50, PowerUp.PowerType.Weapon, m_Player.PlayerShip.Center);
					//m_SpriteList.Add(p);
					PowerUp.DisplayPowerUp(50, 50, m_Player.PlayerShip.Center,PowerUp.PowerType.Weapon);
				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.G))
				{
					if (float.IsNaN(m_Player.PlayerShip.MaxHealth))
					{
						m_Player.PlayerShip.MaxHealth = 100;
					}
					else
					{
						m_Player.PlayerShip.MaxHealth = float.NaN;
					}
				}
				//full heal
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.L))
				{
					m_Player.PlayerShip.Health = m_Player.PlayerShip.MaxHealth;
					m_Player.PlayerShip.Shield = m_Player.PlayerShip.MaxShield;

				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.K))
				{
						m_Player.PlayerShip.Health = 0;
				}
				if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Z))
				{
					if (kill == null)
					{
						kill = new Shot("Soul of Zinglon", m_Player.PlayerShip.Center, 250, 4000, TextureLibrary.getGameTexture("Shot"), 0.75f, true, -MathHelper.PiOver2, Depth.GameLayer.Shot, Collidable.Factions.Player, 0, 0, 10000);
						kill.BlendMode = SpriteBlendMode.Additive;
						kill.Bound = Collidable.Boundings.Rectangle;
						kill.DestroyedOnCollision = false;
						TaskParallel task = new TaskParallel();
						task.addTask(new TaskAttach(m_Player.PlayerShip));
						task.addTask(new TaskRepeatingTimer(2f));
						kill.Task = task;
						kill.m_Ship = m_Player.PlayerShip;
						AddSprite(kill);
					}
					else if (kill.Enabled == false)
					{
						kill.Enabled = true;
					}

				}
				if (InputHandler.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.T))
				{
					Thrown T = new Thrown(new Collidable("GeneratedBloodCell", tail.Center, 50, 50, TextureLibrary.getGameTexture("bloodcell", 1), 1f, true, -MathHelper.PiOver2, Depth.BackGroundLayer.Blood, Collidable.Factions.Player, 100, 25));
					T.Center = tail.Center;
					T.setAnimation("bloodcell", 60);
					T.Animation.StartAnimation();
					float releaseAngle = (float)Math.Atan2(InputHandler.MousePosition.Y - tail.Center.Y, InputHandler.MousePosition.X - tail.Center.X);
					TaskSequence task = new TaskSequence();
					TaskParallel temp = new TaskParallel();
					temp.addTask(new TaskStraightAngle(releaseAngle, 600f));
					temp.addTask(new TaskStationary());
					temp.addTask(new TaskRotateToAngle(releaseAngle));
					temp.addTask(new TaskWaitFor(isSpriteNotVisible));
					task.addTask(temp);
					task.addTask(new TaskRemove());
					T.Task = task;
					AddSprite(T);
				}
#endif
#endif
			}

		}

		//This method will be called to change the games state.
		//This method will determine what actions need to be executed
		//to change to the new state.
		internal void changeState(GameState p_State)
		{
			m_PreviousState = m_State;
			m_State = p_State;
		}

		//This will draw the 
		internal void draw(SpriteBatch p_SpriteBatch)
		{
			if (!(m_State == GameState.DoNotRender))
			{
				p_SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);
				foreach (Sprite s in m_SpriteList)
				{
					if (s.Enabled)
					{
						s.Draw(p_SpriteBatch);
					}
				}
				p_SpriteBatch.End();

				p_SpriteBatch.Begin(SpriteBlendMode.Additive, SpriteSortMode.BackToFront, SaveStateMode.None);
				foreach (Sprite s in m_SpriteListA)
				{
					if (s.Enabled)
					{
						s.Draw(p_SpriteBatch);
					}
				}
				p_SpriteBatch.End();
			}

		}

		//This method will load textures
		internal void LoadTextures()
		{
			//TextureLibrary.LoadTexture("blood");
			TextureLibrary.LoadTexture("bloodcell");
			TextureLibrary.LoadTexture("bump");
			TextureLibrary.LoadTexture("cell");
			TextureLibrary.LoadTexture("crosshairs");
			TextureLibrary.LoadTexture("crosshairsR");
			TextureLibrary.LoadTexture("DNA");
			TextureLibrary.LoadTexture("Enemy1");
			TextureLibrary.LoadTexture("energyball");
			TextureLibrary.LoadTexture("energyballpld");
			TextureLibrary.LoadTexture("Explosion");
			TextureLibrary.LoadTexture("Explosion2");
			TextureLibrary.LoadTexture("ExplosionBig");
			//TextureLibrary.LoadTexture("FireBall");
			TextureLibrary.LoadTexture("gas");
			TextureLibrary.LoadTexture("gate");
			TextureLibrary.LoadTexture("hudPanel");
			TextureLibrary.LoadTexture("poisonsplat");
			TextureLibrary.LoadTexture("Shield");
			//TextureLibrary.LoadTexture("Ship2");
			TextureLibrary.LoadTexture("Shot");
			TextureLibrary.LoadTexture("shot_electric");
			TextureLibrary.LoadTexture("shot_energy");
			TextureLibrary.LoadTexture("shot_greenball");
			TextureLibrary.LoadTexture("spitter_body");
			TextureLibrary.LoadTexture("spitter_mouth");
			TextureLibrary.LoadTexture("tail_segment");
			TextureLibrary.LoadTexture("tailbody");
			TextureLibrary.LoadTexture("temptail");
			TextureLibrary.LoadTexture("trigger");
			TextureLibrary.LoadTexture("veinbg");
			TextureLibrary.LoadTexture("virus1");
			//TextureLibrary.LoadTexture("wall_flat");
			//TextureLibrary.LoadTexture("wall_rand1");
			//TextureLibrary.LoadTexture("wall_rand2");
			//TextureLibrary.LoadTexture("wall_rand3");
			TextureLibrary.LoadTexture("walls\\plaque_clear");
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

			TextureLibrary.LoadTexture("shieldBar");
			TextureLibrary.LoadTexture("healthBar");
			TextureLibrary.LoadTexture("black");
			TextureLibrary.LoadTexture("cross");
			TextureLibrary.LoadTexture("WeaponBar");
			TextureLibrary.LoadTexture("wing");
			TextureLibrary.LoadTexture("turret");
			TextureLibrary.LoadTexture("turretBase");
			TextureLibrary.LoadTexture("turretNeck");
			TextureLibrary.LoadTexture("turretHead");
			TextureLibrary.LoadTexture("supernova");

#if DEBUG
			TextureLibrary.LoadTexture("debugcirc");
			TextureLibrary.LoadTexture("debugdiamond");
			TextureLibrary.LoadTexture("debugsquare");
			TextureLibrary.LoadTexture("debugrect");
#endif
		}

		private void IniDefaults()
		{
			IniBackground();

			IniPlayer();

			IniHUD();

			AddSprites(PowerUp.iniPowerups());
		}

		internal void IniBackground()
		{
			if (m_Background == null)
			{
				m_Background = new YScrollingBackground(TextureLibrary.getGameTexture("veinbg"), Position, m_ViewPortSize);

			}
			AddSprite(m_Background);
		}

		internal void IniPlayer()
		{
			if (m_Player == null)
			{
				m_Player = new Player("THE_PLAYER", Vector2.Zero, 48, 64, TextureLibrary.getGameTexture("wing", 0), 255f, true, 0.0f, Depth.GameLayer.PlayerShip, m_ViewPortSize);
			}
			else
			{
				m_Player.reset();
			}
			m_Player.PlayerShip.Center = new Vector2(512f, 576f);
			AddSprite(m_Player.PlayerShip);

			ICollection<Sprite> m_TailBodySprites = new List<Sprite>();

			for (int i = 0; i < 60; i++)
			{
				if (i % 5 == 0)
				{
					Sprite tailBodySprite = new Sprite(
#if !FINAL
						"tail_segment",
#endif
Vector2.Zero, 20, 20, TextureLibrary.getGameTexture("tail_segment"), 64, true, 0.0f, Depth.GameLayer.TailBody);
					tailBodySprite.Center = m_Player.PlayerShip.Center;
					tailBodySprite.Transparency = 0.5f;
					tailBodySprite.BlendMode = SpriteBlendMode.Additive;
					tailBodySprite.setAnimation("Explosion", 30);
					tailBodySprite.Animation.StartAnimation();

					tailBodySprite.Animation.CurrentFrame = i % 30;
					m_TailBodySprites.Add(tailBodySprite);
				}
				else
				{
					Sprite tailBodySprite = new Sprite(
#if !FINAL
						"shot_energy",
#endif
Vector2.Zero, 10, 10, TextureLibrary.getGameTexture("shot_energy"), 64, true, 0.0f, Depth.GameLayer.TailBody);
					tailBodySprite.Center = m_Player.PlayerShip.Center;
					tailBodySprite.Transparency = 0.2f;
					tailBodySprite.BlendMode = SpriteBlendMode.Additive;
					tailBodySprite.setAnimation("energyball", 30);
					tailBodySprite.Animation.CurrentFrame = i % 30;
					tailBodySprite.Animation.StartAnimation();
					m_TailBodySprites.Add(tailBodySprite);
				}

			}

			GameTexture cursorTexture = TextureLibrary.getGameTexture("crosshairs");
			Sprite crosshairs = new Sprite(
#if !FINAL
				"crosshair",
#endif
Vector2.Zero, cursorTexture.Height, cursorTexture.Width, cursorTexture, 1f, true, 0f, Depth.GameLayer.Cursor);
			crosshairs.Task = new TaskAttachTo(World.getTarget);
			AddSprite(crosshairs);

			AddSprites(m_TailBodySprites);
			GameTexture temp = TextureLibrary.getGameTexture("temptail");
			tail = new Tail(
#if !FINAL
				"Tail",
#endif
Vector2.Zero, temp.Height, temp.Width, temp, 100f, true, 0f, Depth.GameLayer.Tail, Collidable.Factions.Player, float.NaN, 25, m_Player.PlayerShip, 1, m_TailBodySprites, crosshairs);
			tail.Center = m_Player.PlayerShip.Center;
			AddSprite(tail);

			m_Player.PlayerShip.TailSprite = tail;
		}

		internal void IniHUD()
		{
#if !FINAL
			Sprite hudPanel = new Sprite("hudPanel", Vector2.Zero, 64, Game.graphics.GraphicsDevice.Viewport.Width, TextureLibrary.getGameTexture("hudPanel"), 0.5f, true, 0f, Depth.MenuLayer.Background);
			AddSprite(hudPanel);

			Sprite TextFpsExample = new FPSSprite(new Vector2(75, 20), Color.Pink, Depth.MenuLayer.Text);
			AddSprite(TextFpsExample);
#if DEBUG
			AddSprite(listsize);
#if TIME
			AddSprite(coll);
			AddSprite(updt);
#endif
#endif
#endif
			m_Score = new SimpleScore();
#if FINAL
			Sprite score = new TextSprite(m_Score.ScoreString, new Vector2(820, 700), Color.Black, Depth.HUDLayer.Bar);
#else
			Sprite score = new TextSprite(m_Score.ScoreString, new Vector2(820, 0), Color.LightBlue, Depth.MenuLayer.Text);
#endif
			AddSprite(score);

#if DEBUG
			Sprite P = new TextSprite(m_Player.PlayerShip.GetHealthString, new Vector2(420, 0), Color.LightSalmon, Depth.MenuLayer.Text);
			AddSprite(P);

			Sprite u = new TextSprite(m_Player.PlayerShip.GetUpgradeLevelString, new Vector2(450, 25), Color.Beige, Depth.MenuLayer.Text);
			AddSprite(u);
#endif
			//PowerUp p = new PowerUp(50, 12, new Vector2(50, 50));
			//AddSprite(p);

			AddSprite(new HealthBar(m_Player.PlayerShip, new Vector2(50, 700), 75, 10, 55, 75));

			//AddSprite(new WeaponUpgradeBar(m_Player.PlayerShip,new Vector2(100,740),100,20));
			//SpawnPoint sp = new SpawnPoint(3,1000,"ss",new Vector2(100,100),100,100,TextureLibrary.getGameTexture("virus",""),100,true,0,Depth.GameLayer.Ships,Collidable.Factions.Enemy,10000,null,50);
			//sp.setShips("bloodcell", new Vector2(100f, 200f), 50, 50, TextureLibrary.getGameTexture("bloodcell", "1"), 255f, true, 0f, Depth.GameLayer.Ships, Collidable.Factions.Enemy, 100, 0, TextureLibrary.getGameTexture("Explosion", "3"), 50);
			//sp.Target= m_Player.PlayerShip;
			//AddSprite(sp);
		}

		internal void ChangeFile(String p_FileName)
		{
			m_LReader = new LevelReader(p_FileName);
			Position.resetDistance();
			m_LHandler = new LevelHandler(m_LReader.ReadFile(), this);
		}

		internal void AddSprite(Sprite p_Sprite)
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
		internal void AddSprites(IEnumerable<Sprite> p_Sprites)
		{
			foreach (Sprite s in p_Sprites)
			{
				AddSprite(s);
			}
		}

		internal void LoadBMP(String p_FileName)
		{
			m_ELoader.NewFile(System.Environment.CurrentDirectory + "\\Content\\Levels\\" + p_FileName);
			m_ELoader.resetLevelIfEmpty();
		}
		internal void PleaseLoadBMP(String p_FileName)
		{
			m_ELoader.PleaseLoadNextFile(System.Environment.CurrentDirectory + "\\Content\\Levels\\" + p_FileName);
		}

		internal void ChangeSpeed(int p_Speed)
		{
			Position.setSpeed(p_Speed);
		}

		/// <summary>
		/// Returns true if the sprite is currently visible on the screen.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		internal static bool isSpriteVisible(Sprite s)
		{
			return (s.Enabled && (s.Center.X + s.Width) > m_ViewPortSize.X && (s.Center.X - s.Width) < m_ViewPortSize.Width && (s.Center.Y + s.Height) > m_ViewPortSize.Y && (s.Center.Y - s.Height) < m_ViewPortSize.Height);
		}
		internal static bool isSpriteNotVisible(Sprite s)
		{
			return !isSpriteVisible(s);
		}

	}
}
