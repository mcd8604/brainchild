using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Project_blob;

namespace WorldMaker
{
    public partial class LevelEditor : Form
    {
        public delegate void Callback();

        private DrawableInfo _drawableInfo;
        private EventInfo _eventInfo;
        private Game1 _gameRef;
        private ModelSelect _modelSelect;
        private LevelSelect _levelSelect;
        private YesNo _deleteChecker;
        private static List<EventInfo> _eventsToAdd = new List<EventInfo>();
        private static List<String> _drawablesToDelete = new List<String>();
        private static List<DrawableInfo> _drawablesToAdd = new List<DrawableInfo>();

        public static List<EventInfo> EventsToAdd {
            get { return _eventsToAdd; }
        }

        public static List<String> DrawablesToDelete
        {
            get { return _drawablesToDelete; }
        }

        public static List<DrawableInfo> DrawablesToAdd
        {
            get { return _drawablesToAdd; }
        }

        public LevelEditor(Game1 game)
        {
            InitializeComponent();

            _gameRef = game;

        }

        private void areaListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (areaListBox.SelectedIndex != -1)
            {
                areaTextBox.Text = (String)areaListBox.Items[areaListBox.SelectedIndex];
                _gameRef.ActiveArea = Level.Areas[(String)areaListBox.Items[areaListBox.SelectedIndex]];
                modelListBox.Items.Clear();
                foreach (String str in _gameRef.ActiveArea.Drawables.Keys)
                {
                    modelListBox.Items.Add(str);
                    Console.WriteLine(str + " loaded");
                }
                modelListBox.Update();
            }
        }

