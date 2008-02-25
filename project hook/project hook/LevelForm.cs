#if DEBUG
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace project_hook
{
	internal partial class LevelForm : Form
	{

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ComboBox comboBox1;

		private System.ComponentModel.IContainer components = null;

		internal LevelForm(string[] levels)
		{
			InitializeComponent(levels);
		}

		internal string getLevel()
		{
			return this.comboBox1.SelectedItem.ToString();
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
		private void InitializeComponent(object[] levels)
		{
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			this.StartPosition = FormStartPosition.CenterScreen;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(124, 40);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.DialogResult = DialogResult.OK;
			this.AcceptButton = this.button1;

			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(205, 40);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.CancelButton = this.button2;
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(12, 13);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(268, 21);
			this.comboBox1.TabIndex = 2;
			this.comboBox1.Items.AddRange(levels);
			this.comboBox1.SelectedIndex = 0;
			// 
			// LevelForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 74);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "LevelForm";
			this.Text = "Level Selector";
			this.ResumeLayout(false);
		}
	}
}
#endif