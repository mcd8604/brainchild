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



	// Something to consider for handling world vs menu:
	// http://blogs.msdn.com/etayrien/archive/2006/12/12/game-engine-structure.aspx



	/// <summary>
	/// This will be for our main game code
	/// </summary>
	public class Game : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		ContentManager content;
		SpriteBatch m_SpriteBatch;

		World m_World;
		Menu m_Menu;

		InputHandlerState m_InputHandler;

		public enum InputHandlerState
		{
			World,
			Menu
		}

		public Game()
		{
			graphics = new GraphicsDeviceManager(this);
			content = new ContentManager(Services);

			// uncap frame rate to see our true performance
			//graphics.SynchronizeWithVerticalRetrace = false;
			//IsFixedTimeStep = false;


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
			Menus.setCurrentMenu(Menus.MenuScreens.DevLogo);
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
				this.Exit();
			}

			//Checks for full screen
			if (InputHandler.IsKeyPressed(Keys.F))
			{
				graphics.ToggleFullScreen();
			}

			//This checks if a new menu is supposed to be loaded.
			if (Menus.HasChanged == true)
			{
				m_Menu = Menus.getCurrentMenu();
				if (m_Menu != null)
				{
					m_Menu.Load(graphics);
					m_InputHandler = InputHandlerState.Menu;
					m_Menu.Visible = true;
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

			if (World.CreateWorld == true)
			{
				m_World = new World();
				m_World.loadLevel(content);
				m_World.initialize(new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
				m_World.changeState(World.GameState.Running);
				World.CreateWorld = false;
			}

			if (World.DestroyWorld == true)
			{
				m_World = null;
				World.DestroyWorld = false;
			}

			if (World.ResumeWorld == true)
			{
				m_World.changeState(World.GameState.Running);
				World.ResumeWorld = false;
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
				else
				{
					m_World.update(gameTime);
				}
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
			graphics.GraphicsDevice.Clear(Color.Black);

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
			Console.WriteLine("ACTIVATED");
		}

		// I am /slightly/ concerned about these events, what happens if we change the state and the menu in the middle of doing something else?
		// Additional Note - This code /did not/ pause the game on my home machine, It was being called, but to no effect.
		protected override void OnDeactivated(object sender, EventArgs args)
		{
			base.OnDeactivated(sender, args);
			Console.WriteLine("DEACTIVATED");
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
