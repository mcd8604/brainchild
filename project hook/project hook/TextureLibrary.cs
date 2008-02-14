using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Xml;
using System.IO;
using Wintellect.PowerCollections;

namespace project_hook
{

	/// <summary>
	/// Description: This class will laod and store all texture data as well as creating
	///              Game Texture objects 
	/// 
	/// TODO:
	/// 3. Create an easy way to make the XML definitions for the game
	/// 
	/// </summary>
	class TextureLibrary
	{
		//This holds a list of all the 2DTexture Objects.
		//The Key is the name of the Texture asset.
		private static OrderedDictionary<String, Texture2D> m_Textures;

		//This stores a reference to all the game textures
		//The first key is the name of the 2DTexture the GameTexture is using.
		//The key for the second Dictionary is the tag that is defined for that area of the texture
		//Tags and Source rectangles are loaded from the XML file with the same name as the texture.
		//If no XML file is included the Tag for the Texture will be "" and it's source will be the size of the entire Texture
		private static OrderedDictionary<String, OrderedDictionary<String, GameTexture>> m_GameTextures;

		//This is passed in from the Game class.
		private static ContentManager m_TextureManager;

		//Our current path in the system.  
		//This is used to read the XML files
		private static String path = System.Environment.CurrentDirectory + "\\Content\\Textures\\";

		//This method will initialize the Texture Dictionarys and set the content manager      
		public static void iniTextures(ContentManager content)
		{
			if (m_Textures == null && m_GameTextures == null)
			{
				m_TextureManager = content;//new ContentManager(services);
				m_Textures = new OrderedDictionary<String, Texture2D>();
				m_GameTextures = new OrderedDictionary<string, OrderedDictionary<string, GameTexture>>();
			}
		}

		//This method gets a the GameTexture who has the corresponding name and tag.
		//If no texture is found NULL is returned. If we're in Debug an Exception will be thrown.
		public static GameTexture getGameTexture(string name, string tag)
		{
			//Makes sure the Texture lists have been initialized
			if (m_GameTextures == null)
			{
				return null;
			}

			GameTexture r_Texture = null;

			//Checks if the GameTexture is in the Dictionary
			if (m_GameTextures.ContainsKey(name))
			{
				if (m_GameTextures[name].ContainsKey(tag))
				{
					r_Texture = (m_GameTextures[name])[tag];
				}
			}
#if DEBUG
			else
			{
				throw new Exception("Texture not loaded: " + name);
			}
#endif

			return r_Texture;
		}


		//This method gets a the 2DTexture who has the corresponding asset name.
		//If no texture is found NULL is returned. If we're in Debug an Exception will be thrown.
		public static Texture2D getTexture2D(string name, string tag)
		{
			//Makes sure the Texture lists have been initialized
			if (m_Textures == null)
			{
				return null;
			}

			Texture2D r_Texture = null;

			//Checks if the GameTexture is in the Dictionary
			if (m_Textures.ContainsKey(name))
			{
				r_Texture = m_Textures[name];
			}
#if DEBUG
			else
			{
				throw new Exception("Texture not loaded: " + name);
			}
#endif

			return r_Texture;
		}

		//Loads a texture into the content manager and returns a reference to the new 2DTexture
		private static Texture2D loadTextureByName(string textureName)
		{
			return m_TextureManager.Load<Texture2D>(path + textureName);

		}


