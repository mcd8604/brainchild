using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using Physics2;

namespace Project_blob
{
	public class ModelSelectionEditor : UITypeEditor
	{

		public ModelSelectionEditor() { }

		public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			ModelSelector selector;
			if (value is List<DynamicModel>)
			{
				selector = new ModelSelector(value as List<DynamicModel>);
			}
			else
			{
				selector = new ModelSelector();
			}
			selector.ShowDialog();

			//Copy TaskKeyFrameMovements to each model
			if (context.Instance is SwitchEvent)
			{
				foreach (DynamicModel d in selector.getModels())
				{
					//if (d.Tasks == null)
					//{
						d.Tasks = new List<Task>();
					//}
					foreach (Task t in ((SwitchEvent)context.Instance).Tasks)
					{
						if (t is TaskKeyFrameMovement)
						{
							d.Tasks.Add(new TaskKeyFrameMovement(t as TaskKeyFrameMovement));
						}
					}
				}
			}
			return selector.getModels();
		}
	}
}
