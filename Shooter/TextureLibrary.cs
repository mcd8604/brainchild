using System;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Shooter
{
    class TextureLibrary
    {
        private static Hashtable mTextures;
        private static ContentManager mTextureManager;

        public static void iniTextures(GameServiceContainer services)
        {
            if (mTextures == null)
            {
                mTextures = new Hashtable();
                mTextureManager = new ContentManager(services);
            }
        }


        //This code attempts to get a texture reference
        //It will attempt to load the texture if is not in the hashtable
        public static Texture2D get(string textureName){
            if (mTextures == null)
            {
                return null;
            }
            
            Texture2D retVal = null;
            if (mTextures.ContainsKey(textureName))
            {
                retVal =  ((Texture2D)(mTextures[textureName]));
            }
            else
            {
                if (LoadTexture(textureName))
                {
                    retVal = ((Texture2D)(mTextures[textureName]));
                }
            }
            return retVal;
        }

        //This loads a texture and Stores it in the hashtable.
        //The Textures name is it's key
        //If it cannot find or load the texture it will return false;
        public static Boolean LoadTexture(string textureName){
            if (mTextures == null)
            {
                return false;
            }

            try
            {
                
                Texture2D tTexture = mTextureManager.Load<Texture2D>(textureName);
                mTextures.Add(textureName, tTexture);

                return true;

            }catch(Exception e){

                return false;
            }
        }

        public static Boolean unloadAll(){
            try
            {
                mTextureManager.Unload();
                mTextures = null;
                return true;
            }
            catch(Exception e){
                return false;
            }
        }
    }
}
