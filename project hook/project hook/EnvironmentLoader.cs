using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace project_hook
{
	internal sealed class EnvironmentLoader
	{
		internal const int ScreenSpaceWidth = 32;
		internal const int ScreenSpaceHeight = 25;

		internal const int TileDimension = 32;

		internal readonly EnvironmentSprite Environment;

		private Level m_CurrentLevel;
		private Level m_NextLevel;

		System.Threading.Thread BackgroundLoadingThread;

		internal EnvironmentLoader()
		{
			Mapping.initDefaultMap();

			Environment = new EnvironmentSprite();

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


		// temporary flag
		private bool NothingInTheLevel = true;

		/// <summary>
		/// Redraw the entire screen, if the screen is empty, otherwise do nothing.
		/// Temporary until a better solution can be found.
		/// </summary>
		internal void resetLevelIfEmpty()
		{
			if (NothingInTheLevel)
			{
				NothingInTheLevel = false;
				resetLevel();
			}
		}

		internal void resetLevel()
		{
			Environment.resetLevel();
		}



		/// <summary>
		/// Request the EnvironmentLoader to begin loading the given file in the background.
		/// Due to threading, calling this method more than once in a given time period will not result in productive behavior.
		/// 
		/// Actual Timing Data:
		/// On my home Computer, this method always returns in less than 1 milliseconds.
		/// </summary>
		/// <param name="p_FileName">The Absolute FileName</param>
		internal void PleaseLoadNextFile(String p_FileName)
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
		internal void NewFile(String p_FileName)
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
			Environment.changeLevel(m_CurrentLevel);
		}

	}


	static class Mapping
	{

		internal static readonly System.Drawing.Color color_Auto = System.Drawing.Color.FromArgb(255, 255, 255);
		internal static readonly System.Drawing.Color color_Empty = System.Drawing.Color.FromArgb(200, 200, 200);
		internal static readonly System.Drawing.Color color_Fake = System.Drawing.Color.FromArgb(100, 100, 100);
		internal static readonly System.Drawing.Color color_Solid = System.Drawing.Color.FromArgb(50, 50, 50);
		internal static readonly System.Drawing.Color color_Wall = System.Drawing.Color.FromArgb(0, 0, 0);

		internal static readonly System.Drawing.Color color_TopLeftInvert = System.Drawing.Color.FromArgb(200, 200, 255);
		internal static readonly System.Drawing.Color color_TopRightInvert = System.Drawing.Color.FromArgb(100, 100, 0);
		internal static readonly System.Drawing.Color color_Top = System.Drawing.Color.FromArgb(255, 100, 0);

		internal static readonly System.Drawing.Color color_BottomLeftInvert = System.Drawing.Color.FromArgb(255, 100, 255);
		internal static readonly System.Drawing.Color color_BottomRightInvert = System.Drawing.Color.FromArgb(0, 0, 255);
		internal static readonly System.Drawing.Color color_Bottom = System.Drawing.Color.FromArgb(255, 255, 0);

		internal static readonly System.Drawing.Color color_Left = System.Drawing.Color.FromArgb(0, 255, 0);
		internal static readonly System.Drawing.Color color_Right = System.Drawing.Color.FromArgb(255, 0, 0);

		internal static readonly System.Drawing.Color color_TopLeft = System.Drawing.Color.FromArgb(100, 0, 0);
		internal static readonly System.Drawing.Color color_BottomLeft = System.Drawing.Color.FromArgb(0, 100, 0);
		internal static readonly System.Drawing.Color color_TopRight = System.Drawing.Color.FromArgb(255, 0, 255);
		internal static readonly System.Drawing.Color color_BottomRight = System.Drawing.Color.FromArgb(0, 255, 255);


		internal static Tile tile_Empty = new Tile();
		internal static Tile tile_Fake;
		internal static Tile tile_Wall;

		internal static Tile tile_TopLeftInvert;
		internal static Tile tile_TopRightInvert;
		internal static Tile tile_Top;
		internal static Tile tile_BottomLeftInvert;
		internal static Tile tile_BottomRightInvert;
		internal static Tile tile_Bottom;
		internal static Tile tile_Left;
		internal static Tile tile_Right;
		internal static Tile tile_TopLeft;
		internal static Tile tile_BottomLeft;
		internal static Tile tile_TopRight;
		internal static Tile tile_BottomRight;


		internal static Hashtable ColorMap;

		internal static void initDefaultMap()
		{

			// Color mapping
			ColorMap = new Hashtable(15);

			ArrayList gts = new ArrayList(4);
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque"));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque2"));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque3"));
			gts.Add(TextureLibrary.getGameTexture("walls\\plaque4"));
			tile_Wall = new Tile(gts, true);
			tile_Fake = new Tile(gts, false);

			tile_Right = new Tile(TextureLibrary.getGameTexture("walls\\plaque_right"), false);
			tile_Left = new Tile(TextureLibrary.getGameTexture("walls\\plaque_left"), false);
			tile_Bottom = new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm"), false);
			tile_BottomRight = new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_right"), false);
			tile_BottomRightInvert = new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_right_invert"), false);
			tile_BottomLeft = new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_left"), false);
			tile_BottomLeftInvert = new Tile(TextureLibrary.getGameTexture("walls\\plaque_btm_left_invert"), false);
			tile_Top = new Tile(TextureLibrary.getGameTexture("walls\\plaque_top"), false);
			tile_TopRight = new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_right"), false);
			tile_TopRightInvert = new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_right_invert"), false);
			tile_TopLeft = new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_left"), false);
			tile_TopLeftInvert = new Tile(TextureLibrary.getGameTexture("walls\\plaque_top_left_invert"), false);

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
		internal string LevelName;
		internal int Height;
		internal int Width;
		internal Tile[,] TileArray;

		private System.Drawing.Color[,] m_LevelArray;

#if TIME
		private static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
#endif

		internal Level(string p_FileName)
		{
			LevelName = p_FileName;
		}

		internal void Load()
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
		internal GameTexture GameTexture
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
		internal bool Enabled
		{
			get { return m_Enabled; }
		}
		protected Collidable.Factions m_Faction = Collidable.Factions.None;
		internal Collidable.Factions Faction
		{
			get { return m_Faction; }
		}
		//protected float m_Rotation = 0f;


		internal Tile() { }
		internal Tile(GameTexture p_GameTexture, bool p_Collidable)
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

		internal Tile(ArrayList p_GameTextures, bool p_Collidable)
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
