using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Project_blob
{
	public partial class ModelSelector : Form
	{
		public ModelSelector()
		{
			InitializeComponent();
			DynamicModel[] models = new DynamicModel[Level.CurrentArea.Drawables.Values.Count];
			Level.CurrentArea.Drawables.Values.CopyTo(models, 0);
			areaModels.Items.AddRange(models);
		}

		public ModelSelector(List<DynamicModel> models)
		{
			InitializeComponent();
			DynamicModel[] modelArray = new DynamicModel[models.Count];
			models.CopyTo(modelArray, 0);
			currentModels.Items.AddRange(modelArray);
			foreach(Drawable d in Level.CurrentArea.Drawables.Values) 
			{
				if(d is DynamicModel) 
				{
					if(!currentModels.Items.Contains(d)) {
						areaModels.Items.Add(d as DynamicModel);
					}
				}
			}
		}

		public List<DynamicModel> getModels()
		{
			DynamicModel[] models = new DynamicModel[currentModels.Items.Count];
			currentModels.Items.CopyTo(models, 0);
			return new List<DynamicModel>(models);
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			List<DynamicModel> toRemove = new List<DynamicModel>();
			foreach(object o in currentModels.SelectedItems)
			{
				toRemove.Add(o as DynamicModel);
			}
			foreach (DynamicModel d in toRemove)
			{
				areaModels.Items.Add(d);
				currentModels.Items.Remove(d);
			}
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			List<DynamicModel> toAdd = new List<DynamicModel>();
			foreach (object o in areaModels.SelectedItems)
			{
				toAdd.Add(o as DynamicModel);
			}
			foreach (DynamicModel d in toAdd)
			{
				currentModels.Items.Add(d);
				areaModels.Items.Remove(d);
			}
		}
	}
}