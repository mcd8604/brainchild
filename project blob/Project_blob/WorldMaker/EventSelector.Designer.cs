namespace WorldMaker
{
    partial class EventSelector
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
			this.eventBox = new System.Windows.Forms.ListBox();
			this.selectButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// eventBox
			// 
			this.eventBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.eventBox.FormattingEnabled = true;
			this.eventBox.ItemHeight = 16;
			this.eventBox.Items.AddRange(new object[] {
            "Area Transition",
            "Player Warp",
            "Switch Activation",
            "Camera Pan",
            "Delta"});
			this.eventBox.Location = new System.Drawing.Point(12, 12);
			this.eventBox.Name = "eventBox";
			this.eventBox.Size = new System.Drawing.Size(176, 116);
			this.eventBox.TabIndex = 0;
			// 
			// selectButton
			// 
			this.selectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.selectButton.Location = new System.Drawing.Point(12, 141);
			this.selectButton.Name = "selectButton";
			this.selectButton.Size = new System.Drawing.Size(61, 30);
			this.selectButton.TabIndex = 1;
			this.selectButton.Text = "OK";
			this.selectButton.UseVisualStyleBackColor = true;
			this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancelButton.Location = new System.Drawing.Point(121, 141);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(67, 30);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// EventSelector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(200, 183);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.selectButton);
			this.Controls.Add(this.eventBox);
			this.Name = "EventSelector";
			this.Text = "EventSelector";
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox eventBox;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Button cancelButton;
    }
}