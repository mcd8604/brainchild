using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_blob.GameState
{
	class WinScreen : EndScreen
	{

		float m_Time;
		bool addedScore = false;
		NewHighScoreScreen highScoreScreen = null;
		public WinScreen(float time)
			: base("You Win!")
		{
			m_Time = time;
		}

		public void CheckForNewHighScore()
		{
			Score[] areaScores = GameplayScreen.HighScoreManager.getScores(Level.GetAreaName(GameplayScreen.currentArea));
			int tmp = 49;
			char test = (char)tmp;

			if (areaScores[9] == null || m_Time < areaScores[9].Time)
			{
				highScoreScreen = new NewHighScoreScreen(m_Time);
				ScreenManager.AddScreen(highScoreScreen);
			}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
			if (!coveredByOtherScreen && !addedScore && highScoreScreen != null)
			{
				GameplayScreen.HighScoreManager.addScore(Level.GetAreaName(GameplayScreen.currentArea), highScoreScreen.cur_input, m_Time);
				addedScore = true;
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			SpriteBatch m_SpriteBatch = ScreenManager.SpriteBatch;
			SpriteFont font = ScreenManager.Font;

			Score[] areaScores = GameplayScreen.HighScoreManager.getScores(Level.GetAreaName(GameplayScreen.currentArea));

			m_SpriteBatch.Begin();
            String temp = "Best Times";
            m_SpriteBatch.DrawString(font, temp, new Vector2((ScreenManager.graphics.GraphicsDevice.Viewport.Width / 2) - (font.MeasureString(temp).X/2), 220), Color.White);

			for (int i = 0; i < areaScores.Length; i++)
			{
                if (areaScores[i] != null) {
                    //char[] blank = "----------------".ToCharArray();
                    string filler = string.Empty;
                    string t = String.Format("{0:0}", (areaScores[i].Time / 60)) + ":" + String.Format("{0:0.0}", areaScores[i].Time % 60);

                    while (525 - (100 + font.MeasureString(areaScores[i].Name).X) > font.MeasureString(filler).X)
                        filler += ".";

                    //areaScores[i].Name.CopyTo(0, blank, 0, areaScores[i].Name.Length);
                    m_SpriteBatch.DrawString(font, (i+1) + ". " + areaScores[i].Name + filler, new Vector2(100, 265 + (30 * i)), Color.White);
                    m_SpriteBatch.DrawString(font, t, new Vector2(600, 265 + (30 * i)), Color.White);
                }
			}

			m_SpriteBatch.End();
			
		}
	}
}
