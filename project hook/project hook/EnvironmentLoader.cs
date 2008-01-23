using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	static class EnvironmentLoader
	{
		private static Hashtable m_ColorMap = new Hashtable();
		private static System.Drawing.Color[,] m_LevelArray;
		private static Collidable[,] m_CurrentView = new Collidable[16,14];
		private static int m_CurTopRow;
		private static int m_TileDimension = Game1.graphics.GraphicsDevice.Viewport.Width/m_CurrentView.GetLength(0);

		public static void Initialize()
		{
			m_ColorMap.Add(System.Drawing.Color.FromKnownColor(KnownColor.Black).ToArgb(), new Tile(TextureLibrary.getGameTexture("crosshairs", ""),0,true));
			m_ColorMap.Add(System.Drawing.Color.FromKnownColor(KnownColor.White).ToArgb(), new Tile(null, 0, false));
		}

		public static void ReadLevelBmp(string p_FileName, List<Sprite> p_SpriteList)
		{
			Bitmap bmp = new Bitmap(p_FileName);
			int bmpHeight = bmp.Height;
			int bmpWidth = bmp.Width;
			m_LevelArray = new System.Drawing.Color[bmpWidth, bmpHeight];

			for (int height = 0; height < bmpHeight; height++)
			{
				for (int width = 0; width < bmpWidth; width++)
				{
					m_LevelArray[width, height] = bmp.GetPixel(width, height);
				}
			}

			for (int y = 0; y < m_CurrentView.GetLength(1); y++)
			{
				for (int x = 0; x < m_CurrentView.GetLength(0); x++)
				{
					Tile curTile = ((Tile)m_ColorMap[m_LevelArray[x, y].ToArgb()]);

					m_CurrentView[x, y] = new Collidable("environment", new Vector2(x * m_TileDimension, (y-1) * m_TileDimension), m_TileDimension, m_TileDimension, curTile.GTexture,
						1, curTile.Enabled, curTile.Rotation, Depth.ForeGround.Bottom, Collidable.Factions.Environment, -1,null,m_TileDimension/2);

					m_CurrentView[x, y].Bound = Collidable.Boundings.Square;
					p_SpriteList.Add(m_CurrentView[x, y]);
				}
			}

			m_CurTopRow = bmpHeight - m_CurrentView.GetLength(1);
		}

		public static void Update()
		{
			for (int y = 0; y < m_CurrentView.GetLength(0); y++)
			{
				for (int x = 0; y < m_CurrentView.GetLength(1); y++)
				{
					
				}
			}
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
