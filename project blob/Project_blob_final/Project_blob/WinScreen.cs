using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_blob.GameState {
	internal class WinScreen : EndScreen {

		float m_Time;
		bool addedScore = false;
		NewHighScoreScreen highScoreScreen = null;
		public WinScreen(float time)
			: base("You Win!") {
			m_Time = time;
		}

		public void CheckForNewHighScore() {
			Score[] areaScores = GameplayScreen.HighScoreManager.getScores(Level.GetAreaName(GameplayScreen.currentArea));
			int tmp = 49;
			char test = (char)tmp;

			if (areaScores[9] == null || m_Time < areaScores[9].Time) {
				highScoreScreen = new NewHighScoreScreen(m_Time);
				ScreenManager.AddScreen(highScoreScreen);
			}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
			if (!coveredByOtherScreen && !addedScore && highScoreScreen != null) {
				GameplayScreen.HighScoreManager.addScore(Level.GetAreaName(GameplayScreen.currentArea), highScoreScreen.cur_input, m_Time);
				addedScore = true;
			}
		}

		public override void Draw(GameTime gameTime) {
			base.Draw(gameTime);
			SpriteBatch m_SpriteBatch = ScreenManager.SpriteBatch;
			SpriteFont font = ScreenManager.Font;

			Score[] areaScores = GameplayScreen.HighScoreManager.getScores(Level.GetAreaName(GameplayScreen.currentArea));

			m_SpriteBatch.Begin();
			string temp = "Best Times";
			m_SpriteBatch.DrawString(font, temp, new Vector2((ScreenManager.graphics.GraphicsDevice.Viewport.Width / 2) - (font.MeasureString(temp).X / 2), 220), Color.White);

			float maxLength = 0;
			foreach (Score s in areaScores) {
				if (s != null) {
					maxLength = Math.Max(maxLength, font.MeasureString(s.Name).X + font.MeasureString(Format.Time(s.Time)).X + 50);
				}
			}

			float offset = maxLength * 0.5f;
			float middle = ScreenManager.graphics.GraphicsDevice.Viewport.Width * 0.5f;

			int y = 235;
			int i = 0;

			foreach (Score s in areaScores) {
				if (s != null) {
					y += 30;
					string t = Format.Time(s.Time);
					m_SpriteBatch.DrawString(font, ++i + ". " + s.Name, new Vector2(middle - offset, y), Color.White);
					m_SpriteBatch.DrawString(font, t, new Vector2(middle + offset - font.MeasureString(t).X, y), Color.White);
				}
			}

			m_SpriteBatch.End();

		}
	}
}
