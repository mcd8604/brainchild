using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WorldMaker
{
    public partial class EventSelector : Form
    {
        private enum Events{ AREATRANSITION, PLAYERWARP, SWITCHACTIVATION, CAMERAPAN };

        public EventSelector()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            if (eventBox.SelectedIndex != -1)
            {
                this.DialogResult = DialogResult.OK;
                switch ((Events)eventBox.SelectedIndex)
                {
                    case Events.AREATRANSITION:
                        break;
                    case Events.PLAYERWARP:
                        break;
                    case Events.SWITCHACTIVATION:
                        break;
                    case Events.CAMERAPAN:
                        break;
                };
                this.Close();
            }
        }
    }
}