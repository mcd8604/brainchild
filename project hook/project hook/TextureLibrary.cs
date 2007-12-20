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

   /*
   * Description: This class will laod and store all texture data as well as creating
   *              Game Texture objects 
   * 
   * TODO:
   *  1. Convert to the ordered Dictionary
    * 2. Allow retrieval of gametexture objects
    * 3. Create an easy way to make the XML definitions for the game
   */
    class TextureLibrary
    {
        private static Hashtable m_Textures;
        
        private static Hashtable m_GameTextures;

        //This will be what stores the GameTextures!!
        private static OrderedDictionary<String, OrderedDictionary<String, GameTexture>> m_GameTexturePower;

        private static ContentManager m_TextureManager;
        private static String path = System.Environment.CurrentDirectory  + "\\Content\\Textures\\";
        
        //This method will initialize the Textures      
        public static void iniTextures(GameServiceContainer services)
        {
            if (m_Textures == null && m_GameTextures == null)
            {
                m_Textures = new Hashtable();
                m_GameTextures = new Hashtable();
                m_TextureManager = new ContentManager(services);
                m_GameTexturePower = new OrderedDictionary<string,OrderedDictionary<string,GameTexture>>();
                
                
            }
        }

        //This code attempts to get a texture reference
        //It will attempt to load the texture if is not in the hashtable
        public static Texture2D get(string textureName)
        {
            if (m_Textures == null)
            {
                return null;
            }

            Texture2D retVal = null;
        
            if (m_Textures.ContainsKey(textureName))
            {
                retVal = ((Texture2D)(m_Textures[textureName]));
            }
            else
            {
                if (LoadTexture(textureName))
                {
                    retVal = ((Texture2D)(m_Textures[textureName]));
                }
            }
            return retVal;
        }

        //This loads a texture and Stores it in the hashtable.
        //The Textures name is it's key
        //If it cannot find or load the texture it will return false;
        public static Boolean LoadTexture(string textureName)
        {
            if (m_Textures == null)
            {
                return false;
            }

            try
            {

                Texture2D tTexture = m_TextureManager.Load<Texture2D>(path + textureName);
                m_Textures.Add(textureName, tTexture);

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
                        GameTexture t_Texture = new GameTexture(name, tag, tTexture, new Rectangle(x, y, width, height));
                        m_GameTextures.Add(name, t_Texture);
                        OrderedDictionary<String,GameTexture> t_Dic = new OrderedDictionary<string,GameTexture>();
                        t_Dic.Add(tag, t_Texture);
                        m_GameTexturePower.Add(name,t_Dic);
                    }
                    
                }
                else
                    return false;
            }
            catch (ContentLoadException e)
            {

                return false;
            }
            catch (IOException e)
            {
                return false;
            }

            return true;
        }

        public static Boolean unloadAll()
        {
            try
            {
                m_TextureManager.Unload();
                m_Textures = null;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


    }
}

/*
  * Class: TextureLibrary
  * Authors: Karl
  * Date Created: 12/19/2007
  * 
  * Change Log:
  *     12/19/2007 - Karl - Initial Creation,  Created properties, constructor, load and Unload. 
  *     
  */