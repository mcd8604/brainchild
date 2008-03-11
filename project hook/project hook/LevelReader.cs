using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Wintellect.PowerCollections;
using System.Collections;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace project_hook
{
	class LevelReader
	{
#if DEBUG
		private static int maxLoops = 100;
		private static int curLoop = 0;
#endif
		private Dictionary<int, List<Event>> m_Events = new Dictionary<int, List<Event>>();

		private int m_Distance;

		private String m_FileName;
		internal String FileName
		{
			get
			{
				return m_FileName;
			}
			set
			{
				m_FileName = value;
			}
		}

		private String m_FilePath;

		internal LevelReader()
		{
			m_FilePath = System.Environment.CurrentDirectory + "\\Content\\Levels\\";
		}
		internal LevelReader(String p_FileName)
		{
			m_FileName = p_FileName;
			m_FilePath = System.Environment.CurrentDirectory + "\\Content\\Levels\\" + p_FileName;
		}

		internal Dictionary<int, List<Event>> ReadFile()
		{
			//Checks for the XML file
			if (File.Exists(m_FilePath))
			{
				XmlReaderSettings t_Settings = new XmlReaderSettings();
				t_Settings.ConformanceLevel = ConformanceLevel.Fragment;
				t_Settings.IgnoreWhitespace = true;
				t_Settings.IgnoreComments = true;
				XmlReader reader = XmlReader.Create(m_FilePath, t_Settings);

				reader.Read();
				reader.ReadStartElement("level");
				do
				{
					string temp = reader.GetAttribute("dist");
					if (temp != null)
					{
						m_Distance = int.Parse(temp);
					}
					else
					{
						temp = reader.GetAttribute("tile");
						if (temp != null)
						{
							m_Distance = (int.Parse(temp) * EnvironmentLoader.TileDimension);
						}
#if DEBUG
						else
						{
							throw new ArgumentException("Action must specify a dist or a tile");
						}
#endif
					}
					reader.ReadStartElement();
					do
					{
						if (reader.IsStartElement("createGate"))
						{
							reader.ReadStartElement();
							CreateGate(reader);
							reader.ReadEndElement();
						}
						else if (reader.IsStartElement("createBoss"))
						{
							reader.ReadStartElement();
							CreateBoss(reader);
							reader.ReadEndElement();
						}
						else if (reader.IsStartElement("loadBMP"))
						{
							reader.ReadStartElement();
							LoadBMP(reader);
							reader.ReadEndElement();
						}
						else if (reader.IsStartElement("pleaseLoadBMP"))
						{
							reader.ReadStartElement();
							PleaseLoadBMP(reader);
							reader.ReadEndElement();
						}
						else if (reader.IsStartElement("createSpawnPoint"))
						{
							reader.ReadStartElement();
							readSpawnPoint(reader);
							reader.ReadEndElement();
						}
						else if (reader.IsStartElement("createShip"))
						{
							reader.ReadStartElement();
							LoadEnemy(reader);
							reader.ReadEndElement();
						}
						else if (reader.IsStartElement("createTurret"))
						{
							reader.ReadStartElement();
							LoadEnemy(reader);
							reader.ReadEndElement();
						}
						else if (reader.IsStartElement("changeSpeed"))
						{
							reader.ReadStartElement();
							ChangeSpeed(reader);
							reader.ReadEndElement();
						}
						else if (reader.IsStartElement("changeFile"))
						{
							reader.ReadStartElement();
							NextFile(reader);
							reader.ReadEndElement();
						}
						else if (reader.IsStartElement("endGame"))
						{
							reader.ReadStartElement();
							EndGame();
						}
					} while (reader.IsStartElement());
#if DEBUG
					curLoop = 0;
#endif
					reader.ReadEndElement();
#if DEBUG
					if (++curLoop > maxLoops)
					{
						throw new Exception("maxLoops exceeded. LevelReader may be caught in an infinite loop.");
					}
#endif
				} while (reader.IsStartElement("action"));
#if DEBUG
				curLoop = 0;
#endif
			}

			else
			{
				throw new Exception("file not found: " + m_FilePath);
			}

			return m_Events;
		}

		internal void EndGame()
		{
			List<Event> t_List = new List<Event>();

			t_List.Add(new Event(Event.Types.EndGame));

			m_Events.Add(m_Distance, t_List);
		}

		internal void CreateBoss(XmlReader p_Reader)
		{
			Boss t_Boss = (Boss)readShip(p_Reader, typeof(Boss));
			t_Boss.Faction = Collidable.Factions.Enemy;
			t_Boss.Grabbable = false;

			List<Event> t_List = new List<Event>();

			//add the ship to the event list
			if (m_Events.ContainsKey(m_Distance))
			{
				m_Events[m_Distance].Add(new Event(t_Boss));
			}
			else
			{
				t_List.Add(new Event(t_Boss));
				m_Events.Add(m_Distance, t_List);
			}
		}

		private void CreateGate(XmlReader p_Reader)
		{
			List<Sprite> t_Walls = new List<Sprite>();
			List<Sprite> t_Gates = new List<Sprite>();
			GateTrigger t_Trigger = new GateTrigger();

			while (p_Reader.IsStartElement())
			{
				if (p_Reader.IsStartElement("gate"))
				{
					p_Reader.ReadStartElement();
					t_Gates.AddRange(LoadWall(p_Reader));
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("trigger"))
				{
					p_Reader.ReadStartElement();
					LoadTrigger(p_Reader, t_Trigger);
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("wall"))
				{
					p_Reader.ReadStartElement();
					t_Walls.AddRange(LoadWall(p_Reader));
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("guardian"))
				{
					p_Reader.ReadStartElement();
					Ship t_Guardian = readShip(p_Reader, typeof(Ship));
					t_Guardian.Faction = Collidable.Factions.Enemy;
					t_Trigger.Guardian = t_Guardian;
					addEvent(m_Distance, new Event(t_Guardian));
					p_Reader.ReadEndElement();
				}
			}
#if DEBUG
			curLoop = 0;
#endif
			t_Trigger.Gates = t_Gates;
			t_Trigger.Walls = t_Walls;

			addEvent(m_Distance, new Event(t_Gates));
			addEvent(m_Distance, new Event(t_Walls));
			addEvent(m_Distance, new Event(t_Trigger));
		}

		private List<Sprite> LoadWall(XmlReader p_Reader)
		{
			String name = null;
			int numRows = 0;
			int numCols = 0;
			Vector2 startPos = Vector2.Zero;
			GameTexture texture = null;
			String deathEffectName = null;
			int deathEffectTag = 0;
			String deathEffectAnimation = null;
			int deathEffectFPS = 0;

			Task task = null;
			Collidable.Factions faction = Collidable.Factions.None;

			while (p_Reader.IsStartElement())
			{
				if (p_Reader.IsStartElement("name"))
				{
					p_Reader.ReadStartElement("name");
					name = p_Reader.ReadString();
					p_Reader.ReadEndElement();
				}
				if (p_Reader.IsStartElement("numRows"))
				{
					p_Reader.ReadStartElement("numRows");
					numRows = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				if (p_Reader.IsStartElement("numCols"))
				{
					p_Reader.ReadStartElement("numCols");
					numCols = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("startTile"))
				{
					startPos = new Vector2(float.Parse(p_Reader.GetAttribute(0)) * EnvironmentLoader.TileDimension,
													float.Parse(p_Reader.GetAttribute(1)) * EnvironmentLoader.TileDimension);
					p_Reader.ReadStartElement("startTile");
				}
				else if (p_Reader.IsStartElement("texture"))
				{
					String t_Name = p_Reader.GetAttribute(0);
					if (p_Reader.AttributeCount == 1)
					{
						texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0));
					}
					else if (p_Reader.AttributeCount == 2)
					{
						texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0), int.Parse(p_Reader.GetAttribute(1)));
					}

					p_Reader.ReadStartElement("texture");
				}
				else if (p_Reader.IsStartElement("deathEffect"))
				{
					if (p_Reader.AttributeCount == 1)
					{
						deathEffectName = p_Reader.GetAttribute("name");
					}
					else if (p_Reader.AttributeCount == 2)
					{
						deathEffectName = p_Reader.GetAttribute("name");
						deathEffectTag = int.Parse(p_Reader.GetAttribute("tag"));
					}
					else
					{
						deathEffectName = p_Reader.GetAttribute("name");
						deathEffectTag = int.Parse(p_Reader.GetAttribute("tag"));
						deathEffectAnimation = p_Reader.GetAttribute("animation");
						deathEffectFPS = int.Parse(p_Reader.GetAttribute("fps"));
					}
					p_Reader.ReadStartElement("deathEffect");
				}
				else if (p_Reader.IsStartElement("task"))
				{
					task = readTask(p_Reader);
				}
				else if (p_Reader.IsStartElement("faction"))
				{
					p_Reader.ReadStartElement("faction");
					faction = (Collidable.Factions)Enum.Parse(typeof(Collidable.Factions), p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
#if DEBUG
				else
				{
					throw new NotImplementedException("LevelReader LoadEnemy could not understand tag '" + p_Reader.Name + "'");
				}
#endif
#if DEBUG
				if (++curLoop > maxLoops)
				{
					throw new Exception("maxLoops exceeded. LevelReader may be caught in an infinite loop.");
				}
#endif
			}
			List<Sprite> wallList = new List<Sprite>();
			for (int row = 0; row < numRows; row++)
			{
				for (int col = 0; col < numCols; col++)
				{
					Vector2 pos = startPos + new Vector2(col * EnvironmentLoader.TileDimension, row * EnvironmentLoader.TileDimension);
					Collidable p_Wall = new Collidable(
#if !FINAL
						name + "_" + col + "_" + row,
#endif
pos, EnvironmentLoader.TileDimension, EnvironmentLoader.TileDimension, texture, 0.75f, true, 0, Depth.GameLayer.Gate, faction, float.NaN, EnvironmentLoader.TileDimension / 2);
					p_Wall.Bound = Collidable.Boundings.Square;
					p_Wall.Task = task;
					if (deathEffectName != null)
					{
						if (deathEffectAnimation != null)
						{
							p_Wall.setDeathEffect(deathEffectName, deathEffectTag, deathEffectAnimation, deathEffectFPS);
						}
						else
						{
							p_Wall.setDeathEffect(deathEffectName, deathEffectTag);
						}
					}
					wallList.Add(p_Wall);
				}
			}
			return wallList;
		}

		private void LoadTrigger(XmlReader p_Reader, GateTrigger p_Trigger)
		{
			p_Trigger.Faction = Collidable.Factions.Environment;
			p_Trigger.Z = Depth.GameLayer.Trigger;
			p_Trigger.Bound = Collidable.Boundings.Rectangle;
			p_Trigger.Radius = 10;
			p_Trigger.Health = 1;
			p_Trigger.Grabbable = false;
			while (p_Reader.IsStartElement())
			{
				if (p_Reader.IsStartElement("name"))
				{
					p_Reader.ReadStartElement("name");
#if !FINAL
					p_Trigger.Name =
#endif
					p_Reader.ReadString();
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("endGate"))
				{
					p_Reader.ReadStartElement();
					p_Trigger.EndGate = bool.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("startPos"))
				{
					p_Trigger.Position = new Vector2(float.Parse(p_Reader.GetAttribute(0)),
													float.Parse(p_Reader.GetAttribute(1)));
					p_Reader.ReadStartElement("startPos");
				}
				else if (p_Reader.IsStartElement("startCenter"))
				{
					p_Trigger.Center = new Vector2(float.Parse(p_Reader.GetAttribute(0)),
													float.Parse(p_Reader.GetAttribute(1)));
					p_Reader.ReadStartElement("startCenter");
				}
				else if (p_Reader.IsStartElement("startTile"))
				{
					p_Trigger.Position = new Vector2(float.Parse(p_Reader.GetAttribute(0)) * EnvironmentLoader.TileDimension,
													float.Parse(p_Reader.GetAttribute(1)) * EnvironmentLoader.TileDimension);
					p_Reader.ReadStartElement("startTile");
				}
				else if (p_Reader.IsStartElement("startTileCenter"))
				{
					p_Trigger.Center = new Vector2(float.Parse(p_Reader.GetAttribute(0)) * EnvironmentLoader.TileDimension,
													float.Parse(p_Reader.GetAttribute(1)) * EnvironmentLoader.TileDimension);
					p_Reader.ReadStartElement("startTileCenter");
				}
				else if (p_Reader.IsStartElement("height"))
				{
					p_Reader.ReadStartElement("height");
					p_Trigger.Height = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("width"))
				{
					p_Reader.ReadStartElement();
					p_Trigger.Width = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("texture"))
				{
					String t_Name = p_Reader.GetAttribute(0);
					if (p_Reader.AttributeCount == 1)
					{
						p_Trigger.Texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0));
					}
					else if (p_Reader.AttributeCount == 2)
					{
						p_Trigger.Texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0), int.Parse(p_Reader.GetAttribute(1)));
					}

					p_Reader.ReadStartElement("texture");
				}
				else if (p_Reader.IsStartElement("degree"))
				{
					p_Reader.ReadStartElement("degree");
					p_Trigger.RotationDegrees = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("task"))
				{
					p_Trigger.Task = readTask(p_Reader);
				}
				else if (p_Reader.IsStartElement("animation"))
				{
					p_Trigger.setAnimation(p_Reader.GetAttribute("name"), int.Parse(p_Reader.GetAttribute("fps")));
					//p_Gate.Animation.StartAnimation();
					p_Reader.ReadStartElement("animation");
				}
				else if (p_Reader.IsStartElement("bound"))
				{
					p_Trigger.Bound = readBounding(p_Reader);
				}
				else if (p_Reader.IsStartElement("blendMode"))
				{
					p_Trigger.BlendMode = readBlendMode(p_Reader);
				}
