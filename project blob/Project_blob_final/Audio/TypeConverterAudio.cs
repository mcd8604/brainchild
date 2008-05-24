using System.Collections.Generic;
using System.ComponentModel;

namespace Audio {
	public class TypeConverterAudio : TypeConverter {
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
			return true;
		}
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
			return new StandardValuesCollection(AudioManager.getAudioFilenames());
		}
	}
}