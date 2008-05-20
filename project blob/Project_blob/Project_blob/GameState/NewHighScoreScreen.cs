using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_blob.GameState
{
	class NewHighScoreScreen : MenuScreen
	{
		char inputChar = 'A';
		public string cur_input = String.Empty;

		float m_Time;
		public NewHighScoreScreen(float time)
			: base("New Top Time")
		{
			m_Time = time;
		}

		public override void HandleInput()
		{
			// Move to the previous menu entry?
			if (InputHandler.IsActionPressed(Actions.MenuUp))
			{
				if ((int)inputChar == 90)
					inputChar = (char)65;
				else
					inputChar++;
			}

			// Move to the next menu entry?
			if (InputHandler.IsActionPressed(Actions.MenuDown))
			{
				if ((int)inputChar == 65)
					inputChar = (char)90;
				else
					inputChar--;
			}

			if (InputHandler.IsActionPressed(Actions.MenuAccept))
			{
				OnCancel();
			}

			// Accept or cancel the menu?
			if (InputHandler.IsActionPressed(Actions.MenuRight))
			{
				if (cur_input.Length < 20) 
					cur_input += inputChar;
				//cur_input.Insert(cur_input.Length, inputChar);
			}
			else if (InputHandler.IsActionPressed(Actions.MenuLeft))
			{
				if(cur_input.Length > 0)
					cur_input = cur_input.Remove(cur_input.Length - 1);
			}

			
		}

		public override void Draw(GameTime gameTime)
		{
			ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);
			base.Draw(gameTime);
			
			SpriteBatch m_SpriteBatch = ScreenManager.SpriteBatch;
			SpriteFont font = ScreenManager.Font;
			m_SpriteBatch.Begin();

            string t = "Time - " + String.Format("{0:0}", (m_Time / 60)) + ":" + String.Format("{0:0.0}", m_Time % 60);
			m_SpriteBatch.DrawString(font, "Your Time: " + t, new Vector2(100, 200), Color.White);

			m_SpriteBatch.DrawString(font, cur_input, new Vector2(100, 400), Color.White);

			if(cur_input .Length < 20)
				m_SpriteBatch.DrawString(font, inputChar.ToString(), new Vector2( font.MeasureString(cur_input).X + 100, 400), Color.Yellow);
				
		

			m_SpriteBatch.End();

		}
	}
}
