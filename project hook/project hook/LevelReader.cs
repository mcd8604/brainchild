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
		public String FileName
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

		public LevelReader()
		{
			FileName = System.Environment.CurrentDirectory + "\\Content\\Levels\\";
		}
		public LevelReader(String p_FileName)
		{
			FileName = System.Environment.CurrentDirectory + "\\Content\\Levels\\" + p_FileName;
		}

		public Dictionary<int, List<Event>> ReadFile()
		{
			String t_Name = m_FileName;

			//Checks for the XML file
			if (File.Exists(t_Name))
			{
				XmlReaderSettings t_Settings = new XmlReaderSettings();
				t_Settings.ConformanceLevel = ConformanceLevel.Fragment;
				t_Settings.IgnoreWhitespace = true;
				t_Settings.IgnoreComments = true;
				XmlReader reader = XmlReader.Create(m_FileName, t_Settings);

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
						else if (reader.IsStartElement("loadBMP"))
						{
							reader.ReadStartElement();
							LoadBMP(reader);
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
#if DEBUG
			else
			{
				throw new Exception("file not found: " + t_Name);
			}
#endif

			return m_Events;
		}

		private void CreateGate(XmlReader p_Reader)
		{
			Collidable t_Wall = new Collidable();
			Collidable t_Gate = new Collidable();
			GateTrigger t_Trigger = new GateTrigger();

			List<Event> t_List = new List<Event>();

			while (p_Reader.IsStartElement())
			{
				if (p_Reader.IsStartElement("gate"))
				{
					p_Reader.ReadStartElement();
					LoadWall(p_Reader, t_Gate);
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
					LoadWall(p_Reader, t_Wall);
					p_Reader.ReadEndElement();
				}
			}
#if DEBUG
			curLoop = 0;
#endif

			t_Trigger.Gate = t_Gate;
			t_Trigger.Wall = t_Wall;
			t_Wall.Faction = Collidable.Factions.ClearWall;

			if (m_Events.ContainsKey(m_Distance))
			{
				m_Events[m_Distance].Add(new Event(t_Gate));
				m_Events[m_Distance].Add(new Event(t_Trigger));
				m_Events[m_Distance].Add(new Event(t_Wall));
			}
			else
			{
				t_List.Add(new Event(t_Gate));
				m_Events.Add(m_Distance, t_List);
				m_Events[m_Distance].Add(new Event(t_Trigger));
				m_Events[m_Distance].Add(new Event(t_Wall));
			}
		}

		private void LoadWall(XmlReader p_Reader, Collidable p_Wall)
		{
			while (p_Reader.IsStartElement())
			{
				if (p_Reader.IsStartElement("name"))
				{
					p_Reader.ReadStartElement("name");
					p_Wall.Name = p_Reader.ReadString();
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("startPos"))
				{
					p_Wall.Position = new Vector2(float.Parse(p_Reader.GetAttribute(0)),
													float.Parse(p_Reader.GetAttribute(1)));
					p_Reader.ReadStartElement("startPos");
				}
				else if (p_Reader.IsStartElement("startCenter"))
				{
					p_Wall.Center = new Vector2(float.Parse(p_Reader.GetAttribute(0)),
													float.Parse(p_Reader.GetAttribute(1)));
					p_Reader.ReadStartElement("startCenter");
				}
				else if (p_Reader.IsStartElement("startTile"))
				{
					p_Wall.Position = new Vector2(float.Parse(p_Reader.GetAttribute(0)) * EnvironmentLoader.TileDimension,
													float.Parse(p_Reader.GetAttribute(1)) * EnvironmentLoader.TileDimension);
					p_Reader.ReadStartElement("startTile");
				}
				else if (p_Reader.IsStartElement("height"))
				{
					p_Reader.ReadStartElement("height");
					p_Wall.Height = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("width"))
				{
					p_Reader.ReadStartElement();
					p_Wall.Width = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("texture"))
				{
					String t_Name = p_Reader.GetAttribute(0);
					if (p_Reader.AttributeCount == 1)
					{
						p_Wall.Texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0), "");
						//Console.WriteLine("1");
					}
					else if (p_Reader.AttributeCount == 2)
					{
						p_Wall.Texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0),
																		p_Reader.GetAttribute(1));
						//Console.WriteLine("2");
					}

					p_Reader.ReadStartElement("texture");
				}
				else if (p_Reader.IsStartElement("degree"))
				{
					p_Reader.ReadStartElement("degree");
					p_Wall.RotationDegrees = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("task"))
				{
					p_Wall.Task = readTask(p_Reader);
				}
				else if (p_Reader.IsStartElement("animation"))
				{
					p_Wall.setAnimation(p_Reader.GetAttribute("name"), int.Parse(p_Reader.GetAttribute("fps")));
					//p_Gate.Animation.StartAnimation();
					p_Reader.ReadStartElement("animation");
				}
				else if (p_Reader.IsStartElement("bound"))
				{
					p_Wall.Bound = readBounding(p_Reader);
				}
				else if (p_Reader.IsStartElement("faction"))
				{
					String t_Name;
					p_Reader.ReadStartElement();
					t_Name = p_Reader.ReadString();
					if (t_Name.Equals("Blood"))
					{
						p_Wall.Faction = Collidable.Factions.Blood;
					}
					else if (t_Name.Equals("ClearWall"))
					{
						p_Wall.Faction = Collidable.Factions.ClearWall;
					}
					else if (t_Name.Equals("Enemy"))
					{
						p_Wall.Faction = Collidable.Factions.Enemy;
					}
					else if (t_Name.Equals("Environment"))
					{
						p_Wall.Faction = Collidable.Factions.Environment;
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
			p_Wall.Faction = Collidable.Factions.Environment;
			p_Wall.Z = Depth.GameLayer.Environment;
			p_Wall.Bound = Collidable.Boundings.Rectangle;
			p_Wall.Health = float.NaN;
		}

		private void LoadTrigger(XmlReader p_Reader, GateTrigger p_Trigger)
		{
			while (p_Reader.IsStartElement())
			{
				if (p_Reader.IsStartElement("name"))
				{
					p_Reader.ReadStartElement("name");
					p_Trigger.Name = p_Reader.ReadString();
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
						p_Trigger.Texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0), "");
						//Console.WriteLine("1");
					}
					else if (p_Reader.AttributeCount == 2)
					{
						p_Trigger.Texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0),
																		p_Reader.GetAttribute(1));
						//Console.WriteLine("2");
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
			p_Trigger.Faction = Collidable.Factions.Enemy;
			p_Trigger.Z = Depth.GameLayer.Environment;
			p_Trigger.Bound = Collidable.Boundings.Circle;
			p_Trigger.Radius = 10;
			p_Trigger.Health = 1;
			p_Trigger.Grabbable = false;
		}

		private void NextFile(XmlReader p_Reader)
		{
			String m_FileName;

			List<Event> t_List = new List<Event>();

			//read next file name
			p_Reader.ReadStartElement();
			m_FileName = p_Reader.ReadString();
			p_Reader.ReadEndElement();

			//add the file speed to the event list
			if (m_Events.ContainsKey(m_Distance))
			{
				m_Events[m_Distance].Add(new Event(m_FileName, "FileChange"));
			}
			else
			{
				t_List.Add(new Event(m_FileName, "FileChange"));
				m_Events.Add(m_Distance, t_List);
			}
		}

		private void LoadBMP(XmlReader p_Reader)
		{
			String m_FileName;

			List<Event> t_List = new List<Event>();

			//read next file name
			p_Reader.ReadStartElement();
			m_FileName = p_Reader.ReadString();
			p_Reader.ReadEndElement();

			//add the file speed to the event list
			if (m_Events.ContainsKey(m_Distance))
			{
				m_Events[m_Distance].Add(new Event(m_FileName, "LoadBMP"));
			}
			else
			{
				t_List.Add(new Event(m_FileName, "LoadBMP"));
				m_Events.Add(m_Distance, t_List);
			}
		}

		private void ChangeSpeed(XmlReader p_Reader)
		{
			int m_Speed;

			List<Event> t_List = new List<Event>();

			//read speed
			p_Reader.ReadStartElement();
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
			Ship t_Ship = readShip(p_Reader);
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

		private static Ship readShip(XmlReader p_Reader)
		{
			Ship t_Ship = new Ship();

			while (p_Reader.IsStartElement())
			{
				if (p_Reader.IsStartElement("name"))
				{
					p_Reader.ReadStartElement("name");
					t_Ship.Name = p_Reader.ReadString();
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("grabbable"))
				{
					p_Reader.ReadStartElement("grabbable");
					t_Ship.Grabbable = bool.Parse(p_Reader.ReadString());
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
				else if (p_Reader.IsStartElement("texture"))
				{
					String t_Name = p_Reader.GetAttribute(0);
					if (p_Reader.AttributeCount == 1)
					{
						t_Ship.Texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0), "");
						//Console.WriteLine("1");
					}
					else if (p_Reader.AttributeCount == 2)
					{
						t_Ship.Texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0),
																		p_Reader.GetAttribute(1));
						//Console.WriteLine("2");
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
						t_Ship.setDamageEffect(p_Reader.GetAttribute("name"), "");
					}
					else if (p_Reader.AttributeCount == 2)
					{
						t_Ship.setDamageEffect(p_Reader.GetAttribute("name"), p_Reader.GetAttribute("tag"));
					}
					else
					{
						t_Ship.setDamageEffect(p_Reader.GetAttribute("name"), p_Reader.GetAttribute("tag"), p_Reader.GetAttribute("animation"), int.Parse(p_Reader.GetAttribute("fps")));
					}
					p_Reader.ReadStartElement("damageEffect");
				}
				else if (p_Reader.IsStartElement("shieldDamageEffect"))
				{
					if (p_Reader.AttributeCount == 1)
					{
						t_Ship.setShieldDamageEffect(p_Reader.GetAttribute("name"), "");
					}
					else if (p_Reader.AttributeCount == 2)
					{
						t_Ship.setShieldDamageEffect(p_Reader.GetAttribute("name"), p_Reader.GetAttribute("tag"));
					}
					else
					{
						t_Ship.setShieldDamageEffect(p_Reader.GetAttribute("name"), p_Reader.GetAttribute("tag"), p_Reader.GetAttribute("animation"), int.Parse(p_Reader.GetAttribute("fps")));
					}
					p_Reader.ReadStartElement("shieldDamageEffect");
				}
				else if (p_Reader.IsStartElement("deathEffect"))
				{
					if (p_Reader.AttributeCount == 1)
					{
						t_Ship.setDeathEffect(p_Reader.GetAttribute("name"), "");
					}
					else if (p_Reader.AttributeCount == 2)
					{
						t_Ship.setDeathEffect(p_Reader.GetAttribute("name"), p_Reader.GetAttribute("tag"));
					}
					else
					{
						t_Ship.setDeathEffect(p_Reader.GetAttribute("name"), p_Reader.GetAttribute("tag"), p_Reader.GetAttribute("animation"), int.Parse(p_Reader.GetAttribute("fps")));
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
				else if (p_Reader.IsStartElement("shipPart"))
				{
					p_Reader.ReadStartElement("shipPart");

					Vector2 offset = Vector2.Zero;
					if (p_Reader.IsStartElement("offset"))
					{
						offset = new Vector2(float.Parse(p_Reader.GetAttribute(0)),
														float.Parse(p_Reader.GetAttribute(1)));
						p_Reader.ReadStartElement("offset");
					}

					Ship part = null;
					if (p_Reader.IsStartElement("createShip"))
					{
						p_Reader.ReadStartElement();
						part = readShip(p_Reader);
						p_Reader.ReadEndElement();
					}
					if (part != null)
					{
						TaskParallel task = new TaskParallel(part.Task);
						task.addTask(new TaskAttachAt(t_Ship, offset));
						part.Task = task;
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
					dupsequence.setRepeats( int.Parse(p_Reader.GetAttribute("repeats")) );
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
					while (p_Reader.IsStartElement("weapon") ){
						sequence.addWeapon( readWeapon(p_Reader) );
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
					shot.Name = p_Reader.ReadString();
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
						shot.Texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0), "");
					}
					else if (p_Reader.AttributeCount == 2)
					{
						shot.Texture = TextureLibrary.getGameTexture(p_Reader.GetAttribute(0),
																		p_Reader.GetAttribute(1));
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
					t_Obj = readShip(p_Reader);
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
					t_Point.Name = p_Reader.ReadString();
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
	}
}
