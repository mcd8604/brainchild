using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Audio
{
	public class TypeConverterAudio : TypeConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			string[] audio = System.IO.Directory.GetFiles(@"Content\\Audio");
			List<string> result = new List<string>();
			foreach (string file in audio) {
				if (file.EndsWith(".wav")) {
					int start = file.LastIndexOf("\\") + 1;
					int length = file.LastIndexOf(".") - start;
					result.Add(file.Substring(start, length));
				}
			}
			return new StandardValuesCollection(result);
		}
	}
}