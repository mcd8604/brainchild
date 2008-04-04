using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Project_blob;

namespace WorldMaker
{
    public partial class ModelEditor : Form
    {

        const bool DEBUG = false;

        public delegate void Callback();

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
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
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
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
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
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
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
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
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
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
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
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
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
                setScale();
                try
                {

                    ScaleZ.Value = Convert.ToInt32(ScaleZValue.Text);
                    ScaleZValue.ForeColor = Color.Black;
                }
                catch (Exception ex)
                {
                    if (DEBUG) { Console.WriteLine(ex); }
                    ScaleZValue.ForeColor = Color.Orange;
                }
            }
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
                ScaleZValue.ForeColor = Color.Red;
            }
        }

        private void ScaleYValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                setScale();
                try
                {
                    ScaleY.Value = Convert.ToInt32(ScaleYValue.Text);
                    ScaleYValue.ForeColor = Color.Black;
                }
                catch (Exception ex)
                {
                    if (DEBUG) { Console.WriteLine(ex); }
                    ScaleYValue.ForeColor = Color.Orange;
                }
            }
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
                ScaleYValue.ForeColor = Color.Red;
            }
        }

        private void ScaleXValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                setScale();
                try
                {
                    ScaleX.Value = Convert.ToInt32(ScaleXValue.Text);
                    ScaleXValue.ForeColor = Color.Black;
                }
                catch (Exception ex)
                {
                    if (DEBUG) { Console.WriteLine(ex); }
                    ScaleXValue.ForeColor = Color.Orange;
                }

            }
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
                ScaleXValue.ForeColor = Color.Red;
            }
        }

        private void setScale()
        {
            if (m_Game.ActiveDrawable is StaticModel)
            {
                ((StaticModel)m_Game.ActiveDrawable).Scale = Matrix.CreateScale(Convert.ToSingle(ScaleXValue.Text), Convert.ToSingle(ScaleYValue.Text), Convert.ToSingle(ScaleZValue.Text));
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
            try
            {
                SetRotation();
            }
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
            }
        }

        private void RotationY_Scroll(object sender, EventArgs e)
        {
            rotation_y = (float)Convert.ToInt32(RotationY.Value.ToString());
            RotationYValue.Text = rotation_y.ToString();
            try
            {
                SetRotation();
            }
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
            }
        }

        private void RotationZ_Scroll(object sender, EventArgs e)
        {
            rotation_z = (float)Convert.ToInt32(RotationZ.Value.ToString());
            RotationZValue.Text = rotation_z.ToString();
            try
            {
                SetRotation();
            }
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
            }
        }
        #endregion

        #region values
        private void RotationXValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SetRotation();
                try
                {
                    RotationX.Value = Convert.ToInt32(RotationXValue.Text);

                    RotationXValue.ForeColor = Color.Black;
                }
                catch (Exception ex)
                {
                    if (DEBUG) { Console.WriteLine(ex); }
                    RotationXValue.ForeColor = Color.Orange;
                }
            }
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
                RotationXValue.ForeColor = Color.Red;
            }
        }

        private void RotationYValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SetRotation();
                try
                {
                    RotationY.Value = Convert.ToInt32(Convert.ToSingle(RotationYValue.Text));
                    RotationYValue.ForeColor = Color.Black;
                }
                catch (Exception ex)
                {
                    if (DEBUG) { Console.WriteLine(ex); }
                    RotationYValue.ForeColor = Color.Orange;
                }
            }
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
                RotationYValue.ForeColor = Color.Red;
            }
        }

        private void RotationZValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SetRotation();
                try
                {
                    RotationZ.Value = Convert.ToInt32(Convert.ToSingle(RotationZValue.Text));
                    RotationZValue.ForeColor = Color.Black;
                }
                catch (Exception ex)
                {
                    if (DEBUG) { Console.WriteLine(ex); }
                    RotationZValue.ForeColor = Color.Orange;
                }
            }
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
                RotationZValue.ForeColor = Color.Red;
            }
        }
        #endregion

        private void SetRotation()
        {
            if (m_Game.ActiveDrawable is StaticModel)
            {
                ((StaticModel)m_Game.ActiveDrawable).Rotation = Matrix.Multiply(Matrix.CreateRotationX(MathHelper.ToRadians(Convert.ToSingle(RotationXValue.Text))), Matrix.Multiply(Matrix.CreateRotationY(MathHelper.ToRadians(Convert.ToSingle(RotationYValue.Text))), Matrix.CreateRotationZ(MathHelper.ToRadians(Convert.ToSingle(RotationZValue.Text)))));
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
                if (m_Game.ActiveDrawable is StaticModel)
                {
                    ((StaticModel)m_Game.ActiveDrawable).Position = Matrix.CreateTranslation((float)Convert.ToSingle(PositionX.Text), (float)Convert.ToSingle(PositionY.Text), (float)Convert.ToSingle(PositionZ.Text));
                }
                PositionX.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
                PositionX.ForeColor = Color.Red;
            }
        }

        private void PositionY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_Game.ActiveDrawable is StaticModel)
                {
                    ((StaticModel)m_Game.ActiveDrawable).Position = Matrix.CreateTranslation((float)Convert.ToSingle(PositionX.Text), (float)Convert.ToSingle(PositionY.Text), (float)Convert.ToSingle(PositionZ.Text));
                }
                PositionY.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
                PositionY.ForeColor = Color.Red;
            }
        }

        private void PositionZ_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_Game.ActiveDrawable is StaticModel)
                {
                    ((StaticModel)m_Game.ActiveDrawable).Position = Matrix.CreateTranslation((float)Convert.ToSingle(PositionX.Text), (float)Convert.ToSingle(PositionY.Text), (float)Convert.ToSingle(PositionZ.Text));
                }
                PositionZ.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                if (DEBUG) { Console.WriteLine(ex); }
                PositionZ.ForeColor = Color.Red;
            }
        }
        #endregion

        private void Focus_Click(object sender, EventArgs e)
        {
            if (m_Game.ActiveDrawable is StaticModel)
            {
                m_Game.focusPoint = ((StaticModel)m_Game.ActiveDrawable).Position.Translation;
            }
        }

        public void UpdateValues()
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new Callback(this.UpdateValues));
            }
            else
            {

                if (m_Game.ActiveDrawable is StaticModel)
                {

                    Vector3 theTranslation;
                    Quaternion theRotation;
                    Vector3 theScale;

                    ((StaticModel)m_Game.ActiveDrawable).Position.Decompose(out theScale, out theRotation, out theTranslation);

                    PositionX.Text = theTranslation.X.ToString();
                    PositionY.Text = theTranslation.Y.ToString();
                    PositionZ.Text = theTranslation.Z.ToString();

                    //((StaticModel)m_Game.ActiveDrawable).Rotation.Decompose(out theScale, out theRotation, out theTranslation);

                    //RotationXValue.Text = MathHelper.ToDegrees(theRotation.X).ToString();
                    //RotationYValue.Text = MathHelper.ToDegrees(theRotation.Y).ToString();
                    //RotationZValue.Text = MathHelper.ToDegrees(theRotation.Z).ToString();

                    RotationXValue.Text = "-";
                    RotationYValue.Text = "-";
                    RotationZValue.Text = "-";

                    ScaleXValue.Text = theScale.X.ToString();
                    ScaleYValue.Text = theScale.Y.ToString();
                    ScaleZValue.Text = theScale.Z.ToString();
                }
            }

        }

        private void ModelName_TextChanged(object sender, EventArgs e)
        {
            throw new Exception("Not Implemented");
        }
    }
}
