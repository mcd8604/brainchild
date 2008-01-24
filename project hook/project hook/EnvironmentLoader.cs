using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class EnvironmentLoader
	{
		private Hashtable m_ColorMap = new Hashtable();
		private System.Drawing.Color[,] m_LevelArray;
		private Collidable[,] m_CurrentView = new Collidable[16,14];
		private int m_CurTopRow;
		private int m_CurTopBuffer;
		private int m_CurBottomBuffer;
		private int m_ScrollSpeed;
		private Tile curTile;
		private int m_TileDimension;
		private List<Sprite> m_SpriteList;
		private Random m_Index;

		public void Initialize(int p_ScrollSpeed)
		{
			
			m_ColorMap.Add(System.Drawing.Color.FromKnownColor(KnownColor.Black).ToArgb(), new Tile(new GameTexture[4] {TextureLibrary.getGameTexture("wall_rand2", ""), TextureLibrary.getGameTexture("wall_flat", ""), TextureLibrary.getGameTexture("wall_rand1", ""), TextureLibrary.getGameTexture("wall_rand3", "")}, 0, true));
			m_ColorMap.Add(System.Drawing.Color.FromKnownColor(KnownColor.White).ToArgb(), new Tile(new GameTexture[1] { null }, 0, false));
			m_ScrollSpeed = p_ScrollSpeed;
			m_Index = new Random();
			m_TileDimension = Game.graphics.GraphicsDevice.Viewport.Width / m_CurrentView.GetLength(0);
		}

		public void ReadLevelBmp(string p_FileName, List<Sprite> p_SpriteList)
		{
			Bitmap bmp = new Bitmap(p_FileName);
			int bmpHeight = bmp.Height;
			int bmpWidth = bmp.Width;
			m_LevelArray = new System.Drawing.Color[bmpWidth, bmpHeight];
			m_SpriteList = p_SpriteList;

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
					curTile = ((Tile)m_ColorMap[m_LevelArray[x, bmpHeight-m_CurrentView.GetLength(1) + y].ToArgb()]);

					m_CurrentView[x, y] = new Collidable("environment", new Vector2(x * m_TileDimension, (y-1) * m_TileDimension), m_TileDimension, m_TileDimension, curTile.GTexture[m_Index.Next(curTile.GTexture.GetLength(0)-1)],
						1, curTile.Enabled, curTile.Rotation, Depth.ForeGround.Bottom, Collidable.Factions.Environment, -1,null,m_TileDimension/2);

					m_CurrentView[x, y].Bound = Collidable.Boundings.Square;
				}
			}

			m_CurTopRow = bmpHeight - m_CurrentView.GetLength(1);
			m_CurBottomBuffer = m_CurrentView.GetLength(1) - 1;
			m_CurTopBuffer = 0;
		}

		public void Update(GameTime p_GameTime)
		{
			for (int y = 0; y < m_CurrentView.GetLength(1); y++)
			{
				for(int x = 0; x < m_CurrentView.GetLength(0); x++)
				{
					Vector2 temp = m_CurrentView[x,y].Position;
					temp.Y += (m_ScrollSpeed * (float)p_GameTime.ElapsedGameTime.TotalSeconds);
					m_CurrentView[x, y].Position = temp;
				}
			}
			if (m_CurrentView[m_CurrentView.GetLength(0)-1, m_CurBottomBuffer].Position.Y >= Game.graphics.GraphicsDevice.Viewport.Height)
			{
				m_CurBottomBuffer -= 1;
				if (m_CurBottomBuffer == -1)
					m_CurBottomBuffer = m_CurrentView.GetLength(1)-1;

				m_CurTopBuffer -= 1;
				if (m_CurTopBuffer == -1)
					m_CurTopBuffer = m_CurrentView.GetLength(1)-1;

				for (int i = 0; i < m_CurrentView.GetLength(0); i++)
				{
					curTile = ((Tile)m_ColorMap[m_LevelArray[i, m_CurTopRow].ToArgb()]);
					m_CurrentView[i, m_CurTopBuffer].Texture = curTile.GTexture[m_Index.Next(curTile.GTexture.GetLength(0) - 1)];
					m_CurrentView[i, m_CurTopBuffer].Rotation = curTile.Rotation;
					m_CurrentView[i, m_CurTopBuffer].Enabled = curTile.Enabled;
					m_CurrentView[i, m_CurTopBuffer].Position = new Vector2(i * m_TileDimension, 0 - m_TileDimension);
				}

				m_CurTopRow--;
			}
		}

		public void Draw(SpriteBatch p_SpriteBatch)
		{
			for (int y = 0; y < m_CurrentView.GetLength(1); y++)
			{
				for (int x = 0; x < m_CurrentView.GetLength(0); x++)
				{
					m_CurrentView[x, y].Draw(p_SpriteBatch);
				}
			}
		}
	}

	public struct Tile
	{
		public GameTexture[] GTexture;
		public int Rotation;
		public bool Enabled;

		public Tile(GameTexture[] p_Texture, int p_Rotation, bool p_Enabled)
		{
			GTexture = p_Texture;
			Rotation = p_Rotation;
			Enabled = p_Enabled;
		}
	}
}
