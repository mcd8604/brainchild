namespace WorldMaker
{
    partial class DeltaSetter
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
            this.zVelLabel = new System.Windows.Forms.Label();
            this.yVelLabel = new System.Windows.Forms.Label();
            this.xVelLabel = new System.Windows.Forms.Label();
            this.zVelText = new System.Windows.Forms.TextBox();
            this.yVelText = new System.Windows.Forms.TextBox();
            this.xVelText = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // zVelLabel
            // 
            this.zVelLabel.AutoSize = true;
            this.zVelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zVelLabel.Location = new System.Drawing.Point(7, 64);
            this.zVelLabel.Name = "zVelLabel";
            this.zVelLabel.Size = new System.Drawing.Size(51, 20);
            this.zVelLabel.TabIndex = 22;
            this.zVelLabel.Text = "ZVel:";
            // 
            // yVelLabel
            // 
            this.yVelLabel.AutoSize = true;
            this.yVelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yVelLabel.Location = new System.Drawing.Point(8, 38);
            this.yVelLabel.Name = "yVelLabel";
            this.yVelLabel.Size = new System.Drawing.Size(52, 20);
            this.yVelLabel.TabIndex = 21;
            this.yVelLabel.Text = "YVel:";
            // 
            // xVelLabel
            // 
            this.xVelLabel.AutoSize = true;
            this.xVelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xVelLabel.Location = new System.Drawing.Point(7, 12);
            this.xVelLabel.Name = "xVelLabel";
            this.xVelLabel.Size = new System.Drawing.Size(52, 20);
            this.xVelLabel.TabIndex = 20;
            this.xVelLabel.Text = "XVel:";
            // 
            // zVelText
            // 
            this.zVelText.Location = new System.Drawing.Point(65, 64);
            this.zVelText.Name = "zVelText";
            this.zVelText.Size = new System.Drawing.Size(88, 20);
            this.zVelText.TabIndex = 19;
            this.zVelText.Text = "0";
            // 
            // yVelText
            // 
            this.yVelText.Location = new System.Drawing.Point(65, 38);
            this.yVelText.Name = "yVelText";
            this.yVelText.Size = new System.Drawing.Size(88, 20);
            this.yVelText.TabIndex = 18;
            this.yVelText.Text = "0";
            // 
            // xVelText
            // 
            this.xVelText.Location = new System.Drawing.Point(65, 12);
            this.xVelText.Name = "xVelText";
            this.xVelText.Size = new System.Drawing.Size(88, 20);
            this.xVelText.TabIndex = 17;
            this.xVelText.Text = "0";
            // 
            // cancelButton
            // 
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(83, 100);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(70, 23);
            this.cancelButton.TabIndex = 16;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.Location = new System.Drawing.Point(7, 100);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(70, 23);
            this.okButton.TabIndex = 15;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // DeltaSetter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(158, 128);
            this.Controls.Add(this.zVelLabel);
            this.Controls.Add(this.yVelLabel);
            this.Controls.Add(this.xVelLabel);
            this.Controls.Add(this.zVelText);
            this.Controls.Add(this.yVelText);
            this.Controls.Add(this.xVelText);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Name = "DeltaSetter";
            this.Text = "DeltaSetter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label zVelLabel;
        private System.Windows.Forms.Label yVelLabel;
        private System.Windows.Forms.Label xVelLabel;
        private System.Windows.Forms.TextBox zVelText;
        private System.Windows.Forms.TextBox yVelText;
        private System.Windows.Forms.TextBox xVelText;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
    }
}