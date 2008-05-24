namespace WorldMaker
{
	partial class EventSetter
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
			this.properties = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// properties
			// 
			this.properties.Location = new System.Drawing.Point(13, 13);
			this.properties.Name = "properties";
			this.properties.Size = new System.Drawing.Size(259, 239);
			this.properties.TabIndex = 0;
			// 
			// EventSetter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 264);
			this.Controls.Add(this.properties);
			this.Name = "EventSetter";
			this.Text = "EventSetter";
			this.ResumeLayout(false);

		}

		#endregion

		private System.IO.Ports.SerialPort serialPort1;
		private System.Windows.Forms.PropertyGrid properties;
	}
}