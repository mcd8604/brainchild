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
		private OrderedDictionary<int, ArrayList> m_events;

		private String m_Path;
		public String Path
		{
			get
			{
				return m_Path;
			}
			set
			{
				m_Path = value;
			}
		}

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
			Path = System.Environment.CurrentDirectory + "\\Content\\Levels\\";
		}
		public LevelReader(String p_FileName)
		{
			Path = System.Environment.CurrentDirectory + "\\Content\\Levels\\";
			FileName = p_FileName;
		}

		public void ReadFile()
		{
			String t_Name = m_Path + m_FileName;
			int t_Dist = 0;
			
			//Checks for the XML file
			if (File.Exists(t_Name))
			{
				XmlReaderSettings t_Settings = new XmlReaderSettings();
				t_Settings.ConformanceLevel = ConformanceLevel.Fragment;
				t_Settings.IgnoreWhitespace = true;
				t_Settings.IgnoreComments = true;
				XmlReader reader = XmlReader.Create(m_Path + m_FileName, t_Settings);

				reader.Read();
				reader.ReadStartElement("level");

				if (reader.Name.Equals("action"))
				{
					t_Dist = int.Parse(reader.GetAttribute(0));
					reader.ReadStartElement("action");
					if (reader.Name.Equals("createShip"))
					{
						reader.ReadStartElement("createShip");
						LoadEnemy(reader);
					}
				}

				////Lods the XML file
				//XmlDocument doc = new XmlDocument();
				//doc.Load(m_Path + m_FileName);

				////Checks gets the Rectangles to load
				//XmlElement elm = doc.DocumentElement;
				//XmlNodeList lstRect = elm.ChildNodes;

				//Iterates over each rectangle
				//for (int i = 0; i < lstRect.Count; i++)
				//{
					
					//XmlNodeList nodes = lstRect.Item(i).ChildNodes;
					//int j = 0;

					//String name = (String)(nodes.Item(j++).InnerText);
					//String tag = (String)(nodes.Item(j++).InnerText);
					//int x = int.Parse(nodes.Item(j++).InnerText);
					//int y = int.Parse(nodes.Item(j++).InnerText);
					//int width = int.Parse(nodes.Item(j++).InnerText);
					//int height = int.Parse(nodes.Item(j++).InnerText);

					////Stores it in the GameTexture table
					//GameTexture t_GameTexture = new GameTexture(name, tag, tTexture, new Rectangle(x, y, width, height));
					//addGameTexture(name, tag, t_GameTexture);
				//}
			}
			else
			{
				//cann't do anything no file name
			}
		}

		private void LoadEnemy(XmlReader p_Reader)
		{
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
			float m_ZBuff;
			Collidable.Factions m_Faction;
			int m_Health;
			int m_Shield;

			Dictionary<PathStrategy.ValueKeys, Object> dic = new Dictionary<PathStrategy.ValueKeys, object>();
			//dic.Add(PathStrategy.ValueKeys.Base, enemy);
			float pSpeed;//dic.Add(PathStrategy.ValueKeys.Speed, 100f);
			Vector2 pEndPos = new Vector2();//dic.Add(PathStrategy.ValueKeys.End, new Vector2(700, 200));
			float pDuration;//dic.Add(PathStrategy.ValueKeys.Duration, 5f);
			String pType;

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
			gtName=p_Reader.GetAttribute(0);
			if(p_Reader.AttributeCount>1)
			{
				gtTag=p_Reader.GetAttribute(1);
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
			Console.WriteLine(p_Reader.Name);

			//read in faction
			p_Reader.ReadStartElement();
			p_Reader.ReadString();
			Console.WriteLine(p_Reader.Name);
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
			if (p_Reader.AttributeCount>1)
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


			//read in paths
			do
			{
				//type
				p_Reader.ReadStartElement("path");
				pType = p_Reader.ReadString();
				p_Reader.ReadEndElement();
				Console.WriteLine(pType);
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
				p_Reader.ReadEndElement();
			} while (p_Reader.Name.Equals("path"));
		}
	}
}
