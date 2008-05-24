using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Project_blob
{
    class TypeConverterAudio : TypeConverter
    {
        public override bool GetStandardValuesSupported(
                           ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection
                     GetStandardValues(ITypeDescriptorContext context)
        {
            string[] audio = System.IO.Directory.GetFiles(System.Environment.CurrentDirectory + "\\Content\\Audio");
            for (int i = 0; i < audio.Length; ++i) {
                audio[i] = audio[i].Substring(audio[i].LastIndexOf("\\") + 1);
                if (audio[i].EndsWith(".wav")) {
                    audio[i] = audio[i].Substring(0, audio[i].LastIndexOf(".") - 1);
                }
            }
            return new StandardValuesCollection(audio);
        } 
    }
}
