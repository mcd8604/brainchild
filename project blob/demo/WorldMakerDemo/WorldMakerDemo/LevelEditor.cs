using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WorldMakerDemo
{
    public partial class LevelEditor : Form
    {
        private Game1 _gameRef;

        public LevelEditor(Game1 game)
        {
            InitializeComponent();

            _gameRef = game;
            
        }

        private void areaListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _gameRef.ActiveArea = Level.Level.Areas[(String)(areaListBox.Items[areaListBox.SelectedIndex])];
            foreach (String str in _gameRef.ActiveArea.Drawables.Keys)
            {
                modelListBox.Items.Add(str);
                Console.WriteLine(str + " loaded");
            }
            modelListBox.Update();
        }

        private void modelListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _gameRef.ActiveDrawable = _gameRef.ActiveArea.Drawables[(String)(modelListBox.Items[modelListBox.SelectedIndex])];
            if(_gameRef.ActiveDrawable is DrawableModel)
                _gameRef.ActiveArea.Display.CurrentlySelected = ((DrawableModel)_gameRef.ActiveDrawable).Name;
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            foreach (String str in Level.Level.Areas.Keys)
            {
                areaListBox.Items.Add(str);
                Console.WriteLine(str + " loaded");
            }
            areaListBox.Update();
        }
    }
}