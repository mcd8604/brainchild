using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WorldMaker
{
    public partial class LevelSelect : Form
    {
        private string _levelName = string.Empty;

        public string LevelName
        {
            get { return _levelName; }
        }

        public LevelSelect(string[] levels)
        {
            InitializeComponent();

            for (int i = 0; i < levels.Length; ++i)
            {
                levelListBox.Items.Add(levels[i]);
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (levelListBox.SelectedIndex != -1)
            {
                this.DialogResult = DialogResult.OK;
                _levelName = (string)levelListBox.Items[levelListBox.SelectedIndex];
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}