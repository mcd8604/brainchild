namespace WorldMaker
{
    partial class ModelSelect
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
            this.modelBox = new System.Windows.Forms.ListBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.textureBox = new System.Windows.Forms.ListBox();
            this.ModelName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.audioBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // modelBox
            // 
            this.modelBox.FormattingEnabled = true;
            this.modelBox.Location = new System.Drawing.Point(12, 12);
            this.modelBox.Name = "modelBox";
            this.modelBox.Size = new System.Drawing.Size(296, 108);
            this.modelBox.TabIndex = 0;
            this.modelBox.SelectedIndexChanged += new System.EventHandler(this.modelBox_SelectedIndexChanged);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(167, 436);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(141, 41);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Cancel";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(12, 436);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(149, 41);
            this.LoadButton.TabIndex = 2;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // textureBox
            // 
            this.textureBox.FormattingEnabled = true;
            this.textureBox.Location = new System.Drawing.Point(12, 126);
            this.textureBox.Name = "textureBox";
            this.textureBox.Size = new System.Drawing.Size(296, 134);
            this.textureBox.TabIndex = 3;
            this.textureBox.SelectedIndexChanged += new System.EventHandler(this.textureBox_SelectedIndexChanged);
            // 
            // ModelName
            // 
            this.ModelName.Location = new System.Drawing.Point(49, 406);
            this.ModelName.Name = "ModelName";
            this.ModelName.Size = new System.Drawing.Size(259, 20);
            this.ModelName.TabIndex = 4;
            this.ModelName.TextChanged += new System.EventHandler(this.ModelName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 409);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Name:";
            // 
            // audioBox
            // 
            this.audioBox.FormattingEnabled = true;
            this.audioBox.Location = new System.Drawing.Point(12, 266);
            this.audioBox.Name = "audioBox";
            this.audioBox.Size = new System.Drawing.Size(296, 134);
            this.audioBox.TabIndex = 6;
            this.audioBox.SelectedIndexChanged += new System.EventHandler(this.audioBox_SelectedIndexChanged);
            // 
            // ModelSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 489);
            this.Controls.Add(this.audioBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ModelName);
            this.Controls.Add(this.textureBox);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.modelBox);
            this.Name = "ModelSelect";
            this.Text = "ModelSelect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox modelBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.ListBox textureBox;
        private System.Windows.Forms.TextBox ModelName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox audioBox;
    }
}