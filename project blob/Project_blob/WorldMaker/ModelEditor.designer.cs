namespace WorldMaker
{
    partial class ModelEditor
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
            this.ScaleBox = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ScaleZValue = new System.Windows.Forms.TextBox();
            this.ScaleYValue = new System.Windows.Forms.TextBox();
            this.ScaleXValue = new System.Windows.Forms.TextBox();
            this.Rotation = new System.Windows.Forms.GroupBox();
            this.RotationZValue = new System.Windows.Forms.TextBox();
            this.RotationYValue = new System.Windows.Forms.TextBox();
            this.RotationXValue = new System.Windows.Forms.TextBox();
            this.RotationZ = new System.Windows.Forms.TrackBar();
            this.Rotation_Z = new System.Windows.Forms.Label();
            this.RotationY = new System.Windows.Forms.TrackBar();
            this.Rotation_Y = new System.Windows.Forms.Label();
            this.RotationX = new System.Windows.Forms.TrackBar();
            this.Rotation_X = new System.Windows.Forms.Label();
            this.Position = new System.Windows.Forms.GroupBox();
            this.PositionZ = new System.Windows.Forms.TextBox();
            this.Z = new System.Windows.Forms.Label();
            this.PositionY = new System.Windows.Forms.TextBox();
            this.Y = new System.Windows.Forms.Label();
            this.PositionX = new System.Windows.Forms.TextBox();
            this.X = new System.Windows.Forms.Label();
            this.FocusButton = new System.Windows.Forms.Button();
            this.repeatTexture_cb = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textureScaleY = new System.Windows.Forms.TextBox();
            this.textureScaleX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.roomList = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.removeRoom = new System.Windows.Forms.Button();
            this.addRoom = new System.Windows.Forms.Button();
            this.roomTextBox = new System.Windows.Forms.TextBox();
            this.ScaleBox.SuspendLayout();
            this.Rotation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationX)).BeginInit();
            this.Position.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ScaleBox
            // 
            this.ScaleBox.Controls.Add(this.label3);
            this.ScaleBox.Controls.Add(this.label4);
            this.ScaleBox.Controls.Add(this.label5);
            this.ScaleBox.Controls.Add(this.ScaleZValue);
            this.ScaleBox.Controls.Add(this.ScaleYValue);
            this.ScaleBox.Controls.Add(this.ScaleXValue);
            this.ScaleBox.Location = new System.Drawing.Point(12, 12);
            this.ScaleBox.Name = "ScaleBox";
            this.ScaleBox.Size = new System.Drawing.Size(82, 84);
            this.ScaleBox.TabIndex = 0;
            this.ScaleBox.TabStop = false;
            this.ScaleBox.Text = "Scale";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Z";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "X";
            // 
            // ScaleZValue
            // 
            this.ScaleZValue.Location = new System.Drawing.Point(23, 52);
            this.ScaleZValue.Name = "ScaleZValue";
            this.ScaleZValue.Size = new System.Drawing.Size(48, 20);
            this.ScaleZValue.TabIndex = 14;
            this.ScaleZValue.Text = "1";
            this.ScaleZValue.TextChanged += new System.EventHandler(this.ScaleZValue_TextChanged);
            // 
            // ScaleYValue
            // 
            this.ScaleYValue.Location = new System.Drawing.Point(23, 32);
            this.ScaleYValue.Name = "ScaleYValue";
            this.ScaleYValue.Size = new System.Drawing.Size(48, 20);
            this.ScaleYValue.TabIndex = 9;
            this.ScaleYValue.Text = "1";
            this.ScaleYValue.TextChanged += new System.EventHandler(this.ScaleYValue_TextChanged);
            // 
            // ScaleXValue
            // 
            this.ScaleXValue.Location = new System.Drawing.Point(23, 12);
            this.ScaleXValue.Name = "ScaleXValue";
            this.ScaleXValue.Size = new System.Drawing.Size(48, 20);
            this.ScaleXValue.TabIndex = 4;
            this.ScaleXValue.Text = "1";
            this.ScaleXValue.TextChanged += new System.EventHandler(this.ScaleXValue_TextChanged);
            // 
            // Rotation
            // 
            this.Rotation.Controls.Add(this.RotationZValue);
            this.Rotation.Controls.Add(this.RotationYValue);
            this.Rotation.Controls.Add(this.RotationXValue);
            this.Rotation.Controls.Add(this.RotationZ);
            this.Rotation.Controls.Add(this.Rotation_Z);
            this.Rotation.Controls.Add(this.RotationY);
            this.Rotation.Controls.Add(this.Rotation_Y);
            this.Rotation.Controls.Add(this.RotationX);
            this.Rotation.Controls.Add(this.Rotation_X);
            this.Rotation.Location = new System.Drawing.Point(100, 12);
            this.Rotation.Name = "Rotation";
            this.Rotation.Size = new System.Drawing.Size(265, 178);
            this.Rotation.TabIndex = 1;
            this.Rotation.TabStop = false;
            this.Rotation.Text = "Rotation";
            // 
            // RotationZValue
            // 
            this.RotationZValue.Location = new System.Drawing.Point(192, 129);
            this.RotationZValue.Name = "RotationZValue";
            this.RotationZValue.Size = new System.Drawing.Size(65, 20);
            this.RotationZValue.TabIndex = 8;
            this.RotationZValue.Text = "0";
            this.RotationZValue.TextChanged += new System.EventHandler(this.RotationZValue_TextChanged);
            // 
            // RotationYValue
            // 
            this.RotationYValue.Location = new System.Drawing.Point(191, 79);
            this.RotationYValue.Name = "RotationYValue";
            this.RotationYValue.Size = new System.Drawing.Size(65, 20);
            this.RotationYValue.TabIndex = 7;
            this.RotationYValue.Text = "0";
            this.RotationYValue.TextChanged += new System.EventHandler(this.RotationYValue_TextChanged);
            // 
            // RotationXValue
            // 
            this.RotationXValue.Location = new System.Drawing.Point(192, 27);
            this.RotationXValue.Name = "RotationXValue";
            this.RotationXValue.Size = new System.Drawing.Size(65, 20);
            this.RotationXValue.TabIndex = 6;
            this.RotationXValue.Text = "0";
            this.RotationXValue.TextChanged += new System.EventHandler(this.RotationXValue_TextChanged);
            // 
            // RotationZ
            // 
            this.RotationZ.Location = new System.Drawing.Point(6, 129);
            this.RotationZ.Maximum = 180;
            this.RotationZ.Minimum = -180;
            this.RotationZ.Name = "RotationZ";
            this.RotationZ.Size = new System.Drawing.Size(184, 45);
            this.RotationZ.SmallChange = 15;
            this.RotationZ.TabIndex = 5;
            this.RotationZ.TickFrequency = 15;
            this.RotationZ.Scroll += new System.EventHandler(this.RotationZ_Scroll);
            // 
            // Rotation_Z
            // 
            this.Rotation_Z.AutoSize = true;
            this.Rotation_Z.Location = new System.Drawing.Point(3, 118);
            this.Rotation_Z.Name = "Rotation_Z";
            this.Rotation_Z.Size = new System.Drawing.Size(57, 13);
            this.Rotation_Z.TabIndex = 4;
            this.Rotation_Z.Text = "Rotation Z";
            // 
            // RotationY
            // 
            this.RotationY.Location = new System.Drawing.Point(6, 78);
            this.RotationY.Maximum = 180;
            this.RotationY.Minimum = -180;
            this.RotationY.Name = "RotationY";
            this.RotationY.Size = new System.Drawing.Size(184, 45);
            this.RotationY.SmallChange = 15;
            this.RotationY.TabIndex = 3;
            this.RotationY.TickFrequency = 15;
            this.RotationY.Scroll += new System.EventHandler(this.RotationY_Scroll);
            // 
            // Rotation_Y
            // 
            this.Rotation_Y.AutoSize = true;
            this.Rotation_Y.Location = new System.Drawing.Point(3, 67);
            this.Rotation_Y.Name = "Rotation_Y";
            this.Rotation_Y.Size = new System.Drawing.Size(57, 13);
            this.Rotation_Y.TabIndex = 2;
            this.Rotation_Y.Text = "Rotation Y";
            // 
            // RotationX
            // 
            this.RotationX.Location = new System.Drawing.Point(6, 27);
            this.RotationX.Maximum = 180;
            this.RotationX.Minimum = -180;
            this.RotationX.Name = "RotationX";
            this.RotationX.Size = new System.Drawing.Size(184, 45);
            this.RotationX.SmallChange = 15;
            this.RotationX.TabIndex = 1;
            this.RotationX.TickFrequency = 15;
            this.RotationX.Scroll += new System.EventHandler(this.RotationX_Scroll);
            // 
            // Rotation_X
            // 
            this.Rotation_X.AutoSize = true;
            this.Rotation_X.Location = new System.Drawing.Point(3, 16);
            this.Rotation_X.Name = "Rotation_X";
            this.Rotation_X.Size = new System.Drawing.Size(57, 13);
            this.Rotation_X.TabIndex = 0;
            this.Rotation_X.Text = "Rotation X";
            // 
            // Position
            // 
            this.Position.Controls.Add(this.PositionZ);
            this.Position.Controls.Add(this.Z);
            this.Position.Controls.Add(this.PositionY);
            this.Position.Controls.Add(this.Y);
            this.Position.Controls.Add(this.PositionX);
            this.Position.Controls.Add(this.X);
            this.Position.Location = new System.Drawing.Point(12, 106);
            this.Position.Name = "Position";
            this.Position.Size = new System.Drawing.Size(82, 84);
            this.Position.TabIndex = 2;
            this.Position.TabStop = false;
            this.Position.Text = "Position";
            // 
            // PositionZ
            // 
            this.PositionZ.Location = new System.Drawing.Point(16, 55);
            this.PositionZ.Name = "PositionZ";
            this.PositionZ.Size = new System.Drawing.Size(54, 20);
            this.PositionZ.TabIndex = 5;
            this.PositionZ.Text = "0";
            this.PositionZ.TextChanged += new System.EventHandler(this.PositionZ_TextChanged);
            // 
            // Z
            // 
            this.Z.AutoSize = true;
            this.Z.Location = new System.Drawing.Point(3, 55);
            this.Z.Name = "Z";
            this.Z.Size = new System.Drawing.Size(14, 13);
            this.Z.TabIndex = 4;
            this.Z.Text = "Z";
            // 
            // PositionY
            // 
            this.PositionY.Location = new System.Drawing.Point(16, 34);
            this.PositionY.Name = "PositionY";
            this.PositionY.Size = new System.Drawing.Size(54, 20);
            this.PositionY.TabIndex = 3;
            this.PositionY.Text = "0";
            this.PositionY.TextChanged += new System.EventHandler(this.PositionY_TextChanged);
            // 
            // Y
            // 
            this.Y.AutoSize = true;
            this.Y.Location = new System.Drawing.Point(3, 37);
            this.Y.Name = "Y";
            this.Y.Size = new System.Drawing.Size(14, 13);
            this.Y.TabIndex = 2;
            this.Y.Text = "Y";
            // 
            // PositionX
            // 
            this.PositionX.Location = new System.Drawing.Point(16, 13);
            this.PositionX.Name = "PositionX";
            this.PositionX.Size = new System.Drawing.Size(54, 20);
            this.PositionX.TabIndex = 1;
            this.PositionX.Text = "0";
            this.PositionX.TextChanged += new System.EventHandler(this.PositionX_TextChanged);
            // 
            // X
            // 
            this.X.AutoSize = true;
            this.X.Location = new System.Drawing.Point(3, 17);
            this.X.Name = "X";
            this.X.Size = new System.Drawing.Size(14, 13);
            this.X.TabIndex = 0;
            this.X.Text = "X";
            // 
            // FocusButton
            // 
            this.FocusButton.Location = new System.Drawing.Point(370, 147);
            this.FocusButton.Name = "FocusButton";
            this.FocusButton.Size = new System.Drawing.Size(88, 43);
            this.FocusButton.TabIndex = 3;
            this.FocusButton.Text = "Focus";
            this.FocusButton.UseVisualStyleBackColor = true;
            this.FocusButton.Click += new System.EventHandler(this.Focus_Click);
            // 
            // repeatTexture_cb
            // 
            this.repeatTexture_cb.AutoSize = true;
            this.repeatTexture_cb.Location = new System.Drawing.Point(10, 97);
            this.repeatTexture_cb.Name = "repeatTexture_cb";
            this.repeatTexture_cb.Size = new System.Drawing.Size(75, 17);
            this.repeatTexture_cb.TabIndex = 4;
            this.repeatTexture_cb.Text = "Repeating";
            this.repeatTexture_cb.UseVisualStyleBackColor = true;
            this.repeatTexture_cb.CheckedChanged += new System.EventHandler(this.repeatTexture_cb_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textureScaleY);
            this.groupBox1.Controls.Add(this.textureScaleX);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.repeatTexture_cb);
            this.groupBox1.Location = new System.Drawing.Point(370, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(88, 129);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Texture";
            // 
            // textureScaleY
            // 
            this.textureScaleY.Location = new System.Drawing.Point(11, 67);
            this.textureScaleY.Name = "textureScaleY";
            this.textureScaleY.Size = new System.Drawing.Size(52, 20);
            this.textureScaleY.TabIndex = 8;
            this.textureScaleY.Text = "1.0";
            this.textureScaleY.TextChanged += new System.EventHandler(this.textureScaleY_TextChanged);
            // 
            // textureScaleX
            // 
            this.textureScaleX.Location = new System.Drawing.Point(10, 28);
            this.textureScaleX.Name = "textureScaleX";
            this.textureScaleX.Size = new System.Drawing.Size(52, 20);
            this.textureScaleX.TabIndex = 7;
            this.textureScaleX.Text = "1.0";
            this.textureScaleX.TextChanged += new System.EventHandler(this.textureScaleX_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Scale Y";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Scale X";
            // 
            // roomList
            // 
            this.roomList.FormattingEnabled = true;
            this.roomList.Location = new System.Drawing.Point(6, 12);
            this.roomList.Name = "roomList";
            this.roomList.Size = new System.Drawing.Size(47, 108);
            this.roomList.TabIndex = 6;
            this.roomList.SelectedIndexChanged += new System.EventHandler(this.roomList_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.removeRoom);
            this.groupBox2.Controls.Add(this.addRoom);
            this.groupBox2.Controls.Add(this.roomList);
            this.groupBox2.Location = new System.Drawing.Point(464, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(60, 180);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rooms";
            // 
            // removeRoom
            // 
            this.removeRoom.Location = new System.Drawing.Point(33, 149);
            this.removeRoom.Name = "removeRoom";
            this.removeRoom.Size = new System.Drawing.Size(20, 22);
            this.removeRoom.TabIndex = 8;
            this.removeRoom.Text = "-";
            this.removeRoom.UseVisualStyleBackColor = true;
            this.removeRoom.Click += new System.EventHandler(this.removeRoom_Click_1);
            // 
            // addRoom
            // 
            this.addRoom.Location = new System.Drawing.Point(7, 149);
            this.addRoom.Name = "addRoom";
            this.addRoom.Size = new System.Drawing.Size(20, 22);
            this.addRoom.TabIndex = 7;
            this.addRoom.Text = "+";
            this.addRoom.UseVisualStyleBackColor = true;
            this.addRoom.Click += new System.EventHandler(this.addRoom_Click_1);
            // 
            // roomTextBox
            // 
            this.roomTextBox.Location = new System.Drawing.Point(470, 135);
            this.roomTextBox.Name = "roomTextBox";
            this.roomTextBox.Size = new System.Drawing.Size(46, 20);
            this.roomTextBox.TabIndex = 9;
            this.roomTextBox.Text = "0";
            // 
            // ModelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 200);
            this.Controls.Add(this.roomTextBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.FocusButton);
            this.Controls.Add(this.Position);
            this.Controls.Add(this.Rotation);
            this.Controls.Add(this.ScaleBox);
            this.Name = "ModelEditor";
            this.Text = " ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ScaleBox.ResumeLayout(false);
            this.ScaleBox.PerformLayout();
            this.Rotation.ResumeLayout(false);
            this.Rotation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationX)).EndInit();
            this.Position.ResumeLayout(false);
            this.Position.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.GroupBox ScaleBox;
		private System.Windows.Forms.TextBox ScaleXValue;
		private System.Windows.Forms.TextBox ScaleZValue;
		private System.Windows.Forms.TextBox ScaleYValue;
        private System.Windows.Forms.GroupBox Rotation;
        private System.Windows.Forms.TrackBar RotationX;
        private System.Windows.Forms.Label Rotation_X;
        private System.Windows.Forms.TrackBar RotationZ;
        private System.Windows.Forms.Label Rotation_Z;
        private System.Windows.Forms.TrackBar RotationY;
        private System.Windows.Forms.Label Rotation_Y;
        private System.Windows.Forms.GroupBox Position;
        private System.Windows.Forms.TextBox PositionZ;
        private System.Windows.Forms.Label Z;
        private System.Windows.Forms.TextBox PositionY;
        private System.Windows.Forms.Label Y;
        private System.Windows.Forms.TextBox PositionX;
        private System.Windows.Forms.Label X;
        private System.Windows.Forms.Button FocusButton;
        private System.Windows.Forms.TextBox RotationZValue;
        private System.Windows.Forms.TextBox RotationYValue;
        private System.Windows.Forms.TextBox RotationXValue;
        private System.Windows.Forms.CheckBox repeatTexture_cb;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textureScaleY;
		private System.Windows.Forms.TextBox textureScaleX;
		private System.Windows.Forms.ListBox roomList;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button removeRoom;
		private System.Windows.Forms.Button addRoom;
        private System.Windows.Forms.TextBox roomTextBox;
    }
}