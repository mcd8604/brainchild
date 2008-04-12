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
using Project_blob;

namespace WorldMaker
{
    public partial class ModelSelect : Form
    {
        private EventSelector _events;

        public EventTrigger Event {
            get {
                if(_events != null) {
                    return _events.EventTrigger;
                } else {
                    return null;
                }
            }
        }

        StaticModel m_CurrentModel;
        public StaticModel CurrentModel
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

        public ModelSelect(LevelEditor p_LE, string[] models, string[] textures, string[] audio, Game1 game)
        {
            InitializeComponent();
            _gameRef = game;

            levelEditor = p_LE;
            m_CurrentModel = new StaticModel("none", "none", "none");
            Random rand = new Random();
            m_CurrentTexture = new TextureInfo(null, rand.Next());
            m_CurrentModel.TextureKey = m_CurrentTexture;
            for (int i = 0; i < models.Length; i++)
                modelBox.Items.Add(models[i]);

            for (int i = 0; i < textures.Length; i++)
                textureBox.Items.Add(textures[i]);

            for (int i = 0; i < audio.Length; i++)
                audioBox.Items.Add(audio[i]);
        }

        private void modelBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modelBox.SelectedIndex != -1)
            {
                m_CurrentModel.ModelName = ((String)(modelBox.Items[modelBox.SelectedIndex])).Substring(0, ((String)(modelBox.Items[modelBox.SelectedIndex])).LastIndexOf("."));
            }

        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (!m_CurrentModel.ModelName.Equals("none") && !m_CurrentTexture.TextureName.Equals("none") && !m_CurrentModel.Name.Equals(""))
            {
                Console.WriteLine(m_CurrentTexture.TextureName);
                if (m_CurrentTexture.TextureName.Equals("event"))
                {
                    _events = new EventSelector();
                    _events.ShowDialog();
                }
                if( !m_CurrentTexture.TextureName.Equals("event") || _events.DialogResult != DialogResult.Cancel ) {
                    this.DialogResult = DialogResult.OK;
                    this.Close( );
                }
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textureBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textureBox.SelectedIndex != -1)
            {
                m_CurrentTexture.TextureName = ((String)(textureBox.Items[textureBox.SelectedIndex])).Substring(0, ((String)(textureBox.Items[textureBox.SelectedIndex])).LastIndexOf("."));
            }
        }

        private void ModelName_TextChanged(object sender, EventArgs e)
        {
            m_CurrentModel.Name = ModelName.Text;
            if(_gameRef.ActiveArea.Drawables.ContainsKey(m_CurrentModel.ModelName)) {
                m_CurrentModel.ModelName = "";
            }
        }

        private void audioBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (audioBox.SelectedIndex != -1)
            {
                m_CurrentModel.AudioName = ((String)(audioBox.Items[audioBox.SelectedIndex])).Substring(0, ((String)(audioBox.Items[audioBox.SelectedIndex])).LastIndexOf("."));
            }
        }
    }
}