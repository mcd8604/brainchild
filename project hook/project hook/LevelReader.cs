using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Wintellect.PowerCollections;
using System.Collections;
using System.IO;
using Microsoft.Xna.Framework;

namespace project_hook
{
	class LevelReader
	{
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
				while (reader.IsStartElement("action"))
				{
					m_Distance = int.Parse(reader.GetAttribute(0));
					reader.ReadStartElement();
						while (reader.IsStartElement("createShip"))
						{
							reader.ReadStartElement();
							LoadEnemy(reader);
							reader.ReadEndElement();
						}
						while (reader.IsStartElement("changeSpeed"))
						{
							reader.ReadStartElement();
							ChangeSpeed(reader);
							reader.ReadEndElement();
						}
						while (reader.IsStartElement("changeFile"))
						{
							reader.ReadStartElement();
							NextFile(reader);
							reader.ReadEndElement();
						}
					reader.ReadEndElement();
				}
			}
			else
			{
				//cann't do anything no file name
			}

			return m_Events;
		}

		private void NextFile(XmlReader p_Reader)
		{
			String m_FileName;

			List<Event> t_List = new List<Event>();

			//read speed
			p_Reader.ReadStartElement();
			m_FileName = p_Reader.ReadString();
			p_Reader.ReadEndElement();

			//add the change speed to the event list
			if (m_Events.ContainsKey(m_Distance))
			{
				m_Events[m_Distance].Add(new Event(m_FileName));
			}
			else
			{
				t_List.Add(new Event(m_FileName));
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
			Ship t_Ship;

			List<Event> t_List = new List<Event>();

			String m_Name;
			Vector2 m_StartPos = new Vector2();
			int m_Height;
			int m_Width;

			GameTexture m_Texture;
			String gtName;
			String gtTag = "";

			float m_Alpha;
			bool m_Visible;
			float m_Degree;

			float m_ZBuff = 0;
			String zbGround;
			String zbLevel;

			Collidable.Factions m_Faction = 0;
			String faction;

			int m_Health;
			int m_Shield;

			//stuff for task
			//float pSpeed;
			//Vector2 pEndPos = new Vector2();
			//float pDuration;
			//String pType;
			//bool pRotation;

			//stuff for weapon
			//Weapon t_Wep;
			//String wShotName;
			//int wDamage;
			//float wDelay;
			//float wSpeed;
			//float wAngle;

			int m_Speed;

			GameTexture m_DamageTexture;
			String dtName;
			String dtTag = "";
			float m_Radius;

			//read in name
			p_Reader.ReadStartElement();
			m_Name = p_Reader.ReadString();
			p_Reader.ReadEndElement();
			//Console.WriteLine(m_Name);

			//read in start position x y
			m_StartPos.X = int.Parse(p_Reader.GetAttribute(0));
			m_StartPos.Y = int.Parse(p_Reader.GetAttribute(1));
			//Console.WriteLine(m_StartPos.X);
			//Console.WriteLine(m_StartPos.Y);

			//read in height
			p_Reader.ReadStartElement();
			m_Height = int.Parse(p_Reader.ReadString());
			p_Reader.ReadEndElement();
			//Console.WriteLine(m_Height);

			//read in width
			p_Reader.ReadStartElement();
			m_Width = int.Parse(p_Reader.ReadString());
			p_Reader.ReadEndElement();
			//Console.WriteLine(m_Width);

			//read in texture
			gtName = p_Reader.GetAttribute(0);
			if (p_Reader.AttributeCount > 1)
			{
				gtTag = p_Reader.GetAttribute(1);
			}
			m_Texture = TextureLibrary.getGameTexture(gtName, gtTag);

			//read in alpha
			p_Reader.ReadStartElement();
			m_Alpha = float.Parse(p_Reader.ReadString());
			p_Reader.ReadEndElement();

			//read in visible
			p_Reader.ReadStartElement();
			m_Visible = bool.Parse(p_Reader.ReadString());
			p_Reader.ReadEndElement();

			//read in degree
			p_Reader.ReadStartElement();
			m_Degree = float.Parse(p_Reader.ReadString());
			p_Reader.ReadEndElement();

			//read in zBuff
			zbGround = p_Reader.GetAttribute(0);
			zbLevel = p_Reader.GetAttribute(1);
			if (zbGround.Equals("ForeGround"))
			{
				if (zbLevel.Equals("Top"))
				{
					m_ZBuff = Depth.ForeGround.Top;
				}
				else if (zbLevel.Equals("Mid"))
				{
					m_ZBuff = Depth.ForeGround.Mid;
				}
				else if (zbLevel.Equals("Bottom"))
				{
					m_ZBuff = Depth.ForeGround.Bottom;
				}
			}
			else if (zbGround.Equals("BackGround"))
			{
				if (zbLevel.Equals("Top"))
				{
					m_ZBuff = Depth.BackGround.Top;
				}
				else if (zbLevel.Equals("Mid"))
				{
					m_ZBuff = Depth.BackGround.Mid;
				}
				else if (zbLevel.Equals("Bottom"))
				{
					m_ZBuff = Depth.BackGround.Bottom;
				}
			}
			else if (zbGround.Equals("MidGround"))
			{
				if (zbLevel.Equals("Top"))
				{
					m_ZBuff = Depth.MidGround.Top;
				}
				else if (zbLevel.Equals("Mid"))
				{
					m_ZBuff = Depth.MidGround.Mid;
				}
				else if (zbLevel.Equals("Bottom"))
				{
					m_ZBuff = Depth.MidGround.Bottom;
				}
			}

			//read in faction
			p_Reader.ReadStartElement();
			faction = p_Reader.ReadString();
			if (faction.Equals("enemy"))
			{
				m_Faction = Collidable.Factions.Enemy;
			}
			else if (faction.Equals("player"))
			{
				m_Faction = Collidable.Factions.Player;
			}
			else if (faction.Equals("environment"))
			{
				m_Faction = Collidable.Factions.Environment;
			}
			p_Reader.ReadEndElement();

			//read in health
			p_Reader.ReadStartElement();
			m_Health = int.Parse(p_Reader.ReadString());
			p_Reader.ReadEndElement();

			//read in shield
			p_Reader.ReadStartElement();
			m_Shield = int.Parse(p_Reader.ReadString());
			p_Reader.ReadEndElement();

			//read in speed
			p_Reader.ReadStartElement();
			m_Speed = int.Parse(p_Reader.ReadString());
			p_Reader.ReadEndElement();
			//Console.WriteLine(m_Speed);

			//read in damage texture
			dtName = p_Reader.GetAttribute(0);
			if (p_Reader.AttributeCount > 1)
			{
				dtTag = p_Reader.GetAttribute(1);
			}
			m_DamageTexture = TextureLibrary.getGameTexture(dtName, dtTag);

			//read in radius
			p_Reader.ReadStartElement();
			m_Radius = float.Parse(p_Reader.ReadString());
			p_Reader.ReadEndElement();
			//Console.WriteLine(m_Radius);

			//create ship
			t_Ship = new Ship(m_Name, m_StartPos, m_Health, m_Width, m_Texture, m_Alpha, m_Visible, m_Degree, m_ZBuff, m_Faction, m_Health,
								m_Shield, m_DamageTexture, m_Radius);

			while (p_Reader.IsStartElement("weapon"))
			{
				t_Ship.addWeapon(readWeapon(p_Reader));
			}

			if (p_Reader.IsStartElement("task"))
			{
				t_Ship.Task = readTask(p_Reader);
			}

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



		private static Weapon readWeapon(XmlReader p_Reader)
		{
			Weapon weapon = null;
			string pType = p_Reader.GetAttribute("type");
			p_Reader.ReadStartElement("weapon");

			switch (pType)
			{
				case "Example":
					weapon = new WeaponExample();
					break;
				default:
					throw new NotImplementedException("'" + pType + "' is not a recognized Weapon");
			}

			while (p_Reader.IsStartElement())
			{
				if (p_Reader.IsStartElement("shotName"))
				{
					p_Reader.ReadStartElement("shotName");
					weapon.ShotName = p_Reader.ReadString();
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("damage"))
				{
					p_Reader.ReadStartElement("damage");
					weapon.Damage = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("delay"))
				{
					p_Reader.ReadStartElement("delay");
					weapon.Delay = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("speed"))
				{
					p_Reader.ReadStartElement("speed");
					weapon.Speed = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
				else if (p_Reader.IsStartElement("angle"))
				{
					p_Reader.ReadStartElement("angle");
					weapon.Angle = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
				}
			}

			p_Reader.ReadEndElement();
			return weapon;

		}

		private static Task readTask(XmlReader p_Reader)
		{
			Task task;
			string pType = p_Reader.GetAttribute("type");
			p_Reader.ReadStartElement("task");

			switch (pType)
			{
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
				case "RotateAngle":
					TaskRotateAngle rotateAngle = new TaskRotateAngle();
					while (p_Reader.IsStartElement())
					{
						if (p_Reader.IsStartElement("angle"))
						{
							p_Reader.ReadStartElement("angle");
							rotateAngle.Angle = float.Parse(p_Reader.ReadString());
							p_Reader.ReadEndElement();
						}
					}
					task = rotateAngle;
					break;
				case "StraightVelocity":
					TaskStraightVelocity straightVelocity = new TaskStraightVelocity();
					Vector2 v = Vector2.Zero;
					while (p_Reader.IsStartElement())
					{
						if (p_Reader.IsStartElement("velocity"))
						{
							v = new Vector2( float.Parse(p_Reader.GetAttribute("x")), float.Parse(p_Reader.GetAttribute("y")));
							p_Reader.ReadStartElement("velocity");
						}
					}
					straightVelocity.Velocity = v;
					task = straightVelocity;
					break;
				default:
					throw new NotImplementedException("'" + pType + "' is not a recognized Task");
			}

			p_Reader.ReadEndElement();
			return task;
		}
	}
}
