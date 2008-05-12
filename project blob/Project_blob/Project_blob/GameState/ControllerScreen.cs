using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using System.Collections.Generic;
using System.Text;

namespace Project_blob.GameState
{
	class ControllerScreen : MenuScreen
	{
		Texture2D controlTexture;

		public ControllerScreen() : base("Controller")
		{
			IsPopup = true;
		}

		public override void LoadContent()
		{
			controlTexture = ScreenManager.Content.Load<Texture2D>("blank");
		}
		
		public override void HandleInput()
		{
			if (InputHandler.IsActionPressed(Actions.MenuAccept))
			{
				OnCancel();
			}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus,
													   bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);
		}

		public override void Draw(GameTime gameTime)
		{
			ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
			Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
			Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
			byte fade = TransitionAlpha;

			spriteBatch.Begin(SpriteBlendMode.None);

			spriteBatch.Draw(controlTexture, fullscreen,
							 new Color(fade, fade, fade));

			spriteBatch.End();
		}
	}
}