#if DEBUG
				else
				{
					throw new NotImplementedException("LevelReader LoadEnemy could not understand tag '" + p_Reader.Name + "'");
				}
#endif
#if DEBUG
				if (++curLoop > maxLoops)
				{
					throw new Exception("maxLoops exceeded. LevelReader may be caught in an infinite loop.");
				}
#endif
			}
		}

		private void NextFile(XmlReader p_Reader)
		{
			String m_FileName;

			List<Event> t_List = new List<Event>();

			//read next file name
			p_Reader.ReadStartElement("fileName");
			m_FileName = p_Reader.ReadString();
			p_Reader.ReadEndElement();

			//add the file speed to the event list
			if (m_Events.ContainsKey(m_Distance))
			{
				m_Events[m_Distance].Add(new Event(m_FileName, Event.Types.ChangeFile));
			}
			else
			{
				t_List.Add(new Event(m_FileName, Event.Types.ChangeFile));
				m_Events.Add(m_Distance, t_List);
			}
		}

		private void LoadBMP(XmlReader p_Reader)
		{
			String m_FileName;

			List<Event> t_List = new List<Event>();

			//read next file name
			p_Reader.ReadStartElement("fileName");
			m_FileName = p_Reader.ReadString();
			p_Reader.ReadEndElement();

			//add the file speed to the event list
			if (m_Events.ContainsKey(m_Distance))
			{
				m_Events[m_Distance].Add(new Event(m_FileName, Event.Types.LoadBMP));
			}
			else
			{
				t_List.Add(new Event(m_FileName, Event.Types.LoadBMP));
				m_Events.Add(m_Distance, t_List);
			}
		}

		private void PleaseLoadBMP(XmlReader p_Reader)
		{
			//read next file name
			p_Reader.ReadStartElement("fileName");
			String m_FileName = p_Reader.ReadString();
			p_Reader.ReadEndElement();

			//add the file speed to the event list
			if (m_Events.ContainsKey(m_Distance))
			{
				m_Events[m_Distance].Add(new Event(m_FileName, Event.Types.PleaseLoadBMP));
			}
			else
			{
				List<Event> t_List = new List<Event>();
				t_List.Add(new Event(m_FileName, Event.Types.PleaseLoadBMP));
				m_Events.Add(m_Distance, t_List);
			}
		}

		private void ChangeSpeed(XmlReader p_Reader)
		{
			int m_Speed;

			List<Event> t_List = new List<Event>();

			//read speed
			p_Reader.ReadStartElement("speed");
			m_Speed = int.Parse(p_Reader.ReadString());
			p_Reader.ReadEndElement();

			//add the change speed to the event list
			if (m_Events.ContainsKey(m_Distance))
			{
				m_Events[m_Distance].Add(new Event(m_Speed));
			}
			else
			{
				t_List.Add(new Event(m_Speed));
				m_Events.Add(m_Distance, t_List);
			}
		}

		private void LoadEnemy(XmlReader p_Reader)
		{
			Ship t_Ship = readShip(p_Reader, typeof(Ship));
			t_Ship.Z = Depth.GameLayer.Ships;
			t_Ship.Faction = Collidable.Factions.Enemy;

			List<Event> t_List = new List<Event>();

			//add the ship to the event list
			if (m_Events.ContainsKey(m_Distance))
			{
				m_Events[m_Distance].Add(new Event(t_Ship));
			}
			else
			{
				t_List.Add(new Event(t_Ship));
				m_Events.Add(m_Distance, t_List);
			}
		}

		private static Collidable.Boundings readBounding(XmlReader p_Reader)
		{
			p_Reader.ReadStartElement("bound");
			Collidable.Boundings ret = (Collidable.Boundings)Enum.Parse(typeof(Collidable.Boundings), p_Reader.ReadString(), true);
			p_Reader.ReadEndElement();
			return ret;
		}

		private static SpriteBlendMode readBlendMode(XmlReader p_Reader)
		{
			p_Reader.ReadStartElement("blendMode");
			SpriteBlendMode ret = (SpriteBlendMode)Enum.Parse(typeof(SpriteBlendMode), p_Reader.ReadString(), true);
			p_Reader.ReadEndElement();
			return ret;
		}

		private static Ship readShip(XmlReader p_Reader, Type p_shipType)
		{
			Ship t_Ship = new Ship();
			if (p_shipType == typeof(ShipPart))
			{
				t_Ship = new ShipPart();
			}
			else if (p_shipType == typeof(Boss))
			{
				t_Ship = new Boss();
			}
			else if (p_shipType == typeof(Turret))
			{
				t_Ship = new Turret(60);
			}

			while (p_Reader.IsStartElement())
			{
				if (p_Reader.IsStartElement("name"))
				{
					p_Reader.ReadStartElement("name");
#if !FINAL
					t_Ship.Name =
#endif
					p_Reader.ReadString();
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("grabbable"))
				{
					p_Reader.ReadStartElement("grabbable");
					t_Ship.Grabbable = bool.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("damage"))
				{
					p_Reader.ReadStartElement("damage");
					t_Ship.Damage = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("startPos"))
				{
					t_Ship.Position = new Vector2(float.Parse(p_Reader.GetAttribute(0)),
													float.Parse(p_Reader.GetAttribute(1)));
					p_Reader.ReadStartElement("startPos");
				}
				else if (p_Reader.IsStartElement("startCenter"))
				{
					t_Ship.Center = new Vector2(float.Parse(p_Reader.GetAttribute(0)),
													float.Parse(p_Reader.GetAttribute(1)));
					p_Reader.ReadStartElement("startCenter");
				}
				else if (p_Reader.IsStartElement("startTile"))
				{
					t_Ship.Position = new Vector2(float.Parse(p_Reader.GetAttribute(0)) * EnvironmentLoader.TileDimension,
													float.Parse(p_Reader.GetAttribute(1)) * EnvironmentLoader.TileDimension);
					p_Reader.ReadStartElement("startTile");
				}
				else if (p_Reader.IsStartElement("startTileCenter"))
				{
					t_Ship.Center = new Vector2(float.Parse(p_Reader.GetAttribute(0)) * EnvironmentLoader.TileDimension,
													float.Parse(p_Reader.GetAttribute(1)) * EnvironmentLoader.TileDimension);
					p_Reader.ReadStartElement("startTileCenter");
				}
				else if (p_Reader.IsStartElement("height"))
				{
					p_Reader.ReadStartElement("height");
					t_Ship.Height = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("width"))
				{
					p_Reader.ReadStartElement();
					t_Ship.Width = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("depth"))
				{
					p_Reader.ReadStartElement();
					t_Ship.Z = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("texture"))
				{
					String t_Name = p_Reader.GetAttribute(0);
					if (p_Reader.AttributeCount == 1)
					{
						t_Ship.Texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0));
					}
					else if (p_Reader.AttributeCount == 2)
					{
						t_Ship.Texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0), int.Parse(p_Reader.GetAttribute(1)));
					}

					p_Reader.ReadStartElement("texture");
				}
				else if (p_Reader.IsStartElement("transparency"))
				{
					p_Reader.ReadStartElement("transparency");
					t_Ship.Transparency = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("enabled"))
				{
					p_Reader.ReadStartElement("enabled");
					t_Ship.Enabled = bool.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("degree"))
				{
					p_Reader.ReadStartElement("degree");
					t_Ship.RotationDegrees = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("health"))
				{
					p_Reader.ReadStartElement("health");
					t_Ship.MaxHealth = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("shield"))
				{
					p_Reader.ReadStartElement("shield");
					t_Ship.MaxShield = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("damageEffect"))
				{
					if (p_Reader.AttributeCount == 1)
					{
						t_Ship.setDamageEffect(p_Reader.GetAttribute("name"), 0);
					}
					else if (p_Reader.AttributeCount == 2)
					{
						t_Ship.setDamageEffect(p_Reader.GetAttribute("name"), int.Parse(p_Reader.GetAttribute("tag")));
					}
					else
					{
						t_Ship.setDamageEffect(p_Reader.GetAttribute("name"), int.Parse(p_Reader.GetAttribute("tag")), p_Reader.GetAttribute("animation"), int.Parse(p_Reader.GetAttribute("fps")));
					}
					p_Reader.ReadStartElement("damageEffect");
				}
				else if (p_Reader.IsStartElement("shieldDamageEffect"))
				{
					if (p_Reader.AttributeCount == 1)
					{
						t_Ship.setShieldDamageEffect(p_Reader.GetAttribute("name"), 0);
					}
					else if (p_Reader.AttributeCount == 2)
					{
						t_Ship.setShieldDamageEffect(p_Reader.GetAttribute("name"), int.Parse(p_Reader.GetAttribute("tag")));
					}
					else
					{
						t_Ship.setShieldDamageEffect(p_Reader.GetAttribute("name"), int.Parse(p_Reader.GetAttribute("tag")), p_Reader.GetAttribute("animation"), int.Parse(p_Reader.GetAttribute("fps")));
					}
					p_Reader.ReadStartElement("shieldDamageEffect");
				}
				else if (p_Reader.IsStartElement("deathEffect"))
				{
					if (p_Reader.AttributeCount == 1)
					{
						t_Ship.setDeathEffect(p_Reader.GetAttribute("name"), 0);
					}
					else if (p_Reader.AttributeCount == 2)
					{
						t_Ship.setDeathEffect(p_Reader.GetAttribute("name"), int.Parse(p_Reader.GetAttribute("tag")));
					}
					else
					{
						t_Ship.setDeathEffect(p_Reader.GetAttribute("name"), int.Parse(p_Reader.GetAttribute("tag")), p_Reader.GetAttribute("animation"), int.Parse(p_Reader.GetAttribute("fps")));
					}
					p_Reader.ReadStartElement("deathEffect");
				}
				else if (p_Reader.IsStartElement("score"))
				{
					p_Reader.ReadStartElement("score");
					t_Ship.DestructionScore = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("radius"))
				{
					p_Reader.ReadStartElement("radius");
					t_Ship.Radius = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("weapon"))
				{
					t_Ship.addWeapon(readWeapon(p_Reader));
				}
				else if (p_Reader.IsStartElement("task"))
				{
					t_Ship.Task = readTask(p_Reader);
				}
				else if (p_Reader.IsStartElement("animation"))
				{
					t_Ship.setAnimation(p_Reader.GetAttribute("name"), int.Parse(p_Reader.GetAttribute("fps")));
					t_Ship.Animation.StartAnimation();
					p_Reader.ReadStartElement("animation");
				}
				else if (p_Reader.IsStartElement("shootAnimation"))
				{
					t_Ship.setShootAnimation(p_Reader.GetAttribute("name"), int.Parse(p_Reader.GetAttribute("fps")));
					p_Reader.ReadStartElement("shootAnimation");
				}
				else if (p_Reader.IsStartElement("bound"))
				{
					t_Ship.Bound = readBounding(p_Reader);
				}
				else if (p_Reader.IsStartElement("blendMode"))
				{
					t_Ship.BlendMode = readBlendMode(p_Reader);
				}
				else if (p_Reader.IsStartElement("shieldRegenDelay"))
				{
					p_Reader.ReadStartElement();
					t_Ship.ShieldRegenDelay = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("shieldRegenRate"))
				{
					p_Reader.ReadStartElement();
					t_Ship.ShieldRegenRate = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("faction"))
				{
					p_Reader.ReadStartElement("faction");
					t_Ship.Faction = (Collidable.Factions)Enum.Parse(typeof(Collidable.Factions), p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("shipPart"))
				{
					p_Reader.ReadStartElement("shipPart");

					float offsetDistance = 0;
					if (p_Reader.IsStartElement("offsetDistance"))
					{
						p_Reader.ReadStartElement("offsetDistance");
						offsetDistance = float.Parse(p_Reader.ReadString());
						p_Reader.ReadEndElement();
					}

					int offsetAngleDegrees = 0;
					if (p_Reader.IsStartElement("offsetAngleDegrees"))
					{
						p_Reader.ReadStartElement("offsetAngleDegrees");
						offsetAngleDegrees = int.Parse(p_Reader.ReadString());
						p_Reader.ReadEndElement();
					}

					bool fixedRotation = false;
					if (p_Reader.IsStartElement("fixedRotation"))
					{
						p_Reader.ReadStartElement("fixedRotation");
						fixedRotation = bool.Parse(p_Reader.ReadString());
						p_Reader.ReadEndElement();
					}

					bool transfersDamage = false;
					if (p_Reader.IsStartElement("transfersDamage"))
					{
						p_Reader.ReadStartElement("transfersDamage");
						transfersDamage = bool.Parse(p_Reader.ReadString());
						p_Reader.ReadEndElement();
					}

					ShipPart part = null;
					if (p_Reader.IsStartElement("createShip"))
					{
						String type = "";
						if (p_Reader.AttributeCount > 0)
						{
							type = p_Reader.GetAttribute("type");
						}
						p_Reader.ReadStartElement();
						if (type.Equals("turret"))
						{
							part = (Turret)readShip(p_Reader, typeof(Turret));
						}
						else
						{
							part = (ShipPart)readShip(p_Reader, typeof(ShipPart));
						}
						part.TransfersDamage = transfersDamage;
						part.ParentShip = t_Ship;
						p_Reader.ReadEndElement();
					}
					if (part != null)
					{
						TaskParallel newTask = new TaskParallel();
						if (fixedRotation)
						{
							newTask.addTask(new TaskRotateWithTarget(t_Ship));
						}
						newTask.addTask(new TaskRotateAroundTarget(t_Ship, offsetDistance, offsetAngleDegrees));
						if (part.Task != null)
						{
							newTask.addTask(part.Task);
						}
						part.Task = newTask;
						t_Ship.attachSpritePart(part);
					}

					p_Reader.ReadEndElement();
				}
#if DEBUG
				else
				{
					throw new NotImplementedException("LevelReader LoadEnemy could not understand tag '" + p_Reader.Name + "'");
				}
#endif
#if DEBUG
				if (++curLoop > maxLoops)
				{
					throw new Exception("maxLoops exceeded. LevelReader may be caught in an infinite loop.");
				}
#endif
			}