        private void modelListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modelListBox.SelectedIndex != -1)
            {
                _gameRef.ActiveDrawable = _gameRef.ActiveArea.Drawables[(String)(modelListBox.Items[modelListBox.SelectedIndex])];
                if (_gameRef.ActiveDrawable is StaticModel)
                    _gameRef.ActiveArea.Display.CurrentlySelected = ((StaticModel)_gameRef.ActiveDrawable).Name;
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            string levelDir = System.Environment.CurrentDirectory + "\\Content\\Levels";
            if (!System.IO.Directory.Exists(levelDir))
            {
                System.IO.Directory.CreateDirectory(levelDir);
            }
            string[] levels = System.IO.Directory.GetFiles(levelDir);
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i] = levels[i].Substring(levels[i].LastIndexOf("\\") + 1);
            }
            _levelSelect = new LevelSelect(levels);
            _levelSelect.ShowDialog();
            if (_levelSelect.DialogResult.Equals(DialogResult.OK))
            {
                foreach (String str in modelListBox.Items)
                {
                    DrawablesToDelete.Add(str);
                }
                modelListBox.Items.Clear();
                modelListBox.Update();
                levelName.Text = _levelSelect.LevelName.Substring(0, _levelSelect.LevelName.LastIndexOf("."));
                Level.LoadLevel(levelName.Text, _gameRef.EffectName);
                areaListBox.Items.Clear();
                foreach (String str in Level.Areas.Keys)
                {
                    areaListBox.Items.Add(str);
                    Console.WriteLine(str + " loaded");
                }
                areaListBox.Update();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (!levelName.Text.Equals(""))
            {
                Level.SaveLevel(levelName.Text);
            }
        }

        private void modelAddButton_Click(object sender, EventArgs e)
        {
            if (areaListBox.SelectedIndex != -1)
            {
                string[] models = System.IO.Directory.GetFiles(System.Environment.CurrentDirectory + "\\Content\\Models");
                for (int i = 0; i < models.Length; i++)
                    models[i] = models[i].Substring(models[i].LastIndexOf("\\") + 1);

                string[] textures = System.IO.Directory.GetFiles(System.Environment.CurrentDirectory + "\\Content\\Textures");
                for (int i = 0; i < textures.Length; i++)
                    textures[i] = textures[i].Substring(textures[i].LastIndexOf("\\") + 1);

                string[] audio = System.IO.Directory.GetFiles(System.Environment.CurrentDirectory + "\\Content\\Audio");
                for (int i = 0; i < audio.Length; i++)
                    audio[i] = audio[i].Substring(audio[i].LastIndexOf("\\") + 1);

                _modelSelect = new ModelSelect(this, models, textures, audio, _gameRef);
                _modelSelect.ShowDialog();
                if (_modelSelect.DialogResult == DialogResult.OK && !_modelSelect.CurrentModel.ModelName.Equals("") && TextureManager.getSingleton.GetTexture(_modelSelect.CurrentTexture.TextureName) != null)
                {
                    Console.WriteLine(_modelSelect.CurrentModel.Name);
                    _drawableInfo.name = _modelSelect.CurrentModel.Name;
                    _drawableInfo.textureInfo = _modelSelect.CurrentTexture;
                    _drawableInfo.drawable = _modelSelect.CurrentModel;
                    _drawablesToAdd.Add(_drawableInfo);
                    if(_modelSelect.Event != null) {
                        _eventInfo.eventTrigger = _modelSelect.Event;
                        _eventInfo.name = _modelSelect.CurrentModel.Name;
                        _eventsToAdd.Add(_eventInfo);
                    } 
                    modelListBox.Items.Add(_modelSelect.CurrentModel.Name);
                    modelListBox.Update();
                }
            }
        }

        private void modelDelButton_Click(object sender, EventArgs e)
        {
            if (modelListBox.SelectedIndex != -1)
            {
                _deleteChecker = new YesNo();
                _deleteChecker.ShowDialog();
                if (_deleteChecker.DialogResult == DialogResult.Yes)
                {
                    _drawablesToDelete.Add((String)modelListBox.Items[modelListBox.SelectedIndex]);
                    modelListBox.Items.RemoveAt(modelListBox.SelectedIndex);
                    modelListBox.Update();
                }
            }
        }

        private void areaAddButton_Click(object sender, EventArgs e) {
            if(!areaTextBox.Text.Equals("")) {
                if(!Level.Areas.ContainsKey(areaTextBox.Text)) {
                    Area tempArea;
                    if(_gameRef.EFFECT_TYPE.Equals("basic")) {
                        tempArea = new Area(_gameRef.WorldMatrix, _gameRef.ViewMatrix, _gameRef.ProjectionMatrix);
                    } else if(_gameRef.EFFECT_TYPE.Equals("effects")) {
                        tempArea = new Area(_gameRef.WorldMatrix, _gameRef.EffectName, "xWorld", "xTexture", "Textured");
                    } else if(_gameRef.EFFECT_TYPE.Equals("Cel")) {
                        tempArea = new Area(_gameRef.WorldMatrix, _gameRef.EffectName, "World", "NONE", null);
                    } else {
                        tempArea = new Area(_gameRef.WorldMatrix, _gameRef.ViewMatrix, _gameRef.ProjectionMatrix);
                    }
                    Level.AddArea(areaTextBox.Text, tempArea);
                    areaListBox.Items.Add(areaTextBox.Text);
                    areaListBox.Update();
                }
            }
        }

        private void areaDelButton_Click(object sender, EventArgs e)
        {
            if (areaListBox.SelectedIndex != -1)
            {
                _deleteChecker = new YesNo();
                _deleteChecker.ShowDialog();
                if (_deleteChecker.DialogResult == DialogResult.Yes)
                {
                    Level.RemoveArea((String)areaListBox.Items[areaListBox.SelectedIndex]);
                    areaListBox.Items.RemoveAt(areaListBox.SelectedIndex);
                    areaListBox.Update();
                    foreach (String str in modelListBox.Items)
                    {
                        DrawablesToDelete.Add(str);
                    }
                    modelListBox.Items.Clear();
                }
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (modelListBox.SelectedIndex != -1)
            {
                StaticModel current = (StaticModel)(_gameRef.ActiveArea.GetDrawable(_gameRef.ActiveArea.Display.CurrentlySelected));

                string[] models = System.IO.Directory.GetFiles(System.Environment.CurrentDirectory + "\\Content\\Models");
                for (int i = 0; i < models.Length; i++)
                    models[i] = models[i].Substring(models[i].LastIndexOf("\\") + 1);

                string[] textures = System.IO.Directory.GetFiles(System.Environment.CurrentDirectory + "\\Content\\Textures");
                for (int i = 0; i < textures.Length; i++)
                    textures[i] = textures[i].Substring(textures[i].LastIndexOf("\\") + 1);

                string[] audio = System.IO.Directory.GetFiles(System.Environment.CurrentDirectory + "\\Content\\Audio");
                for (int i = 0; i < audio.Length; i++)
                    audio[i] = audio[i].Substring(audio[i].LastIndexOf("\\") + 1);

                _modelSelect = new ModelSelect(this, models, textures, audio, _gameRef);
                _modelSelect.ShowDialog();
                if (_modelSelect.DialogResult == DialogResult.OK && !_modelSelect.CurrentModel.ModelName.Equals("") && TextureManager.getSingleton.GetTexture(_modelSelect.CurrentTexture.TextureName) != null)
                {
                    _drawablesToDelete.Add(_gameRef.ActiveArea.Display.CurrentlySelected);
                    Console.WriteLine(_modelSelect.CurrentModel.Name);
                    _drawableInfo.name = _modelSelect.CurrentModel.Name;
                    _drawableInfo.textureInfo = _modelSelect.CurrentTexture;
                    StaticModel temp = _modelSelect.CurrentModel;
                    temp.Rotation = current.Rotation;
                    temp.Position = current.Position;
                    temp.Scale = current.Scale;
                    _drawableInfo.drawable = temp;
                    _drawablesToAdd.Add(_drawableInfo);
                    if(_modelSelect.Event != null) {
                        _eventInfo.eventTrigger = _modelSelect.Event;
                        _eventInfo.name = _modelSelect.CurrentModel.Name;
                        _eventsToAdd.Add(_eventInfo);
                    } 
                    modelListBox.Items.Add(_modelSelect.CurrentModel.Name);
                    modelListBox.Items.RemoveAt(modelListBox.SelectedIndex);
                    modelListBox.Update();
                }


            }
        }

        private void nameButton_Click(object sender, EventArgs e)
        {
            if (areaListBox.SelectedIndex != -1 && !areaTextBox.Text.Equals("") && !areaTextBox.Text.Equals((String)areaListBox.Items[areaListBox.SelectedIndex]))
            {
                Level.Areas.Add(areaTextBox.Text, Level.Areas[(String)areaListBox.Items[areaListBox.SelectedIndex]]);
                Level.RemoveArea((String)areaListBox.Items[areaListBox.SelectedIndex]);
                areaListBox.Items[areaListBox.SelectedIndex] = areaTextBox.Text;
            }
        }

    }
}