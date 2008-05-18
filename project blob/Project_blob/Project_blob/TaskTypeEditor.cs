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
			if(context.Instance is SwitchEvent) {
				foreach (DynamicModel d in ((SwitchEvent)context.Instance).Models)
				{
					d.Tasks = new List<Task>(editor.Tasks);
				}
			}
			return new List<Task>(editor.Tasks);
		}
	}
}