#if DEBUG
			curLoop = 0;
#endif
			return t_Ship;
		}

		private static Weapon readWeapon(XmlReader p_Reader)
		{
			Weapon weapon = null;
			string pType = p_Reader.GetAttribute("type");

			switch (pType)
			{
				case "Straight":
					WeaponStraight straight = new WeaponStraight();
					straight.AngleDegrees = float.Parse(p_Reader.GetAttribute("angle"));
					weapon = straight;
					p_Reader.ReadStartElement("weapon");
					break;
				case "Seek":
					WeaponSeek seek = new WeaponSeek();
					string target = p_Reader.GetAttribute("target");
					if (target == "Player")
					{
						seek.Target = World.m_Player.PlayerShip;
					}
					weapon = seek;
					p_Reader.ReadStartElement("weapon");
					break;
				case "Complex":
					WeaponComplex complex = new WeaponComplex();
					complex.OffsetDegrees = float.Parse(p_Reader.GetAttribute("offset"));
					string comtarget = p_Reader.GetAttribute("target");
					if (comtarget == "Player")
					{
						complex.Target = World.m_Player.PlayerShip;
					}
					p_Reader.ReadStartElement("weapon");
					Task task = readTask(p_Reader);
					complex.ShotTask = task;
					weapon = complex;
					break;
				case "DupSequence":
					WeaponDupSequence dupsequence = new WeaponDupSequence();
					dupsequence.RecycleDelay = float.Parse(p_Reader.GetAttribute("recycle"));
					dupsequence.setRepeats(int.Parse(p_Reader.GetAttribute("repeats")));
					p_Reader.ReadStartElement("weapon");
					while (p_Reader.IsStartElement("weapon"))
					{
						dupsequence.setWeapon(readWeapon(p_Reader));
					}
					weapon = dupsequence;
					break;
				case "Sequence":
					WeaponSequence sequence = new WeaponSequence();
					sequence.RecycleDelay = float.Parse(p_Reader.GetAttribute("recycle"));
					p_Reader.ReadStartElement("weapon");
					while (p_Reader.IsStartElement("weapon"))
					{
						sequence.addWeapon(readWeapon(p_Reader));
					}
					weapon = sequence;
					break;
				default:
#if DEBUG
					throw new NotImplementedException("'" + pType + "' is not a recognized Weapon");
#else
					break;
#endif
			}


			while (p_Reader.IsStartElement())
			{
				if (p_Reader.IsStartElement("delay"))
				{
					p_Reader.ReadStartElement("delay");
					weapon.Delay = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("initialCooldown"))
				{
					p_Reader.ReadStartElement("initialCooldown");
					weapon.Cooldown = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("speed"))
				{
					p_Reader.ReadStartElement("speed");
					weapon.Speed = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("shot"))
				{
					weapon.ShotType = readShot(p_Reader);
				}
#if DEBUG
				else
				{
					throw new NotImplementedException("LevelReader readWeapon could not understand tag '" + p_Reader.Name + "'");
				}
#endif
#if DEBUG
				if (++curLoop > maxLoops)
				{
					throw new Exception("maxLoops exceeded. LevelReader may be caught in an infinite loop.");
				}
#endif
			}
#if DEBUG
			curLoop = 0;
#endif

			p_Reader.ReadEndElement();


			return weapon;
		}

		private static Shot readShot(XmlReader p_Reader)
		{
			Shot shot = new Shot();
			p_Reader.ReadStartElement("shot");

			while (p_Reader.IsStartElement())
			{
				if (p_Reader.IsStartElement("name"))
				{
					p_Reader.ReadStartElement("name");
#if !FINAL
					shot.Name =
#endif
					p_Reader.ReadString();
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("damage"))
				{
					p_Reader.ReadStartElement("damage");
					shot.Damage = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("height"))
				{
					p_Reader.ReadStartElement("height");
					shot.Height = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("width"))
				{
					p_Reader.ReadStartElement("width");
					shot.Width = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("radius"))
				{
					p_Reader.ReadStartElement("radius");
					shot.Radius = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("animation"))
				{
					shot.setAnimation(p_Reader.GetAttribute("name"), int.Parse(p_Reader.GetAttribute("fps")));
					shot.Animation.StartAnimation();
					p_Reader.ReadStartElement("animation");
				}
				else if (p_Reader.IsStartElement("texture"))
				{
					String t_Name = p_Reader.GetAttribute(0);
					if (p_Reader.AttributeCount == 1)
					{
						shot.Texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0));
					}
					else if (p_Reader.AttributeCount == 2)
					{
						shot.Texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0), int.Parse(p_Reader.GetAttribute(1)));
					}

					p_Reader.ReadStartElement("texture");
				}
				else if (p_Reader.IsStartElement("bound"))
				{
					shot.Bound = readBounding(p_Reader);
				}
				else if (p_Reader.IsStartElement("blendMode"))
				{
					shot.BlendMode = readBlendMode(p_Reader);
				}
				else if (p_Reader.IsStartElement("trail"))
				{
					shot.TrailEffect = readShotTrail(p_Reader, shot);
				}
