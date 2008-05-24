namespace WorldMaker
{
    partial class CameraPanSetter
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
            this.pointBox = new System.Windows.Forms.ListBox();
            this.pointsLabel = new System.Windows.Forms.Label();
            this.xPosText = new System.Windows.Forms.TextBox();
            this.xPosLabel = new System.Windows.Forms.Label();
            this.yPosLabel = new System.Windows.Forms.Label();
            this.yPosText = new System.Windows.Forms.TextBox();
            this.zPosLabel = new System.Windows.Forms.Label();
            this.zPosText = new System.Windows.Forms.TextBox();
            this.xLookLabel = new System.Windows.Forms.Label();
            this.xLookText = new System.Windows.Forms.TextBox();
            this.yLookLabel = new System.Windows.Forms.Label();
            this.yLookText = new System.Windows.Forms.TextBox();
            this.zLookLabel = new System.Windows.Forms.Label();
            this.zLookText = new System.Windows.Forms.TextBox();
            this.xUpLabel = new System.Windows.Forms.Label();
            this.xUpText = new System.Windows.Forms.TextBox();
            this.yUpLabel = new System.Windows.Forms.Label();
            this.yUpText = new System.Windows.Forms.TextBox();
            this.zUpLabel = new System.Windows.Forms.Label();
            this.zUpText = new System.Windows.Forms.TextBox();
            this.editButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.doneButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pointBox
            // 
            this.pointBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pointBox.FormattingEnabled = true;
            this.pointBox.Location = new System.Drawing.Point(12, 30);
            this.pointBox.Name = "pointBox";
            this.pointBox.Size = new System.Drawing.Size(145, 251);
            this.pointBox.TabIndex = 0;
            this.pointBox.SelectedIndexChanged += new System.EventHandler(this.pointBox_SelectedIndexChanged);
            // 
            // pointsLabel
            // 
            this.pointsLabel.AutoSize = true;
            this.pointsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pointsLabel.Location = new System.Drawing.Point(9, 9);
            this.pointsLabel.Name = "pointsLabel";
            this.pointsLabel.Size = new System.Drawing.Size(148, 16);
            this.pointsLabel.TabIndex = 1;
            this.pointsLabel.Text = "List of camera views";
            // 
            // xPosText
            // 
            this.xPosText.Location = new System.Drawing.Point(246, 30);
            this.xPosText.Name = "xPosText";
            this.xPosText.Size = new System.Drawing.Size(100, 20);
            this.xPosText.TabIndex = 2;
            // 
            // xPosLabel
            // 
            this.xPosLabel.AutoSize = true;
            this.xPosLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xPosLabel.Location = new System.Drawing.Point(163, 31);
            this.xPosLabel.Name = "xPosLabel";
            this.xPosLabel.Size = new System.Drawing.Size(77, 16);
            this.xPosLabel.TabIndex = 3;
            this.xPosLabel.Text = "X Position";
            // 
            // yPosLabel
            // 
            this.yPosLabel.AutoSize = true;
            this.yPosLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yPosLabel.Location = new System.Drawing.Point(352, 31);
            this.yPosLabel.Name = "yPosLabel";
            this.yPosLabel.Size = new System.Drawing.Size(78, 16);
            this.yPosLabel.TabIndex = 4;
            this.yPosLabel.Text = "Y Position";
            // 
            // yPosText
            // 
            this.yPosText.Location = new System.Drawing.Point(436, 30);
            this.yPosText.Name = "yPosText";
            this.yPosText.Size = new System.Drawing.Size(100, 20);
            this.yPosText.TabIndex = 5;
            // 
            // zPosLabel
            // 
            this.zPosLabel.AutoSize = true;
            this.zPosLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zPosLabel.Location = new System.Drawing.Point(542, 31);
            this.zPosLabel.Name = "zPosLabel";
            this.zPosLabel.Size = new System.Drawing.Size(77, 16);
            this.zPosLabel.TabIndex = 6;
            this.zPosLabel.Text = "Z Position";
            // 
            // zPosText
            // 
            this.zPosText.Location = new System.Drawing.Point(618, 31);
            this.zPosText.Name = "zPosText";
            this.zPosText.Size = new System.Drawing.Size(100, 20);
            this.zPosText.TabIndex = 7;
            // 
            // xLookLabel
            // 
            this.xLookLabel.AutoSize = true;
            this.xLookLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xLookLabel.Location = new System.Drawing.Point(163, 69);
            this.xLookLabel.Name = "xLookLabel";
            this.xLookLabel.Size = new System.Drawing.Size(69, 16);
            this.xLookLabel.TabIndex = 8;
            this.xLookLabel.Text = "X LookAt";
            // 
            // xLookText
            // 
            this.xLookText.Location = new System.Drawing.Point(246, 68);
            this.xLookText.Name = "xLookText";
            this.xLookText.Size = new System.Drawing.Size(100, 20);
            this.xLookText.TabIndex = 9;
            // 
            // yLookLabel
            // 
            this.yLookLabel.AutoSize = true;
            this.yLookLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yLookLabel.Location = new System.Drawing.Point(352, 69);
            this.yLookLabel.Name = "yLookLabel";
            this.yLookLabel.Size = new System.Drawing.Size(70, 16);
            this.yLookLabel.TabIndex = 10;
            this.yLookLabel.Text = "Y LookAt";
            // 
            // yLookText
            // 
            this.yLookText.Location = new System.Drawing.Point(436, 68);
            this.yLookText.Name = "yLookText";
            this.yLookText.Size = new System.Drawing.Size(100, 20);
            this.yLookText.TabIndex = 11;
            // 
            // zLookLabel
            // 
            this.zLookLabel.AutoSize = true;
            this.zLookLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zLookLabel.Location = new System.Drawing.Point(542, 69);
            this.zLookLabel.Name = "zLookLabel";
            this.zLookLabel.Size = new System.Drawing.Size(69, 16);
            this.zLookLabel.TabIndex = 12;
            this.zLookLabel.Text = "Z LookAt";
            // 
            // zLookText
            // 
            this.zLookText.Location = new System.Drawing.Point(618, 69);
            this.zLookText.Name = "zLookText";
            this.zLookText.Size = new System.Drawing.Size(100, 20);
            this.zLookText.TabIndex = 13;
            // 
            // xUpLabel
            // 
            this.xUpLabel.AutoSize = true;
            this.xUpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xUpLabel.Location = new System.Drawing.Point(163, 104);
            this.xUpLabel.Name = "xUpLabel";
            this.xUpLabel.Size = new System.Drawing.Size(41, 16);
            this.xUpLabel.TabIndex = 14;
            this.xUpLabel.Text = "X Up";
            // 
            // xUpText
            // 
            this.xUpText.Location = new System.Drawing.Point(246, 103);
            this.xUpText.Name = "xUpText";
            this.xUpText.Size = new System.Drawing.Size(100, 20);
            this.xUpText.TabIndex = 15;
            // 
            // yUpLabel
            // 
            this.yUpLabel.AutoSize = true;
            this.yUpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yUpLabel.Location = new System.Drawing.Point(352, 104);
            this.yUpLabel.Name = "yUpLabel";
            this.yUpLabel.Size = new System.Drawing.Size(42, 16);
            this.yUpLabel.TabIndex = 16;
            this.yUpLabel.Text = "Y Up";
            // 
            // yUpText
            // 
            this.yUpText.Location = new System.Drawing.Point(436, 104);
            this.yUpText.Name = "yUpText";
            this.yUpText.Size = new System.Drawing.Size(100, 20);
            this.yUpText.TabIndex = 17;
            // 
            // zUpLabel
            // 
            this.zUpLabel.AutoSize = true;
            this.zUpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zUpLabel.Location = new System.Drawing.Point(542, 104);
            this.zUpLabel.Name = "zUpLabel";
            this.zUpLabel.Size = new System.Drawing.Size(41, 16);
            this.zUpLabel.TabIndex = 18;
            this.zUpLabel.Text = "Z Up";
            // 
            // zUpText
            // 
            this.zUpText.Location = new System.Drawing.Point(618, 103);
            this.zUpText.Name = "zUpText";
            this.zUpText.Size = new System.Drawing.Size(100, 20);
            this.zUpText.TabIndex = 19;
            // 
            // editButton
            // 
            this.editButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editButton.Location = new System.Drawing.Point(303, 143);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(127, 60);
            this.editButton.TabIndex = 20;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // addButton
            // 
            this.addButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addButton.Location = new System.Drawing.Point(446, 143);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(127, 60);
            this.addButton.TabIndex = 21;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeButton.Location = new System.Drawing.Point(591, 142);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(127, 61);
            this.removeButton.TabIndex = 22;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // runButton
            // 
            this.runButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runButton.Location = new System.Drawing.Point(163, 143);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(127, 60);
            this.runButton.TabIndex = 23;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(591, 219);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(127, 60);
            this.cancelButton.TabIndex = 24;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // doneButton
            // 
            this.doneButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doneButton.Location = new System.Drawing.Point(446, 219);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(127, 60);
            this.doneButton.TabIndex = 25;
            this.doneButton.Text = "Done";
            this.doneButton.UseVisualStyleBackColor = true;
            this.doneButton.Click += new System.EventHandler(this.doneButton_Click);
            // 
            // CameraPanSetter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 294);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.zUpText);
            this.Controls.Add(this.zUpLabel);
            this.Controls.Add(this.yUpText);
            this.Controls.Add(this.yUpLabel);
            this.Controls.Add(this.xUpText);
            this.Controls.Add(this.xUpLabel);
            this.Controls.Add(this.zLookText);
            this.Controls.Add(this.zLookLabel);
            this.Controls.Add(this.yLookText);
            this.Controls.Add(this.yLookLabel);
            this.Controls.Add(this.xLookText);
            this.Controls.Add(this.xLookLabel);
            this.Controls.Add(this.zPosText);
            this.Controls.Add(this.zPosLabel);
            this.Controls.Add(this.yPosText);
            this.Controls.Add(this.yPosLabel);
            this.Controls.Add(this.xPosLabel);
            this.Controls.Add(this.xPosText);
            this.Controls.Add(this.pointsLabel);
            this.Controls.Add(this.pointBox);
            this.Name = "CameraPanSetter";
            this.Text = "CameraPanSetter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox pointBox;
        private System.Windows.Forms.Label pointsLabel;
        private System.Windows.Forms.TextBox xPosText;
        private System.Windows.Forms.Label xPosLabel;
        private System.Windows.Forms.Label yPosLabel;
        private System.Windows.Forms.TextBox yPosText;
        private System.Windows.Forms.Label zPosLabel;
        private System.Windows.Forms.TextBox zPosText;
        private System.Windows.Forms.Label xLookLabel;
        private System.Windows.Forms.TextBox xLookText;
        private System.Windows.Forms.Label yLookLabel;
        private System.Windows.Forms.TextBox yLookText;
        private System.Windows.Forms.Label zLookLabel;
        private System.Windows.Forms.TextBox zLookText;
        private System.Windows.Forms.Label xUpLabel;
        private System.Windows.Forms.TextBox xUpText;
        private System.Windows.Forms.Label yUpLabel;
        private System.Windows.Forms.TextBox yUpText;
        private System.Windows.Forms.Label zUpLabel;
        private System.Windows.Forms.TextBox zUpText;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button doneButton;
    }
}