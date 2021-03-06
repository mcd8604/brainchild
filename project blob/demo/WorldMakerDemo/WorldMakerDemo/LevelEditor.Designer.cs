namespace WorldMakerDemo
{
    partial class LevelEditor
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
            this.areaListBox = new System.Windows.Forms.ListBox();
            this.modelListBox = new System.Windows.Forms.ListBox();
            this.nameDisplay = new System.Windows.Forms.Label();
            this.loadButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.areaLabel = new System.Windows.Forms.Label();
            this.modelList = new System.Windows.Forms.Label();
            this.areaDelButton = new System.Windows.Forms.Button();
            this.areaAddButton = new System.Windows.Forms.Button();
            this.modelDelButton = new System.Windows.Forms.Button();
            this.modelAddButton = new System.Windows.Forms.Button();
            this.levelName = new System.Windows.Forms.TextBox();
            this.EditButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.areaTextBox = new System.Windows.Forms.TextBox();
            this.nameButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // areaListBox
            // 
            this.areaListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.areaListBox.FormattingEnabled = true;
            this.areaListBox.ItemHeight = 16;
            this.areaListBox.Location = new System.Drawing.Point(12, 161);
            this.areaListBox.Name = "areaListBox";
            this.areaListBox.Size = new System.Drawing.Size(269, 132);
            this.areaListBox.TabIndex = 0;
            this.areaListBox.SelectedIndexChanged += new System.EventHandler(this.areaListBox_SelectedIndexChanged);
            // 
            // modelListBox
            // 
            this.modelListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modelListBox.FormattingEnabled = true;
            this.modelListBox.ItemHeight = 16;
            this.modelListBox.Location = new System.Drawing.Point(12, 369);
            this.modelListBox.Name = "modelListBox";
            this.modelListBox.Size = new System.Drawing.Size(269, 308);
            this.modelListBox.TabIndex = 1;
            this.modelListBox.SelectedIndexChanged += new System.EventHandler(this.modelListBox_SelectedIndexChanged);
            // 
            // nameDisplay
            // 
            this.nameDisplay.AutoSize = true;
            this.nameDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameDisplay.Location = new System.Drawing.Point(12, 9);
            this.nameDisplay.Name = "nameDisplay";
            this.nameDisplay.Size = new System.Drawing.Size(56, 20);
            this.nameDisplay.TabIndex = 2;
            this.nameDisplay.Text = "Level:";
            // 
            // loadButton
            // 
            this.loadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadButton.Location = new System.Drawing.Point(12, 35);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(125, 44);
            this.loadButton.TabIndex = 3;
            this.loadButton.Text = "Load Level";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveButton.Location = new System.Drawing.Point(156, 35);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(125, 44);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save Level";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // areaLabel
            // 
            this.areaLabel.AutoSize = true;
            this.areaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.areaLabel.Location = new System.Drawing.Point(12, 138);
            this.areaLabel.Name = "areaLabel";
            this.areaLabel.Size = new System.Drawing.Size(177, 20);
            this.areaLabel.TabIndex = 5;
            this.areaLabel.Text = "List of Areas in Level";
            // 
            // modelList
            // 
            this.modelList.AutoSize = true;
            this.modelList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modelList.Location = new System.Drawing.Point(12, 346);
            this.modelList.Name = "modelList";
            this.modelList.Size = new System.Drawing.Size(187, 20);
            this.modelList.TabIndex = 6;
            this.modelList.Text = "List of Models in Level";
            // 
            // areaDelButton
            // 
            this.areaDelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.areaDelButton.Location = new System.Drawing.Point(12, 299);
            this.areaDelButton.Name = "areaDelButton";
            this.areaDelButton.Size = new System.Drawing.Size(125, 44);
            this.areaDelButton.TabIndex = 7;
            this.areaDelButton.Text = "Delete Area";
            this.areaDelButton.UseVisualStyleBackColor = true;
            this.areaDelButton.Click += new System.EventHandler(this.areaDelButton_Click);
            // 
            // areaAddButton
            // 
            this.areaAddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.areaAddButton.Location = new System.Drawing.Point(156, 299);
            this.areaAddButton.Name = "areaAddButton";
            this.areaAddButton.Size = new System.Drawing.Size(125, 44);
            this.areaAddButton.TabIndex = 8;
            this.areaAddButton.Text = "Add Area";
            this.areaAddButton.UseVisualStyleBackColor = true;
            this.areaAddButton.Click += new System.EventHandler(this.areaAddButton_Click);
            // 
            // modelDelButton
            // 
            this.modelDelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modelDelButton.Location = new System.Drawing.Point(12, 683);
            this.modelDelButton.Name = "modelDelButton";
            this.modelDelButton.Size = new System.Drawing.Size(125, 44);
            this.modelDelButton.TabIndex = 9;
            this.modelDelButton.Text = "Delete Model";
            this.modelDelButton.UseVisualStyleBackColor = true;
            this.modelDelButton.Click += new System.EventHandler(this.modelDelButton_Click);
            // 
            // modelAddButton
            // 
            this.modelAddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modelAddButton.Location = new System.Drawing.Point(156, 683);
            this.modelAddButton.Name = "modelAddButton";
            this.modelAddButton.Size = new System.Drawing.Size(125, 44);
            this.modelAddButton.TabIndex = 10;
            this.modelAddButton.Text = "Add Model";
            this.modelAddButton.UseVisualStyleBackColor = true;
            this.modelAddButton.Click += new System.EventHandler(this.modelAddButton_Click);
            // 
            // levelName
            // 
            this.levelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.levelName.Location = new System.Drawing.Point(74, 7);
            this.levelName.Name = "levelName";
            this.levelName.Size = new System.Drawing.Size(207, 22);
            this.levelName.TabIndex = 11;
            // 
            // EditButton
            // 
            this.EditButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditButton.Location = new System.Drawing.Point(12, 733);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(269, 47);
            this.EditButton.TabIndex = 12;
            this.EditButton.Text = "Edit Model";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "Area Name:";
            // 
            // areaTextBox
            // 
            this.areaTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.areaTextBox.Location = new System.Drawing.Point(16, 113);
            this.areaTextBox.Name = "areaTextBox";
            this.areaTextBox.Size = new System.Drawing.Size(138, 22);
            this.areaTextBox.TabIndex = 15;
            // 
            // nameButton
            // 
            this.nameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameButton.Location = new System.Drawing.Point(160, 90);
            this.nameButton.Name = "nameButton";
            this.nameButton.Size = new System.Drawing.Size(121, 45);
            this.nameButton.TabIndex = 16;
            this.nameButton.Text = "Change Name";
            this.nameButton.UseVisualStyleBackColor = true;
            this.nameButton.Click += new System.EventHandler(this.nameButton_Click);
            // 
            // LevelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 784);
            this.Controls.Add(this.nameButton);
            this.Controls.Add(this.areaTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.levelName);
            this.Controls.Add(this.modelAddButton);
            this.Controls.Add(this.modelDelButton);
            this.Controls.Add(this.areaAddButton);
            this.Controls.Add(this.areaDelButton);
            this.Controls.Add(this.modelList);
            this.Controls.Add(this.areaLabel);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.nameDisplay);
            this.Controls.Add(this.modelListBox);
            this.Controls.Add(this.areaListBox);
            this.Name = "LevelEditor";
            this.Text = "LevelEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox areaListBox;
        private System.Windows.Forms.ListBox modelListBox;
        private System.Windows.Forms.Label nameDisplay;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label areaLabel;
        private System.Windows.Forms.Label modelList;
        private System.Windows.Forms.Button areaDelButton;
        private System.Windows.Forms.Button areaAddButton;
        private System.Windows.Forms.Button modelDelButton;
        private System.Windows.Forms.Button modelAddButton;
        private System.Windows.Forms.TextBox levelName;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox areaTextBox;
        private System.Windows.Forms.Button nameButton;
    }
}