#if DEBUG
				else
				{
					throw new NotImplementedException("LevelReader readShot could not understand tag '" + p_Reader.Name + "'");
				}
#endif
#if DEBUG
				if (++curLoop > maxLoops)
				{
					throw new Exception("maxLoops exceeded. LevelReader may be caught in an infinite loop.");
				}
#endif
			}
#if DEBUG
			curLoop = 0;
#endif

			p_Reader.ReadEndElement();

			return shot;

		}

		private static ExplosionSpriteParticleSystem readShotTrail(XmlReader p_Reader, Shot shot)
		{
			ExplosionSpriteParticleSystem esps = new ExplosionSpriteParticleSystem();
#if !FINAL
			esps.Name = shot.Name + "_ParticleSystem";
#endif
			p_Reader.ReadStartElement();
			while (p_Reader.IsStartElement())
			{
				if (p_Reader.IsStartElement("texture"))
				{
					esps.TextureName = p_Reader.GetAttribute("name");
					esps.TextureTag = 0;
					p_Reader.ReadStartElement("texture");
				}
				else if (p_Reader.IsStartElement("animation"))
				{
					esps.Animated = true;
					esps.AnimationName = p_Reader.GetAttribute("name");
					esps.AnimationFPS = int.Parse(p_Reader.GetAttribute("fps"));
					p_Reader.ReadStartElement("animation");
				}
				else if (p_Reader.IsStartElement("HowManyEffects"))
				{
					p_Reader.ReadStartElement("HowManyEffects");
					esps.HowManyEffects = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("MinNumParticles"))
				{
					p_Reader.ReadStartElement("MinNumParticles");
					esps.MinNumParticles = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("MaxNumParticles"))
				{
					p_Reader.ReadStartElement("MaxNumParticles");
					esps.MaxNumParticles = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("MinLifetime"))
				{
					p_Reader.ReadStartElement("MinLifetime");
					esps.MinLifetime = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("MaxLifetime"))
				{
					p_Reader.ReadStartElement("MaxLifetime");
					esps.MaxLifetime = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("MinScale"))
				{
					p_Reader.ReadStartElement("MinScale");
					esps.MinScale = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("MaxScale"))
				{
					p_Reader.ReadStartElement("MaxScale");
					esps.MaxScale = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("MinInitialSpeed"))
				{
					p_Reader.ReadStartElement("MinInitialSpeed");
					esps.MinInitialSpeed = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("MaxInitialSpeed"))
				{
					p_Reader.ReadStartElement("MaxInitialSpeed");
					esps.MaxInitialSpeed = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("blendMode"))
				{
					esps.BlendMode = readBlendMode(p_Reader);
				}
				else if (p_Reader.IsStartElement("transparency"))
				{
					p_Reader.ReadStartElement("transparency");
					esps.Transparency = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
			}
			p_Reader.ReadEndElement();
			return esps;
		}

		private static Task readTask(XmlReader p_Reader)
		{
			Task task = null;
			Vector2 v = Vector2.Zero;
			string pType = p_Reader.GetAttribute("type");
			p_Reader.ReadStartElement("task");

			switch (pType)
			{
				case "Attach":
					TaskAttach attach = new TaskAttach();
					while (p_Reader.IsStartElement())
					{
						if (p_Reader.IsStartElement("target"))
						{
							p_Reader.ReadStartElement("target");
							string target = p_Reader.ReadString();
							if (target == "Player")
							{
								attach.Target = World.m_Player.PlayerShip;
							}
							else if (target == "Parent")
							{
								throw new NotImplementedException();
							}
							p_Reader.ReadEndElement();
						}
					}
					task = attach;
					break;
				case "Fire":
					return new TaskFire();
				case "Parallel":
					TaskParallel parallel = new TaskParallel();
					while (p_Reader.IsStartElement("task"))
					{
						parallel.addTask(readTask(p_Reader));
					}
					task = parallel;
					break;
				case "RepeatingSequence":
					TaskRepeatingSequence repeatingSequence = new TaskRepeatingSequence();
					while (p_Reader.IsStartElement("task"))
					{
						repeatingSequence.addTask(readTask(p_Reader));
					}
					task = repeatingSequence;
					break;
				case "RepeatingTimer":
					TaskRepeatingTimer repeatingtimer = new TaskRepeatingTimer();
					while (p_Reader.IsStartElement())
					{
						if (p_Reader.IsStartElement("duration"))
						{
							p_Reader.ReadStartElement("duration");
							repeatingtimer.Duration = float.Parse(p_Reader.ReadString());
							p_Reader.ReadEndElement();
						}
					}
					task = repeatingtimer;
					break;
				case "RotateToAngle":
					TaskRotateToAngle rotateToAngle = new TaskRotateToAngle();
					while (p_Reader.IsStartElement())
					{
						if (p_Reader.IsStartElement("angle"))
						{
							p_Reader.ReadStartElement("angle");
							rotateToAngle.Angle = float.Parse(p_Reader.ReadString());
							p_Reader.ReadEndElement();
						}
						else if (p_Reader.IsStartElement("degree"))
						{
							p_Reader.ReadStartElement("degree");
							rotateToAngle.Angle = MathHelper.ToRadians(float.Parse(p_Reader.ReadString()));
							p_Reader.ReadEndElement();
						}
					}
					task = rotateToAngle;
					break;
				case "RotateByAngle":
					TaskRotateByAngle rotateByAngle = new TaskRotateByAngle();
					while (p_Reader.IsStartElement())
					{
						if (p_Reader.IsStartElement("angle"))
						{
							p_Reader.ReadStartElement("angle");
							rotateByAngle.Angle = float.Parse(p_Reader.ReadString());
							p_Reader.ReadEndElement();
						}
						else if (p_Reader.IsStartElement("degree"))
						{
							p_Reader.ReadStartElement("degree");
							rotateByAngle.Angle = MathHelper.ToRadians(float.Parse(p_Reader.ReadString()));
							p_Reader.ReadEndElement();
						}
					}
					task = rotateByAngle;
					break;
				case "RotateFaceTarget":
					TaskRotateFaceTarget rotateFaceTarget = new TaskRotateFaceTarget();
					while (p_Reader.IsStartElement())
					{
						if (p_Reader.IsStartElement("target"))
						{
							p_Reader.ReadStartElement("target");
							string target = p_Reader.ReadString();
							if (target == "Player")
							{
								rotateFaceTarget.Target = World.m_Player.PlayerShip;
							}
							p_Reader.ReadEndElement();
						}
						else if (p_Reader.IsStartElement("offset"))
						{
							p_Reader.ReadStartElement("offset");
							rotateFaceTarget.Offset = MathHelper.ToRadians(float.Parse(p_Reader.ReadString()));
							p_Reader.ReadEndElement();
						}
					}
					task = rotateFaceTarget;
					break;
				case "SeekTarget":
					TaskSeekTarget seekTarget = new TaskSeekTarget();
					while (p_Reader.IsStartElement())
					{
						if (p_Reader.IsStartElement("target"))
						{
							p_Reader.ReadStartElement("target");
							string target = p_Reader.ReadString();
							if (target == "Player")
							{
								seekTarget.Target = World.m_Player.PlayerShip;
							}
							p_Reader.ReadEndElement();
						}
						else if (p_Reader.IsStartElement("speed"))
						{
							p_Reader.ReadStartElement("speed");
							seekTarget.Speed = float.Parse(p_Reader.ReadString());
							p_Reader.ReadEndElement();
						}
					}
					task = seekTarget;
					break;
				case "SeekPoint":
					TaskSeekPoint seekPoint = new TaskSeekPoint();
					while (p_Reader.IsStartElement())
					{
						if (p_Reader.IsStartElement("goal"))
						{
							v = new Vector2(float.Parse(p_Reader.GetAttribute("x")), float.Parse(p_Reader.GetAttribute("y")));
							p_Reader.ReadStartElement("goal");
						}
						else if (p_Reader.IsStartElement("speed"))
						{
							p_Reader.ReadStartElement("speed");
							seekPoint.Speed = float.Parse(p_Reader.ReadString());
							p_Reader.ReadEndElement();
						}
					}
					seekPoint.Goal = v;
					task = seekPoint;
					break;
				case "Sequence":
					TaskSequence sequence = new TaskSequence();
					while (p_Reader.IsStartElement("task"))
					{
						sequence.addTask(readTask(p_Reader));
					}
					task = sequence;
					break;
				case "Stationary":
					return new TaskStationary();
				case "StationaryBackground":
					return new TaskStationaryBackground();
				case "StraightAngle":
					TaskStraightAngle straightAngle = new TaskStraightAngle();
					while (p_Reader.IsStartElement())
					{
						if (p_Reader.IsStartElement("angle"))
						{
							p_Reader.ReadStartElement("angle");
							straightAngle.Angle = float.Parse(p_Reader.ReadString());
							p_Reader.ReadEndElement();
						}
						else if (p_Reader.IsStartElement("degree"))
						{
							p_Reader.ReadStartElement("degree");
							straightAngle.Angle = MathHelper.ToRadians(float.Parse(p_Reader.ReadString()));
							p_Reader.ReadEndElement();
						}
						else if (p_Reader.IsStartElement("speed"))
						{
							p_Reader.ReadStartElement("speed");
							straightAngle.Speed = float.Parse(p_Reader.ReadString());
							p_Reader.ReadEndElement();
						}
					}
					task = straightAngle;
					break;
				case "StraightVelocity":
					TaskStraightVelocity straightVelocity = new TaskStraightVelocity();
					while (p_Reader.IsStartElement())
					{
						if (p_Reader.IsStartElement("velocity"))
						{
							v = new Vector2(float.Parse(p_Reader.GetAttribute("x")), float.Parse(p_Reader.GetAttribute("y")));
							p_Reader.ReadStartElement("velocity");
						}
					}
					straightVelocity.Velocity = v;
					task = straightVelocity;
					break;
				case "Timer":
					TaskTimer timer = new TaskTimer();
					while (p_Reader.IsStartElement())
					{
						if (p_Reader.IsStartElement("duration"))
						{
							p_Reader.ReadStartElement("duration");
							timer.Duration = float.Parse(p_Reader.ReadString());
							p_Reader.ReadEndElement();
						}
					}
					task = timer;
					break;
				default:
#if DEBUG
					throw new NotImplementedException("'" + pType + "' is not a recognized Task");
#else
					break;
#endif
			}

			p_Reader.ReadEndElement();
			return task;
		}

		private void readSpawnPoint(XmlReader p_Reader)
		{
			SpawnPoint t_Point = new SpawnPoint();
			Ship t_Obj = new Ship();

			List<Event> t_List = new List<Event>();

			while (p_Reader.IsStartElement())
			{
				if (p_Reader.IsStartElement("delay"))
				{
					p_Reader.ReadStartElement("delay");
					t_Point.Delay = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("object"))
				{
					p_Reader.ReadStartElement("object");
					t_Obj = readShip(p_Reader, typeof(Ship));
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("type"))
				{
					p_Reader.ReadStartElement("type");
					t_Point.Type = (SpawnPoint.SpawnType)Enum.Parse(typeof(SpawnPoint.SpawnType), p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("count"))
				{
					p_Reader.ReadStartElement("count");
					t_Point.Count = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("name"))
				{
					p_Reader.ReadStartElement("name");
#if !FINAL
					t_Point.Name =
#endif
					p_Reader.ReadString();
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("startPos"))
				{
					t_Point.Position = new Vector2(float.Parse(p_Reader.GetAttribute(0)),
													float.Parse(p_Reader.GetAttribute(1)));
					p_Reader.ReadStartElement("startPos");
				}
				else if (p_Reader.IsStartElement("startCenter"))
				{
					t_Point.Center = new Vector2(float.Parse(p_Reader.GetAttribute(0)),
													float.Parse(p_Reader.GetAttribute(1)));
					p_Reader.ReadStartElement("startCenter");
				}
				else if (p_Reader.IsStartElement("startTile"))
				{
					t_Point.Position = new Vector2(float.Parse(p_Reader.GetAttribute(0)) * EnvironmentLoader.TileDimension,
													float.Parse(p_Reader.GetAttribute(1)) * EnvironmentLoader.TileDimension);
					p_Reader.ReadStartElement("startTile");
				}
				else if (p_Reader.IsStartElement("startTileCenter"))
				{
					t_Point.Center = new Vector2(float.Parse(p_Reader.GetAttribute(0)) * EnvironmentLoader.TileDimension,
													float.Parse(p_Reader.GetAttribute(1)) * EnvironmentLoader.TileDimension);
					p_Reader.ReadStartElement("startTileCenter");
				}
				else if (p_Reader.IsStartElement("task"))
				{
					t_Point.Task = readTask(p_Reader);
				}
#if DEBUG
				else
				{
					throw new NotImplementedException("LevelReader LoadEnemy could not understand tag '" + p_Reader.Name + "'");
				}
#endif
#if DEBUG
				if (++curLoop > maxLoops)
				{
					throw new Exception("maxLoops exceeded. LevelReader may be caught in an infinite loop.");
				}
#endif
			}
#if DEBUG
			curLoop = 0;
#endif
			t_Obj.Faction = Collidable.Factions.Enemy;
			t_Point.SpawnObj = t_Obj;
			//add the ship to the event list
			if (m_Events.ContainsKey(m_Distance))
			{
				m_Events[m_Distance].Add(new Event(t_Point));
			}
			else
			{
				t_List.Add(new Event(t_Point));
				m_Events.Add(m_Distance, t_List);
			}
		}

		private void addEvent(int p_distance, Event p_event)
		{
			if (m_Events.ContainsKey(p_distance))
			{
				m_Events[p_distance].Add(p_event);
			}
			else
			{
				List<Event> eventList = new List<Event>();
				eventList.Add(p_event);
				m_Events.Add(m_Distance, eventList);
			}
		}

		private void addEvent(int p_distance, List<Event> p_eventList)
		{
			if (m_Events.ContainsKey(p_distance))
			{
				m_Events[p_distance].AddRange(p_eventList);
			}
			else
			{
				m_Events.Add(m_Distance, p_eventList);
			}
		}
	}
}
