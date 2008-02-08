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
		private static int m_ScreenSpaceWidth = 32;
		private static int m_ScreenSpaceHeight = 25;
		public static int TileCount
		{
			get
			{
				return m_ScreenSpaceHeight * m_ScreenSpaceWidth;
			}
		}

		private Level m_CurrentLevel;
		private Level m_NextLevel;

		private List<Sprite> m_CurrentView;
		public List<Sprite> CurrentView
		{
			get { return m_CurrentView; }
		}
		private int m_CurTopRow;
		private int m_CurTopBuffer;
		private int m_CurBottomBuffer;
		private static int m_TileDimension = 32;
		public static int TileDimension
		{
			get { return m_TileDimension; }
		}

		System.Threading.Thread BackgroundLoadingThread;

		public EnvironmentLoader()
		{
			Colors.initDefaultMap();

			m_CurrentView = new List<Sprite>();
			for (int y = 0; y < m_ScreenSpaceHeight; y++)
			{
				for (int x = 0; x < m_ScreenSpaceWidth; x++)
				{
					Collidable temp = new Collidable("environment", new Vector2(x * m_TileDimension, (y - 1) * m_TileDimension), m_TileDimension, m_TileDimension, null, 1f, false, 0, Depth.GameLayer.Environment, Collidable.Factions.Environment, float.NaN, m_TileDimension * 0.5f);
					temp.Bound = Collidable.Boundings.Square;
					m_CurrentView.Add(temp);
				}
			}

			BackgroundLoadingThread = new System.Threading.Thread(WaitLoadNextFile);
			BackgroundLoadingThread.IsBackground = true;
			BackgroundLoadingThread.Name = "Background Loading Thread";
			BackgroundLoadingThread.Priority = System.Threading.ThreadPriority.Lowest;
			BackgroundLoadingThread.Start();
		}

		public void Update(GameTime p_GameTime)
		{
			for (int y = 0; y < m_ScreenSpaceHeight; y++)
			{
				for (int x = 0; x < m_ScreenSpaceWidth; x++)
				{
					Vector2 temp = m_CurrentView[getPosition(x, y)].Position;
					temp.Y += (World.Position.Speed * (float)p_GameTime.ElapsedGameTime.TotalSeconds);
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
					}

					m_CurrentView[getPosition(i, m_CurTopBuffer)].Position = new Vector2(i * m_TileDimension, m_CurrentView[getPosition(i, (m_CurTopBuffer + 1) % m_ScreenSpaceHeight)].Position.Y - m_TileDimension);
					m_CurrentView[getPosition(i, m_CurTopBuffer)].Texture = m_CurrentLevel.TileArray[i, m_CurTopRow].getGameTexture();
					((Collidable)m_CurrentView[getPosition(i, m_CurTopBuffer)]).Faction = m_CurrentLevel.TileArray[i, m_CurTopRow].getFaction();
					m_CurrentView[getPosition(i, m_CurTopBuffer)].RotationDegrees = m_CurrentLevel.TileArray[i, m_CurTopRow].Rotation;
					m_CurrentView[getPosition(i, m_CurTopBuffer)].Enabled = m_CurrentLevel.TileArray[i, m_CurTopRow].Enabled;

				}

				m_CurTopRow--;
			}
		}
		private static int getPosition(int x, int y)
		{
			return ((y * m_ScreenSpaceWidth) + x);
		}



		// temporary flag
		private bool NothingInTheLevel = true;

		/// <summary>
		/// Redraw the entire screen, if the screen is empty, otherwise do nothing.
		/// Temporary until a better solution can be found.
		/// </summary>
		public void resetLevelIfEmpty()
		{
			if (NothingInTheLevel)
			{
				NothingInTheLevel = false;
				resetLevel();
			}
		}

		/// <summary>
		/// Redraw the entire screen, then continue from the bottom of the currently loaded bitmap.
		/// </summary>
		public void resetLevel()
		{

			for (int y = 0; y < m_ScreenSpaceHeight; y++)
			{
				for (int x = 0; x < m_ScreenSpaceWidth; x++)
				{

					m_CurrentView[getPosition(x, y)].Position = new Vector2(x * m_TileDimension, (y - 1) * m_TileDimension);
					m_CurrentView[getPosition(x, y)].Texture = m_CurrentLevel.TileArray[x, m_CurrentLevel.Height - 1].getGameTexture();
					((Collidable)m_CurrentView[getPosition(x, y)]).Faction = m_CurrentLevel.TileArray[x, m_CurrentLevel.Height - 1].getFaction();
					m_CurrentView[getPosition(x, y)].Rotation = m_CurrentLevel.TileArray[x, m_CurrentLevel.Height - 1].Rotation;
					m_CurrentView[getPosition(x, y)].Enabled = m_CurrentLevel.TileArray[x, m_CurrentLevel.Height - 1].Enabled;

				}
			}
			m_CurBottomBuffer = m_ScreenSpaceHeight - 1;
			m_CurTopBuffer = 0;

			m_CurTopRow = m_CurrentLevel.Height - 1;

		}



		/// <summary>
		/// Request the EnvironmentLoader to begin loading the given file in the background.
		/// Due to threading, calling this method more than once in a given time period will not result in productive behavior.
		/// </summary>
		/// <param name="p_FileName"></param>
		public void PleaseLoadNextFile(String p_FileName)
		{
			if (m_NextLevel != null)
			{
				throw new Exception("Possible Exception in PleaseLoadNextFile?");
			}
			m_NextLevel = new Level(p_FileName);
			lock (this) System.Threading.Monitor.Pulse(this);
		}

		private void WaitLoadNextFile()
		{
			string lastName = string.Empty;
			while (true)
			{
				if (m_NextLevel != null && lastName != m_NextLevel.LevelName)
				{
					lastName = m_NextLevel.LevelName;
					m_NextLevel.Load();
					lock (this) System.Threading.Monitor.Pulse(this);
				}
				do
				{
					lock (this) System.Threading.Monitor.Wait(this);
				} while (m_NextLevel == null || lastName == m_NextLevel.LevelName);

			}
		}

		/// <summary>
		/// Request the EnvironmentLoader to immediately begin reading environment information from the given file.
		/// If PleaseLoadNextFile has previously been called with the same filename, this transition will be almost instant.
		/// </summary>
		/// <param name="p_FileName"></param>
		public void NewFile(String p_FileName)
		{
			if (m_CurrentLevel == null || p_FileName != m_CurrentLevel.LevelName)
			{
				if (m_NextLevel != null && p_FileName == m_NextLevel.LevelName)
				{
					while (m_NextLevel.TileArray == null)
					{
						lock (this) System.Threading.Monitor.Wait(this);
					}
					m_CurrentLevel = m_NextLevel;
					m_NextLevel = null;
				}
				else
				{
					m_CurrentLevel = new Level(p_FileName);
					m_CurrentLevel.Load();
				}
			}
			m_CurTopRow = m_CurrentLevel.Height - 1;
		}

	}


	static class Colors
	{

		public static System.Drawing.Color color_Auto = System.Drawing.Color.FromArgb(255, 255, 255);
		public static System.Drawing.Color color_Empty = System.Drawing.Color.FromArgb(200, 200, 200);
		public static System.Drawing.Color color_Wall = System.Drawing.Color.FromArgb(0, 0, 0);

		public static System.Drawing.Color color_TopLeftInvert = System.Drawing.Color.FromArgb(200, 200, 255);
		public static System.Drawing.Color color_TopRightInvert = System.Drawing.Color.FromArgb(100, 100, 0);
		public static System.Drawing.Color color_Top = System.Drawing.Color.FromArgb(255, 100, 0);

		public static System.Drawing.Color color_BottomLeftInvert = System.Drawing.Color.FromArgb(255, 100, 255);
		public static System.Drawing.Color color_BottomRightInvert = System.Drawing.Color.FromArgb(0, 0, 255);
		public static System.Drawing.Color color_Bottom = System.Drawing.Color.FromArgb(255, 255, 0);

		public static System.Drawing.Color color_Left = System.Drawing.Color.FromArgb(0, 255, 0);
		public static System.Drawing.Color color_Right = System.Drawing.Color.FromArgb(255, 0, 0);

		public static System.Drawing.Color color_TopLeft = System.Drawing.Color.FromArgb(100, 0, 0);
		public static System.Drawing.Color color_BottomLeft = System.Drawing.Color.FromArgb(0, 100, 0);
		public static System.Drawing.Color color_TopRight = System.Drawing.Color.FromArgb(255, 0, 255);
		public static System.Drawing.Color color_BottomRight = System.Drawing.Color.FromArgb(0, 255, 255);

		public static Hashtable ColorMap;

		public static void initDefaultMap()
		{

			// Color mapping
			ColorMap = new Hashtable();

			ArrayList gts = new ArrayList();
			gts = new ArrayList();
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque", ""));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque2", ""));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque3", ""));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque4", ""));
			ColorMap.Add(color_Wall.ToArgb(), new Tile(gts, 0, true, true));
			//ColorMap.Add(System.Drawing.Color.FromArgb(255, 150, 100).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\wall_left", ""), 0, true, true));
			ColorMap.Add(color_Right.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_right", ""), 0, true, false));
			ColorMap.Add(color_Left.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_left", ""), 0, true, false));
			ColorMap.Add(color_Bottom.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm", ""), 0, true, false));
			ColorMap.Add(color_BottomRight.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_right", ""), 0, true, false));
			ColorMap.Add(color_BottomRightInvert.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_right_invert", ""), 0, true, false));
			ColorMap.Add(color_BottomLeft.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_left", ""), 0, true, false));
			ColorMap.Add(color_BottomLeftInvert.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_left_invert", ""), 0, true, false));
			ColorMap.Add(color_Top.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top", ""), 0, true, false));
			ColorMap.Add(color_TopRight.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_right", ""), 0, true, false));
			ColorMap.Add(color_TopRightInvert.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_right_invert", ""), 0, true, false));
			ColorMap.Add(color_TopLeft.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_left", ""), 0, true, false));
			ColorMap.Add(color_TopLeftInvert.ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_left_invert", ""), 0, true, false));
			//ColorMap.Add(System.Drawing.Color.FromArgb(100, 0, 100).ToArgb(), new Tile(TextureLibrary.getGameTexture("walls\\plaque_clear", ""), 0, true, false));
			ColorMap.Add(color_Empty.ToArgb(), new Tile(0, false, false));
			ColorMap.Add(color_Auto.ToArgb(), new Tile(0, false, false));

		}

	}




	class Level
	{
		public string LevelName;
		public int Height;
		public int Width;
		public Tile[,] TileArray;

		private System.Drawing.Color[,] m_LevelArray;

#if DEBUG
		private static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
#endif

		public Level(string p_FileName)
		{
			LevelName = p_FileName;
		}

		public void Load()
		{
#if DEBUG
			stopwatch.Start();
#endif
			using (Bitmap bmp = new Bitmap(LevelName))
			{
				Height = bmp.Height;
				Width = bmp.Width;
				m_LevelArray = new System.Drawing.Color[Width, Height];
				for (int height = 0; height < Height; ++height)
				{
					for (int width = 0; width < Width; ++width)
					{
						m_LevelArray[width, height] = bmp.GetPixel(width, height);
					}
				}
			}
			TileArray = new Tile[Width, Height];
			System.Drawing.Color thisColor;
			for (int height = Height - 1; height >= 0; --height)
			{
				for (int width = Width - 1; width >= 0; --width)
				{

					processPixel(width, height);

					thisColor = m_LevelArray[width, height];

					if (Colors.ColorMap.ContainsKey(thisColor.ToArgb()))
					{
						TileArray[width, height] = (Tile)Colors.ColorMap[thisColor.ToArgb()];
					}
					else
					{
						TileArray[width, height] = (Tile)Colors.ColorMap[Colors.color_Empty.ToArgb()];
					}

				}
			}
#if DEBUG
			stopwatch.Stop();
			int p = LevelName.LastIndexOf("\\") + 1;
			Console.WriteLine("> Read in " + LevelName.Substring(p, LevelName.Length - p) + " in " + stopwatch.Elapsed.TotalMilliseconds + " milliseconds.");
			stopwatch.Reset();
#endif
		}

		private void processPixel(int x, int y)
		{
			if (m_LevelArray[x, y] == Colors.color_Auto)
			{
				if (tryGetPixel(x, y + 1) == Colors.color_Wall)
				{
					if (tryGetPixel(x, y - 1) != Colors.color_Wall)
					{
						if (tryGetPixel(x + 1, y) == Colors.color_Wall)
						{
							if (tryGetPixel(x - 1, y) != Colors.color_Wall)
							{
								m_LevelArray[x, y] = Colors.color_TopLeftInvert;
								return;
							}
						}
						else
						{
							if (tryGetPixel(x - 1, y) == Colors.color_Wall)
							{
								m_LevelArray[x, y] = Colors.color_TopRightInvert;
								return;
							}
							else
							{
								m_LevelArray[x, y] = Colors.color_Top;
								return;
							}
						}
					}
				}
				else
				{
					if (tryGetPixel(x, y - 1) == Colors.color_Wall)
					{
						if (tryGetPixel(x + 1, y) == Colors.color_Wall)
						{
							if (tryGetPixel(x - 1, y) != Colors.color_Wall)
							{
								m_LevelArray[x, y] = Colors.color_BottomLeftInvert;
								return;
							}
						}
						else
						{
							if (tryGetPixel(x - 1, y) == Colors.color_Wall)
							{
								m_LevelArray[x, y] = Colors.color_BottomRightInvert;
								return;
							}
							else
							{
								m_LevelArray[x, y] = Colors.color_Bottom;
								return;
							}
						}
					}
					else
					{
						if (tryGetPixel(x + 1, y) == Colors.color_Wall)
						{
							if (tryGetPixel(x - 1, y) != Colors.color_Wall)
							{
								m_LevelArray[x, y] = Colors.color_Left;
								return;
							}
						}
						else
						{
							if (tryGetPixel(x - 1, y) == Colors.color_Wall)
							{
								m_LevelArray[x, y] = Colors.color_Right;
								return;
							}
							else
							{
								if (tryGetPixel(x - 1, y - 1) == Colors.color_Wall)
								{
									if (tryGetPixel(x + 1, y + 1) != Colors.color_Wall && tryGetPixel(x + 1, y - 1) != Colors.color_Wall &&
										tryGetPixel(x - 1, y + 1) != Colors.color_Wall)
									{
										m_LevelArray[x, y] = Colors.color_BottomRight;
										return;
									}
								}
								else
								{
									if (tryGetPixel(x - 1, y + 1) == Colors.color_Wall)
									{
										if (tryGetPixel(x + 1, y + 1) != Colors.color_Wall && tryGetPixel(x + 1, y - 1) != Colors.color_Wall)
										{
											m_LevelArray[x, y] = Colors.color_TopRight;
											return;
										}
									}
									else
									{
										if (tryGetPixel(x + 1, y + 1) == Colors.color_Wall)
										{
											if (tryGetPixel(x + 1, y - 1) != Colors.color_Wall)
											{
												m_LevelArray[x, y] = Colors.color_TopLeft;
												return;
											}
										}
										else
										{
											if (tryGetPixel(x + 1, y + 1) != Colors.color_Wall && tryGetPixel(x + 1, y - 1) == Colors.color_Wall)
											{
												m_LevelArray[x, y] = Colors.color_BottomLeft;
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
			if (x < 0 || x >= Width || y < 0 || y >= Height)
			{
				return Colors.color_Empty;
			}
			else
			{
				return m_LevelArray[x, y];
			}
		}

	}


	class Tile
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
