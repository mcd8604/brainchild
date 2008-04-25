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
	public partial class EventSetter : Form
	{
		public EventSetter(EventTrigger e)
		{
			InitializeComponent();
			Console.WriteLine("Event Setter called with: " + e);
			properties.SelectedObject = e;
		}
	}
}