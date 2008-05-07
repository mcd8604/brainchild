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
            this.audioLabel = new System.Windows.Forms.Label();
            this.textureLabel = new System.Windows.Forms.Label();
            this.modelLabel = new System.Windows.Forms.Label();
            this.ModelType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // modelBox
            // 
            this.modelBox.FormattingEnabled = true;
            this.modelBox.Location = new System.Drawing.Point(12, 58);
            this.modelBox.Name = "modelBox";
            this.modelBox.Size = new System.Drawing.Size(296, 108);
            this.modelBox.TabIndex = 0;
            this.modelBox.SelectedIndexChanged += new System.EventHandler(this.modelBox_SelectedIndexChanged);
            // 
            // closeButton
            // 
            this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.Location = new System.Drawing.Point(167, 518);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(141, 41);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Cancel";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadButton.Location = new System.Drawing.Point(12, 518);
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
            this.textureBox.Location = new System.Drawing.Point(12, 192);
            this.textureBox.Name = "textureBox";
            this.textureBox.Size = new System.Drawing.Size(296, 134);
            this.textureBox.TabIndex = 3;
            this.textureBox.SelectedIndexChanged += new System.EventHandler(this.textureBox_SelectedIndexChanged);
            // 
            // ModelName
            // 
            this.ModelName.Location = new System.Drawing.Point(68, 492);
            this.ModelName.Name = "ModelName";
            this.ModelName.Size = new System.Drawing.Size(240, 20);
            this.ModelName.TabIndex = 4;
            this.ModelName.TextChanged += new System.EventHandler(this.ModelName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 495);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Name:";
            // 
            // audioBox
            // 
            this.audioBox.FormattingEnabled = true;
            this.audioBox.Location = new System.Drawing.Point(12, 352);
            this.audioBox.Name = "audioBox";
            this.audioBox.Size = new System.Drawing.Size(296, 134);
            this.audioBox.TabIndex = 6;
            this.audioBox.SelectedIndexChanged += new System.EventHandler(this.audioBox_SelectedIndexChanged);
            // 
            // audioLabel
            // 
            this.audioLabel.AutoSize = true;
            this.audioLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.audioLabel.Location = new System.Drawing.Point(12, 329);
            this.audioLabel.Name = "audioLabel";
            this.audioLabel.Size = new System.Drawing.Size(60, 20);
            this.audioLabel.TabIndex = 7;
            this.audioLabel.Text = "Audio:";
            // 
            // textureLabel
            // 
            this.textureLabel.AutoSize = true;
            this.textureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textureLabel.Location = new System.Drawing.Point(12, 169);
            this.textureLabel.Name = "textureLabel";
            this.textureLabel.Size = new System.Drawing.Size(83, 20);
            this.textureLabel.TabIndex = 8;
            this.textureLabel.Text = "Textures:";
            // 
            // modelLabel
            // 
            this.modelLabel.AutoSize = true;
            this.modelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modelLabel.Location = new System.Drawing.Point(12, 35);
            this.modelLabel.Name = "modelLabel";
            this.modelLabel.Size = new System.Drawing.Size(71, 20);
            this.modelLabel.TabIndex = 9;
            this.modelLabel.Text = "Models:";
            // 
            // ModelType
            // 
            this.ModelType.FormattingEnabled = true;
            this.ModelType.Location = new System.Drawing.Point(68, 11);
            this.ModelType.Name = "ModelType";
            this.ModelType.Size = new System.Drawing.Size(240, 21);
            this.ModelType.TabIndex = 10;
            this.ModelType.SelectedIndexChanged += new System.EventHandler(this.ModelType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Type:";
            // 
            // ModelSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 566);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ModelType);
            this.Controls.Add(this.modelLabel);
            this.Controls.Add(this.textureLabel);
            this.Controls.Add(this.audioLabel);
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
        private System.Windows.Forms.Label audioLabel;
        private System.Windows.Forms.Label textureLabel;
        private System.Windows.Forms.Label modelLabel;
        private System.Windows.Forms.ComboBox ModelType;
        private System.Windows.Forms.Label label2;
    }
}