#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace project_hook
{
    /// <summary>
    /// This is the main type for your game
    /// This will be for our main game code
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ContentManager content;
        SpriteBatch m_SpriteBatch;

        KeyHandler m_KeyHandler;

        World m_World;
        Menu m_Menu;

        InputHandlerState m_InputHandler;

        public enum InputHandlerState
        {
            World,
            Menu
        }

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //This sets our graphics options
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;

            m_SpriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            // This will initialize any libraries or static classes needed
            TextureLibrary.iniTextures(content);

            m_KeyHandler = new KeyHandler();


            m_World = new World();
            m_World.loadLevel(content);
            m_World.initialize( new Rectangle(0,0,graphics.PreferredBackBufferWidth,graphics.PreferredBackBufferHeight));
            m_InputHandler = InputHandlerState.World;
            m_World.changeState(World.GameState.Running);
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
                // TODO: Load any ResourceManagementMode.Automatic content
            }

            
            // TODO: Load any ResourceManagementMode.Manual content
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
                // TODO: Unload any ResourceManagementMode.Automatic content
                content.Unload();
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
            if (m_KeyHandler.IsKeyPressed(Keys.F))
            {
                graphics.ToggleFullScreen();
            }

            //This will check if the game world is created.  
            if (m_World != null)
            {
                //This checks if the world is supposed to be receiving key input
                //If it's not it will just update the game without key input
                if (m_InputHandler == InputHandlerState.World)
                {
                    m_World.update(gameTime, m_KeyHandler);
                }
                else
                {
                    m_World.update(gameTime);
                }
            }

        
            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            if (m_World != null)
            {
                m_World.draw(m_SpriteBatch);

            }

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
