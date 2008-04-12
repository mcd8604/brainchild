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
            this.listBox1 = new System.Windows.Forms.ListBox( );
            this.textBox1 = new System.Windows.Forms.TextBox( );
            this.areaLabel = new System.Windows.Forms.Label( );
            this.xPosLabel = new System.Windows.Forms.Label( );
            this.yPosLabel = new System.Windows.Forms.Label( );
            this.zPosLabel = new System.Windows.Forms.Label( );
            this.textBox2 = new System.Windows.Forms.TextBox( );
            this.textBox3 = new System.Windows.Forms.TextBox( );
            this.SuspendLayout( );
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point( 12, 27 );
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size( 168, 121 );
            this.listBox1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point( 71, 163 );
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size( 109, 20 );
            this.textBox1.TabIndex = 1;
            // 
            // areaLabel
            // 
            this.areaLabel.AutoSize = true;
            this.areaLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.areaLabel.Location = new System.Drawing.Point( 12, 8 );
            this.areaLabel.Name = "areaLabel";
            this.areaLabel.Size = new System.Drawing.Size( 53, 16 );
            this.areaLabel.TabIndex = 2;
            this.areaLabel.Text = "Areas:";
            // 
            // xPosLabel
            // 
            this.xPosLabel.AutoSize = true;
            this.xPosLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.xPosLabel.Location = new System.Drawing.Point( 12, 164 );
            this.xPosLabel.Name = "xPosLabel";
            this.xPosLabel.Size = new System.Drawing.Size( 48, 16 );
            this.xPosLabel.TabIndex = 3;
            this.xPosLabel.Text = "XPos:";
            // 
            // yPosLabel
            // 
            this.yPosLabel.AutoSize = true;
            this.yPosLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.yPosLabel.Location = new System.Drawing.Point( 12, 190 );
            this.yPosLabel.Name = "yPosLabel";
            this.yPosLabel.Size = new System.Drawing.Size( 49, 16 );
            this.yPosLabel.TabIndex = 4;
            this.yPosLabel.Text = "YPos:";
            // 
            // zPosLabel
            // 
            this.zPosLabel.AutoSize = true;
            this.zPosLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.zPosLabel.Location = new System.Drawing.Point( 12, 216 );
            this.zPosLabel.Name = "zPosLabel";
            this.zPosLabel.Size = new System.Drawing.Size( 48, 16 );
            this.zPosLabel.TabIndex = 5;
            this.zPosLabel.Text = "ZPos:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point( 71, 189 );
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size( 109, 20 );
            this.textBox2.TabIndex = 6;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point( 71, 215 );
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size( 109, 20 );
            this.textBox3.TabIndex = 7;
            // 
            // TransitionSetter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 192, 273 );
            this.Controls.Add( this.textBox3 );
            this.Controls.Add( this.textBox2 );
            this.Controls.Add( this.zPosLabel );
            this.Controls.Add( this.yPosLabel );
            this.Controls.Add( this.xPosLabel );
            this.Controls.Add( this.areaLabel );
            this.Controls.Add( this.textBox1 );
            this.Controls.Add( this.listBox1 );
            this.Name = "TransitionSetter";
            this.Text = "TransitionSetter";
            this.ResumeLayout( false );
            this.PerformLayout( );

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label areaLabel;
        private System.Windows.Forms.Label xPosLabel;
        private System.Windows.Forms.Label yPosLabel;
        private System.Windows.Forms.Label zPosLabel;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
    }
}