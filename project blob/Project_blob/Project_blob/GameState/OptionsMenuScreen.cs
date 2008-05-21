using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;
using Physics2;
using Engine;

namespace Project_blob.GameState
{
    class OptionsMenuScreen : MenuScreen
    {
        MenuEntry threadedMenuEntry;
        MenuEntry cameraMenuEntry;

        PhysicsManager.ParallelSetting Threading = PhysicsManager.enableParallel;
        public GameplayScreen.CameraType cam = GameplayScreen.CurCamera;

        public OptionsMenuScreen(Boolean popup)
            : base("Options")
        {
            if (popup)
            {
                IsPopup = true;
            }

            threadedMenuEntry = new MenuEntry();
			cameraMenuEntry = new MenuEntry();
			MenuEntry videoMenuEntry = new MenuEntry("Video");
			MenuEntry audioMenuEntry = new MenuEntry("Audio");
            MenuEntry controlMenuEntry = new MenuEntry("Controls");
            MenuEntry applyMenuEntry = new MenuEntry("Apply");
            MenuEntry backMenuEntry = new MenuEntry("Back");

            setMenuText();

            threadedMenuEntry.Selected += threadedSelected;
            cameraMenuEntry.Selected += cameraSelected;
			videoMenuEntry.Selected += videoSelected;
			audioMenuEntry.Selected += audioSelected;
            controlMenuEntry.Selected += controlSelected;
            applyMenuEntry.Selected += apply;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(threadedMenuEntry);
            MenuEntries.Add(cameraMenuEntry);
			MenuEntries.Add(videoMenuEntry);
			MenuEntries.Add(audioMenuEntry);
            MenuEntries.Add(controlMenuEntry);
            MenuEntries.Add(applyMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }

        void setMenuText()
        {
            threadedMenuEntry.Text = "Multithreading: " + Threading;
            cameraMenuEntry.Text = "Camera Mode: " + cam;
        }

        void threadedSelected(object sender, EventArgs e)
        {
            if (Threading == PhysicsManager.ParallelSetting.Automatic)
            {
                Threading = PhysicsManager.ParallelSetting.Always;
            }
            else if (Threading == PhysicsManager.ParallelSetting.Always)
            {
                Threading = PhysicsManager.ParallelSetting.Never;
            }
            else
            {
                Threading = PhysicsManager.ParallelSetting.Automatic;
            }
            setMenuText();
        }

        void cameraSelected(object sender, EventArgs e)
        {
            if (cam == GameplayScreen.CameraType.Follow)
            {
                cam = GameplayScreen.CameraType.Chase;
            }
            else
            {
                cam = GameplayScreen.CameraType.Follow;
            }
            setMenuText();
        }

		void videoSelected(object sender, EventArgs e)
		{
			ScreenManager.AddScreen(new VideoMenuScreen());
		}

		void audioSelected(object sender, EventArgs e)
		{
			ScreenManager.AddScreen(new AudioMenuScreen());
		}

        void controlSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new ControllerScreen());
        }

        void apply(object sender, EventArgs e)
        {
            PhysicsManager.enableParallel = Threading;

            if (IsPopup)
            {
                GameplayScreen.CurCamera = cam;
                if (cam == GameplayScreen.CameraType.Chase)
                {
                    CameraManager.getSingleton.SetActiveCamera("chase");
                    //((ChaseCamera)CameraManager.getSingleton.ActiveCamera).ChasePosition = GameplayScreen.theBlob.getCenter();
                    ((ChaseCamera)CameraManager.getSingleton.ActiveCamera).Reset();
                }
                else
                {
                    CameraManager.getSingleton.SetActiveCamera("default");
                }
            }

            setMenuText();
        }

        public override void Draw(GameTime gameTime)
        {

            if (IsPopup)
            {
                ScreenManager.FadeBackBufferToBlack(TransitionAlpha);
            }

            base.Draw(gameTime);
        }
    }
}