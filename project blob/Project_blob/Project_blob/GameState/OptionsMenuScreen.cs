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
        MenuEntry resolutionMenuEntry;
        MenuEntry fullscreenMenuEntry;
        MenuEntry aliasingMenuEntry;
        MenuEntry audioMenuEntry;
        MenuEntry threadedMenuEntry;
        MenuEntry vsyncMenuEntry;
        MenuEntry cameraMenuEntry;

        bool Fullscreen = ScreenManager.IsFullScreen;
        bool AntiAliasing = ScreenManager.IsAntiAliasing;
        bool audio = Audio.AudioManager.Enabled;
        PhysicsManager.ParallelSetting Threading = PhysicsManager.enableParallel;
        Resolution resolution = ScreenManager.CurrentResolution;
        bool vsync = ScreenManager.VSync;
        GameplayScreen.CameraType cam = GameplayScreen.CurCamera;

        public OptionsMenuScreen(Boolean popup)
            : base("Options")
        {
            if (popup)
            {
                IsPopup = true;
            }

            resolutionMenuEntry = new MenuEntry();
            fullscreenMenuEntry = new MenuEntry();
            aliasingMenuEntry = new MenuEntry();
            audioMenuEntry = new MenuEntry();
            threadedMenuEntry = new MenuEntry();
            vsyncMenuEntry = new MenuEntry();
            cameraMenuEntry = new MenuEntry();
            MenuEntry controlMenuEntry = new MenuEntry("Controls");
            MenuEntry applyMenuEntry = new MenuEntry("Apply");
            MenuEntry backMenuEntry = new MenuEntry("Back");

            setMenuText();

            resolutionMenuEntry.Selected += resolutionSelected;
            fullscreenMenuEntry.Selected += fullscreenSelected;
            aliasingMenuEntry.Selected += aliasingSelected;
            audioMenuEntry.Selected += audioSelected;
            threadedMenuEntry.Selected += threadedSelected;
            vsyncMenuEntry.Selected += vsyncSelected;
            cameraMenuEntry.Selected += cameraSelected;
            controlMenuEntry.Selected += controlSelected;
            applyMenuEntry.Selected += apply;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(resolutionMenuEntry);
            MenuEntries.Add(fullscreenMenuEntry);
            MenuEntries.Add(aliasingMenuEntry);
            MenuEntries.Add(audioMenuEntry);
            MenuEntries.Add(threadedMenuEntry);
            MenuEntries.Add(vsyncMenuEntry);
            MenuEntries.Add(cameraMenuEntry);
            MenuEntries.Add(controlMenuEntry);
            MenuEntries.Add(applyMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }

        void setMenuText()
        {
            resolutionMenuEntry.Text = "Resolution: " + resolution;
            fullscreenMenuEntry.Text = "Fullscreen: " + (Fullscreen ? "On" : "Off");
            aliasingMenuEntry.Text = "Anti-Aliasing: " + (AntiAliasing ? "On" : "Off");
            audioMenuEntry.Text = "Audio: " + (audio ? "On" : "Off");
            threadedMenuEntry.Text = "Multithreading: " + Threading;
            vsyncMenuEntry.Text = "VSync: " + (vsync ? "On" : "Off");
            cameraMenuEntry.Text = "Camera: " + cam;
        }

        void resolutionSelected(object sender, EventArgs e)
        {
            resolution = ScreenManager.Resolutions[(ScreenManager.Resolutions.IndexOf(resolution) + 1) % ScreenManager.Resolutions.Count];
            setMenuText();
        }

        void fullscreenSelected(object sender, EventArgs e)
        {
            Fullscreen = !Fullscreen;
            setMenuText();
        }

        void aliasingSelected(object sender, EventArgs e)
        {
            AntiAliasing = !AntiAliasing;
            setMenuText();
        }

        void audioSelected(object sender, EventArgs e)
        {
            audio = !audio;
            setMenuText();
        }

        void threadedSelected(object sender, EventArgs e)
        {

            if (PhysicsManager.enableParallel == PhysicsManager.ParallelSetting.Automatic)
            {
                Threading = PhysicsManager.ParallelSetting.Always;
            }
            else if (PhysicsManager.enableParallel == PhysicsManager.ParallelSetting.Always)
            {
                Threading = PhysicsManager.ParallelSetting.Never;
            }
            else
            {
                Threading = PhysicsManager.ParallelSetting.Automatic;
            }

            setMenuText();
        }

        void vsyncSelected(object sender, EventArgs e)
        {
            vsync = !vsync;
            setMenuText();
        }

        void cameraSelected(object sender, EventArgs e)
        {
            if (GameplayScreen.CurCamera == GameplayScreen.CameraType.follow)
            {
                cam = GameplayScreen.CameraType.chase;
            }
            else
            {
                cam = GameplayScreen.CameraType.follow;
            }
            setMenuText();
        }

        void controlSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new ControllerScreen());
        }

        void apply(object sender, EventArgs e)
        {

            ScreenManager.setResolution(resolution);

            if (Fullscreen != ScreenManager.IsFullScreen)
            {
                ScreenManager.ToggleFullScreen();
            }

            if (AntiAliasing != ScreenManager.IsAntiAliasing)
            {
                ScreenManager.ToggleAntiAliasing();
            }

            Audio.AudioManager.Enabled = audio;

            PhysicsManager.enableParallel = Threading;

            ScreenManager.VSync = vsync;

            if (IsPopup)
            {
                GameplayScreen.CurCamera = cam;
                if (cam == GameplayScreen.CameraType.chase)
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