using System;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace project_hook
{

   /*
   * Description: This class will laod and store all texture data as well as creating
   *              Game Texture objects 
   * 
   * TODO:
   *  1. Add GameTexture support
   */
    class TextureLibrary
    {
        private static Hashtable m_Textures;
        private static Hashtable m_GameTextures;
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

                return true;

            }
            catch (ContentLoadException e)
            {

                return false;
            }
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