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

        /*TextureInfo m_CurrentTexture;
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
        }*/

        private String originalModel;
        private String originalTextureName;
        //private int originalTextureSort;

        LevelEditor levelEditor;
        Game1 _gameRef;

        public ModelSelect(LevelEditor p_LE, string[] models, string[] textures, string[] audio, Game1 game, bool editMode)
        {
            InitializeComponent();
            _gameRef = game;

            levelEditor = p_LE;

            for (int i = 0; i < models.Length; i++)
                modelBox.Items.Add(models[i]);

            for (int i = 0; i < textures.Length; i++)
                textureBox.Items.Add(textures[i]);

            for (int i = 0; i < audio.Length; i++)
                audioBox.Items.Add(audio[i]);

            System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom("Project_blob.exe");
            foreach (Type t in asm.GetTypes())
            {
                if (typeof(Project_blob.StaticModel).IsAssignableFrom(t))
                {
                    ModelType.Items.Add(t);
                }
            }

            if (editMode && _gameRef.ActiveDrawable is StaticModel)
            {
                m_CurrentModel = (StaticModel)_gameRef.ActiveDrawable;
                
                if (m_CurrentModel.AudioName != null && !m_CurrentModel.AudioName.Equals("none"))
                {
                    audioBox.SelectedItem = m_CurrentModel.AudioName + ".xnb";
                }
            }
            else
            {
                //string modelName = ((string)(modelBox.Items[0])).Substring(0, ((String)(modelBox.Items[0])).LastIndexOf("."));
                string modelName = (string)modelBox.Items[0];
                //string textureName = ((string)(textureBox.Items[0])).Substring(0, ((String)(textureBox.Items[0])).LastIndexOf("."));
                string textureName = (string)textureBox.Items[0];
                m_CurrentModel = new StaticModel("", modelName, "none", textureName, new List<short>());
                m_CurrentModel.initialize();
            }

            originalModel = m_CurrentModel.ModelName;

            /*if (m_CurrentModel.TextureKey == null)
            {
                m_CurrentTexture = new TextureInfo();
                m_CurrentModel.TextureKey = m_CurrentTexture;
            }
            else
            {
                m_CurrentTexture = m_CurrentModel.TextureKey;
            }*/

            originalTextureName = m_CurrentModel.TextureName;
            //originalTextureSort = m_CurrentTexture.SortNumber;

            modelBox.SelectedItem = m_CurrentModel.ModelName;// +".xnb";
            textureBox.SelectedItem = m_CurrentModel.TextureName;// +".xnb";
            ModelType.SelectedItem = m_CurrentModel.GetType();

            ModelName.Text = m_CurrentModel.Name;
        }

        private void modelBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modelBox.SelectedIndex != -1)
            {
                //m_CurrentModel.ModelName = ((String)(modelBox.Items[modelBox.SelectedIndex])).Substring(0, ((String)(modelBox.Items[modelBox.SelectedIndex])).LastIndexOf("."));
                m_CurrentModel.ModelName = (String)modelBox.SelectedItem;
                m_CurrentModel.initialize();
            }

        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (!m_CurrentModel.ModelName.Equals("none") && !m_CurrentModel.TextureName.Equals("none") && !m_CurrentModel.Name.Equals(""))
            {
                //Console.WriteLine(m_CurrentModel.TextureName);
                /*if (m_CurrentModel.TextureName.Equals("event"))
                {
                    _events = new EventSelector();
                    _events.ShowDialog();
                }
                if (!m_CurrentModel.TextureName.Equals("event") || _events.DialogResult != DialogResult.Cancel)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close( );
                }*/
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            m_CurrentModel.ModelName = originalModel;
            //m_CurrentModel.TextureKey.TextureName = originalTextureName;
            //m_CurrentModel.TextureKey.SortNumber = originalTextureSort;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textureBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textureBox.SelectedIndex != -1)
            {
                //m_CurrentModel.TextureName = ((String)(textureBox.Items[textureBox.SelectedIndex])).Substring(0, ((String)(textureBox.Items[textureBox.SelectedIndex])).LastIndexOf("."));
                m_CurrentModel.TextureName = (String)textureBox.SelectedItem;
                //m_CurrentModel.TextureKey = m_CurrentTexture;
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

        private void ModelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Type newType = ((Type)ModelType.SelectedItem);
            if (!m_CurrentModel.GetType().Equals(newType))
            {
                Type[] types = { typeof(StaticModel) };
                Object[] parameters = { m_CurrentModel };
                m_CurrentModel = newType.GetConstructor(types).Invoke(parameters) as StaticModel;
            }
        }

		private void PropertyButton_Click(object sender, EventArgs e)
		{
			PropertyEditor propEditor = new PropertyEditor(m_CurrentModel, false);
			propEditor.ShowDialog();
		}
    }
}
