using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;

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
			return selector.getModels();
		}
	}
}
