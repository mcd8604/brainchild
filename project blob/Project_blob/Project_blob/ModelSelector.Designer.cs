namespace Project_blob
{
	partial class ModelSelector
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
			this.currentModels = new System.Windows.Forms.ListBox();
			this.areaModels = new System.Windows.Forms.ListBox();
			this.removeButton = new System.Windows.Forms.Button();
			this.addButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// currentModels
			// 
			this.currentModels.FormattingEnabled = true;
			this.currentModels.Location = new System.Drawing.Point(12, 25);
			this.currentModels.Name = "currentModels";
			this.currentModels.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.currentModels.Size = new System.Drawing.Size(100, 290);
			this.currentModels.TabIndex = 0;
			// 
			// areaModels
			// 
			this.areaModels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.areaModels.FormattingEnabled = true;
			this.areaModels.Location = new System.Drawing.Point(118, 25);
			this.areaModels.Name = "areaModels";
			this.areaModels.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.areaModels.Size = new System.Drawing.Size(100, 290);
			this.areaModels.TabIndex = 1;
			// 
			// removeButton
			// 
			this.removeButton.Location = new System.Drawing.Point(12, 327);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(100, 23);
			this.removeButton.TabIndex = 2;
			this.removeButton.Text = "Remove Model";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// addButton
			// 
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.addButton.Location = new System.Drawing.Point(118, 327);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(100, 23);
			this.addButton.TabIndex = 3;
			this.addButton.Text = "Add Model";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(92, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Current Models";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(118, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Area Models";
			// 
			// ModelSelector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(231, 362);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.areaModels);
			this.Controls.Add(this.currentModels);
			this.Name = "ModelSelector";
			this.Text = "ModelSelector";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox currentModels;
		private System.Windows.Forms.ListBox areaModels;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}