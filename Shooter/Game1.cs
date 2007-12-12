#region Using Statements
using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

#endregion

namespace Shooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
               
        SpriteBatch mSpriteBatch;
        ArrayList mSpriteManager;
        World mWorld;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //content = new ContentManager(Services);
            
        }


        public void ToggleFullScreen()
        {
            graphics.ToggleFullScreen();
            graphics.ApplyChanges();

        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            TextureLibrary.iniTextures(Services);
            KeyBoardManager.iniKeyboard();
            ShotManager.iniShotManager();
            
            KeyBoardManager.addMapping(Keys.Enter,new KeyBoardAction(this,"ToggleFullScreen","Toggle Full Screen"));

            //mSpriteManager = new ArrayList();
            //mSpriteManager.Add(new Sprite(new Rectangle(250,250,50,50),new Rectangle(0,0,51,51), TextureLibrary.get("Ship2")));
            mWorld = new World();

            base.Initialize();
        }


        /// <summary>
        /// Load your graphics content.  If loadAllContent is true, you should
        /// load content from both ResourceManagementMode pools.  Otherwise, just
        /// load ResourceManagementMode.Manual content.
        /// </summary>
        /// <param name="loadAllContent">Which type of content to load.</param>
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                mSpriteBatch = new SpriteBatch(graphics.GraphicsDevice);
                //TextureLibrary.iniTextures(Services);
            }

        }


        /// <summary>
        /// Unload your graphics content.  If unloadAllContent is true, you should
        /// unload content from both ResourceManagementMode pools.  Otherwise, just
        /// unload ResourceManagementMode.Manual content.  Manual content will get
        /// Disposed by the GraphicsDevice during a Reset.
        /// </summary>
        /// <param name="unloadAllContent">Which type of content to unload.</param>
        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent)
            {
                TextureLibrary.unloadAll();
                mSpriteBatch = null;
                mSpriteManager = null;
            }

            // TODO: Unload any ResourceManagementMode.Manual content
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Window.Title = "Frame Rate - " + FrameRate.CalculateFrameRate() ;
            KeyBoardManager.checkKeyBoard((float)(gameTime.ElapsedGameTime.TotalSeconds));
            mWorld.update((float)(gameTime.ElapsedGameTime.TotalSeconds));
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            mSpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                  

            mWorld.draw(mSpriteBatch);
            /*
            foreach (Sprite tSprite in mSpriteManager)
            {
                tSprite.draw(mSpriteBatch);
            }
             * */


            mSpriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
