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
		public static int TileCount
		{
			get
			{
				return m_ScreenSpaceHeight * m_ScreenSpaceWidth;
			}
		}

		private Hashtable m_ColorMap;
		private System.Drawing.Color[,] m_LevelArray;
		private List<Sprite> m_CurrentView;
		private int m_CurTopRow;
		private int m_CurTopBuffer;
		private int m_CurBottomBuffer;
		private WorldPosition m_Position;
		private Tile curTile;
		private static int m_TileDimension;
		public static int TileDimension
		{
			get
			{
				return m_TileDimension;
			}
		}

#if DEBUG
		private static Random random = new Random(0);
#else 
        private static Random random = new Random();
#endif


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

			ArrayList gts = new ArrayList();
			gts = new ArrayList();
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque", ""));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque2", ""));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque3", ""));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque4", ""));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(0, 0, 0).ToArgb(), new Tile(gts, 0, true, true));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(255, 150, 100).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\wall_left", ""), 0, true, true));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(255, 0, 0).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_right", ""), 0, true, false));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(0, 255, 0).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_left", ""), 0, true, false));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(255, 255, 0).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm", ""), 0, true, false));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(0, 255, 255).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_right", ""), 0, true, false));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(0, 0, 255).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_right_invert", ""), 0, true, false));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(0, 100, 0).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_left", ""), 0, true, false));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(255, 100, 255).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_left_invert", ""), 0, true, false));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(255, 100, 0).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top", ""), 0, true, false));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(255, 0, 255).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_right", ""), 0, true, false));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(100, 100, 0).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_right_invert", ""), 0, true, false));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(100, 0, 0).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_left", ""), 0, true, false));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(200, 200, 255).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_left_invert", ""), 0, true, false));
			//m_ColorMap.Add(System.Drawing.Color.FromArgb(100, 0, 100).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_clear", ""), 0, true, false));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(200, 200, 200).ToArgb(), new Tile(0, false, false));
			m_ColorMap.Add(System.Drawing.Color.FromArgb(255, 255, 255).ToArgb(), new Tile(0, false, false));


			// Create all sprites
			m_CurrentView = new List<Sprite>();
			for (int y = 0; y < m_ScreenSpaceHeight; y++)
			{
				for (int x = 0; x < m_ScreenSpaceWidth; x++)
				{
					Collidable temp = new Collidable("environment", new Vector2(x * m_TileDimension, (y - 1) * m_TileDimension), m_TileDimension, m_TileDimension, null,
						1, false, 0, Depth.GameLayer.Environment, Collidable.Factions.Environment, float.NaN, m_TileDimension * 0.5f);
					temp.Bound = Collidable.Boundings.Square;
					m_CurrentView.Add(temp);
				}
			}


			// read in level
			m_LevelArray = readBitmap(bmp);


			// create initial screen
			for (int y = 0; y < m_ScreenSpaceHeight; y++)
			{
				for (int x = 0; x < m_ScreenSpaceWidth; x++)
				{
					if (m_ColorMap.ContainsKey(m_LevelArray[x, bmpHeight - m_ScreenSpaceHeight + y].ToArgb()))
					{
						curTile = ((Tile)m_ColorMap[m_LevelArray[x, bmpHeight - m_ScreenSpaceHeight + y].ToArgb()]);

						m_CurrentView[getPosition(x, y)].Texture = (GameTexture)curTile.gameTextures[random.Next(0, curTile.gameTextures.Count)];

						if (curTile.Collidable)
						{
							((Collidable)m_CurrentView[getPosition(x, y)]).Faction = Collidable.Factions.Environment;
						}
						else
						{
							((Collidable)m_CurrentView[getPosition(x, y)]).Faction = Collidable.Factions.None;
						}

						m_CurrentView[getPosition(x, y)].RotationDegrees = curTile.Rotation;
						m_CurrentView[getPosition(x, y)].Enabled = curTile.Enabled;

					}
					else
					{

						((Collidable)m_CurrentView[getPosition(x, y)]).Faction = Collidable.Factions.None;
						m_CurrentView[getPosition(x, y)].Texture = null;
						m_CurrentView[getPosition(x, y)].RotationDegrees = 0f;
						m_CurrentView[getPosition(x, y)].Enabled = false;

					}

					m_CurrentView[getPosition(x, y)].Position = new Vector2(x * m_TileDimension, (y - 1) * m_TileDimension);
				}
			}

			m_CurTopRow = bmpHeight - m_ScreenSpaceHeight - 1;
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
						//m_Position.setSpeed(0);
					}

					if (m_ColorMap.ContainsKey(m_LevelArray[i, m_CurTopRow].ToArgb()))
					{
						curTile = ((Tile)m_ColorMap[m_LevelArray[i, m_CurTopRow].ToArgb()]);

						if (curTile.Collidable)
						{
							((Collidable)m_CurrentView[getPosition(i, m_CurTopBuffer)]).Faction = Collidable.Factions.Environment;
						}
						else
						{
							((Collidable)m_CurrentView[getPosition(i, m_CurTopBuffer)]).Faction = Collidable.Factions.None;
						}

						m_CurrentView[getPosition(i, m_CurTopBuffer)].Texture = (GameTexture)curTile.gameTextures[random.Next(0, curTile.gameTextures.Count)];
						m_CurrentView[getPosition(i, m_CurTopBuffer)].RotationDegrees = curTile.Rotation;
						m_CurrentView[getPosition(i, m_CurTopBuffer)].Enabled = curTile.Enabled;
					}
					else
					{
						((Collidable)m_CurrentView[getPosition(i, m_CurTopBuffer)]).Faction = Collidable.Factions.None;
						m_CurrentView[getPosition(i, m_CurTopBuffer)].Texture = null;
						m_CurrentView[getPosition(i, m_CurTopBuffer)].RotationDegrees = 0;
						m_CurrentView[getPosition(i, m_CurTopBuffer)].Enabled = false;
					}

					m_CurrentView[getPosition(i, m_CurTopBuffer)].Position = new Vector2(i * m_TileDimension, m_CurrentView[getPosition(i, (m_CurTopBuffer + 1) % m_ScreenSpaceHeight)].Position.Y - m_TileDimension);

				}

				m_CurTopRow--;
			}
		}

		public static int getPosition(int x, int y)
		{
			return ((y * m_ScreenSpaceWidth) + x);
		}

		public void NewFile(String p_FileName)
		{
			// Set up variables
			Bitmap bmp = new Bitmap(p_FileName);

			int bmpHeight = bmp.Height;
			int bmpWidth = bmp.Width;

			// read in level
			m_LevelArray = readBitmap(bmp);


			m_CurTopRow = bmpHeight - 1;
		}

		private static System.Drawing.Color[,] readBitmap(Bitmap bmp)
		{
			System.Drawing.Color[,] m_LevelArray = new System.Drawing.Color[bmp.Width, bmp.Height];


			for (int height = 0; height < bmp.Height; height++)
			{
				for (int width = 0; width < bmp.Width; width++)
				{
					m_LevelArray[width, height] = processPixel(bmp, width, height);
				}
			}

			return m_LevelArray;
		}

		private static System.Drawing.Color cWhite = System.Drawing.Color.FromArgb(255, 255, 255, 255);
		private static System.Drawing.Color cBlack = System.Drawing.Color.FromArgb(255, 0, 0, 0);

		private static System.Drawing.Color processPixel(Bitmap bmp, int x, int y)
		{

			if (bmp.GetPixel(x, y) == cWhite)
			{


				if (tryGetPixel(bmp, x, y + 1) == cBlack)
				{
					// top
					if (tryGetPixel(bmp, x, y - 1) != cBlack &&
						tryGetPixel(bmp, x + 1, y) != cBlack && tryGetPixel(bmp, x - 1, y) != cBlack)
					{
						return System.Drawing.Color.FromArgb(255, 100, 0);
					}

					// top left invert
					if (tryGetPixel(bmp, x, y - 1) != cBlack &&
						tryGetPixel(bmp, x + 1, y) == cBlack && tryGetPixel(bmp, x - 1, y) != cBlack)
					{
						return System.Drawing.Color.FromArgb(200, 200, 255);
					}

					// top right invert
					if (tryGetPixel(bmp, x, y - 1) != cBlack &&
						tryGetPixel(bmp, x + 1, y) != cBlack && tryGetPixel(bmp, x - 1, y) == cBlack)
					{
						return System.Drawing.Color.FromArgb(100, 100, 0);
					}

				}
				else
				{

					if (tryGetPixel(bmp, x, y - 1) == cBlack)
					{

						// bottom
						if (tryGetPixel(bmp, x + 1, y) != cBlack && tryGetPixel(bmp, x - 1, y) != cBlack)
						{
							return System.Drawing.Color.FromArgb(255, 255, 0);
						}

						// bottom left invert
						if (tryGetPixel(bmp, x + 1, y) == cBlack && tryGetPixel(bmp, x - 1, y) != cBlack)
						{
							return System.Drawing.Color.FromArgb(255, 100, 255);
						}

						// bottom right invert
						if (tryGetPixel(bmp, x + 1, y) != cBlack && tryGetPixel(bmp, x - 1, y) == cBlack)
						{
							return System.Drawing.Color.FromArgb(0, 0, 255);
						}

					}
					else
					{

						// left
						if (tryGetPixel(bmp, x + 1, y) == cBlack && tryGetPixel(bmp, x - 1, y) != cBlack)
						{
							return System.Drawing.Color.FromArgb(0, 255, 0);
						}

						// right
						if (tryGetPixel(bmp, x + 1, y) != cBlack && tryGetPixel(bmp, x - 1, y) == cBlack)
						{
							return System.Drawing.Color.FromArgb(255, 0, 0);
						}

						// bottom left
						if (tryGetPixel(bmp, x + 1, y) != cBlack && tryGetPixel(bmp, x - 1, y) != cBlack &&
							tryGetPixel(bmp, x + 1, y + 1) != cBlack && tryGetPixel(bmp, x + 1, y - 1) == cBlack &&
							tryGetPixel(bmp, x - 1, y + 1) != cBlack && tryGetPixel(bmp, x - 1, y - 1) != cBlack)
						{
							return System.Drawing.Color.FromArgb(0, 100, 0);
						}

						// bottom right
						if (tryGetPixel(bmp, x + 1, y) != cBlack && tryGetPixel(bmp, x - 1, y) != cBlack &&
							tryGetPixel(bmp, x + 1, y + 1) != cBlack && tryGetPixel(bmp, x + 1, y - 1) != cBlack &&
							tryGetPixel(bmp, x - 1, y + 1) != cBlack && tryGetPixel(bmp, x - 1, y - 1) == cBlack)
						{
							return System.Drawing.Color.FromArgb(0, 255, 255);
						}

						// top left
						if (tryGetPixel(bmp, x + 1, y) != cBlack && tryGetPixel(bmp, x - 1, y) != cBlack &&
							tryGetPixel(bmp, x + 1, y + 1) == cBlack && tryGetPixel(bmp, x + 1, y - 1) != cBlack &&
							tryGetPixel(bmp, x - 1, y + 1) != cBlack && tryGetPixel(bmp, x - 1, y - 1) != cBlack)
						{
							return System.Drawing.Color.FromArgb(100, 0, 0);
						}

						// top right
						if (tryGetPixel(bmp, x + 1, y) != cBlack && tryGetPixel(bmp, x - 1, y) != cBlack &&
							tryGetPixel(bmp, x + 1, y + 1) != cBlack && tryGetPixel(bmp, x + 1, y - 1) != cBlack &&
							tryGetPixel(bmp, x - 1, y + 1) == cBlack && tryGetPixel(bmp, x - 1, y - 1) != cBlack)
						{
							return System.Drawing.Color.FromArgb(255, 0, 255);
						}

					}

				}

				

				

				

				

				

				

				

				

			}

			return bmp.GetPixel(x, y);

		}

		private static System.Drawing.Color tryGetPixel(Bitmap bmp, int x, int y)
		{
			if (x < 0 || x >= bmp.Width || y < 0 || y >= bmp.Height)
			{
				return System.Drawing.Color.FromArgb(255, 255, 255, 255);
			}
			else
			{
				return bmp.GetPixel(x, y);
			}
		}
	}


	public struct Tile
	{
		public ArrayList gameTextures;
		public int Rotation;
		public bool Enabled;
		public bool Collidable;

		public Tile(int p_Rotation, bool p_Enabled, bool p_Collidable)
		{
			gameTextures = new ArrayList();
			gameTextures.Add(null);
			Rotation = p_Rotation;
			Enabled = p_Enabled;
			Collidable = p_Collidable;
		}

		public Tile(ArrayList p_GameTextures, int p_Rotation, bool p_Enabled, bool p_Collidable)
		{
			gameTextures = p_GameTextures;
			Rotation = p_Rotation;
			Enabled = p_Enabled;
			Collidable = p_Collidable;
		}

		public Tile(GameTexture p_GameTexture, int p_Rotation, bool p_Enabled, bool p_Collidable)
		{
			gameTextures = new ArrayList();
			gameTextures.Add(p_GameTexture);
			Rotation = p_Rotation;
			Enabled = p_Enabled;
			Collidable = p_Collidable;
		}
	}
}
