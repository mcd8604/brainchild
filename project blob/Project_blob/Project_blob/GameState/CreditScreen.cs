using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using System.Collections.Generic;
using System.Text;

namespace Project_blob.GameState
{
	class CreditScreen : MenuScreen
	{
		 public CreditScreen()
            : base("Credits")
        {
            
        }

		public override void HandleInput()
		{
			if (InputHandler.IsActionPressed(Actions.MenuAccept) || InputHandler.IsActionPressed(Actions.MenuCancel))
			{
				OnCancel();
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
			SpriteFont font = ScreenManager.Font;

			spriteBatch.Begin();

			spriteBatch.DrawString(font, "Eric Baker", new Vector2(100, 100), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "Mike Dapiran", new Vector2(100, 140), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "Mike De Mauro", new Vector2(100, 180), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "Matt Jacobs", new Vector2(100, 220), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "Brian Murphy", new Vector2(100, 260), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "Adam Nabinger", new Vector2(100, 300), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "Josh Wilson", new Vector2(100, 340), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);

			spriteBatch.End();
		}
	}
}
