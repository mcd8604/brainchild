using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Wintellect.PowerCollections;

namespace project_hook
{
    class VisualEffect:Sprite
    {
        OrderedDictionary<String,GameTexture> frames;

        float frameLength = 1f / 5f;
        float timer = 0f;
        int currentFrame = 0;
        bool updateAnimation = true;


        public int FramesPerSecond
        {
            get { return (int)(1f / frameLength); }
            set { frameLength = 1f / (float)value; }
        }

        public GameTexture CurrentFrame
        {
            get { return frames[currentFrame.ToString()]; }
        }

        public VisualEffect(String p_Name, Vector2 p_Position, int p_Height, int p_Width, GameTexture p_Texture, float p_Alpha, bool p_Visible, float p_Degree, float p_zBuff)
            :base(p_Name, p_Position, p_Height, p_Width, p_Texture, p_Alpha, p_Visible, p_Degree, p_zBuff)
        {
            frames = TextureLibrary.getSpriteSheet(p_Name);
            Texture = frames[currentFrame.ToString()];
        }

        public override void Update(GameTime gameTime)
        {
            if (updateAnimation)
            {


                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timer >= frameLength)
                {
                    timer = 0f;
                    currentFrame = (currentFrame + 1) % frames.Count;
                    Texture = frames[currentFrame.ToString()];
                }
            }
        }

        public void Reset()
        {
            currentFrame = 0;
            timer = 0f;
        }

        public void StartAnimation()
        {
            updateAnimation = true;
        }

        public void StopAnimation()
        {
            updateAnimation = false;
        }





    }
}