		//This loads a texture and Stores it in the hashtable.
		//The Textures name is it's key
		//If it cannot find or load the texture it will return false;
		public static Boolean LoadTexture(string textureName)
		{
			if (m_Textures == null || m_GameTextures == null)
			{
				return false;
			}

			try
			{
				Texture2D tTexture = loadTextureByName(textureName);

				if (m_Textures.ContainsKey(textureName))
				{
					m_Textures.Replace(textureName, tTexture);

				}
				else
				{
					m_Textures.Add(textureName, tTexture);
				}

				//This code will load up a textures rectangle Description

				string strFilename = path + textureName + ".xml";

				//Checks for the XML file
				if (File.Exists(strFilename))
				{
					//Lods the XML file
					XmlDocument doc = new XmlDocument();
					doc.Load(strFilename);

					//Checks gets the Rectangles to load
					XmlElement elm = doc.DocumentElement;

					if (elm.HasChildNodes)
					{
						XmlNodeList lstRect = elm.ChildNodes;

						//Iterates over each rectangle
						for (int i = 0; i < lstRect.Count; i++)
						{
							XmlNodeList nodes = lstRect.Item(i).ChildNodes;
							int j = 0;

							String name = (String)(nodes.Item(j++).InnerText);
							String tag = (String)(nodes.Item(j++).InnerText);
							int x = int.Parse(nodes.Item(j++).InnerText);
							int y = int.Parse(nodes.Item(j++).InnerText);
							int width = int.Parse(nodes.Item(j++).InnerText);
							int height = int.Parse(nodes.Item(j++).InnerText);

							//Stores it in the GameTexture table
							GameTexture t_GameTexture = new GameTexture(name, tag, tTexture, new Rectangle(x, y, width, height));
							addGameTexture(name, tag, t_GameTexture);

						}
					}
					else
					{
						if (elm.HasAttribute("cellWidth") &&
							elm.HasAttribute("cellHeight") &&
							elm.HasAttribute("numRows") &&
							elm.HasAttribute("numCols"))
						{
							int cellWidth = int.Parse(elm.GetAttribute("cellWidth"));
							int cellHeight = int.Parse(elm.GetAttribute("cellHeight"));
							int numRows = int.Parse(elm.GetAttribute("numRows"));
							int numCols = int.Parse(elm.GetAttribute("numCols"));
							for (int row = 0; row < numRows; row++)
							{
								for (int col = 0; col < numCols; col++)
								{
									int index = (row * numCols) + col;
									String tag = index.ToString();
									int x = col * cellWidth;
									int y = row * cellHeight;

									//Stores it in the GameTexture table
									GameTexture t_GameTexture = new GameTexture(textureName, tag, tTexture, new Rectangle(x, y, cellWidth, cellHeight));
									addGameTexture(textureName, tag, t_GameTexture);
								}
							}
						}
						else
						{
							throw new ContentLoadException("Invalid Texture XML: " + strFilename);
						}
					}

				}
				else
				{
					GameTexture t_GameTexture = new GameTexture(textureName, "", tTexture, new Rectangle(0, 0, tTexture.Width, tTexture.Height));
					addGameTexture(textureName, "", t_GameTexture);
				}
			}
			catch (ContentLoadException e)
			{

				Game.Out.WriteLine("TextureLibrary.LoadTexure.ContentLoadException: " + e);
				return false;
			}
			catch (IOException e)
			{

				Game.Out.WriteLine("TextureLibrary.LoadTexure.IOException: " + e);
				return false;
			}
#if DEBUG
			System.Diagnostics.Debug.Assert(m_Textures.ContainsKey(textureName));
#endif
			return true;
		}

		public static OrderedDictionary<String, GameTexture> getSpriteSheet(String name)
		{
			if (m_GameTextures != null && m_GameTextures.ContainsKey(name))
			{
				return m_GameTextures[name];
			}
			else
			{
				return null;
			}
		}

		//This method adds the 
		private static void addGameTexture(string p_name, string p_tag, GameTexture p_GameTexture)
		{
			if (m_GameTextures.ContainsKey(p_name))
			{
				if ((m_GameTextures[p_name]).ContainsKey(p_tag))
				{
					m_GameTextures[p_name].Replace(p_tag, p_GameTexture);
				}
				else
				{
					m_GameTextures[p_name].Add(p_tag, p_GameTexture);
				}

			}
			else
			{
				OrderedDictionary<String, GameTexture> t_Dic = new OrderedDictionary<string, GameTexture>();
				t_Dic.Add(p_tag, p_GameTexture);
				m_GameTextures.Add(p_name, t_Dic);
			}

		}

		public static Boolean unloadAll()
		{
			//m_TextureManager.Unload();
			//m_Textures = null;
			//m_GameTextures = null;
			return true;

		}

		public static void reloadAll()
		{
			foreach (String key in m_GameTextures.Keys)
			{

				OrderedDictionary<String, GameTexture> gd = m_GameTextures[key];
				Texture2D reload = null;

				foreach (string Key2 in gd.Keys)
				{
					gd.ContainsKey(Key2);
					GameTexture gt = gd[Key2];
					if (reload == null)
					{
						reload = loadTextureByName(key); ;
					}
					gt.Texture = reload;
				}

			}

		}


		// A Quick Method to get access to what I need - temporary
		public static SpriteFont getFont(String name)
		{
			return m_TextureManager.Load<SpriteFont>(System.Environment.CurrentDirectory + "\\Content\\Fonts\\" + name);
		}

	}

}