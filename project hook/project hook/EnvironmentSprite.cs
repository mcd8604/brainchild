using System;
using System.Collections;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal class EnvironmentSprite : Sprite
	{
		private Collidable[,] Tiles;

		private readonly int TileDimension = EnvironmentLoader.TileDimension;
		private readonly int ScreenSpaceHeight = EnvironmentLoader.ScreenSpaceHeight;
		private readonly int ScreenSpaceWidth = EnvironmentLoader.ScreenSpaceWidth;

		private Level m_CurrentLevel;

		private int m_CurTopRow;
		private int m_CurTopBuffer;
		private int m_CurBottomBuffer;

		internal EnvironmentSprite()
		{
#if !FINAL
			Name = "Environment";
#endif

			Tiles = new Collidable[ScreenSpaceWidth, ScreenSpaceHeight];

			for (int y = 0; y < ScreenSpaceHeight; y++)
			{
				for (int x = 0; x < ScreenSpaceWidth; x++)
				{
					Collidable temp = new Collidable(
#if !FINAL
						String.Empty,
#endif
new Vector2(x * TileDimension, (y - 1) * TileDimension), TileDimension, TileDimension, null, 1f, false, 0, Depth.GameLayer.Environment, Collidable.Factions.Environment, float.NaN, TileDimension * 0.5f);
					temp.Bound = Collidable.Boundings.Square;
					Tiles[x, y] = temp;
					attachSpritePart(temp);
				}
			}
		}

		internal override void Update(GameTime p_GameTime)
		{
			for (int y = 0; y < ScreenSpaceHeight; y++)
			{
				for (int x = 0; x < ScreenSpaceWidth; x++)
				{
					Vector2 temp = Tiles[x, y].Center;
					temp.Y += (World.Position.Speed * (float)p_GameTime.ElapsedGameTime.TotalSeconds);
					Tiles[x, y].Center = temp;
#if !FINAL
					Tiles[x, y].Update(p_GameTime);
#endif
				}
			}
			if (Tiles[ScreenSpaceWidth - 1, m_CurBottomBuffer].Center.Y >= Game.graphics.GraphicsDevice.Viewport.Height + (TileDimension * 0.5f))
			{
				m_CurBottomBuffer -= 1;
				if (m_CurBottomBuffer == -1)
					m_CurBottomBuffer = ScreenSpaceHeight - 1;

				m_CurTopBuffer -= 1;
				if (m_CurTopBuffer == -1)
					m_CurTopBuffer = ScreenSpaceHeight - 1;

				for (int i = 0; i < ScreenSpaceWidth; i++)
				{
					if (m_CurTopRow < 0)
					{
						m_CurTopRow = 0;
					}

					Vector2 temp = Tiles[i, m_CurTopBuffer].Center;
					temp.Y = Tiles[i, (m_CurTopBuffer + 1) % ScreenSpaceHeight].Center.Y - TileDimension;
					Tiles[i, m_CurTopBuffer].Center = temp;

					Tiles[i, m_CurTopBuffer].Texture = m_CurrentLevel.TileArray[i, m_CurTopRow].GameTexture;
					Tiles[i, m_CurTopBuffer].Faction = m_CurrentLevel.TileArray[i, m_CurTopRow].Faction;
					Tiles[i, m_CurTopBuffer].Enabled = m_CurrentLevel.TileArray[i, m_CurTopRow].Enabled;

				}

				m_CurTopRow--;
			}
		}

		internal void changeLevel(Level newLevel)
		{
			m_CurrentLevel = newLevel;
			m_CurTopRow = m_CurrentLevel.Height - 1;
		}

		/// <summary>
		/// Redraw the entire screen, then continue from the bottom of the currently loaded bitmap.
		/// </summary>
		internal void resetLevel()
		{
			for (int y = 0; y < ScreenSpaceHeight; y++)
			{
				for (int x = 0; x < ScreenSpaceWidth; x++)
				{

					Tiles[x, y].Position = new Vector2(x * TileDimension, (y - 1) * TileDimension);
					Tiles[x, y].Texture = m_CurrentLevel.TileArray[x, m_CurrentLevel.Height - 1].GameTexture;
					Tiles[x, y].Faction = m_CurrentLevel.TileArray[x, m_CurrentLevel.Height - 1].Faction;
					Tiles[x, y].Enabled = m_CurrentLevel.TileArray[x, m_CurrentLevel.Height - 1].Enabled;

				}
			}
			m_CurBottomBuffer = ScreenSpaceHeight - 1;
			m_CurTopBuffer = 0;

			m_CurTopRow = m_CurrentLevel.Height - 1;
		}

	}
}
