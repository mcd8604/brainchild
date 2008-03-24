namespace WorldMakerDemo
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
            this.Scale = new System.Windows.Forms.GroupBox();
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
            this.Focus = new System.Windows.Forms.Button();
            this.Model_Name = new System.Windows.Forms.Label();
            this.ModelName = new System.Windows.Forms.TextBox();
            this.Scale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleX)).BeginInit();
            this.Rotation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationX)).BeginInit();
            this.Position.SuspendLayout();
            this.SuspendLayout();
            // 
            // Scale
            // 
            this.Scale.Controls.Add(this.ScaleZValue);
            this.Scale.Controls.Add(this.MaxScaleZ);
            this.Scale.Controls.Add(this.ScaleZ);
            this.Scale.Controls.Add(this.MinScaleZ);
            this.Scale.Controls.Add(this.Scale_Z);
            this.Scale.Controls.Add(this.ScaleYValue);
            this.Scale.Controls.Add(this.MaxScaleY);
            this.Scale.Controls.Add(this.ScaleY);
            this.Scale.Controls.Add(this.MinScaleY);
            this.Scale.Controls.Add(this.Scale_Y);
            this.Scale.Controls.Add(this.ScaleXValue);
            this.Scale.Controls.Add(this.MaxScaleX);
            this.Scale.Controls.Add(this.ScaleX);
            this.Scale.Controls.Add(this.MinScaleX);
            this.Scale.Controls.Add(this.Scale_X);
            this.Scale.Location = new System.Drawing.Point(21, 48);
            this.Scale.Name = "Scale";
            this.Scale.Size = new System.Drawing.Size(277, 194);
            this.Scale.TabIndex = 0;
            this.Scale.TabStop = false;
            this.Scale.Text = "Scale";
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
            this.Rotation.Controls.Add(this.RotationZ);
            this.Rotation.Controls.Add(this.Rotation_Z);
            this.Rotation.Controls.Add(this.RotationY);
            this.Rotation.Controls.Add(this.Rotation_Y);
            this.Rotation.Controls.Add(this.RotationX);
            this.Rotation.Controls.Add(this.Rotation_X);
            this.Rotation.Location = new System.Drawing.Point(304, 48);
            this.Rotation.Name = "Rotation";
            this.Rotation.Size = new System.Drawing.Size(200, 194);
            this.Rotation.TabIndex = 1;
            this.Rotation.TabStop = false;
            this.Rotation.Text = "Rotation";
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
            this.Position.Location = new System.Drawing.Point(510, 48);
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
            // Focus
            // 
            this.Focus.Location = new System.Drawing.Point(510, 138);
            this.Focus.Name = "Focus";
            this.Focus.Size = new System.Drawing.Size(75, 23);
            this.Focus.TabIndex = 3;
            this.Focus.Text = "Focus";
            this.Focus.UseVisualStyleBackColor = true;
            this.Focus.Click += new System.EventHandler(this.Focus_Click);
            // 
            // Model_Name
            // 
            this.Model_Name.AutoSize = true;
            this.Model_Name.Location = new System.Drawing.Point(21, 15);
            this.Model_Name.Name = "Model_Name";
            this.Model_Name.Size = new System.Drawing.Size(67, 13);
            this.Model_Name.TabIndex = 4;
            this.Model_Name.Text = "Model Name";
            // 
            // ModelName
            // 
            this.ModelName.Location = new System.Drawing.Point(94, 12);
            this.ModelName.Name = "ModelName";
            this.ModelName.Size = new System.Drawing.Size(204, 20);
            this.ModelName.TabIndex = 5;
            this.ModelName.TextChanged += new System.EventHandler(this.ModelName_TextChanged);
            // 
            // ModelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 264);
            this.Controls.Add(this.ModelName);
            this.Controls.Add(this.Model_Name);
            this.Controls.Add(this.Focus);
            this.Controls.Add(this.Position);
            this.Controls.Add(this.Rotation);
            this.Controls.Add(this.Scale);
            this.Name = "ModelEditor";
            this.Text = "ModelEditor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Scale.ResumeLayout(false);
            this.Scale.PerformLayout();
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox Scale;
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
        private System.Windows.Forms.Button Focus;
        private System.Windows.Forms.Label Model_Name;
        private System.Windows.Forms.TextBox ModelName;
    }
}