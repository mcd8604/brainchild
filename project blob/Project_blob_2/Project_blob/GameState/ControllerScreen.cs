using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using System.Collections.Generic;
using System.Text;

namespace Project_blob.GameState
{
    class ControllerScreen : MenuScreen
    {
        Texture2D controlTexture;

        MenuEntry invertedMenuEntry;

        bool viewInvert = GameplayScreen.cameraInvert;

        public ControllerScreen()
            : base("Control Options")
        {
            IsPopup = true;

            invertedMenuEntry = new MenuEntry();
            MenuEntry applyMenuEntry = new MenuEntry("Apply");
            MenuEntry backMenuEntry = new MenuEntry("Back");

            setMenuText();

            invertedMenuEntry.Selected += invertedSelected;
            applyMenuEntry.Selected += apply;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(invertedMenuEntry);
            MenuEntries.Add(applyMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }

        void setMenuText()
        {
            invertedMenuEntry.Text = "Camera: " + (viewInvert ? "Normal" : "Inverted");
        }

        void invertedSelected(object sender, EventArgs e)
        {
            viewInvert = !viewInvert;
            setMenuText();
        }

        void apply(object sender, EventArgs e)
        {
            GameplayScreen.cameraInvert = viewInvert;
            setMenuText();
        }

        public override void LoadContent()
        {
            controlTexture = ScreenManager.Content.Load<Texture2D>(@"MenuSprites\\controller");
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha);

            base.Draw(gameTime);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            float x = (viewport.Width - controlTexture.Width);
			float y = (viewport.Height - controlTexture.Height) * 0.5f;
            Rectangle fullscreen = new Rectangle((int)x, (int)y, (int)controlTexture.Width, (int)controlTexture.Height);
            byte fade = TransitionAlpha;

            spriteBatch.Begin();

            spriteBatch.Draw(controlTexture, fullscreen,
                             new Color(fade, fade, fade));

            spriteBatch.End();
        }
    }
}
