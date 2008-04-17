using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Project_blob;

namespace WorldMaker
{
    public partial class CameraPanSetter : Form
    {
        private int _cameraPointCount;
        private Dictionary<String, Vector3> _cameraUps;
        private Dictionary<String, Vector3> _cameraLooks;
        private Dictionary<String, Vector3> _cameraPos;

        private CameraEvent _cameraEvent;
        public CameraEvent CameraPan { get { return _cameraEvent; } }

        public CameraPanSetter()
        {
            _cameraPointCount = 1;
            _cameraUps = new Dictionary<String, Vector3>();
            _cameraLooks = new Dictionary<String, Vector3>();
            _cameraPos = new Dictionary<String, Vector3>();
            InitializeComponent();
        }

        private void runButton_Click(object sender, EventArgs e)
        {

        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (pointBox.SelectedIndex != -1)
            {
                try
                {
                }
                catch (Exception)
                {
                }
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                Vector3 tempPos = new Vector3();
                Vector3 tempLook = new Vector3();
                Vector3 tempUp = new Vector3();

                tempPos.X = float.Parse(xPosText.Text);
                tempPos.Y = float.Parse(yPosText.Text);
                tempPos.Z = float.Parse(zPosText.Text);

                tempLook.X = float.Parse(xLookText.Text);
                tempLook.Y = float.Parse(yLookText.Text);
                tempLook.Z = float.Parse(zLookText.Text);

                tempUp.X = float.Parse(xUpText.Text);
                tempUp.Y = float.Parse(yUpText.Text);
                tempUp.Z = float.Parse(zUpText.Text);

                _cameraPos.Add("Camera Point " + _cameraPointCount, tempPos);
                _cameraLooks.Add("Camera Point " + _cameraPointCount, tempLook);
                _cameraUps.Add("Camera Point " + _cameraPointCount, tempUp);

                pointBox.Items.Add("Camera Point " + _cameraPointCount);
                pointBox.Update();

                _cameraPointCount++;
            }
            catch (Exception)
            {
            } 
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (pointBox.SelectedIndex != -1)
            {
                _cameraUps.Remove((String)pointBox.Items[pointBox.SelectedIndex]);
                _cameraLooks.Remove((String)pointBox.Items[pointBox.SelectedIndex]);
                _cameraPos.Remove((String)pointBox.Items[pointBox.SelectedIndex]);
                pointBox.Items.Remove((String)pointBox.Items[pointBox.SelectedIndex]);
                pointBox.Update();
            }
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            if (_cameraPos.Count > 0)
            {
                List<Vector3> tempPos = new List<Vector3>();
                List<Vector3> tempLooks = new List<Vector3>();
                List<Vector3> tempUps = new List<Vector3>();

                foreach (Vector3 vec in _cameraPos.Values)
                {
                    tempPos.Add(vec);
                }
                foreach (Vector3 vec in _cameraLooks.Values)
                {
                    tempLooks.Add(vec);
                }
                foreach (Vector3 vec in _cameraUps.Values)
                {
                    tempUps.Add(vec);
                }
                _cameraEvent = new CameraEvent(tempPos, tempLooks, tempUps);
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pointBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}