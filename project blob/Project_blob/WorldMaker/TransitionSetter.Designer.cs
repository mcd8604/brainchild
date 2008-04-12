namespace WorldMaker
{
    partial class TransitionSetter
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
            this.areaBox = new System.Windows.Forms.ListBox();
            this.xPosText = new System.Windows.Forms.TextBox();
            this.areaLabel = new System.Windows.Forms.Label();
            this.xPosLabel = new System.Windows.Forms.Label();
            this.yPosLabel = new System.Windows.Forms.Label();
            this.zPosLabel = new System.Windows.Forms.Label();
            this.yPosText = new System.Windows.Forms.TextBox();
            this.zPosText = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // areaBox
            // 
            this.areaBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.areaBox.FormattingEnabled = true;
            this.areaBox.Location = new System.Drawing.Point(12, 27);
            this.areaBox.Name = "areaBox";
            this.areaBox.Size = new System.Drawing.Size(168, 121);
            this.areaBox.TabIndex = 0;
            // 
            // xPosText
            // 
            this.xPosText.Location = new System.Drawing.Point(71, 163);
            this.xPosText.Name = "xPosText";
            this.xPosText.Size = new System.Drawing.Size(109, 20);
            this.xPosText.TabIndex = 1;
            // 
            // areaLabel
            // 
            this.areaLabel.AutoSize = true;
            this.areaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.areaLabel.Location = new System.Drawing.Point(12, 8);
            this.areaLabel.Name = "areaLabel";
            this.areaLabel.Size = new System.Drawing.Size(53, 16);
            this.areaLabel.TabIndex = 2;
            this.areaLabel.Text = "Areas:";
            // 
            // xPosLabel
            // 
            this.xPosLabel.AutoSize = true;
            this.xPosLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xPosLabel.Location = new System.Drawing.Point(12, 164);
            this.xPosLabel.Name = "xPosLabel";
            this.xPosLabel.Size = new System.Drawing.Size(48, 16);
            this.xPosLabel.TabIndex = 3;
            this.xPosLabel.Text = "XPos:";
            // 
            // yPosLabel
            // 
            this.yPosLabel.AutoSize = true;
            this.yPosLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yPosLabel.Location = new System.Drawing.Point(12, 190);
            this.yPosLabel.Name = "yPosLabel";
            this.yPosLabel.Size = new System.Drawing.Size(49, 16);
            this.yPosLabel.TabIndex = 4;
            this.yPosLabel.Text = "YPos:";
            // 
            // zPosLabel
            // 
            this.zPosLabel.AutoSize = true;
            this.zPosLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zPosLabel.Location = new System.Drawing.Point(12, 216);
            this.zPosLabel.Name = "zPosLabel";
            this.zPosLabel.Size = new System.Drawing.Size(48, 16);
            this.zPosLabel.TabIndex = 5;
            this.zPosLabel.Text = "ZPos:";
            // 
            // yPosText
            // 
            this.yPosText.Location = new System.Drawing.Point(71, 189);
            this.yPosText.Name = "yPosText";
            this.yPosText.Size = new System.Drawing.Size(109, 20);
            this.yPosText.TabIndex = 6;
            // 
            // zPosText
            // 
            this.zPosText.Location = new System.Drawing.Point(71, 215);
            this.zPosText.Name = "zPosText";
            this.zPosText.Size = new System.Drawing.Size(109, 20);
            this.zPosText.TabIndex = 7;
            // 
            // okButton
            // 
            this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.Location = new System.Drawing.Point(12, 241);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(80, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(100, 241);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // TransitionSetter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(192, 273);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.zPosText);
            this.Controls.Add(this.yPosText);
            this.Controls.Add(this.zPosLabel);
            this.Controls.Add(this.yPosLabel);
            this.Controls.Add(this.xPosLabel);
            this.Controls.Add(this.areaLabel);
            this.Controls.Add(this.xPosText);
            this.Controls.Add(this.areaBox);
            this.Name = "TransitionSetter";
            this.Text = "TransitionSetter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox areaBox;
        private System.Windows.Forms.TextBox xPosText;
        private System.Windows.Forms.Label areaLabel;
        private System.Windows.Forms.Label xPosLabel;
        private System.Windows.Forms.Label yPosLabel;
        private System.Windows.Forms.Label zPosLabel;
        private System.Windows.Forms.TextBox yPosText;
        private System.Windows.Forms.TextBox zPosText;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}