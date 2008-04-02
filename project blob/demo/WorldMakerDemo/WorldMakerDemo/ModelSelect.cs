using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WorldMakerDemo
{
    public partial class ModelSelect : Form
    {
        DrawableModel m_CurrentModel;
        public DrawableModel CurrentModel
        {
            get
            {
                return m_CurrentModel;
            }
            set
            {
                m_CurrentModel = value;
            }
        }

        TextureInfo m_CurrentTexture;
        public TextureInfo CurrentTexture
        {
            get
            {
                return m_CurrentTexture;
            }
            set
            {
                m_CurrentTexture = value;
            }
        }

        LevelEditor levelEditor;
        Game1 _gameRef;

        public ModelSelect(LevelEditor p_LE, string[] models, string[] textures, Game1 game)
        {
            InitializeComponent();
            _gameRef = game;

            levelEditor = p_LE;
            m_CurrentModel = new DrawableModel("none", "none");
            Random rand = new Random();
            m_CurrentTexture = new TextureInfo(null, rand.Next());

            for (int i = 0; i < models.Length; i++)
                listBox1.Items.Add(models[i]);

            for (int i = 0; i < textures.Length; i++)
                listBox2.Items.Add(textures[i]);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                m_CurrentModel.ModelName = ((String)(listBox1.Items[listBox1.SelectedIndex])).Substring(0, ((String)(listBox1.Items[listBox1.SelectedIndex])).LastIndexOf("."));
            }

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

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                m_CurrentTexture.TextureName = ((String)(listBox2.Items[listBox2.SelectedIndex])).Substring(0, ((String)(listBox2.Items[listBox2.SelectedIndex])).LastIndexOf("."));
            }
        }

        private void ModelName_TextChanged(object sender, EventArgs e)
        {
            m_CurrentModel.Name = ModelName.Text;
        }
    }
}