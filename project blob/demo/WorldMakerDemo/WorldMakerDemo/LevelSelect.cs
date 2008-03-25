using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WorldMakerDemo
{
    public partial class LevelSelect : Form
    {
        public LevelSelect(string[] levels)
        {
            InitializeComponent();

            for (int i = 0; i < levels.Length; i++)
            {
                levelListBox.Items.Add(levels[i]);
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}