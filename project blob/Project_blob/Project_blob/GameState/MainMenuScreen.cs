using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project_blob.GameState
{
    class MainMenuScreen : MenuScreen
	{
		Texture2D titleTexture;

        public MainMenuScreen() : base("")
        {
            
        }

        void startMenuEntrySelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, new GameplayScreen());
		}

		public override void LoadContent()
		{
			base.LoadContent();
			titleTexture = ScreenManager.Content.Load<Texture2D>(@"MenuSprites\\title");
		}

        void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(false));
        }

		void CreditsMenuEntrySelected(object sender, EventArgs e)
		{
			ScreenManager.AddScreen(new CreditScreen());
		}

        protected override void OnCancel()
        {
            ScreenManager.Exit();
        }

		public void LoadMenuSprites()
		{
			// Create our menu entries.
			MenuEntry startMenuEntry = new MenuEntry(ScreenManager.Content.Load<Texture2D>(@"MenuSprites\\Start"));
			MenuEntry optionsMenuEntry = new MenuEntry(ScreenManager.Content.Load<Texture2D>(@"MenuSprites\\Options"));
			MenuEntry creditsMenuEntry = new MenuEntry(ScreenManager.Content.Load<Texture2D>(@"MenuSprites\\Credits"));
			MenuEntry exitMenuEntry = new MenuEntry(ScreenManager.Content.Load<Texture2D>(@"MenuSprites\\Exit"));

			// Hook up menu event handlers.
			startMenuEntry.Selected += startMenuEntrySelected;
			optionsMenuEntry.Selected += OptionsMenuEntrySelected;
			creditsMenuEntry.Selected += CreditsMenuEntrySelected;
			exitMenuEntry.Selected += OnCancel;

			// Add entries to the menu.
			MenuEntries.Add(startMenuEntry);
			MenuEntries.Add(optionsMenuEntry);
			MenuEntries.Add(creditsMenuEntry);
			MenuEntries.Add(exitMenuEntry);
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
		{
			// Draw the menu title.
			//Vector2 vSize = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);
			//Vector2 titleSize = new Vector2(titleTexture.Width, titleTexture.Height);
			Vector2 titlePosition = new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - titleTexture.Width) / 2, 10);
			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.Draw(titleTexture, titlePosition, Color.White);
			ScreenManager.SpriteBatch.End();
			base.Draw(gameTime);
		}
    }
}
