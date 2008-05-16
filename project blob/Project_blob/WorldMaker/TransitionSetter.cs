using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Project_blob;

namespace WorldMaker
{
    public partial class TransitionSetter : Form {

        private TransitionEvent _transition;
        public TransitionEvent Transition { get { return _transition; } }

        public TransitionSetter() {
            InitializeComponent();
            foreach(string area in Level.Areas.Keys) {
                areaBox.Items.Add(area);
            }
            areaBox.Update();
        }

        private void okButton_Click(object sender, EventArgs e) {
            if(areaBox.SelectedIndex != -1 && !xPosText.Text.Equals("") && !yPosText.Text.Equals("") && !zPosText.Text.Equals("")) {
                try {
                    _transition = new TransitionEvent((string)areaBox.Items[areaBox.SelectedIndex], float.Parse(xPosText.Text),
                        float.Parse(yPosText.Text), float.Parse(zPosText.Text));
                    this.Close();
                } catch(Exception ex) {
					Log.Out.WriteLine(ex);
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }

    }
}