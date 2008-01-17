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
			float m_Alpha;
			bool m_Visible;
			float m_Degree;
			float m_ZBuff;
			//Factions m_Faction;
			int m_Health;
			int m_Shield;
			Path m_Path;
			int m_Speed;
			GameTexture m_DamageTexture;
			float m_Radius;
			
			//read in name
			Console.WriteLine(p_Reader.ReadInnerXml());
			m_Name = p_Reader.ReadInnerXml();
		
		}
	}
}
