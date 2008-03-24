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
    public partial class ModelEditor : Form
    {
        private Game1 m_Game;
        private Drawable _drawableRef;

        public Drawable DrawableRef
        {
            get { return _drawableRef; }
            set { _drawableRef = value; }
        }

        public ModelEditor(Game1 p_Game)
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
                MinScaleX.ForeColor = Color.Black;
            }
            catch (Exception) {
                MinScaleX.ForeColor = Color.Red;
            }
        }

        private void MinScaleY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ScaleY.Minimum = Convert.ToInt32(MinScaleY.Text);
                MinScaleY.ForeColor = Color.Black;
            }
            catch (Exception)
            {
                MinScaleY.ForeColor = Color.Red;
            }
        }

        private void MinScaleZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ScaleZ.Minimum = Convert.ToInt32(MinScaleZ.Text);
                MinScaleZ.ForeColor = Color.Black;
            }
            catch (Exception)
            {
                MinScaleZ.ForeColor = Color.Red;
            }
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
                MaxScaleX.ForeColor = Color.Black;
            }
            catch (Exception)
            {
                MaxScaleX.ForeColor = Color.Red;
            }
        }

        private void MaxScaleY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ScaleY.Maximum = Convert.ToInt32(MaxScaleY.Text);
                MaxScaleY.ForeColor = Color.Black;
            }
            catch (Exception)
            {
                MaxScaleX.ForeColor = Color.Red;
            }
        }

        private void MaxScaleZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ScaleZ.Maximum = Convert.ToInt32(MaxScaleZ.Text);
                MaxScaleZ.ForeColor = Color.Black;
            }
            catch (Exception)
            {
                MaxScaleZ.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Bars
        /*
         * TrackBars
         */
        private void ScaleX_ValueChanged(object sender, EventArgs e)
        {
            ScaleXValue.Text = ScaleX.Value.ToString();
        }

        private void ScaleY_ValueChanged(object sender, EventArgs e)
        {
            ScaleYValue.Text = ScaleY.Value.ToString();
        }

        private void ScaleZ_ValueChanged(object sender, EventArgs e)
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
            try
            {
                ScaleZ.Value = Convert.ToInt32(ScaleZValue.Text);
                ScaleZValue.ForeColor = Color.Black;
                setScale();
            }
            catch (Exception)
            {
                ScaleZValue.ForeColor = Color.Red;
            }
        }

        private void ScaleYValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ScaleY.Value = Convert.ToInt32(ScaleYValue.Text);
                ScaleYValue.ForeColor = Color.Black;
                setScale();
            }
            catch (Exception)
            {
                ScaleYValue.ForeColor = Color.Red;
            }
        }

        private void ScaleXValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ScaleX.Value = Convert.ToInt32(ScaleXValue.Text);
                ScaleXValue.ForeColor = Color.Black;
                setScale();
            }
            catch (Exception)
            {
                ScaleXValue.ForeColor = Color.Red;
            }
        }

        private void setScale()
        {
            if (m_Game.ActiveDrawable is DrawableModel)
            {
                
                ((DrawableModel)m_Game.ActiveDrawable).Scale = Matrix.CreateScale((float)Convert.ToInt32(ScaleXValue.Text), (float)Convert.ToInt32(ScaleYValue.Text), (float)Convert.ToInt32(ScaleZValue.Text));
            }
        }

        #endregion
        #endregion

        #region Rotations
        /*
         * Rotation
         */
        private float rotation_x, rotation_y, rotation_z;

        #region scroll
        private void RotationX_Scroll(object sender, EventArgs e)
        {
            rotation_x = (float)Convert.ToInt32(RotationX.Value.ToString());
            RotationXValue.Text = rotation_x.ToString();
            SetRotation();
            //m_Game.model.Rotation = Matrix.Multiply(m_Game.model.Rotation,Matrix.CreateRotationX(MathHelper.ToRadians((float)Convert.ToInt32(RotationX.Value.ToString()))));
        }

        private void RotationY_Scroll(object sender, EventArgs e)
        {
            rotation_y = (float)Convert.ToInt32(RotationY.Value.ToString());
            RotationYValue.Text = rotation_y.ToString();
            SetRotation();
            //m_Game.model.Rotation = Matrix.Multiply(m_Game.model.Rotation,Matrix.CreateRotationY(MathHelper.ToRadians((float)Convert.ToInt32(RotationY.Value.ToString()))));
        }

        private void RotationZ_Scroll(object sender, EventArgs e)
        {
            rotation_z = (float)Convert.ToInt32(RotationZ.Value.ToString());
            RotationZValue.Text = rotation_z.ToString();
            SetRotation();
            //m_Game.model.Rotation = Matrix.Multiply(m_Game.model.Rotation,Matrix.CreateRotationZ(MathHelper.ToRadians((float)Convert.ToInt32(RotationZ.Value.ToString()))));
        }
        #endregion

        #region values
        private void RotationXValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                rotation_x = (float)Convert.ToDouble(RotationXValue.Text);
                RotationX.Value = (int)rotation_x;
                SetRotation();
            }
            catch (Exception) { }
        }

        private void RotationYValue_TextChanged(object sender, EventArgs e)
        {
             try
            {
                rotation_y = (float)Convert.ToDouble(RotationYValue.Text);
                RotationY.Value = (int)rotation_y;
                SetRotation();
            }
            catch (Exception) { }
        }

        private void RotationZValue_TextChanged(object sender, EventArgs e)
        {
           try
            {
                rotation_z = (float)Convert.ToDouble(RotationZValue.Text);
                RotationZ.Value = (int)rotation_z;
                SetRotation();
            }
            catch (Exception) { }
        }
        #endregion

        private void SetRotation()
        {
            if (m_Game.ActiveDrawable is DrawableModel)
            {
                ((DrawableModel)m_Game.ActiveDrawable).RotationPriority = 0;
                ((DrawableModel)m_Game.ActiveDrawable).TranslationPriority = 1;
                ((DrawableModel)m_Game.ActiveDrawable).Rotation = Matrix.Multiply(Matrix.CreateRotationX(MathHelper.ToRadians(rotation_x)), Matrix.Multiply(Matrix.CreateRotationY(MathHelper.ToRadians(rotation_y)), Matrix.CreateRotationZ(MathHelper.ToRadians(rotation_z))));
            }
        }
        #endregion

        #region Positions
        /*
         * Position
         */
        private void PositionX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_Game.ActiveDrawable is DrawableModel)
                {
                    ((DrawableModel)m_Game.ActiveDrawable).Position = Matrix.CreateTranslation((float)Convert.ToInt32(PositionX.Text), (float)Convert.ToInt32(PositionY.Text), (float)Convert.ToInt32(PositionZ.Text));
                }
            }
            catch (Exception) { }
        }

        private void PositionY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_Game.ActiveDrawable is DrawableModel)
                {
                    ((DrawableModel)m_Game.ActiveDrawable).Position = Matrix.CreateTranslation((float)Convert.ToInt32(PositionX.Text), (float)Convert.ToInt32(PositionY.Text), (float)Convert.ToInt32(PositionZ.Text));
                }
            }
            catch (Exception) { }
        }

        private void PositionZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_Game.ActiveDrawable is DrawableModel)
                {
                    ((DrawableModel)m_Game.ActiveDrawable).Position = Matrix.CreateTranslation((float)Convert.ToInt32(PositionX.Text), (float)Convert.ToInt32(PositionY.Text), (float)Convert.ToInt32(PositionZ.Text));
                }
            }
            catch (Exception) { }
        }
        #endregion

        private void Focus_Click(object sender, EventArgs e)
        {
            if (m_Game.ActiveDrawable is DrawableModel)
            {
                m_Game.focusPoint = ((DrawableModel)m_Game.ActiveDrawable).Position.Translation;
            }
        }

        private void ModelName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}