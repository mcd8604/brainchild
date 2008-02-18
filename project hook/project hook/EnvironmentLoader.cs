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
			Mapping.initDefaultMap();

			m_CurrentView = new List<Sprite>();
			for (int y = 0; y < m_ScreenSpaceHeight; y++)
			{
				for (int x = 0; x < m_ScreenSpaceWidth; x++)
				{
					Collidable temp = new Collidable(
#if !FINAL
						"environment",
#endif
						new Vector2(x * m_TileDimension, (y - 1) * m_TileDimension), m_TileDimension, m_TileDimension, null, 1f, false, 0, Depth.GameLayer.Environment, Collidable.Factions.Environment, float.NaN, m_TileDimension * 0.5f);
					temp.Bound = Collidable.Boundings.Square;
					m_CurrentView.Add(temp);
				}
			}

			BackgroundLoadingThread = new System.Threading.Thread(delegate()
			{
				string lastName = string.Empty;
				do
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

				} while (true);
			});
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
					m_CurrentView[getPosition(i, m_CurTopBuffer)].Texture = m_CurrentLevel.TileArray[i, m_CurTopRow].GameTexture;
					((Collidable)m_CurrentView[getPosition(i, m_CurTopBuffer)]).Faction = m_CurrentLevel.TileArray[i, m_CurTopRow].Faction;
					//m_CurrentView[getPosition(i, m_CurTopBuffer)].RotationDegrees = m_CurrentLevel.TileArray[i, m_CurTopRow].Rotation;
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
					m_CurrentView[getPosition(x, y)].Texture = m_CurrentLevel.TileArray[x, m_CurrentLevel.Height - 1].GameTexture;
					((Collidable)m_CurrentView[getPosition(x, y)]).Faction = m_CurrentLevel.TileArray[x, m_CurrentLevel.Height - 1].Faction;
					//m_CurrentView[getPosition(x, y)].Rotation = m_CurrentLevel.TileArray[x, m_CurrentLevel.Height - 1].Rotation;
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
		/// 
		/// Actual Timing Data:
		/// On my home Computer, this method always returns in less than 1 milliseconds.
		/// </summary>
		/// <param name="p_FileName">The Absolute FileName</param>
		public void PleaseLoadNextFile(String p_FileName)
		{
#if DEBUG
			if (m_NextLevel != null)
			{
				throw new Exception("Possible Exception in PleaseLoadNextFile?");
			}
#endif
			m_NextLevel = new Level(p_FileName);
			lock (this) System.Threading.Monitor.Pulse(this);
		}

		/// <summary>
		/// Request the EnvironmentLoader to immediately begin reading environment information from the given file.
		/// If PleaseLoadNextFile has previously been called with the same filename, this transition will be almost instant.
		/// 
		/// Actual Timing Data:
		/// On my home Computer, this method took 30 milliseconds to read in Level3.bmp;
		/// If PleaseLoadNextfile was called in advance, it takes less than 1 milliseconds.
		/// </summary>
		/// <param name="p_FileName">The Absolute Filename</param>
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


	static class Mapping
	{

		public static readonly System.Drawing.Color color_Auto = System.Drawing.Color.FromArgb(255, 255, 255);
		public static readonly System.Drawing.Color color_Empty = System.Drawing.Color.FromArgb(200, 200, 200);
		public static readonly System.Drawing.Color color_Fake = System.Drawing.Color.FromArgb(100, 100, 100);
		public static readonly System.Drawing.Color color_Solid = System.Drawing.Color.FromArgb(50, 50, 50);
		public static readonly System.Drawing.Color color_Wall = System.Drawing.Color.FromArgb(0, 0, 0);

		public static readonly System.Drawing.Color color_TopLeftInvert = System.Drawing.Color.FromArgb(200, 200, 255);
		public static readonly System.Drawing.Color color_TopRightInvert = System.Drawing.Color.FromArgb(100, 100, 0);
		public static readonly System.Drawing.Color color_Top = System.Drawing.Color.FromArgb(255, 100, 0);

		public static readonly System.Drawing.Color color_BottomLeftInvert = System.Drawing.Color.FromArgb(255, 100, 255);
		public static readonly System.Drawing.Color color_BottomRightInvert = System.Drawing.Color.FromArgb(0, 0, 255);
		public static readonly System.Drawing.Color color_Bottom = System.Drawing.Color.FromArgb(255, 255, 0);

		public static readonly System.Drawing.Color color_Left = System.Drawing.Color.FromArgb(0, 255, 0);
		public static readonly System.Drawing.Color color_Right = System.Drawing.Color.FromArgb(255, 0, 0);

		public static readonly System.Drawing.Color color_TopLeft = System.Drawing.Color.FromArgb(100, 0, 0);
		public static readonly System.Drawing.Color color_BottomLeft = System.Drawing.Color.FromArgb(0, 100, 0);
		public static readonly System.Drawing.Color color_TopRight = System.Drawing.Color.FromArgb(255, 0, 255);
		public static readonly System.Drawing.Color color_BottomRight = System.Drawing.Color.FromArgb(0, 255, 255);


		public static Tile tile_Empty = new Tile();
		public static Tile tile_Fake;
		public static Tile tile_Wall;

		public static Tile tile_TopLeftInvert;
		public static Tile tile_TopRightInvert;
		public static Tile tile_Top;
		public static Tile tile_BottomLeftInvert;
		public static Tile tile_BottomRightInvert;
		public static Tile tile_Bottom;
		public static Tile tile_Left;
		public static Tile tile_Right;
		public static Tile tile_TopLeft;
		public static Tile tile_BottomLeft;
		public static Tile tile_TopRight;
		public static Tile tile_BottomRight;


		public static Hashtable ColorMap;

		public static void initDefaultMap()
		{

			// Color mapping
			ColorMap = new Hashtable(15);

			ArrayList gts = new ArrayList(4);
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque", ""));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque2", ""));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque3", ""));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque4", ""));
			tile_Wall = new Tile(gts, true);
			tile_Fake = new Tile(gts, false);

			tile_Right = new Tile(TextureLibrary.getGameTexture("walls\\plaque_right", ""), false);
			tile_Left = new Tile(TextureLibrary.getGameTexture("walls\\plaque_left", ""), false);
			tile_Bottom = new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm", ""), false);
			tile_BottomRight = new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_right", ""), false);
			tile_BottomRightInvert = new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_right_invert", ""), false);
			tile_BottomLeft = new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_left", ""), false);
			tile_BottomLeftInvert = new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_left_invert", ""), false);
			tile_Top = new Tile(TextureLibrary.getGameTexture("walls\\plaque_top", ""), false);
			tile_TopRight = new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_right", ""), false);
			tile_TopRightInvert = new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_right_invert", ""), false);
			tile_TopLeft = new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_left", ""), false);
			tile_TopLeftInvert = new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_left_invert", ""), false);

			ColorMap.Add(color_Wall.ToArgb(), tile_Wall);
			ColorMap.Add(color_Solid.ToArgb(), tile_Wall);
			ColorMap.Add(color_Fake.ToArgb(), tile_Fake);
			ColorMap.Add(color_Right.ToArgb(), tile_Right);
			ColorMap.Add(color_Left.ToArgb(), tile_Left);
			ColorMap.Add(color_Bottom.ToArgb(), tile_Bottom);
			ColorMap.Add(color_BottomRight.ToArgb(), tile_BottomRight);
			ColorMap.Add(color_BottomRightInvert.ToArgb(), tile_BottomRightInvert);
			ColorMap.Add(color_BottomLeft.ToArgb(), tile_BottomLeft);
			ColorMap.Add(color_BottomLeftInvert.ToArgb(), tile_BottomLeftInvert);
			ColorMap.Add(color_Top.ToArgb(), tile_Top);
			ColorMap.Add(color_TopRight.ToArgb(), tile_TopRight);
			ColorMap.Add(color_TopRightInvert.ToArgb(), tile_TopRightInvert);
			ColorMap.Add(color_TopLeft.ToArgb(), tile_TopLeft);
			ColorMap.Add(color_TopLeftInvert.ToArgb(), tile_TopLeftInvert);
			ColorMap.Add(color_Empty.ToArgb(), tile_Empty);
			ColorMap.Add(color_Auto.ToArgb(), tile_Empty);

		}

	}



	class Level
	{
		public string LevelName;
		public int Height;
		public int Width;
		public Tile[,] TileArray;

		private System.Drawing.Color[,] m_LevelArray;

#if TIME
		private static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
#endif

		public Level(string p_FileName)
		{
			LevelName = p_FileName;
		}

		public void Load()
		{
#if TIME
			stopwatch.Start();
#endif
			using (Bitmap bmp = new Bitmap(LevelName))
			{
				Height = bmp.Height;
				Width = bmp.Width;
#if TESTING
				// Testing
				Game.Out.WriteLine("PF: " + bmp.PixelFormat + " " + (bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed));
				if (bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed)
				{
					byte[] bytes = (byte[])System.ComponentModel.TypeDescriptor.GetConverter(bmp).ConvertTo(bmp, typeof(byte[])); // 1 - 2 ms
					Game.Out.WriteLine(bytes.Length + ", " + ((bytes.Length - 62) * 8) + ", " + (((bytes.Length - 62) * 8) == (Height * Width)));
					BitArray t = new BitArray(bytes);
					TileArray = new Tile[Width, Height];
					for (int height = 0; height < Height; ++height)
					{
						for (int width = 0; width < Width; ++width)
						{
							if (t[((height * Width) + width)])
							{
								TileArray[width, height] = (Tile)Colors.ColorMap[Colors.color_Empty.ToArgb()];
							}
							else
							{
								TileArray[width, height] = (Tile)Colors.ColorMap[Colors.color_Wall.ToArgb()];

							}

						}
					}

					//testing
#if TIME
					stopwatch.Stop();
					int x = LevelName.LastIndexOf("\\") + 1;
					Game.Out.WriteLine("> Testing " + LevelName.Substring(x, LevelName.Length - x) + " in " + stopwatch.Elapsed.TotalMilliseconds + " milliseconds.");
					stopwatch.Reset();
					stopwatch.Start();
#endif
				}
#endif



				m_LevelArray = new System.Drawing.Color[Width, Height];
				for (int height = 0; height < Height; ++height)
				{
					for (int width = 0; width < Width; ++width)
					{
						m_LevelArray[width, height] = bmp.GetPixel(width, height);
					}
				}

				TileArray = new Tile[Width, Height];
				System.Drawing.Color thisColor;
				for (int y = Height - 1; y >= 0; --y)
				{
					for (int x = Width - 1; x >= 0; --x)
					{

						thisColor = m_LevelArray[x, y];

						if (thisColor == Mapping.color_Auto)
						{
							if (y + 1 < Height && m_LevelArray[x, y + 1] == Mapping.color_Wall)
							{
								if (y == 0 || m_LevelArray[x, y - 1] != Mapping.color_Wall)
								{
									if (x + 1 < Width && m_LevelArray[x + 1, y] == Mapping.color_Wall)
									{
										if (x == 0 || m_LevelArray[x - 1, y] != Mapping.color_Wall)
										{
											TileArray[x, y] = Mapping.tile_TopLeftInvert;
											continue;
										}
									}
									else
									{
										if (x > 0 && m_LevelArray[x - 1, y] == Mapping.color_Wall)
										{
											TileArray[x, y] = Mapping.tile_TopRightInvert;
											continue;
										}
										else
										{
											TileArray[x, y] = Mapping.tile_Top;
											continue;
										}
									}
								}
							}
							else
							{
								if (y > 0 && m_LevelArray[x, y - 1] == Mapping.color_Wall)
								{
									if (x + 1 < Width && m_LevelArray[x + 1, y] == Mapping.color_Wall)
									{
										if (x == 0 || m_LevelArray[x - 1, y] != Mapping.color_Wall)
										{
											TileArray[x, y] = Mapping.tile_BottomLeftInvert;
											continue;
										}
									}
									else
									{
										if (x > 0 && m_LevelArray[x - 1, y] == Mapping.color_Wall)
										{
											TileArray[x, y] = Mapping.tile_BottomRightInvert;
											continue;
										}
										else
										{
											TileArray[x, y] = Mapping.tile_Bottom;
											continue;
										}
									}
								}
								else
								{
									if (x + 1 < Width && m_LevelArray[x + 1, y] == Mapping.color_Wall)
									{
										if (x == 0 || m_LevelArray[x - 1, y] != Mapping.color_Wall)
										{
											TileArray[x, y] = Mapping.tile_Left;
											continue;
										}
									}
									else
									{
										if (x > 0 && m_LevelArray[x - 1, y] == Mapping.color_Wall)
										{
											TileArray[x, y] = Mapping.tile_Right;
											continue;
										}
										else
										{
											if (x > 0 && y > 0 && m_LevelArray[x - 1, y - 1] == Mapping.color_Wall)
											{
												if ((x + 1 == Width || y + 1 == Height || m_LevelArray[x + 1, y + 1] != Mapping.color_Wall) &&
													(x + 1 == Width || y == 0 || m_LevelArray[x + 1, y - 1] != Mapping.color_Wall) &&
													(x == 0 || y + 1 == Height || m_LevelArray[x - 1, y + 1] != Mapping.color_Wall))
												{
													TileArray[x, y] = Mapping.tile_BottomRight;
													continue;
												}
											}
											else
											{
												if (x > 0 && y + 1 < Height && m_LevelArray[x - 1, y + 1] == Mapping.color_Wall)
												{
													if ((x + 1 == Width || y + 1 == Height || m_LevelArray[x + 1, y + 1] != Mapping.color_Wall) &&
														(x + 1 == Width || y == 0 || m_LevelArray[x + 1, y - 1] != Mapping.color_Wall))
													{
														TileArray[x, y] = Mapping.tile_TopRight;
														continue;
													}
												}
												else
												{
													if (x + 1 < Width && y + 1 < Height && m_LevelArray[x + 1, y + 1] == Mapping.color_Wall)
													{
														if (x + 1 == Height || y == 0 || m_LevelArray[x + 1, y - 1] != Mapping.color_Wall)
														{
															TileArray[x, y] = Mapping.tile_TopLeft;
															continue;
														}
													}
													else
													{
														if (x + 1 < Width && y > 0 && m_LevelArray[x + 1, y - 1] == Mapping.color_Wall)
														{
															TileArray[x, y] = Mapping.tile_BottomLeft;
															continue;
														}
													}
												}
											}
										}
									}
								}
							}
						}
						else if ((thisColor == Mapping.color_Wall) &&
								(y + 1 == Height || m_LevelArray[x, y + 1] == Mapping.color_Wall) &&
								(y == 0 || m_LevelArray[x, y - 1] == Mapping.color_Wall) &&
								(x + 1 == Width || m_LevelArray[x + 1, y] == Mapping.color_Wall) &&
								(x == 0 || m_LevelArray[x - 1, y] == Mapping.color_Wall))
						{
							TileArray[x, y] = Mapping.tile_Fake;
							continue;
						}

						if (Mapping.ColorMap.ContainsKey(thisColor.ToArgb()))
						{
							TileArray[x, y] = (Tile)Mapping.ColorMap[thisColor.ToArgb()];
						}
						else
						{
							TileArray[x, y] = (Tile)Mapping.ColorMap[Mapping.color_Empty.ToArgb()];
						}

					}
				}
#if TIME
				stopwatch.Stop();
				int p = LevelName.LastIndexOf("\\") + 1;
				Game.Out.WriteLine("> Read in " + LevelName.Substring(p, LevelName.Length - p) + " in " + stopwatch.Elapsed.TotalMilliseconds + " milliseconds.");
				stopwatch.Reset();
#endif
			}
		}

	}


	class Tile
	{

		protected ArrayList m_gameTextures = null;
		protected GameTexture m_gameTexture = null;
		public GameTexture GameTexture
		{
			get
			{
				if (m_gameTextures == null)
				{
					return m_gameTexture;
				}
				else
				{
					return (GameTexture)m_gameTextures[Game.Random.Next(0, m_gameTextures.Count)];
				}
			}
		}
		protected bool m_Enabled = false;
		public bool Enabled
		{
			get { return m_Enabled; }
		}
		protected Collidable.Factions m_Faction = Collidable.Factions.None;
		public Collidable.Factions Faction
		{
			get { return m_Faction; }
		}
		//protected float m_Rotation = 0f;


		public Tile() { }
		public Tile(GameTexture p_GameTexture, bool p_Collidable)
		{
			m_gameTexture = p_GameTexture;
			m_Enabled = true;
			if (p_Collidable)
			{
				m_Faction = Collidable.Factions.Environment;
			}
			m_gameTextures = null;
			//m_Rotation = p_Rotation;
		}

		public Tile(ArrayList p_GameTextures, bool p_Collidable)
		{
			m_gameTextures = p_GameTextures;
			m_Enabled = true;
			if (p_Collidable)
			{
				m_Faction = Collidable.Factions.Environment;
			}
			m_gameTexture = null;
			//m_Rotation = p_Rotation;
		}

	}
}
