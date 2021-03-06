using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Project_blob;
using Microsoft.Xna.Framework.Graphics;
using Audio;
using Physics2;
using Microsoft.Xna.Framework;

namespace WorldMaker
{
	public partial class LevelEditor : Form
	{
		public delegate void Callback();

		private DrawableInfo _drawableInfo;
		//private EventInfo _eventInfo;
		private Game1 _gameRef;
		private ModelSelect _modelSelect;
		private LevelSelect _levelSelect;
		private YesNo _deleteChecker;
		private static List<EventInfo> _eventsToAdd = new List<EventInfo>();
		private static List<string> _drawablesToDelete = new List<string>();
		private static List<DrawableInfo> _drawablesToAdd = new List<DrawableInfo>();

		/*public static List<EventInfo> EventsToAdd
		{
			get { return _eventsToAdd; }
		}*/

		public static List<string> DrawablesToDelete
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
				_gameRef.nextArea = Level.Areas[(string)areaListBox.Items[areaListBox.SelectedIndex]];
				EventButton.Enabled = false;
			}
		}

		/*public void CreateRenderTargets()
        {
			_gameRef.draw = false; 

			_gameRef.ActiveArea.Display.Distort = EffectManager.getSingleton.GetEffect("distort");
			_gameRef.ActiveArea.Display.Distorter = EffectManager.getSingleton.GetEffect("distorter");
			_gameRef.ActiveArea.Display.CartoonEffect = EffectManager.getSingleton.GetEffect("cartoonEffect");
			_gameRef.ActiveArea.Display.PostProcessEffect = EffectManager.getSingleton.GetEffect("postprocessEffect");

			PresentationParameters pp = _gameRef.GraphicsDevice.PresentationParameters;

			RenderTarget2D sceneRenderTarget = new RenderTarget2D(_gameRef.GraphicsDevice,
				pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

			RenderTarget2D normalDepthRenderTarget = new RenderTarget2D(_gameRef.GraphicsDevice,
				pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

			RenderTarget2D distortionMap = new RenderTarget2D(_gameRef.GraphicsDevice,
				pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

			ResolveTexture2D tempRenderTarget = new ResolveTexture2D(_gameRef.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat);

			RenderTarget2D depthBufferRenderTarget = new RenderTarget2D(_gameRef.GraphicsDevice,
				pp.BackBufferWidth, pp.BackBufferHeight, 1,
				pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

			_gameRef.ActiveArea.Display.SceneRanderTarget = sceneRenderTarget;
			_gameRef.ActiveArea.Display.NormalDepthRenderTarget = normalDepthRenderTarget;
			_gameRef.ActiveArea.Display.DistortionMap = distortionMap;
			_gameRef.ActiveArea.Display.TempRenderTarget = tempRenderTarget;
			_gameRef.ActiveArea.Display.DepthMapRenderTarget = depthBufferRenderTarget;

			_gameRef.draw = true;

        }*/

		private void modelListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (modelListBox.SelectedIndex != -1)
            {
                _gameRef.ActiveDrawable = _gameRef.ActiveArea.Drawables[(string)(modelListBox.Items[modelListBox.SelectedIndex])];
                if (_gameRef.ActiveDrawable is StaticModel)
                {
                    _gameRef.ActiveArea.Display.CurrentlySelected = ((StaticModel)_gameRef.ActiveDrawable).Name;

                    EventButton.Enabled = true;
                    //if (_gameRef.ActiveArea.Events.ContainsKey(((StaticModel)_gameRef.ActiveDrawable).Name))
                    if(((StaticModel)_gameRef.ActiveDrawable).Event != null)
                    {
                        EventButton.Text = "Edit Event";
                    }
                    else
                    {
                        EventButton.Text = "Add Event";
                    }

                    if (_gameRef.ActiveDrawable is DynamicModel)
                    {
                        EditTasksButton.Enabled = true;
                    }
                    else
                    {
                        EditTasksButton.Enabled = false;
                    }
                }
            }
            else
            {
                EventButton.Enabled = false;
            }
		}

		private void modelListBox_DoubleClick(object sender, EventArgs e)
		{
			if (_gameRef.ActiveDrawable is StaticModel)
			{
				_gameRef.focusPoint = ((StaticModel)_gameRef.ActiveDrawable).Position.Translation;
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
			for (int i = 0; i < levels.Length; ++i)
			{
				levels[i] = levels[i].Substring(levels[i].LastIndexOf("\\") + 1);
			}
			_levelSelect = new LevelSelect(levels);
			_levelSelect.ShowDialog();
			if (_levelSelect.DialogResult.Equals(DialogResult.OK))
			{
				foreach (string str in modelListBox.Items)
				{
					DrawablesToDelete.Add(str);
				}
				modelListBox.Items.Clear();
				modelListBox.Update();
				levelName.Text = _levelSelect.LevelName.Substring(0, _levelSelect.LevelName.LastIndexOf("."));
				Level.LoadLevel(levelName.Text, _gameRef.EffectName);
				areaListBox.Items.Clear();
				foreach (string str in Level.Areas.Keys)
				{
					areaListBox.Items.Add(str);
				}
				areaListBox.Update();
			}
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(levelName.Text))
			{
				Level.SaveLevel(levelName.Text);
			}
		}

		private void modelAddButton_Click(object sender, EventArgs e)
		{
			if (areaListBox.SelectedIndex != -1)
			{
				_modelSelect = new ModelSelect(this, ModelManager.GetModelNames(), TextureManager.GetTextureNames(), Audio.AudioManager.getAudioFilenames().ToArray(), _gameRef, false);
				_modelSelect.ShowDialog();
				if (_modelSelect.DialogResult == DialogResult.OK && !string.IsNullOrEmpty(_modelSelect.CurrentModel.ModelName))
				{
					_drawableInfo.name = _modelSelect.CurrentModel.Name;
					_drawableInfo.textureID = _modelSelect.CurrentModel.GetTextureID();
					_drawableInfo.drawable = _modelSelect.CurrentModel;
					_drawablesToAdd.Add(_drawableInfo);
					/*if (_modelSelect.Event != null)
					{
						_eventInfo.eventTrigger = _modelSelect.Event;
						_eventInfo.name = _modelSelect.CurrentModel.Name;
						_eventsToAdd.Add(_eventInfo);
					}*/
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
					_drawablesToDelete.Add((string)modelListBox.Items[modelListBox.SelectedIndex]);
					modelListBox.Items.RemoveAt(modelListBox.SelectedIndex);
					modelListBox.Update();
				}
			}
		}

		private void areaAddButton_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(areaTextBox.Text))
			{
				if (!Level.Areas.ContainsKey(areaTextBox.Text))
				{
					Area tempArea;
					if (_gameRef.EFFECT_TYPE.Equals("basic"))
					{
						tempArea = new Area(_gameRef.WorldMatrix, _gameRef.ViewMatrix, _gameRef.ProjectionMatrix);
					}
					else if (_gameRef.EFFECT_TYPE.Equals("effects"))
					{
						tempArea = new Area(_gameRef.WorldMatrix, _gameRef.EffectName, "xWorld", "xTexture", "Textured");
					}
					else if (_gameRef.EFFECT_TYPE.Equals("Cel"))
					{
						tempArea = new Area(_gameRef.WorldMatrix, _gameRef.EffectName, "World", "NONE", null);
					}
					else
					{
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
					Level.RemoveArea((string)areaListBox.Items[areaListBox.SelectedIndex]);
					areaListBox.Items.RemoveAt(areaListBox.SelectedIndex);
					areaListBox.Update();
					foreach (string str in modelListBox.Items)
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

				_modelSelect = new ModelSelect(this, ModelManager.GetModelNames(), TextureManager.GetTextureNames(), Audio.AudioManager.getAudioFilenames().ToArray(), _gameRef, true);
				_modelSelect.ShowDialog();
				if (_modelSelect.DialogResult == DialogResult.OK && !string.IsNullOrEmpty(_modelSelect.CurrentModel.ModelName))
				{
					_drawablesToDelete.Add(_gameRef.ActiveArea.Display.CurrentlySelected);
					//_modelSelect.CurrentModel.updateTextureCoords();
					_drawableInfo.name = _modelSelect.CurrentModel.Name;
					_drawableInfo.textureID = _modelSelect.CurrentModel.GetTextureID();
					StaticModel temp = _modelSelect.CurrentModel;
					temp.Rotation = current.Rotation;
					temp.Position = current.Position;
					temp.Scale = current.Scale;
					_drawableInfo.drawable = temp;
					_drawablesToAdd.Add(_drawableInfo);
					/*if (_modelSelect.Event != null)
					{
						_eventInfo.eventTrigger = _modelSelect.Event;
						_eventInfo.name = _modelSelect.CurrentModel.Name;
						_eventsToAdd.Add(_eventInfo);
					}*/
					modelListBox.Items.Add(_modelSelect.CurrentModel.Name);
					modelListBox.Items.RemoveAt(modelListBox.SelectedIndex);
					modelListBox.Update();
				}

                //PropertyEditor pe = new PropertyEditor(current);
                //pe.ShowDialog();


			}
		}

		private void nameButton_Click(object sender, EventArgs e)
		{
			if (areaListBox.SelectedIndex != -1 && !string.IsNullOrEmpty(areaTextBox.Text) && !areaTextBox.Text.Equals((string)areaListBox.Items[areaListBox.SelectedIndex]))
			{
				Level.Areas.Add(areaTextBox.Text, Level.Areas[(string)areaListBox.Items[areaListBox.SelectedIndex]]);
				Level.RemoveArea((string)areaListBox.Items[areaListBox.SelectedIndex]);
				areaListBox.Items[areaListBox.SelectedIndex] = areaTextBox.Text;
			}
		}

		private void EventButton_Click(object sender, EventArgs e)
		{
            if (_gameRef.ActiveDrawable != null && _gameRef.ActiveDrawable is StaticModel)
            {
                /*EventTrigger existingEvent = null;
                if (_gameRef.ActiveArea.Events.ContainsKey(((StaticModel)_gameRef.ActiveDrawable).Name))
                {
                    existingEvent = _gameRef.ActiveArea.Events[((StaticModel)_gameRef.ActiveDrawable).Name];
                }*/
                EventTrigger existingEvent = ((StaticModel)_gameRef.ActiveDrawable).Event;

                if (existingEvent == null)
                {
                    EventSelector selector = new EventSelector();
                    selector.ShowDialog();
                    if (selector.EventTrigger != null)
                    {
                        /*EventInfo newEvent = new EventInfo();
                        newEvent.eventTrigger = selector.EventTrigger;
                        newEvent.name = ((StaticModel)_gameRef.ActiveDrawable).Name;
                        existingEvent = newEvent.eventTrigger;
                        _eventsToAdd.Add(newEvent);*/
                        ((StaticModel)_gameRef.ActiveDrawable).Event = selector.EventTrigger;
                    }
                }


                if (existingEvent != null)
                {
                    EventSetter setter = new EventSetter(existingEvent);
                    setter.ShowDialog();
                }

            }
		}

        private void AddPortalButton_Click(object sender, EventArgs e)
        {
            _gameRef.ActiveArea.Portals.Add(new Portal());
            updatePortalList();
        }

        private void RemovePortalButton_Click(object sender, EventArgs e)
        {
            if (portalList.SelectedIndex != -1)
            {
                _gameRef.ActiveArea.Portals.Remove(portalList.SelectedItem as Portal);
                updatePortalList();
            }
        }

        private void EditPortalButton_Click(object sender, EventArgs e)
        {
            if (portalList.SelectedIndex != -1)
            {
                PropertyEditor pe = new PropertyEditor(portalList.SelectedItem);
                pe.ShowDialog();
                updatePortalList();
            }
        }

        private void updatePortalList()
        {
            if (_gameRef.ActiveArea.Portals == null)
            {
                _gameRef.ActiveArea.Portals = new List<Portal>();
            }
            portalList.Items.Clear();
            foreach (Portal p in _gameRef.ActiveArea.Portals)
            {
                portalList.Items.Add(p);
            }
            portalList.Update();
        }

        private void updateAmbienceList() {
            if (_gameRef.ActiveArea.AmbientSounds == null) {
                _gameRef.ActiveArea.AmbientSounds = new List<AmbientSoundInfo>();
            }
            ambienceListBox.Items.Clear();
            foreach (AmbientSoundInfo sound in _gameRef.ActiveArea.AmbientSounds) {
                ambienceListBox.Items.Add(sound);
            }
            ambienceListBox.Update();
		}

		private void updateModelList()
		{
			modelListBox.Items.Clear();
			foreach (string str in _gameRef.ActiveArea.Drawables.Keys)
			{
				modelListBox.Items.Add(str);
			}
			modelListBox.Update();
		}

        private void EditTasksButton_Click(object sender, EventArgs e)
        {
            if (modelListBox.SelectedIndex != -1 && _gameRef.ActiveDrawable is DynamicModel)
            {
                TaskEditor te = new TaskEditor(((DynamicModel)_gameRef.ActiveDrawable).Tasks);
                te.ShowDialog();
                //PropertyEditor pe = new PropertyEditor(((DynamicModel)_gameRef.ActiveDrawable));
                //pe.ShowDialog();
            }
        }

        private void EditAreaButton_Click(object sender, EventArgs e)
        {
            if (areaListBox.SelectedIndex != -1)
            {
                PropertyEditor pe = new PropertyEditor(Level.GetArea(areaListBox.SelectedItem as string), true);
                pe.ShowDialog();
            }
        }

        private void addSoundButton_Click(object sender, EventArgs e) {
            if (_gameRef.ActiveArea.AmbientSounds == null) {
                _gameRef.ActiveArea.AmbientSounds = new List<AmbientSoundInfo>();
            }
            _gameRef.ActiveArea.AmbientSounds.Add(new AmbientSoundInfo());
            updateAmbienceList();
        }

        private void editSoundButton_Click(object sender, EventArgs e) {
            if (ambienceListBox.SelectedIndex != -1) {
                PropertyEditor pe = new PropertyEditor(ambienceListBox.SelectedItem);
                pe.ShowDialog();
                updateAmbienceList();
            }
        }

        private void deleteSoundButton_Click(object sender, EventArgs e) {
            if (ambienceListBox.SelectedIndex != -1) {
                _gameRef.ActiveArea.AmbientSounds.Remove(ambienceListBox.SelectedItem as AmbientSoundInfo);
                updateAmbienceList();
            }
        }

		public void UpdateLists()
		{
			updateModelList();
			updatePortalList();
			updateAmbienceList();
		}

		/// <summary>
		/// Temporary button for rapid level editing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void copyButton_Click(object sender, EventArgs e)
		{
			//removeTasks();
            //convertToDynamic();
			//addTaskToModels();
			//editPositionsRelative();
            //DEV_setEventCoolDowns();
		}
		/// <summary>
		/// Temporary function for rapid level editing
		/// </summary>
        private void DEV_setEventCoolDowns()
        {
            foreach (object o in modelListBox.SelectedItems)
            {
                StaticModel s = (StaticModel)_gameRef.ActiveArea.Drawables[o as string];
                if (s.Event != null)
                {
                    s.Event.CoolDown = 1;
                }
            }
        }

		/// <summary>
		/// Temporary function for rapid level editing
		/// </summary>
		private void editPositionsRelative()
		{
			foreach (object o in modelListBox.SelectedItems)
			{
				float deltaX = 0;
				float deltaY = -14;
				float deltaZ = 0;
				StaticModel s = (StaticModel)_gameRef.ActiveArea.Drawables[o as string];
				Vector3 theTranslation;
				Quaternion theRotation;
				Vector3 theScale;
				s.Transform.Decompose(out theScale, out theRotation, out theTranslation);
				theTranslation.X += deltaX;
				theTranslation.Y += deltaY;
				theTranslation.Z += deltaZ;
				s.Position = Matrix.CreateTranslation(theTranslation);
			}
		}

		/// <summary>
		/// Temporary function for rapid level editing
		/// </summary>
		private void removeTasks()
		{
			foreach (object o in modelListBox.SelectedItems)
			{
				Drawable d = _gameRef.ActiveArea.Drawables[o as string];
				if (d is DynamicModel)
				{
					((DynamicModel)d).Tasks = new List<Task>();
				}
			}
		}

		/// <summary>
		/// Temporary function for rapid level editing
		/// </summary>
		private void convertToDynamic()
		{
			foreach (object o in modelListBox.SelectedItems)
			{
				Drawable d = _gameRef.ActiveArea.Drawables[o as string];
				if (!d.GetType().IsSubclassOf(typeof(StaticModel)) && d is StaticModel)
				{
					_gameRef.ActiveArea.Drawables[o as string] = new DynamicModel(d as StaticModel);
				}
			}
		}

		/// <summary>
		/// Temporary function for rapid level editing
		/// -Currently only works for TaskKeyFrameMovement
		/// </summary>
		private void addTaskToModels()
		{
			TaskEditor te = new TaskEditor();
			te.ShowDialog();
			foreach (object o in modelListBox.SelectedItems)
			{
				Drawable d = _gameRef.ActiveArea.Drawables[o as string];
				if (d is DynamicModel)
				{
					DynamicModel y = d as DynamicModel;
					if (y.Tasks == null)
					{
						y.Tasks = new List<Task>();
					}
					foreach (Task t in te.Tasks)
					{
						if (t is TaskKeyFrameMovement)
						{
							y.Tasks.Add(new TaskKeyFrameMovement(t as TaskKeyFrameMovement));
						}
					}
				}
			}
		}
	}
}
