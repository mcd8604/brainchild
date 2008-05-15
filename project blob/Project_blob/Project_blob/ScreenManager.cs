using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Collections;
using Project_blob.GameState;
using System.Text;

using Engine;
using Audio;

namespace Project_blob
{
	public class ScreenManager : Game
	{
		public GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		List<GameScreen> screens = new List<GameScreen>();
		List<GameScreen> screensToUpdate = new List<GameScreen>();

		SpriteFont font;
		Texture2D blankTexture;

		bool isInitialized;

		bool traceEnabled;

		public SpriteBatch SpriteBatch
		{
			get { return spriteBatch; }
		}

		public SpriteFont Font
		{
			get { return font; }
		}

		// And what does this do, exactly?
		public bool TraceEnabled
		{
			get { return traceEnabled; }
			set { traceEnabled = value; }
		}

		public ScreenManager()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferMultiSampling = true;
			Content.RootDirectory = "Content";

			//graphics.SynchronizeWithVerticalRetrace = false;
			//IsFixedTimeStep = false;

			AddScreen(new MainMenuScreen());
		}

		protected override void Initialize()
		{
			// Sets up audio
			AudioManager.initialize();

			InputHandler.LoadDefaultBindings();

			GameScreen.ScreenManager = this;

			GraphicsDevice.RenderState.PointSize = 5;
			GraphicsDevice.PresentationParameters.MultiSampleType = MultiSampleType.SixteenSamples;
			GraphicsDevice.PresentationParameters.MultiSampleQuality = 8;
			GraphicsDevice.RenderState.MultiSampleAntiAlias = true;
			GraphicsDevice.RenderState.Wrap0 = TextureWrapCoordinates.Three;

			CurrentResolution = new Resolution(GraphicsDevice.Viewport.Height, GraphicsDevice.Viewport.Width);
			Resolutions.Add(CurrentResolution);

			foreach (DisplayMode d in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
			{
				addResolution(d);
			}

			Resolutions.Sort(Resolution.comparison);

			graphics.PreferredBackBufferHeight = CurrentResolution.Height;
			graphics.PreferredBackBufferWidth = CurrentResolution.Width;

			graphics.ApplyChanges();

			base.Initialize();

			isInitialized = true;
		}

		protected override void LoadContent()
		{
			// Load content belonging to the screen manager.
			//ContentManager content = Game.Content;

			spriteBatch = new SpriteBatch(GraphicsDevice);
			font = Content.Load<SpriteFont>(@"Fonts\\Courier New");
			blankTexture = Content.Load<Texture2D>("blank");

			// Tell each of the screens to load their content.
			foreach (GameScreen screen in screens)
			{
				screen.LoadContent();
			}
		}

		protected override void UnloadContent()
		{
			// Tell each of the screens to unload their content.
			foreach (GameScreen screen in screens)
			{
				screen.UnloadContent();
			}
		}

		protected override void Update(GameTime gameTime)
		{
			// Updates audio info
			AudioManager.update();

			// Read the keyboard and gamepad.
			InputHandler.Update();

			// Make a copy of the master screen list, to avoid confusion if
			// the process of updating one screen adds or removes others.
			screensToUpdate.Clear();

			foreach (GameScreen screen in screens)
				screensToUpdate.Add(screen);

			bool otherScreenHasFocus = !this.IsActive;
			bool coveredByOtherScreen = false;

			// Loop as long as there are screens waiting to be updated.
			while (screensToUpdate.Count > 0)
			{
				// Pop the topmost screen off the waiting list.
				GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

				screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

				// Update the screen.
				screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

				if (screen.ScreenState == ScreenState.TransitionOn ||
					screen.ScreenState == ScreenState.Active)
				{
					// If this is the first active screen we came across,
					// give it a chance to handle input.
					if (!otherScreenHasFocus)
					{
						screen.HandleInput();

						otherScreenHasFocus = true;
					}

					// If this is an active non-popup, inform any subsequent
					// screens that they are covered by it.
					if (!screen.IsPopup)
						coveredByOtherScreen = true;
				}
			}

		}

		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			foreach (GameScreen screen in screens)
			{
				if (screen.ScreenState == ScreenState.Hidden)
					continue;

				screen.Draw(gameTime);
			}

			base.Draw(gameTime);
		}

		public bool IsFullScreen
		{
			get
			{
				return graphics.IsFullScreen;
			}
		}

		public void ToggleFullScreen()
		{
			graphics.ToggleFullScreen();
		}

		public bool IsAntiAliasing
		{
			get
			{
				return GraphicsDevice.RenderState.MultiSampleAntiAlias;
			}
		}

		public void ToggleAntiAliasing()
		{
			GraphicsDevice.RenderState.MultiSampleAntiAlias = !GraphicsDevice.RenderState.MultiSampleAntiAlias;
		}

		public void AddScreen(GameScreen screen)
		{
			screen.IsExiting = false;

			// If we have a graphics device, tell the screen to load content.
			if (isInitialized)
			{
				screen.LoadContent();
			}

			screens.Add(screen);
		}

		public void RemoveScreen(GameScreen screen)
		{
			// If we have a graphics device, tell the screen to unload content.
			if (isInitialized)
			{
				screen.UnloadContent();
			}

			screens.Remove(screen);
			screensToUpdate.Remove(screen);
		}

		public GameScreen[] GetScreens()
		{
			return screens.ToArray();
		}

		public void FadeBackBufferToBlack(int alpha)
		{
			Viewport viewport = GraphicsDevice.Viewport;

			spriteBatch.Begin();

			spriteBatch.Draw(blankTexture,
							 new Rectangle(0, 0, viewport.Width, viewport.Height),
							 new Color(0, 0, 0, (byte)alpha));

			spriteBatch.End();
		}

		private void addResolution(DisplayMode d)
		{
			foreach (Resolution r in Resolutions)
			{
				if (d.Height == r.Height && d.Width == r.Width)
				{
					return;
				}
			}
			Resolutions.Add(new Resolution(d));
		}

		public void setResolution(Resolution r)
		{
			CurrentResolution = r;
			graphics.PreferredBackBufferHeight = r.Height;
			graphics.PreferredBackBufferWidth = r.Width;
			graphics.ApplyChanges();

			foreach (GameScreen gs in GetScreens())
				if (gs is GameplayScreen)
					((GameplayScreen)gs).CreateRenderTargets();
		}

		public Resolution CurrentResolution;
		public List<Resolution> Resolutions = new List<Resolution>();
	}

	public struct Resolution
	{

		public int Height;
		public int Width;

		public Resolution(DisplayMode d)
		{
			Height = d.Height;
			Width = d.Width;
		}
		public Resolution(int height, int width)
		{
			Height = height;
			Width = width;
		}

		public override string ToString()
		{
			return Width.ToString() + "x" + Height.ToString();
		}

		public static int comparison(Resolution x, Resolution y)
		{
			if (x.Width != y.Width)
			{
				return x.Width - y.Width;
			}
			else
			{
				return x.Height - y.Height;
			}
		}

	}
}
