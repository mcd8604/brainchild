using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using Physics2;

namespace Project_blob
{
	public class TaskTypeEditor : UITypeEditor
	{

		public TaskTypeEditor() { }

		public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			TaskEditor editor;
			if (value == null)
			{
				value = new List<Task>();
			}
			editor = new TaskEditor(value as List<Task>);
			editor.ShowDialog();

			//Copy TaskKeyFrameMovements to each model
			if(context.Instance is SwitchEvent) {
				foreach (DynamicModel d in ((SwitchEvent)context.Instance).Models)
				{
					if(d.Tasks == null) {
						d.Tasks = new List<Task>();
					}
					foreach (Task t in editor.Tasks)
					{
						if (t is TaskKeyFrameMovement)
						{
							d.Tasks.Add(new TaskKeyFrameMovement(t as TaskKeyFrameMovement));
						}
					}
				}
			}
			return new List<Task>(editor.Tasks);
		}
	}
}
