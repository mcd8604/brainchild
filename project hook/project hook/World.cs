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

		private List<Sprite> m_SpriteList;

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

		public World()
		{

			m_SpriteList = new List<Sprite>();

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

					List<Shot> t_Shots = m_Player.Shoot(p_GameTime);

					foreach (Sprite s in t_Shots)
					{
						if (s.Name.Equals("no_Shot"))
						{
							//used so when the weapon can't shoot
						}
						else
						{
							m_SpriteList.Add(s);
						}
					}

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
					tail.TailAttack(InputHandler.MousePostion, p_GameTime);
				}
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

				//back1.DrawPlayer(gameTime, m_spriteBatch);

				foreach (Sprite s in m_SpriteList)
				{
					if (s.Visible == true)
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
		}

		private void IniDefaults()
		{
			GameTexture cloudTexture = TextureLibrary.getGameTexture("Cloud", "");

			//test scrolling background
			YScrollingBackground back = new YScrollingBackground(TextureLibrary.getGameTexture("veinbg", ""));

			m_Player = new Player("Ship", new Vector2(100.0f, 100.0f), 100, 100, TextureLibrary.getGameTexture("Ship2", "1"), 255f, true, 0.0f, Depth.ForeGround.Bottom, m_ViewPortSize);
			// Sprite back2 = new Sprite("back", new Vector2(100.0f, 100.0f), 500, 600, TextureLibrary.getGameTexture("Back", ""), 100, true, 0.0f, Depth.MidGround.Bottom);
			Sprite cloud = new Sprite("Cloud", new Vector2(0f, 0f), cloudTexture.Height, cloudTexture.Width, cloudTexture, 255f, true, 0, Depth.BackGround.Top);
			Ship enemy = new Ship("Enemy", new Vector2(100f, 200f), 100, 100, TextureLibrary.getGameTexture("Enemy1", ""), 255f, true, 0f, Depth.MidGround.Bottom, Collidable.Factions.Enemy, 100, 0, null, 100, TextureLibrary.getGameTexture("Explosion", "3"), 50);
			ArrayList m_TailBodySprites = new ArrayList();

			crosshairs = new Sprite("crosshair", new Vector2(100f, 100f), TextureLibrary.getGameTexture("crosshairs", "").Height, TextureLibrary.getGameTexture("crosshairs", "").Width, TextureLibrary.getGameTexture("crosshairs", ""), 100f, true, 0f, Depth.MidGround.Mid);
			for (int i = 0; i < 60; i++)
			{
				TailBodySprite tailBodySprite = new TailBodySprite("tailbody", new Vector2(100f, 100f), 20, 20, TextureLibrary.getGameTexture("tailbody", ""), 255, true, 0.0f, Depth.MidGround.Bottom);
				m_TailBodySprites.Add(tailBodySprite);
			}
			tail = new Tail("Tail", m_Player.PlayerShip.Position, TextureLibrary.getGameTexture("temptail", "").Height, TextureLibrary.getGameTexture("temptail", "").Width, TextureLibrary.getGameTexture("temptail", ""), 100f, true, 0f, Depth.ForeGround.Bottom, Collidable.Factions.Player, -1, 0, null, 30, m_Player.PlayerShip, 700, m_TailBodySprites);
			tail.Health = int.MinValue;


			Dictionary<PathStrategy.ValueKeys, Object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
			dic.Add(PathStrategy.ValueKeys.Start, enemy.Center);
			dic.Add(PathStrategy.ValueKeys.End, new Vector2(700, 200));
			dic.Add(PathStrategy.ValueKeys.Duration, 5000.0f);
			dic.Add(PathStrategy.ValueKeys.Base, enemy);
			enemy.Path = new Path(Paths.Line, dic);
			enemy.Update(new GameTime());


			m_SpriteList.Add(back);
			//s  m_SpriteList.Add(back2);
			m_SpriteList.Add(cloud);
			m_SpriteList.Add(enemy);
			m_SpriteList.Add(tail);
			m_SpriteList.Add(m_Player.PlayerShip);
			m_SpriteList.Add(crosshairs);
			foreach (Sprite s in m_TailBodySprites)
				m_SpriteList.Add(s);


			Sprite TextFpsExample = new FPSSprite(new Vector2(100, 20), Color.Pink);
			m_SpriteList.Add(TextFpsExample);


		}
	}
}
