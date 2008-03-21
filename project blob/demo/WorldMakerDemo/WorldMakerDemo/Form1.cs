using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace WorldMakerDemo
{
    public partial class Form1 : Form
    {
        private Game1 m_Game;
        
        public Form1(Game1 p_Game)
        {
            InitializeComponent();

            m_Game = p_Game;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

#region Scale
        /*
         * Scale
         */

        #region Mins
        /*
         * minumum TrackBar value
         */
        private void MinScaleX_TextChanged(object sender, EventArgs e)
        {
            try
            {
              ScaleX.Minimum = Convert.ToInt32(MinScaleX.Text);
            }
            catch (Exception ex){}
        }

        private void MinScaleY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ScaleY.Minimum = Convert.ToInt32(MinScaleY.Text);
            }
            catch (Exception ex) { }
        }

        private void MinScaleZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ScaleZ.Minimum = Convert.ToInt32(MinScaleZ.Text);
            }
            catch (Exception ex) { }
        }
        #endregion

        #region Maxs
        /*
         * Maximum TrackBar values
         */
        private void MaxScaleX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ScaleX.Maximum = Convert.ToInt32(MaxScaleX.Text);
            }
            catch (Exception ex) { }
        }

        private void MaxScaleY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ScaleY.Maximum = Convert.ToInt32(MaxScaleY.Text);
            }
            catch (Exception ex) { }
        }

        private void MaxScaleZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ScaleZ.Maximum = Convert.ToInt32(MaxScaleZ.Text);
            }
            catch (Exception ex) { }
        }
        #endregion

        #region Bars
        /*
         * TrackBars
         */
        private void ScaleX_Scroll(object sender, EventArgs e)
        {
            ScaleXValue.Text = ScaleX.Value.ToString();
        }

        private void ScaleY_Scroll(object sender, EventArgs e)
        {
            ScaleYValue.Text = ScaleY.Value.ToString();
        }

        private void ScaleZ_Scroll(object sender, EventArgs e)
        {
            ScaleZValue.Text = ScaleZ.Value.ToString();
        }
        #endregion

        #region Values
        /*
         * Values
         */
        private void ScaleZValue_TextChanged(object sender, EventArgs e)
        {
            m_Game.model.Scale = Matrix.CreateScale((float)Convert.ToInt32(ScaleXValue.Text), (float)Convert.ToInt32(ScaleYValue.Text), (float)Convert.ToInt32(ScaleZValue.Text));
        }

        private void ScaleYValue_TextChanged(object sender, EventArgs e)
        {
            m_Game.model.Scale = Matrix.CreateScale((float)Convert.ToInt32(ScaleXValue.Text), (float)Convert.ToInt32(ScaleYValue.Text), (float)Convert.ToInt32(ScaleZValue.Text));
        }

        private void ScaleXValue_TextChanged(object sender, EventArgs e)
        {
            m_Game.model.Scale = Matrix.CreateScale((float)Convert.ToInt32(ScaleXValue.Text), (float)Convert.ToInt32(ScaleYValue.Text), (float)Convert.ToInt32(ScaleZValue.Text));
        }
        #endregion
#endregion

#region Rotations
        /*
         * Rotation
         */
        private void RotationX_Scroll(object sender, EventArgs e)
        {
            m_Game.model.Rotation = Matrix.CreateRotationX(MathHelper.ToRadians((float)Convert.ToInt32(RotationX.Value.ToString())));
        }

        private void RotationY_Scroll(object sender, EventArgs e)
        {
            m_Game.model.Rotation = Matrix.CreateRotationY(MathHelper.ToRadians((float)Convert.ToInt32(RotationY.Value.ToString())));
        }

        private void RotationZ_Scroll(object sender, EventArgs e)
        {
            m_Game.model.Rotation = Matrix.CreateRotationZ(MathHelper.ToRadians((float)Convert.ToInt32(RotationZ.Value.ToString())));
        }
#endregion

#region Positions
        /*
         * Position
         */
        private void PositionX_TextChanged(object sender, EventArgs e)
        {
            m_Game.model.Position=Matrix.CreateTranslation((float)Convert.ToInt32(PositionX.Text), (float)Convert.ToInt32(PositionY.Text), (float)Convert.ToInt32(PositionZ.Text));
        }

        private void PositionY_TextChanged(object sender, EventArgs e)
        {
            m_Game.model.Position=Matrix.CreateTranslation((float)Convert.ToInt32(PositionX.Text), (float)Convert.ToInt32(PositionY.Text), (float)Convert.ToInt32(PositionZ.Text));
        }

        private void PositionZ_TextChanged(object sender, EventArgs e)
        {
            m_Game.model.Position=Matrix.CreateTranslation((float)Convert.ToInt32(PositionX.Text), (float)Convert.ToInt32(PositionY.Text), (float)Convert.ToInt32(PositionZ.Text));
        }
#endregion

        private void Focus_Click(object sender, EventArgs e)
        {
            m_Game.focusPoint = m_Game.model.Position.Translation;
        }

        private void ModelName_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}