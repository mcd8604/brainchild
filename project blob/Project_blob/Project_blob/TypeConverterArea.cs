using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Project_blob
{
    class TypeConverterArea : TypeConverter
    {
        public override bool GetStandardValuesSupported(
                           ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection
                     GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(Level.GetAreaNames());
        } 
    }
}
