using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;

namespace Project_blob
{
	public class EventCollectionEditor : CollectionEditor
	{
		public EventCollectionEditor(Type type)
			: base(type) { }

		protected override Type[] CreateNewItemTypes()
		{
			List<Type> types = new List<Type>();
			System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom("Project_blob.exe");
			foreach (Type t in asm.GetTypes())
			{
				if (typeof(EventTrigger).IsSubclassOf(t))
				{
					types.Add(t);
				}
			}
			return types.ToArray();
		}
	}
}
