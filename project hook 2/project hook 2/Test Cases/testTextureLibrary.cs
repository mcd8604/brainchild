using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace project_hook.UseCases
{
   [TestFixture]
    public class testTextureLibrary
    {
        Game g;

        [SetUp]
        public void Init()
        {
            g= new Game();
                //TextureLibrary.iniTextures();
            
        }

        [Test]
        public void testLoad()
        {
            string name = "Ship2";

            Assert.IsTrue(TextureLibrary.LoadTexture(name));
            Texture2D ret = TextureLibrary.getTexture(name);
            Assert.IsNotNull(ret);

            Assert.IsTrue(ret.Name.Equals(name));
        }


    }
}
