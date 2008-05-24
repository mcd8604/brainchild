using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;

namespace Project_blob
{
	public class MultiTypeCollectionEditor : CollectionEditor
	{
		public MultiTypeCollectionEditor(Type type)
			: base(type) { }

		protected override Type[] CreateNewItemTypes()
		{
			List<Type> types = new List<Type>();
			System.Reflection.Assembly asm = System.Reflection.Assembly.GetAssembly(CollectionItemType);
			foreach (Type t in asm.GetTypes())
			{
				if (CollectionItemType.IsAssignableFrom(t) && !t.IsInterface)
				{
					types.Add(t);
				}
			}
			return types.ToArray();
		}
	}
}
