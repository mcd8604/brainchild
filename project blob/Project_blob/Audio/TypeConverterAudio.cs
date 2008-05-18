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
			string[] audio = System.IO.Directory.GetFiles(@"\\Content\\Audio");
			for (int i = audio.Length - 1; i > 0; --i)
			{
				audio[i] = audio[i].Substring(audio[i].LastIndexOf("\\") + 1);
				if (audio[i].EndsWith(".wav"))
				{
					audio[i] = audio[i].Substring(0, audio[i].LastIndexOf("."));
				}
			}
			return new StandardValuesCollection(audio);
		}
	}
}