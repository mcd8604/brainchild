using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Project_blob
{
    class TypeConverterModel : TypeConverter
    {
        public override bool GetStandardValuesSupported(
                           ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection
                     GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(ModelManager.GetModelNames());
        } 
    }
}
