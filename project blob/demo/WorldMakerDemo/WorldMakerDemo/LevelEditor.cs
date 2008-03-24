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
        ModelSelect _modelSelect;

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

        private void modelAddButton_Click(object sender, EventArgs e)
        {
            string[] models = System.IO.Directory.GetFiles(System.Environment.CurrentDirectory + "\\Content\\Models");
            for (int i = 0; i < models.Length; i++)
                models[i] = models[i].Substring(models[i].LastIndexOf("\\") + 1);

            _modelSelect = new ModelSelect(this, models, _gameRef);
            _modelSelect.ShowDialog();
            if (_modelSelect.DialogResult == DialogResult.OK)
            {
                Console.WriteLine(_modelSelect.CurrentSelection.Name);

                _gameRef.ActiveArea.Display.DrawnList.Values[0].Add(_modelSelect.CurrentSelection);
            }
        }

        private void modelDelButton_Click(object sender, EventArgs e)
        {

        }
    }
}