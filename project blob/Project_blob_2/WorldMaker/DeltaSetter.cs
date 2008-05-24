using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Project_blob;
using Microsoft.Xna.Framework;

namespace WorldMaker
{
    public partial class DeltaSetter : Form
    {
        private DeltaEvent _delta;
        public DeltaEvent Delta { get { return _delta; } }

        public DeltaSetter()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(xVelText.Text) && !string.IsNullOrEmpty(yVelText.Text) && !string.IsNullOrEmpty(zVelText.Text))
            {
                try
                {
                    _delta = new DeltaEvent(new Vector3(float.Parse(xVelText.Text), float.Parse(yVelText.Text), float.Parse(zVelText.Text)));
                    this.Close();
                }
                catch (Exception ex)
                {
					Log.Out.WriteLine(ex);
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}