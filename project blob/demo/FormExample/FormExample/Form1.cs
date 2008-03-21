using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                trackBar1.Minimum = Convert.ToInt32(textBox1.Text);
            }
            catch (Exception ex) { }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                trackBar1.Maximum = Convert.ToInt32(textBox2.Text);
            }
            catch (Exception ex) { }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox3.Text = trackBar1.Value.ToString();
        }
    }
}