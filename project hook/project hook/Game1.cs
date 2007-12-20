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
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		ContentManager content;
		KeyHandler.KeyHandler keyhandler;
        SpriteBatch m_spriteBatch;


		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			content = new ContentManager(Services);
			keyhandler = new KeyHandler.KeyHandler();
		}


		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
            
            TextureLibrary.iniTextures(Services);
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
                TextureLibrary.LoadTexture("Ship2");
                TextureLibrary.LoadTexture("Back");
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
			keyhandler.Update();
			// Allows the game to exit
			if (keyhandler.IsActionDown(KeyHandler.KeyHandler.Actions.Pause))
			{
				this.Exit();
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
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
             m_spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

             Sprite back = new Sprite("back", new Vector2(0.0f, 0.0f), graphics.PreferredBackBufferHeight, 
                                        graphics.PreferredBackBufferWidth, TextureLibrary.getGameTexture("Back", ""), 100, true, 0.15f, 0);

             m_spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

             back.Draw(m_spriteBatch);

             m_spriteBatch.End();
            
			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}
	}
}
