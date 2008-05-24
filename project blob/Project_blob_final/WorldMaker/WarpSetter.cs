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
	public partial class WarpSetter : Form
	{
		private WarpEvent _warp;
		public WarpEvent Warp { get { return _warp; } }

		public WarpSetter()
		{
			InitializeComponent();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(xPosText.Text) && !string.IsNullOrEmpty(yPosText.Text) && !string.IsNullOrEmpty(zPosText.Text) &&
				!string.IsNullOrEmpty(xVelText.Text) && !string.IsNullOrEmpty(yVelText.Text) && !string.IsNullOrEmpty(zVelText.Text))
			{
				try
				{
					_warp = new WarpEvent(float.Parse(xPosText.Text), float.Parse(yPosText.Text), float.Parse(zPosText.Text),
						float.Parse(xVelText.Text), float.Parse(yVelText.Text), float.Parse(zVelText.Text));
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