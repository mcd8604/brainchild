#if DEBUG
using System;
using System.Collections.Generic;

using System.Windows.Forms;

namespace Project_blob
{
	internal partial class SelectForm : Form
	{

		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.ComboBox List;

		private System.ComponentModel.IContainer components = null;

		internal SelectForm(String[] options)
		{
			InitializeComponent(options);
		}

		internal string getSelected()
		{
			return this.List.SelectedItem.ToString();
		}

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

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent(String[] options)
		{
			this.OK = new System.Windows.Forms.Button();
			this.Cancel = new System.Windows.Forms.Button();
			this.List = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			this.StartPosition = FormStartPosition.CenterScreen;
			// 
			// button1
			// 
			this.OK.Location = new System.Drawing.Point(124, 40);
			this.OK.Name = "OK";
			this.OK.Size = new System.Drawing.Size(75, 23);
			this.OK.TabIndex = 0;
			this.OK.Text = "OK";
			this.OK.UseVisualStyleBackColor = true;
			this.OK.DialogResult = DialogResult.OK;
			this.AcceptButton = this.OK;

			// 
			// button2
			// 
			this.Cancel.Location = new System.Drawing.Point(205, 40);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(75, 23);
			this.Cancel.TabIndex = 1;
			this.Cancel.Text = "Cancel";
			this.Cancel.UseVisualStyleBackColor = true;
			this.CancelButton = this.Cancel;
			// 
			// comboBox1
			// 
			this.List.FormattingEnabled = true;
			this.List.Location = new System.Drawing.Point(12, 13);
			this.List.Name = "comboBox1";
			this.List.Size = new System.Drawing.Size(268, 21);
			this.List.TabIndex = 2;
			this.List.Items.AddRange(options);
			this.List.SelectedIndex = 0;
			// 
			// LevelForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 74);
			this.Controls.Add(this.List);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.OK);
			this.Name = "Form";
			this.Text = "Debug Area Selector";
			this.ResumeLayout(false);
		}
	}
}
#endif