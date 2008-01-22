using System;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace ObjectManagerExample
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        ObjectManager objectManager;
        AnimatedSprite player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            objectManager = new ObjectManager(this);
            objectManager.CustomInstanceCreator = DefaultCustomInstanceCreator;
            objectManager.AddCustomLoader("TextureLoader", DefaultTextureLoader);
            objectManager.AddGlobalVariable("Game1", this);
            objectManager.LoadFromXML("Content/XMLFile.xml");

            player = objectManager.GetObject<AnimatedSprite>("Player", true);
            Components.Add(player);

            base.Initialize();
        }

        protected bool DefaultCustomInstanceCreator(ref object targetInstance, Type targetType)
        {
            if (objectManager.DefaultCustomInstanceCreator(ref targetInstance, targetType))
                return true;

            if (typeof(DrawableGameComponent).IsAssignableFrom(targetType))
            {
                try
                {
                    targetInstance = Activator.CreateInstance(targetType, this);
                    return true;
                }
                catch (Exception)
                {
                }
            }

            return false;
        }

        protected void DefaultTextureLoader(ref object targetInstance, XmlNode xmlNode)
        {
            try
            {
                targetInstance = Content.Load<Texture2D>(xmlNode.ChildNodes[0].InnerText);
            }
            catch (Exception)
            {
            }
        }

        public Vector2 GetRandomPosition()
        {
            Random random = new Random();

            Vector2 position = Vector2.Zero;
            position.X = random.Next(0, 300);
            position.Y = random.Next(0, 300);

            return position;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            if (keyboardState.IsKeyDown(Keys.C))
                if (player != null)
                    player.PlayAnimation("center");
            if (keyboardState.IsKeyDown(Keys.D))
                if (player != null)
                    player.PlayAnimation("die");

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
}
