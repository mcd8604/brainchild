using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Wintellect.PowerCollections;
using System.Collections;
using System.IO;

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
			Path = System.Environment.CurrentDirectory + "\\Content\\Textures\\";
		}
		public LevelReader(String p_Path)
		{
			Path = p_Path;
		}
		public LevelReader(String p_Path, String p_FileName)
		{
			Path = p_Path;
			FileName = p_FileName;
		}

		public void ReadFile()
		{
			//Checks for the XML file
			if (File.Exists(m_Path+m_FileName))
			{
				//Lods the XML file
				XmlDocument doc = new XmlDocument();
				doc.Load(m_Path + m_FileName);

				//Checks gets the Rectangles to load
				XmlElement elm = doc.DocumentElement;
				XmlNodeList lstRect = elm.ChildNodes;

				//Iterates over each rectangle
				for (int i = 0; i < lstRect.Count; i++)
				{
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
				}
			}
			else
			{
				//cann't do anything no file name
			}
		}

		private void LoadEnemy(XmlNodeList p_NodeList)
		{
		}
	}
}
