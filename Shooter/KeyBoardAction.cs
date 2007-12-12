using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Shooter
{
    //This class stores a method to be run.
    class KeyBoardAction
    {
        //The source object that the method will invoke;
        private Object mSource;
        //The methods name
        private String mMethodName;
        //The anme of the action
        private String mName;
        private MethodInfo mMethod;

        public KeyBoardAction(Object source, String methodName, String name)
        {
            mSource = source;
            mMethodName = methodName;
            mName = name;
            mMethod = mSource.GetType().GetMethod(mMethodName);
        }

        public void invoke(float elapsed)
        {
            //MethodInfo method = mSource.GetType().GetMethod(mMethodName);
            mMethod.Invoke(mSource, new Object[] { elapsed });          
            // method.Invoke(mSource, new Object[] { elapsed, null });
        }

        public override String ToString()
        {
            return mName;
        }
        
    }
}
