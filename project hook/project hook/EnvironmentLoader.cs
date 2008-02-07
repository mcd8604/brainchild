using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	public sealed class EnvironmentLoader
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
		private string m_LevelName;
		private int AWidth;
		private int AHeight;
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

		private static System.Drawing.Color color_Auto = System.Drawing.Color.FromArgb(255, 255, 255);
		private static System.Drawing.Color color_Empty = System.Drawing.Color.FromArgb(200, 200, 200);
		private static System.Drawing.Color color_Wall = System.Drawing.Color.FromArgb(0, 0, 0);

		private static System.Drawing.Color color_TopLeftInvert = System.Drawing.Color.FromArgb(200, 200, 255);
		private static System.Drawing.Color color_TopRightInvert = System.Drawing.Color.FromArgb(100, 100, 0);
		private static System.Drawing.Color color_Top = System.Drawing.Color.FromArgb(255, 100, 0);

		private static System.Drawing.Color color_BottomLeftInvert = System.Drawing.Color.FromArgb(255, 100, 255);
		private static System.Drawing.Color color_BottomRightInvert = System.Drawing.Color.FromArgb(0, 0, 255);
		private static System.Drawing.Color color_Bottom = System.Drawing.Color.FromArgb(255, 255, 0);

		private static System.Drawing.Color color_Left = System.Drawing.Color.FromArgb(0, 255, 0);
		private static System.Drawing.Color color_Right = System.Drawing.Color.FromArgb(255, 0, 0);

		private static System.Drawing.Color color_TopLeft = System.Drawing.Color.FromArgb(100, 0, 0);
		private static System.Drawing.Color color_BottomLeft = System.Drawing.Color.FromArgb(0, 100, 0);
		private static System.Drawing.Color color_TopRight = System.Drawing.Color.FromArgb(255, 0, 255);
		private static System.Drawing.Color color_BottomRight = System.Drawing.Color.FromArgb(0, 255, 255);

		public EnvironmentLoader(WorldPosition p_Position)
		{

			// Set up variables
			m_Position = p_Position;

			// Color mapping
			m_ColorMap = new Hashtable();

			ArrayList gts = new ArrayList();
			gts = new ArrayList();
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque", ""));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque2", ""));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque3", ""));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque4", ""));
			m_ColorMap.Add(color_Wall.ToArgb(), new Tile(gts, 0, true, true));
			//m_ColorMap.Add(System.Drawing.Color.FromArgb(255, 150, 100).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\wall_left", ""), 0, true, true));
			m_ColorMap.Add(color_Right.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_right", ""), 0, true, false));
			m_ColorMap.Add(color_Left.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_left", ""), 0, true, false));
			m_ColorMap.Add(color_Bottom.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm", ""), 0, true, false));
			m_ColorMap.Add(color_BottomRight.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_right", ""), 0, true, false));
			m_ColorMap.Add(color_BottomRightInvert.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_right_invert", ""), 0, true, false));
			m_ColorMap.Add(color_BottomLeft.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_left", ""), 0, true, false));
			m_ColorMap.Add(color_BottomLeftInvert.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_left_invert", ""), 0, true, false));
			m_ColorMap.Add(color_Top.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top", ""), 0, true, false));
			m_ColorMap.Add(color_TopRight.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_right", ""), 0, true, false));
			m_ColorMap.Add(color_TopRightInvert.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_right_invert", ""), 0, true, false));
			m_ColorMap.Add(color_TopLeft.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_left", ""), 0, true, false));
			m_ColorMap.Add(color_TopLeftInvert.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_left_invert", ""), 0, true, false));
			//m_ColorMap.Add(System.Drawing.Color.FromArgb(100, 0, 100).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_clear", ""), 0, true, false));
			m_ColorMap.Add(color_Empty.ToArgb(), new Tile(0, false, false));
			m_ColorMap.Add(color_Auto.ToArgb(), new Tile(0, false, false));
		}

		public List<Sprite> Initialize(string p_FileName)
		{

			// read in level
			readFile(p_FileName);

			m_ScreenSpaceWidth = AWidth;
			m_TileDimension = Game.graphics.GraphicsDevice.Viewport.Width / m_ScreenSpaceWidth;
			m_ScreenSpaceHeight = (Game.graphics.GraphicsDevice.Viewport.Height / m_TileDimension) + 1;

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

			// create initial screen
			for (int y = 0; y < m_ScreenSpaceHeight; y++)
			{
				for (int x = 0; x < m_ScreenSpaceWidth; x++)
				{
					if (m_ColorMap.ContainsKey(m_LevelArray[x, AHeight - m_ScreenSpaceHeight + y].ToArgb()))
					{
						curTile = ((Tile)m_ColorMap[m_LevelArray[x, AHeight - m_ScreenSpaceHeight + y].ToArgb()]);

						m_CurrentView[getPosition(x, y)].Texture = curTile.getGameTexture();
						((Collidable)m_CurrentView[getPosition(x, y)]).Faction = curTile.getFaction();
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

			m_CurTopRow = AHeight - m_ScreenSpaceHeight - 1;
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

						m_CurrentView[getPosition(i, m_CurTopBuffer)].Texture = curTile.getGameTexture();
						((Collidable)m_CurrentView[getPosition(i, m_CurTopBuffer)]).Faction = curTile.getFaction();
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
			// read in level
			if (p_FileName != m_LevelName)
			{
				readFile(p_FileName);
			}

			m_CurTopRow = AHeight - 1;
		}

		public void resetEnvironment()
		{
			foreach (Sprite s in m_CurrentView)
			{
				s.Enabled = false;
			}

			m_CurTopRow = AHeight - 1;
		}

#if DEBUG
		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
#endif

		private void readFile(string p_FileName)
		{
			m_LevelName = p_FileName;
#if DEBUG
			stopwatch.Start();
#endif
			using (Bitmap bmp = new Bitmap(p_FileName))
			{
				AHeight = bmp.Height;
				AWidth = bmp.Width;
				m_LevelArray = new System.Drawing.Color[AWidth, AHeight];

				for (int height = 0; height < AHeight; ++height)
				{
					for (int width = 0; width < AWidth; ++width)
					{
						m_LevelArray[width, height] = bmp.GetPixel(width, height);
					}
				}
			}
#if DEBUG
			stopwatch.Stop();
			int p = p_FileName.LastIndexOf("\\") + 1;
			Console.WriteLine("> Read in " + p_FileName.Substring(p, p_FileName.Length - p) + " in " + stopwatch.Elapsed.TotalMilliseconds + " milliseconds.");
			stopwatch.Reset();
			stopwatch.Start();
#endif
			for (int height = AHeight - 1; height >= 0; --height)
			{
				for (int width = AWidth - 1; width >= 0; --width)
				{
					processPixel(width, height);
				}
			}
#if DEBUG
			stopwatch.Stop();
			Console.WriteLine("> Processed level in " + stopwatch.Elapsed.TotalMilliseconds + " milliseconds.");
			stopwatch.Reset();
#endif

			// Test



		}

		private void processPixel(int x, int y)
		{
			if (m_LevelArray[x, y] == color_Auto)
			{
				if (tryGetPixel(x, y + 1) == color_Wall)
				{
					if (tryGetPixel(x, y - 1) != color_Wall)
					{
						if (tryGetPixel(x + 1, y) == color_Wall)
						{
							if (tryGetPixel(x - 1, y) != color_Wall)
							{
								m_LevelArray[x, y] = color_TopLeftInvert;
								return;
							}
						}
						else
						{
							if (tryGetPixel(x - 1, y) == color_Wall)
							{
								m_LevelArray[x, y] = color_TopRightInvert;
								return;
							}
							else
							{
								m_LevelArray[x, y] = color_Top;
								return;
							}
						}
					}
				}
				else
				{
					if (tryGetPixel(x, y - 1) == color_Wall)
					{
						if (tryGetPixel(x + 1, y) == color_Wall)
						{
							if (tryGetPixel(x - 1, y) != color_Wall)
							{
								m_LevelArray[x, y] = color_BottomLeftInvert;
								return;
							}
						}
						else
						{
							if (tryGetPixel(x - 1, y) == color_Wall)
							{
								m_LevelArray[x, y] = color_BottomRightInvert;
								return;
							}
							else
							{
								m_LevelArray[x, y] = color_Bottom;
								return;
							}
						}
					}
					else
					{
						if (tryGetPixel(x + 1, y) == color_Wall)
						{
							if (tryGetPixel(x - 1, y) != color_Wall)
							{
								m_LevelArray[x, y] = color_Left;
								return;
							}
						}
						else
						{
							if (tryGetPixel(x - 1, y) == color_Wall)
							{
								m_LevelArray[x, y] = color_Right;
								return;
							}
							else
							{
								if (tryGetPixel(x - 1, y - 1) == color_Wall)
								{
									if (tryGetPixel(x + 1, y + 1) != color_Wall && tryGetPixel(x + 1, y - 1) != color_Wall &&
										tryGetPixel(x - 1, y + 1) != color_Wall)
									{
										m_LevelArray[x, y] = color_BottomRight;
										return;
									}
								}
								else
								{
									if (tryGetPixel(x - 1, y + 1) == color_Wall)
									{
										if (tryGetPixel(x + 1, y + 1) != color_Wall && tryGetPixel(x + 1, y - 1) != color_Wall)
										{
											m_LevelArray[x, y] = color_TopRight;
											return;
										}
									}
									else
									{
										if (tryGetPixel(x + 1, y + 1) == color_Wall)
										{
											if (tryGetPixel(x + 1, y - 1) != color_Wall)
											{
												m_LevelArray[x, y] = color_TopLeft;
												return;
											}
										}
										else
										{
											if (tryGetPixel(x + 1, y + 1) != color_Wall && tryGetPixel(x + 1, y - 1) == color_Wall)
											{
												m_LevelArray[x, y] = color_BottomLeft;
												return;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		private System.Drawing.Color tryGetPixel(int x, int y)
		{
			if (x < 0 || x >= AWidth || y < 0 || y >= AHeight)
			{
				return color_Empty;
			}
			else
			{
				return m_LevelArray[x, y];
			}
		}
	}

	public struct Tile
	{

#if DEBUG
		private static Random random = new Random(0);
#else 
        private static Random random = new Random();
#endif

		public ArrayList gameTextures;
		public GameTexture gameTexture;
		public int Rotation;
		public bool Enabled;
		public bool m_Collidable;

		public Tile(int p_Rotation, bool p_Enabled, bool p_Collidable)
		{
			Rotation = p_Rotation;
			Enabled = p_Enabled;
			m_Collidable = p_Collidable;
			gameTextures = null;
			gameTexture = null;
		}

		public Tile(ArrayList p_GameTextures, int p_Rotation, bool p_Enabled, bool p_Collidable)
		{
			gameTextures = p_GameTextures;
			Rotation = p_Rotation;
			Enabled = p_Enabled;
			m_Collidable = p_Collidable;
			gameTexture = null;
		}

		public Tile(GameTexture p_GameTexture, int p_Rotation, bool p_Enabled, bool p_Collidable)
		{
			gameTexture = p_GameTexture;
			Rotation = p_Rotation;
			Enabled = p_Enabled;
			m_Collidable = p_Collidable;
			gameTextures = null;
		}

		public Collidable.Factions getFaction()
		{
			if (m_Collidable)
			{
				return Collidable.Factions.Environment;
			}
			else
			{
				return Collidable.Factions.None;
			}
		}

		public GameTexture getGameTexture()
		{
			if (gameTextures == null)
			{
				return gameTexture;
			}
			else
			{
				return (GameTexture)gameTextures[random.Next(0, gameTextures.Count)];
			}
		}
	}
}
