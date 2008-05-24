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

			spriteBatch.DrawString(font, "Eric Baker", new Vector2(50, 100), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "Mike Dapiran", new Vector2(50, 140), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "Mike De Mauro", new Vector2(50, 180), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "Matt Jacobs", new Vector2(50, 220), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "Brian Murphy", new Vector2(50, 260), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "Adam Nabinger", new Vector2(50, 300), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "Josh Wilson", new Vector2(50, 340), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);

			spriteBatch.DrawString(font, "Music from Newgrounds.com Audio Portal", new Vector2(50, 400), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "\"Afternoon Breeze\" by ismiller", new Vector2(50, 440), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "\"-MrMaestro- (ST) Menu\" by DavidOrr", new Vector2(50, 480), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "\"Scorpion Trekk\" by TheOrichalcon", new Vector2(50, 520), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, "\"Super Charged\" by Xenogenocide", new Vector2(50, 560), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
			spriteBatch.End();
		}
	}
}
