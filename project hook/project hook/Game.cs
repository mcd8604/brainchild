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
	internal sealed class Game : Microsoft.Xna.Framework.Game
	{
		internal static GraphicsDeviceManager graphics;
		private ContentManager content;
		private SpriteBatch m_SpriteBatch;

		private World m_World;
		private Menu m_Menu;

		private InputHandlerState m_InputHandlerState;

#if FINAL
		private System.Drawing.Rectangle DefaultClippingBounds;
#endif

		internal enum InputHandlerState
		{
			World,
			Menu
		}


#if DEBUG
		private static Random random = new Random(0);
#else
		private static Random random = new Random();
#endif
		internal static Random Random
		{
			get
			{
				return random;
			}
		}

		internal static System.IO.TextWriter Out
		{
			get
			{
				return Program.Out;
			}
		}


		internal static HighScore HighScores = new HighScore();


		internal Game()
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
			m_InputHandlerState = InputHandlerState.Menu;

			base.Initialize();

		}


		/// <summary>
		/// Load your graphics content.  If loadAllContent is true, you should
		/// load content from both ResourceManagementMode pools.  Otherwise, just
		/// load ResourceManagementMode.Manual content.
		/// </summary>
		/// <param name="loadAllContent">Which type of content to load.</param>
		protected override void LoadContent()
		{
				TextureLibrary.reloadAll();
				m_SpriteBatch = new SpriteBatch(graphics.GraphicsDevice);
		}


		/// <summary>
		/// Unload your graphics content.  If unloadAllContent is true, you should
		/// unload content from both ResourceManagementMode pools.  Otherwise, just
		/// unload ResourceManagementMode.Manual content.  Manual content will get
		/// Disposed by the GraphicsDevice during a Reset.
		/// </summary>
		/// <param name="unloadAllContent">Which type of content to unload.</param>
		protected override void UnloadContent()
		{
				content.Unload();
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
					m_InputHandlerState = InputHandlerState.Menu;
					m_Menu.Enabled = true;
				}
				else
				{
					m_InputHandlerState = InputHandlerState.World;
				}
			}

			//If a menu is loaded
			if (m_Menu != null)
			{
				if (m_InputHandlerState == InputHandlerState.Menu)
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
				Music.Stop("bg2");
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
				if (m_InputHandlerState == InputHandlerState.World)
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
