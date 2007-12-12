using System;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Shooter
{
    class World
    {
        //Theses will contains sprites!!!
        ArrayList Foreground; //Contains anything that would be shown over the player over the  
        ArrayList Midground; // Contains enmeies and bullets
        ArrayList Background; //Contains Anything that moves in the background
        ArrayList BackgroundTextures; // Contains the textures that are the background, like the scrolling stuff.

        PlayerSprite mPlayer;
        Sprite ene;
        public World()
        {
            Foreground = new ArrayList(); //Contains anything that would be shown over the player over the  
            Midground = new ArrayList();
            Background = new ArrayList();
            BackgroundTextures = new ArrayList();

            mPlayer = new PlayerSprite(new Rectangle(250, 250, 100, 100), new Rectangle(0, 0, 51, 51), TextureLibrary.get("Ship2"),
                                        new Rectangle(0, 0, 800, 600));
            mPlayer.assignKeys();

            ene = new Sprite(new Rectangle(100, 100, 64, 128), new Rectangle(0, 0, 64, 128), TextureLibrary.get("Enemy1"));
            Sprite back = new Sprite(new Rectangle(0,0,800,600),new Rectangle(0,0,800,600),TextureLibrary.get("Back"));
            Sprite cloud = new Sprite(new Rectangle(130,205,400,300), new Rectangle(0,0,800,600), TextureLibrary.get("Cloud"));

            BackgroundTextures.Add(back);
            Foreground.Add(cloud);
            Midground.Add(ene);


        }

        public void update(float elapsed)
        {
            ShotManager.update(elapsed);
            mPlayer.update(elapsed);
            if (Collisions.IntersectPixels(mPlayer, ene))
            {
                ene.Texture = TextureLibrary.get("Explosion");

            }
            else
            {
                ene.Texture = TextureLibrary.get("Enemy1");

            }
        }

        public void draw(SpriteBatch theSpriteBatch)
        {
            drawArrayList(BackgroundTextures, theSpriteBatch);
            drawArrayList(Background, theSpriteBatch);

            ShotManager.draw(theSpriteBatch);
            
            drawArrayList(Midground, theSpriteBatch);

            mPlayer.draw(theSpriteBatch);

            drawArrayList(Foreground, theSpriteBatch);
        }

        public void drawArrayList(ArrayList array,SpriteBatch theStpriteBatch)
        {
            for (int i = 0; i < array.Count; i++)
            {
                ((Sprite)(array[i])).draw(theStpriteBatch);

            }

        }


    }
}
