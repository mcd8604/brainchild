using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ObjectManagerExample
{
    public class Animation
    {
        protected string name;
        protected Texture2D texture;
        protected List<AnimationFrame> frames;
        protected int currentFrameIndex;
        protected int elapsedAnimationTime;
        protected int timesToPlay;
        protected int playedAnimations;

        public Animation()
        {
            frames = new List<AnimationFrame>(16);
            currentFrameIndex = 0;
        }

        public Texture2D Texture
        {
            set
            {
                texture = value;
            }
            get
            {
                return texture;
            }
        }

        public string Name
        {
            set
            {
                name = value;
            }
            get
            {
                return name;
            }
        }

        public Rectangle SourceRectangle
        {
            get
            {
                if (currentFrameIndex < frames.Count)
                    return frames[currentFrameIndex].Region;
                else
                    return new Rectangle();
            }
        }

        public AnimationFrame[] Frames
        {
            set
            {
                this.frames.AddRange(value);
            }
            get
            {
                return frames.ToArray();
            }
        }

        public int TimesToPlay
        {
            set
            {
                timesToPlay = value;
            }
        }

        public void ApplyFrameExpansion(string autoAssign)
        {
            if ((frames == null) || (frames.Count == 0))
                return;

            AnimationFrame animationFrame = frames[frames.Count - 1];
            Rectangle region = animationFrame.Region;
            int duration = animationFrame.Duration;

            switch (autoAssign)
            {
                case "horizontal":
                    {
                        int diffToRight = Texture.Width - region.X;
                        int framesToAdd = diffToRight / region.Width;
                        for (int i = 1; i < framesToAdd; i++)
                        {
                            Rectangle rect = new Rectangle((i * region.Width), region.Y, region.Width, region.Height);
                            AddFrame(new AnimationFrame(rect, duration));
                        }
                    }
                    break;

                case "vertical":
                    {
                        int diffToBottom = Texture.Height - region.Y;
                        int framesToAdd = diffToBottom / region.Height;
                        for (int i = 1; i < framesToAdd; i++)
                        {
                            Rectangle rect = new Rectangle(region.X, (i * region.Y), region.Width, region.Height);
                            AddFrame(new AnimationFrame(rect, duration));
                        }
                    }
                    break;
            }
        }

        public void AddFrame(AnimationFrame frame)
        {
            frames.Add(frame);
        }

        public bool IsFinished
        {
            get
            {
                return (timesToPlay != 0) && (playedAnimations >= timesToPlay);
            }
        }

        public void Restart()
        {
            playedAnimations = 0;
            currentFrameIndex = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (frames.Count <= 1)
                return;

            elapsedAnimationTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedAnimationTime > frames[currentFrameIndex].Duration)
            {
                currentFrameIndex++;

                if (currentFrameIndex == frames.Count)
                {
                    currentFrameIndex = 0;
                    playedAnimations++;
                }

                elapsedAnimationTime = 0;
            }
        }
    }

    public class AnimationFrame
    {
        protected Rectangle region;
        protected int duration;

        public AnimationFrame()
        {
        }

        public AnimationFrame(Rectangle region, int duration)
        {
            this.region = region;
            this.duration = duration;
        }

        public Rectangle Region
        {
            set
            {
                this.region = value;
            }
            get
            {
                return this.region;
            }
        }

        public int Duration
        {
            set
            {
                this.duration = value;
            }
            get
            {
                return this.duration;
            }
        }
    }

    public class AnimatedSprite : DrawableGameComponent, ICloneable
    {
        protected Dictionary<string, Animation> animations = new Dictionary<string, Animation>(4);
        protected Animation currentAnimation;
        protected Vector2 position = Vector2.Zero;

        public AnimatedSprite(Game game)
            : base(game)
        {
        }

        public Vector2 Position
        {
            set
            {
                this.position = value;
            }
            get
            {
                return this.position;
            }
        }

        public void AddAnimation(Animation animation, bool isDefault)
        {
            if (isDefault)
                currentAnimation = animation;

            if (!animations.ContainsKey(animation.Name))
            {
                animations.Add(animation.Name, animation);
            }
            else
            {
#if DEBUG
                Debug.WriteLine(string.Format("duplicated animation: '{0}'", animation.Name));
#endif
            }
        }

        public void PlayAnimation(string animationName)
        {
            if ((currentAnimation != null) && (currentAnimation.Name == animationName))
                return;

            if (!animations.ContainsKey(animationName))
            {
#if DEBUG
                Debug.WriteLine(string.Format("animation not found: '{0}'", animationName));
#endif
                return;
            }

            currentAnimation = animations[animationName];
            currentAnimation.Restart();
        }

        #region Draw and Update
        public override void Draw(GameTime gameTime)
        {
            if (currentAnimation == null)
                return;

            //to keep things easier get the SpriteBatch from the game class
            SpriteBatch spriteBatch = Game1.spriteBatch;

            if (!currentAnimation.IsFinished)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(currentAnimation.Texture, this.Position, currentAnimation.SourceRectangle, Color.White);
                spriteBatch.End();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (currentAnimation == null)
                return;

            currentAnimation.Update(gameTime);
        }
        #endregion

        public object Clone()
        {
            AnimatedSprite clone = new AnimatedSprite(this.Game);
            clone.animations = this.animations;
            clone.currentAnimation = this.currentAnimation;
            clone.position = this.position;

            return clone;
        }
    }
}
