using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using System.Collections.Generic;
using System.Text;

namespace Project_blob.GameState
{
	class ColorMenuScreen : MenuScreen
	{
        MenuEntry colorMenuEntry;

		MyColor color = ScreenManager.CurrentColor;

		public ColorMenuScreen()
            : base("Color Changer")
        {
            IsPopup = true;

            colorMenuEntry = new MenuEntry();
            MenuEntry applyMenuEntry = new MenuEntry("Apply");
            MenuEntry backMenuEntry = new MenuEntry("Back");

            setMenuText();

            colorMenuEntry.Selected += colorSelected;
            applyMenuEntry.Selected += apply;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(colorMenuEntry);
            MenuEntries.Add(applyMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }

        void setMenuText()
        {
			colorMenuEntry.Text = "Blob Color: " + color;
        }

        void colorSelected(object sender, EventArgs e)
        {
			color = ScreenManager.Colors[(ScreenManager.Colors.IndexOf(color) + 1) % ScreenManager.Colors.Count];
            setMenuText();
        }

        void apply(object sender, EventArgs e)
        {
			//GameplayScreen.cameraInvert = viewInvert;
			GameplayScreen.BlobColor = new Color(color.Rgba);
			GameplayScreen.ChangeBlobColor = true;
            setMenuText();
        }

        public override void Draw(GameTime gameTime)
        {
			ScreenManager.FadeBackBufferToBlack(TransitionAlpha);

            base.Draw(gameTime);
        }
	}
}