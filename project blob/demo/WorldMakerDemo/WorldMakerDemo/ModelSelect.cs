using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WorldMakerDemo
{
    public partial class ModelSelect : Form
    {
        DrawableModel m_CurrentSelection;
        public DrawableModel CurrentSelection
        {
            get
            {
                return m_CurrentSelection;
            }
            set
            {
                m_CurrentSelection = value;
            }
        }

        LevelEditor levelEditor;
        Game1 _gameRef;

        public ModelSelect(LevelEditor p_LE, string[] models, Game1 game)
        {
            InitializeComponent();
            _gameRef = game;
            
            levelEditor = p_LE;
            m_CurrentSelection = new DrawableModel("none");

            for( int i = 0; i < models.Length; i++)
                listBox1.Items.Add(models[i]);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_CurrentSelection.ModelObject = _gameRef.Content.Load<Model>(@"Models\\" + ((String)(listBox1.Items[listBox1.SelectedIndex])).Substring(0, ((String)(listBox1.Items[listBox1.SelectedIndex])).LastIndexOf(".")));
            m_CurrentSelection.setGraphicsDevice(_gameRef.GraphicsDevice);
            m_CurrentSelection.Name = (String)(listBox1.Items[listBox1.SelectedIndex]);

        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}