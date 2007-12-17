using System;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class ShotManager
    {
        private static ArrayList shots;

        public static void iniShotManager(){
            shots = new ArrayList();
        }

        public static void addShot(Shot newShot){

            shots.Add(newShot);
        }

        public static void update(float elapsed)
        {
            for (int i = 0; i < shots.Count; i++)
            {   
                ((Shot)(shots[i])).update(elapsed);

                if (!((Shot)(shots[i])).Visible)
                {
                   shots.RemoveAt(i);
                }
            }
        }

        public static void draw(SpriteBatch theSpriteBatch)
        {
            for (int i = 0; i < shots.Count; i++)
            {
                ((Shot)(shots[i])).draw(theSpriteBatch);
            }
        }
    }
}
