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
		private Dictionary<int, List<Event>> m_Events=new Dictionary<int, List<Event>>();

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
			FileName = System.Environment.CurrentDirectory + "\\Content\\Levels\\"+p_FileName;
		}

		public void ReadFile()
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
				//needs to be placed in a loop
				if (reader.Name.Equals("action"))
				{
					m_Distance = int.Parse(reader.GetAttribute(0));
					reader.ReadStartElement("action");
					if (reader.Name.Equals("createShip"))
					{
						reader.ReadStartElement("createShip");
						LoadEnemy(reader);
					}
				}
				//that reads to the end of the file
			}
			else
			{
				//cann't do anything no file name
			}
		}

		private void LoadEnemy(XmlReader p_Reader)
		{
			Ship t_Ship;

			Dictionary<Events.ValueKeys, object> t_dic = new Dictionary<Events.ValueKeys, object>();
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

			float m_ZBuff=0;
			String zbGround;
			String zbLevel;

			Collidable.Factions m_Faction=0;
			String faction;

			int m_Health;
			int m_Shield;

			Dictionary<PathStrategy.ValueKeys, Object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
			//dic.Add(PathStrategy.ValueKeys.Base, enemy);
			float pSpeed;//dic.Add(PathStrategy.ValueKeys.Speed, 100f);
			Vector2 pEndPos = new Vector2();//dic.Add(PathStrategy.ValueKeys.End, new Vector2(700, 200));
			float pDuration;//dic.Add(PathStrategy.ValueKeys.Duration, 5f);
			String pType;
			bool pRotation;

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
			if (faction.Equals("Enemy"))
			{
				m_Faction = Collidable.Factions.Enemy;
			}
			else if (faction.Equals("Player"))
			{
				m_Faction = Collidable.Factions.Player;
			}
			else if (faction.Equals("Environment"))
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
								m_Shield, null, m_Speed, m_DamageTexture, m_Radius);

			//read in paths
			do
			{
				dic = new Dictionary<PathStrategy.ValueKeys, object>();
				//type
				p_Reader.ReadStartElement("path");
				pType = p_Reader.ReadString();
				p_Reader.ReadEndElement();
				if(pType.Equals("Straight"))
				{					
					//x y
					pEndPos.X = int.Parse(p_Reader.GetAttribute(0));
					pEndPos.Y = int.Parse(p_Reader.GetAttribute(1));
					//duration
					p_Reader.ReadStartElement();
					pDuration = float.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
					//speed
					p_Reader.ReadStartElement();
					pSpeed = int.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();
					//rotation
					p_Reader.ReadStartElement();
					pRotation = bool.Parse(p_Reader.ReadString());
					p_Reader.ReadEndElement();

					dic.Add(PathStrategy.ValueKeys.Base, t_Ship);
					dic.Add(PathStrategy.ValueKeys.Speed, pSpeed);
					dic.Add(PathStrategy.ValueKeys.End, pEndPos);
					dic.Add(PathStrategy.ValueKeys.Duration, pDuration);
					dic.Add(PathStrategy.ValueKeys.Rotation, false);
					t_Ship.PathList.AddPath(new Path(Paths.Straight, dic));
				}
				//t_Ship.PathList.AddPath(m_Path);
				p_Reader.ReadEndElement();
			} while (p_Reader.Name.Equals("path"));
			
			//add the ship to the event list
			t_dic.Add(Events.ValueKeys.Sprite,t_Ship);
			if (m_Events.ContainsKey(m_Distance))
			{
				m_Events[m_Distance].Add(new Event(Type.create, t_dic));
			}
			else
			{
				t_List.Add(new Event(Type.create, t_dic));
				m_Events.Add(m_Distance, t_List);
			}
		}
	}
}
