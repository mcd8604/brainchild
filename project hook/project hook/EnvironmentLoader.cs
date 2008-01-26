using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public class EnvironmentLoader
	{
		private static int m_ScreenSpaceWidth = 16;
		private static int m_ScreenSpaceHeight = 14;

		private Hashtable m_ColorMap;
		private System.Drawing.Color[,] m_LevelArray;
		private List<Sprite> m_CurrentView;
		private int m_CurTopRow;
		private int m_CurTopBuffer;
		private int m_CurBottomBuffer;
		private WorldPosition m_Position;
		private Tile curTile;
		private int m_TileDimension;

		public List<Sprite> Initialize(WorldPosition p_Position, string p_FileName)
		{
			// Set up variables
			m_Position = p_Position;
			Bitmap bmp = new Bitmap(p_FileName);

			int bmpHeight = bmp.Height;
			int bmpWidth = bmp.Width;

			m_ScreenSpaceWidth = bmpWidth;
			m_TileDimension = Game.graphics.GraphicsDevice.Viewport.Width / m_ScreenSpaceWidth;
			m_ScreenSpaceHeight = (Game.graphics.GraphicsDevice.Viewport.Height / m_TileDimension) + 1;
			
			// Color mapping
			m_ColorMap = new Hashtable();
			m_ColorMap.Add(System.Drawing.Color.Black.ToArgb(), new Tile(TextureLibrary.getGameTexture("plaque", ""), 0, true));
			m_ColorMap.Add(System.Drawing.Color.White.ToArgb(), new Tile(null, 0, false));
			m_ColorMap.Add(System.Drawing.Color.Green.ToArgb(), new Tile(TextureLibrary.getGameTexture("wall_flat", ""), 0, true));
			m_ColorMap.Add(System.Drawing.Color.Red.ToArgb(), new Tile(TextureLibrary.getGameTexture("wall_flat", ""), 180, true));
				

			// Create all sprites
			m_CurrentView = new List<Sprite>();
			for (int y = 0; y < m_ScreenSpaceHeight; y++)
			{
				for (int x = 0; x < m_ScreenSpaceWidth; x++)
				{
					Collidable temp = new Collidable("environment", new Vector2(x * m_TileDimension, (y - 1) * m_TileDimension), m_TileDimension, m_TileDimension, null,
						1, false, 0, Depth.GameLayer.Environment, Collidable.Factions.Environment, float.NaN, null, m_TileDimension / 2);
					temp.Bound = Collidable.Boundings.Square;
					m_CurrentView.Add(temp);
				}
			}

			// read in level
			m_LevelArray = new System.Drawing.Color[bmpWidth, bmpHeight];

			for (int height = 0; height < bmpHeight; height++)
			{
				for (int width = 0; width < bmpWidth; width++)
				{
					m_LevelArray[width, height] = bmp.GetPixel(width, height);
				}
			}

			// create initial screen
			for (int y = 0; y < m_ScreenSpaceHeight; y++)
			{
				for (int x = 0; x < m_ScreenSpaceWidth; x++)
				{
					if (m_ColorMap.ContainsKey(m_LevelArray[x, bmpHeight - m_ScreenSpaceHeight + y].ToArgb()))
					{
						curTile = ((Tile)m_ColorMap[m_LevelArray[x, bmpHeight - m_ScreenSpaceHeight + y].ToArgb()]);

						m_CurrentView[getPosition(x, y)].Texture = curTile.GTexture;
						m_CurrentView[getPosition(x, y)].RotationDegrees = curTile.Rotation;
						m_CurrentView[getPosition(x, y)].Enabled = curTile.Enabled;

					}
					else
					{

						m_CurrentView[getPosition(x, y)].Texture = null;
						m_CurrentView[getPosition(x, y)].RotationDegrees = 0f;
						m_CurrentView[getPosition(x, y)].Enabled = false;

					}

					m_CurrentView[getPosition(x, y)].Position = new Vector2(x * m_TileDimension, (y - 1) * m_TileDimension);
				}
			}

			m_CurTopRow = bmpHeight - m_ScreenSpaceHeight;
			m_CurBottomBuffer = m_ScreenSpaceHeight - 1;
			m_CurTopBuffer = 0;





			return m_CurrentView;

		}

		public void Update(GameTime p_GameTime)
		{
			for (int y = 0; y < m_ScreenSpaceHeight; y++)
			{
				for (int x = 0; x < m_ScreenSpaceWidth; x++)
				{
					Vector2 temp = m_CurrentView[getPosition(x, y)].Position;
					temp.Y += (m_Position.Speed * (float)p_GameTime.ElapsedGameTime.TotalSeconds);
					m_CurrentView[getPosition(x, y)].Position = temp;
				}
			}
			if (m_CurrentView[getPosition(m_ScreenSpaceWidth - 1, m_CurBottomBuffer)].Position.Y >= Game.graphics.GraphicsDevice.Viewport.Height)
			{
				m_CurBottomBuffer -= 1;
				if (m_CurBottomBuffer == -1)
					m_CurBottomBuffer = m_ScreenSpaceHeight - 1;

				m_CurTopBuffer -= 1;
				if (m_CurTopBuffer == -1)
					m_CurTopBuffer = m_ScreenSpaceHeight - 1;

				for (int i = 0; i < m_ScreenSpaceWidth; i++)
				{
                    if (m_CurTopRow < 0)
                    {
                       m_CurTopRow = 0;
					   m_Position.setSpeed(0);
                    }

					if (m_ColorMap.ContainsKey(m_LevelArray[i, m_CurTopRow].ToArgb())) 
					{
						curTile = ((Tile)m_ColorMap[m_LevelArray[i, m_CurTopRow].ToArgb()]);

						m_CurrentView[getPosition(i, m_CurTopBuffer)].Texture = curTile.GTexture;
						m_CurrentView[getPosition(i, m_CurTopBuffer)].RotationDegrees = curTile.Rotation;
						m_CurrentView[getPosition(i, m_CurTopBuffer)].Enabled = curTile.Enabled;

					}
					else
					{

						m_CurrentView[getPosition(i, m_CurTopBuffer)].Texture = null;
						m_CurrentView[getPosition(i, m_CurTopBuffer)].RotationDegrees = 0;
						m_CurrentView[getPosition(i, m_CurTopBuffer)].Enabled = false;

					}

					m_CurrentView[getPosition(i, m_CurTopBuffer)].Position = new Vector2(i * m_TileDimension, 0 - m_TileDimension);

				}

				m_CurTopRow--;
			}
		}

		public static int getPosition(int x, int y)
		{
			return ((y * m_ScreenSpaceWidth) + x);
		}

	}

	public struct Tile
	{
		public GameTexture GTexture;
		public int Rotation;
		public bool Enabled;

		public Tile(GameTexture p_Texture, int p_Rotation, bool p_Enabled)
		{
			GTexture = p_Texture;
			Rotation = p_Rotation;
			Enabled = p_Enabled;
		}
	}
}
