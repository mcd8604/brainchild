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
            this.ScaleZValue = new System.Windows.Forms.TextBox();
            this.MaxScaleZ = new System.Windows.Forms.TextBox();
            this.ScaleZ = new System.Windows.Forms.TrackBar();
            this.MinScaleZ = new System.Windows.Forms.TextBox();
            this.Scale_Z = new System.Windows.Forms.Label();
            this.ScaleYValue = new System.Windows.Forms.TextBox();
            this.MaxScaleY = new System.Windows.Forms.TextBox();
            this.ScaleY = new System.Windows.Forms.TrackBar();
            this.MinScaleY = new System.Windows.Forms.TextBox();
            this.Scale_Y = new System.Windows.Forms.Label();
            this.ScaleXValue = new System.Windows.Forms.TextBox();
            this.MaxScaleX = new System.Windows.Forms.TextBox();
            this.ScaleX = new System.Windows.Forms.TrackBar();
            this.MinScaleX = new System.Windows.Forms.TextBox();
            this.Scale_X = new System.Windows.Forms.Label();
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
            this.ScaleBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleX)).BeginInit();
            this.Rotation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationX)).BeginInit();
            this.Position.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ScaleBox
            // 
            this.ScaleBox.Controls.Add(this.ScaleZValue);
            this.ScaleBox.Controls.Add(this.MaxScaleZ);
            this.ScaleBox.Controls.Add(this.ScaleZ);
            this.ScaleBox.Controls.Add(this.MinScaleZ);
            this.ScaleBox.Controls.Add(this.Scale_Z);
            this.ScaleBox.Controls.Add(this.ScaleYValue);
            this.ScaleBox.Controls.Add(this.MaxScaleY);
            this.ScaleBox.Controls.Add(this.ScaleY);
            this.ScaleBox.Controls.Add(this.MinScaleY);
            this.ScaleBox.Controls.Add(this.Scale_Y);
            this.ScaleBox.Controls.Add(this.ScaleXValue);
            this.ScaleBox.Controls.Add(this.MaxScaleX);
            this.ScaleBox.Controls.Add(this.ScaleX);
            this.ScaleBox.Controls.Add(this.MinScaleX);
            this.ScaleBox.Controls.Add(this.Scale_X);
            this.ScaleBox.Location = new System.Drawing.Point(12, 12);
            this.ScaleBox.Name = "ScaleBox";
            this.ScaleBox.Size = new System.Drawing.Size(277, 194);
            this.ScaleBox.TabIndex = 0;
            this.ScaleBox.TabStop = false;
            this.ScaleBox.Text = "Scale";
            // 
            // ScaleZValue
            // 
            this.ScaleZValue.Location = new System.Drawing.Point(208, 150);
            this.ScaleZValue.Name = "ScaleZValue";
            this.ScaleZValue.Size = new System.Drawing.Size(48, 20);
            this.ScaleZValue.TabIndex = 14;
            this.ScaleZValue.Text = "1";
            this.ScaleZValue.TextChanged += new System.EventHandler(this.ScaleZValue_TextChanged);
            // 
            // MaxScaleZ
            // 
            this.MaxScaleZ.Location = new System.Drawing.Point(165, 150);
            this.MaxScaleZ.Name = "MaxScaleZ";
            this.MaxScaleZ.Size = new System.Drawing.Size(28, 20);
            this.MaxScaleZ.TabIndex = 13;
            this.MaxScaleZ.Text = "10";
            this.MaxScaleZ.TextChanged += new System.EventHandler(this.MaxScaleZ_TextChanged);
            // 
            // ScaleZ
            // 
            this.ScaleZ.Location = new System.Drawing.Point(55, 150);
            this.ScaleZ.Name = "ScaleZ";
            this.ScaleZ.Size = new System.Drawing.Size(104, 42);
            this.ScaleZ.TabIndex = 12;
            this.ScaleZ.Value = 1;
            this.ScaleZ.ValueChanged += new System.EventHandler(this.ScaleZ_ValueChanged);
            // 
            // MinScaleZ
            // 
            this.MinScaleZ.Location = new System.Drawing.Point(21, 150);
            this.MinScaleZ.Name = "MinScaleZ";
            this.MinScaleZ.Size = new System.Drawing.Size(28, 20);
            this.MinScaleZ.TabIndex = 11;
            this.MinScaleZ.Text = "0";
            this.MinScaleZ.TextChanged += new System.EventHandler(this.MinScaleZ_TextChanged);
            // 
            // Scale_Z
            // 
            this.Scale_Z.AutoSize = true;
            this.Scale_Z.Location = new System.Drawing.Point(18, 134);
            this.Scale_Z.Name = "Scale_Z";
            this.Scale_Z.Size = new System.Drawing.Size(44, 13);
            this.Scale_Z.TabIndex = 10;
            this.Scale_Z.Text = "Scale Z";
            // 
            // ScaleYValue
            // 
            this.ScaleYValue.Location = new System.Drawing.Point(208, 97);
            this.ScaleYValue.Name = "ScaleYValue";
            this.ScaleYValue.Size = new System.Drawing.Size(48, 20);
            this.ScaleYValue.TabIndex = 9;
            this.ScaleYValue.Text = "1";
            this.ScaleYValue.TextChanged += new System.EventHandler(this.ScaleYValue_TextChanged);
            // 
            // MaxScaleY
            // 
            this.MaxScaleY.Location = new System.Drawing.Point(165, 96);
            this.MaxScaleY.Name = "MaxScaleY";
            this.MaxScaleY.Size = new System.Drawing.Size(28, 20);
            this.MaxScaleY.TabIndex = 8;
            this.MaxScaleY.Text = "10";
            this.MaxScaleY.TextChanged += new System.EventHandler(this.MaxScaleY_TextChanged);
            // 
            // ScaleY
            // 
            this.ScaleY.Location = new System.Drawing.Point(55, 96);
            this.ScaleY.Name = "ScaleY";
            this.ScaleY.Size = new System.Drawing.Size(104, 42);
            this.ScaleY.TabIndex = 7;
            this.ScaleY.Value = 1;
            this.ScaleY.ValueChanged += new System.EventHandler(this.ScaleY_ValueChanged);
            // 
            // MinScaleY
            // 
            this.MinScaleY.Location = new System.Drawing.Point(21, 96);
            this.MinScaleY.Name = "MinScaleY";
            this.MinScaleY.Size = new System.Drawing.Size(28, 20);
            this.MinScaleY.TabIndex = 6;
            this.MinScaleY.Text = "0";
            this.MinScaleY.TextChanged += new System.EventHandler(this.MinScaleY_TextChanged);
            // 
            // Scale_Y
            // 
            this.Scale_Y.AutoSize = true;
            this.Scale_Y.Location = new System.Drawing.Point(18, 80);
            this.Scale_Y.Name = "Scale_Y";
            this.Scale_Y.Size = new System.Drawing.Size(44, 13);
            this.Scale_Y.TabIndex = 5;
            this.Scale_Y.Text = "Scale Y";
            // 
            // ScaleXValue
            // 
            this.ScaleXValue.Location = new System.Drawing.Point(208, 42);
            this.ScaleXValue.Name = "ScaleXValue";
            this.ScaleXValue.Size = new System.Drawing.Size(48, 20);
            this.ScaleXValue.TabIndex = 4;
            this.ScaleXValue.Text = "1";
            this.ScaleXValue.TextChanged += new System.EventHandler(this.ScaleXValue_TextChanged);
            // 
            // MaxScaleX
            // 
            this.MaxScaleX.Location = new System.Drawing.Point(165, 42);
            this.MaxScaleX.Name = "MaxScaleX";
            this.MaxScaleX.Size = new System.Drawing.Size(28, 20);
            this.MaxScaleX.TabIndex = 3;
            this.MaxScaleX.Text = "10";
            this.MaxScaleX.TextChanged += new System.EventHandler(this.MaxScaleX_TextChanged);
            // 
            // ScaleX
            // 
            this.ScaleX.Location = new System.Drawing.Point(55, 42);
            this.ScaleX.Name = "ScaleX";
            this.ScaleX.Size = new System.Drawing.Size(104, 42);
            this.ScaleX.TabIndex = 2;
            this.ScaleX.Value = 1;
            this.ScaleX.ValueChanged += new System.EventHandler(this.ScaleX_ValueChanged);
            // 
            // MinScaleX
            // 
            this.MinScaleX.Location = new System.Drawing.Point(21, 42);
            this.MinScaleX.Name = "MinScaleX";
            this.MinScaleX.Size = new System.Drawing.Size(28, 20);
            this.MinScaleX.TabIndex = 1;
            this.MinScaleX.Text = "0";
            this.MinScaleX.TextChanged += new System.EventHandler(this.MinScaleX_TextChanged);
            // 
            // Scale_X
            // 
            this.Scale_X.AutoSize = true;
            this.Scale_X.Location = new System.Drawing.Point(18, 26);
            this.Scale_X.Name = "Scale_X";
            this.Scale_X.Size = new System.Drawing.Size(44, 13);
            this.Scale_X.TabIndex = 0;
            this.Scale_X.Text = "Scale X";
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
            this.Rotation.Location = new System.Drawing.Point(304, 12);
            this.Rotation.Name = "Rotation";
            this.Rotation.Size = new System.Drawing.Size(272, 194);
            this.Rotation.TabIndex = 1;
            this.Rotation.TabStop = false;
            this.Rotation.Text = "Rotation";
            // 
            // RotationZValue
            // 
            this.RotationZValue.Location = new System.Drawing.Point(201, 150);
            this.RotationZValue.Name = "RotationZValue";
            this.RotationZValue.Size = new System.Drawing.Size(65, 20);
            this.RotationZValue.TabIndex = 8;
            this.RotationZValue.Text = "0";
            this.RotationZValue.TextChanged += new System.EventHandler(this.RotationZValue_TextChanged);
            // 
            // RotationYValue
            // 
            this.RotationYValue.Location = new System.Drawing.Point(200, 97);
            this.RotationYValue.Name = "RotationYValue";
            this.RotationYValue.Size = new System.Drawing.Size(65, 20);
            this.RotationYValue.TabIndex = 7;
            this.RotationYValue.Text = "0";
            this.RotationYValue.TextChanged += new System.EventHandler(this.RotationYValue_TextChanged);
            // 
            // RotationXValue
            // 
            this.RotationXValue.Location = new System.Drawing.Point(201, 42);
            this.RotationXValue.Name = "RotationXValue";
            this.RotationXValue.Size = new System.Drawing.Size(65, 20);
            this.RotationXValue.TabIndex = 6;
            this.RotationXValue.Text = "0";
            this.RotationXValue.TextChanged += new System.EventHandler(this.RotationXValue_TextChanged);
            // 
            // RotationZ
            // 
            this.RotationZ.Location = new System.Drawing.Point(10, 150);
            this.RotationZ.Maximum = 180;
            this.RotationZ.Minimum = -180;
            this.RotationZ.Name = "RotationZ";
            this.RotationZ.Size = new System.Drawing.Size(184, 42);
            this.RotationZ.SmallChange = 15;
            this.RotationZ.TabIndex = 5;
            this.RotationZ.TickFrequency = 15;
            this.RotationZ.Scroll += new System.EventHandler(this.RotationZ_Scroll);
            // 
            // Rotation_Z
            // 
            this.Rotation_Z.AutoSize = true;
            this.Rotation_Z.Location = new System.Drawing.Point(7, 134);
            this.Rotation_Z.Name = "Rotation_Z";
            this.Rotation_Z.Size = new System.Drawing.Size(57, 13);
            this.Rotation_Z.TabIndex = 4;
            this.Rotation_Z.Text = "Rotation Z";
            // 
            // RotationY
            // 
            this.RotationY.Location = new System.Drawing.Point(10, 96);
            this.RotationY.Maximum = 180;
            this.RotationY.Minimum = -180;
            this.RotationY.Name = "RotationY";
            this.RotationY.Size = new System.Drawing.Size(184, 42);
            this.RotationY.SmallChange = 15;
            this.RotationY.TabIndex = 3;
            this.RotationY.TickFrequency = 15;
            this.RotationY.Scroll += new System.EventHandler(this.RotationY_Scroll);
            // 
            // Rotation_Y
            // 
            this.Rotation_Y.AutoSize = true;
            this.Rotation_Y.Location = new System.Drawing.Point(7, 80);
            this.Rotation_Y.Name = "Rotation_Y";
            this.Rotation_Y.Size = new System.Drawing.Size(57, 13);
            this.Rotation_Y.TabIndex = 2;
            this.Rotation_Y.Text = "Rotation Y";
            // 
            // RotationX
            // 
            this.RotationX.Location = new System.Drawing.Point(10, 42);
            this.RotationX.Maximum = 180;
            this.RotationX.Minimum = -180;
            this.RotationX.Name = "RotationX";
            this.RotationX.Size = new System.Drawing.Size(184, 42);
            this.RotationX.SmallChange = 15;
            this.RotationX.TabIndex = 1;
            this.RotationX.TickFrequency = 15;
            this.RotationX.Scroll += new System.EventHandler(this.RotationX_Scroll);
            // 
            // Rotation_X
            // 
            this.Rotation_X.AutoSize = true;
            this.Rotation_X.Location = new System.Drawing.Point(7, 26);
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
            this.Position.Location = new System.Drawing.Point(582, 12);
            this.Position.Name = "Position";
            this.Position.Size = new System.Drawing.Size(190, 84);
            this.Position.TabIndex = 2;
            this.Position.TabStop = false;
            this.Position.Text = "Position";
            // 
            // PositionZ
            // 
            this.PositionZ.Location = new System.Drawing.Point(127, 42);
            this.PositionZ.Name = "PositionZ";
            this.PositionZ.Size = new System.Drawing.Size(54, 20);
            this.PositionZ.TabIndex = 5;
            this.PositionZ.Text = "0";
            this.PositionZ.TextChanged += new System.EventHandler(this.PositionZ_TextChanged);
            // 
            // Z
            // 
            this.Z.AutoSize = true;
            this.Z.Location = new System.Drawing.Point(126, 26);
            this.Z.Name = "Z";
            this.Z.Size = new System.Drawing.Size(14, 13);
            this.Z.TabIndex = 4;
            this.Z.Text = "Z";
            // 
            // PositionY
            // 
            this.PositionY.Location = new System.Drawing.Point(67, 42);
            this.PositionY.Name = "PositionY";
            this.PositionY.Size = new System.Drawing.Size(54, 20);
            this.PositionY.TabIndex = 3;
            this.PositionY.Text = "0";
            this.PositionY.TextChanged += new System.EventHandler(this.PositionY_TextChanged);
            // 
            // Y
            // 
            this.Y.AutoSize = true;
            this.Y.Location = new System.Drawing.Point(66, 26);
            this.Y.Name = "Y";
            this.Y.Size = new System.Drawing.Size(14, 13);
            this.Y.TabIndex = 2;
            this.Y.Text = "Y";
            // 
            // PositionX
            // 
            this.PositionX.Location = new System.Drawing.Point(7, 42);
            this.PositionX.Name = "PositionX";
            this.PositionX.Size = new System.Drawing.Size(54, 20);
            this.PositionX.TabIndex = 1;
            this.PositionX.Text = "0";
            this.PositionX.TextChanged += new System.EventHandler(this.PositionX_TextChanged);
            // 
            // X
            // 
            this.X.AutoSize = true;
            this.X.Location = new System.Drawing.Point(6, 26);
            this.X.Name = "X";
            this.X.Size = new System.Drawing.Size(14, 13);
            this.X.TabIndex = 0;
            this.X.Text = "X";
            // 
            // FocusButton
            // 
            this.FocusButton.Location = new System.Drawing.Point(582, 184);
            this.FocusButton.Name = "FocusButton";
            this.FocusButton.Size = new System.Drawing.Size(190, 22);
            this.FocusButton.TabIndex = 3;
            this.FocusButton.Text = "Focus";
            this.FocusButton.UseVisualStyleBackColor = true;
            this.FocusButton.Click += new System.EventHandler(this.Focus_Click);
            // 
            // repeatTexture_cb
            // 
            this.repeatTexture_cb.AutoSize = true;
            this.repeatTexture_cb.Location = new System.Drawing.Point(10, 53);
            this.repeatTexture_cb.Name = "repeatTexture_cb";
            this.repeatTexture_cb.Size = new System.Drawing.Size(75, 17);
            this.repeatTexture_cb.TabIndex = 4;
            this.repeatTexture_cb.Text = "Repeating";
            this.repeatTexture_cb.UseVisualStyleBackColor = true;
            this.repeatTexture_cb.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textureScaleY);
            this.groupBox1.Controls.Add(this.textureScaleX);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.repeatTexture_cb);
            this.groupBox1.Location = new System.Drawing.Point(582, 102);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(190, 76);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Texture";
            // 
            // textureScaleY
            // 
            this.textureScaleY.Location = new System.Drawing.Point(67, 28);
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
            this.label2.Location = new System.Drawing.Point(64, 14);
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
            // ModelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 219);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.FocusButton);
            this.Controls.Add(this.Position);
            this.Controls.Add(this.Rotation);
            this.Controls.Add(this.ScaleBox);
            this.Name = "ModelEditor";
            this.Text = "ModelEditor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ScaleBox.ResumeLayout(false);
            this.ScaleBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleX)).EndInit();
            this.Rotation.ResumeLayout(false);
            this.Rotation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationX)).EndInit();
            this.Position.ResumeLayout(false);
            this.Position.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ScaleBox;
        private System.Windows.Forms.TrackBar ScaleX;
        private System.Windows.Forms.TextBox MinScaleX;
        private System.Windows.Forms.Label Scale_X;
        private System.Windows.Forms.TextBox ScaleXValue;
        private System.Windows.Forms.TextBox MaxScaleX;
        private System.Windows.Forms.TextBox ScaleZValue;
        private System.Windows.Forms.TextBox MaxScaleZ;
        private System.Windows.Forms.TrackBar ScaleZ;
        private System.Windows.Forms.TextBox MinScaleZ;
        private System.Windows.Forms.Label Scale_Z;
        private System.Windows.Forms.TextBox ScaleYValue;
        private System.Windows.Forms.TextBox MaxScaleY;
        private System.Windows.Forms.TrackBar ScaleY;
        private System.Windows.Forms.TextBox MinScaleY;
        private System.Windows.Forms.Label Scale_Y;
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
    }
}