#region Using Statements
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace project_hook
{



	// Something to consider for handling world vs menu:
	// http://blogs.msdn.com/etayrien/archive/2006/12/12/game-engine-structure.aspx



	/// <summary>
	/// This will be for our main game code
	/// </summary>
	public class Game : Microsoft.Xna.Framework.Game
	{
		public static GraphicsDeviceManager graphics;
		ContentManager content;
		SpriteBatch m_SpriteBatch;

		World m_World;
		Menu m_Menu;

		InputHandlerState m_InputHandler;

#if FINAL
		System.Drawing.Rectangle DefaultClippingBounds;
#endif

		public enum InputHandlerState
		{
			World,
			Menu
		}


#if DEBUG
		protected static Random random = new Random(0);
#else
		protected static Random random = new Random();
#endif
		public static Random Random
		{
			get
			{
				return random;
			}
		}

#if FINAL
		private const string outfilename = "err.log";
		protected static System.IO.TextWriter writer = new System.IO.StreamWriter(outfilename);
#else
		protected static System.IO.TextWriter writer = Console.Out;
#endif
		public static System.IO.TextWriter Out
		{
			get
			{
				return writer;
			}
		}


		public static HighScore HighScores = new HighScore();


		public Game()
		{
			graphics = new GraphicsDeviceManager(this);
			content = new ContentManager(Services);
#if DEBUG
			// uncap frame rate to see our actual performance
			graphics.SynchronizeWithVerticalRetrace = false;
			IsFixedTimeStep = false;
#endif
#if FINAL
			DefaultClippingBounds = Cursor.Clip;
#endif

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

			graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;
			graphics.PreferredBackBufferHeight = 768;
			graphics.PreferredBackBufferWidth = 1024;
			graphics.ApplyChanges();

			m_SpriteBatch = new SpriteBatch(graphics.GraphicsDevice);
			// This will initialize any libraries or static classes needed
			TextureLibrary.iniTextures(content);

			Menus.ini();
			Menus.setCurrentMenu(Menus.MenuScreens.BrainChildLogo);
			m_InputHandler = InputHandlerState.Menu;

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
				TextureLibrary.reloadAll();
				m_SpriteBatch = new SpriteBatch(graphics.GraphicsDevice);
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

			// Suspend updating (by returning out of the method) if we are not the active window focus.
			if (!IsActive)
				return;


			InputHandler.Update();

			if (Menus.Exit)
			{
#if FINAL
				Out.Flush();
#endif
				Exit();
			}

#if DEBUG
			//Checks for full screen
			if (InputHandler.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.F))
			{
				graphics.ToggleFullScreen();
			}
#endif

			//This checks if a new menu is supposed to be loaded.
			if (Menus.HasChanged)
			{
				m_Menu = Menus.getCurrentMenu();
				if (m_Menu != null)
				{
					m_Menu.Load();
					m_InputHandler = InputHandlerState.Menu;
					m_Menu.Enabled = true;
				}
				else
				{
					m_InputHandler = InputHandlerState.World;
				}
			}

			//If a menu is loaded
			if (m_Menu != null)
			{
				if (m_InputHandler == InputHandlerState.Menu)
				{
					m_Menu.Update(gameTime);
				}
			}

			if (World.CreateWorld)
			{
				World.CreateWorld = false;
				Rectangle r = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
				m_World = new World(r);
				m_World.loadLevel();
				m_World.changeState(World.GameState.Running);
			}

			if (World.RestartLevel)
			{
				m_World.restartLevel();
				World.RestartLevel = false;
			}

			if (World.DestroyWorld)
			{
				if (m_World.State == World.GameState.Won)
				{
					HighScores.addScore(Convert.ToInt32(World.m_Score.Score));
					World.m_Score.reset();
				}
				m_World = null;
				World.DestroyWorld = false;
			}

			if (World.ResumeWorld)
			{
				m_World.changeState(World.GameState.Running);
				World.ResumeWorld = false;
			}

			if (World.PlayerDead)
			{
				HighScores.addScore(Convert.ToInt32(World.m_Score.Score));
				World.m_Score.reset();
				if (Menus.SelectedMenu == Menus.MenuScreens.None)
				{
					Menus.setCurrentMenu(Menus.MenuScreens.GameOver);
				}
			}

			//This will check if the game world is created.  
			if (m_World != null)
			{
				//This checks if the world is supposed to be receiving key input
				//If it's not it will just update the game without key input
				if (m_InputHandler == InputHandlerState.World)
				{
					m_World.checkKeys(gameTime);
				}
				m_World.update(gameTime);
			}

			// TODO: Add your update logic here

			base.Update(gameTime);
		}


		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
#if DEBUG
			graphics.GraphicsDevice.Clear(new Color(62, 227, 57)); // Neon Green
#elif !FINAL
			graphics.GraphicsDevice.Clear(Color.Black);
#endif
#if FINAL
			if (m_Menu != null)
			{
				graphics.GraphicsDevice.Clear(Color.Black);
			}
#endif
			if (m_World != null)
			{
				m_World.draw(m_SpriteBatch);
			}

			if (m_Menu != null)
			{
				m_Menu.Draw(m_SpriteBatch);
			}

			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}

		protected override void OnActivated(object sender, EventArgs args)
		{
			base.OnActivated(sender, args);

#if FINAL
			Cursor.Clip = new System.Drawing.Rectangle(this.Window.ClientBounds.X, this.Window.ClientBounds.Y, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
#endif

#if VERBOSE
			Game.Out.WriteLine("ACTIVATED");
#endif
		}

		// I am /slightly/ concerned about these events, what happens if we change the state and the menu in the middle of doing something else?
		// Additional Note - This code /did not/ pause the game on my home machine, It was being called, but to no effect.
		protected override void OnDeactivated(object sender, EventArgs args)
		{
			base.OnDeactivated(sender, args);
#if FINAL
			Cursor.Clip = DefaultClippingBounds;
#endif
#if VERBOSE
			Game.Out.WriteLine("DEACTIVATED");
#endif
			if (m_World != null)
			{
				if (m_World.State == World.GameState.Running)
				{
					m_World.changeState(World.GameState.Paused);
					Menus.setCurrentMenu(Menus.MenuScreens.Pause);
				}
			}
		}
	}
}
