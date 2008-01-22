using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ObjectManagerExample
{
    public delegate bool CustomInstanceCreator(ref object targetInstance, Type targetType);
    public delegate void CustomLoader(ref object targetInstance, XmlNode xmlNode);
    
    public class ObjectManager
    {
        protected CustomInstanceCreator customInstanceCreator;
        protected Dictionary<string, CustomLoader> customLoaderDelegates = new Dictionary<string, CustomLoader>();

        protected Dictionary<string, object> objects = new Dictionary<string, object>();

        protected Dictionary<string, object> globalVariables = new Dictionary<string, object>();
        protected Dictionary<string, XmlNode> templateNodes = new Dictionary<string, XmlNode>();

        public ObjectManager(Game game)
        {
            game.Services.AddService(typeof(ObjectManager), this);

            this.customInstanceCreator = this.DefaultCustomInstanceCreator;
        }

        public T GetObject<T>(string objectName, bool clone)
        {
            if (!objects.ContainsKey(objectName))
                return default(T);

            object obj = objects[objectName];

            if (clone)
            {
                return (T)((ICloneable)obj).Clone();
            }
            else
            {
                return (T)obj;
            }
        }

        public CustomInstanceCreator CustomInstanceCreator
        {
            set
            {
                this.customInstanceCreator = value;
            }
        }

        public bool DefaultCustomInstanceCreator(ref object targetInstance, Type targetType)
        {
            if (targetType == typeof(string))
            {
                targetInstance = "";
                return true;
            }

            return false;
        }

        public void AddCustomLoader(string loaderName, CustomLoader loaderDelegate)
        {
            this.customLoaderDelegates[loaderName] = loaderDelegate;
        }

        public void AddGlobalVariable(string varName, object globalObject)
        {
            this.globalVariables[varName] = globalObject;
        }

        public void LoadFromXML(string filename)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList nodeList;

            xmlDoc.Load(filename);

            ReplaceIncludeNodes(xmlDoc, "include");
            CollectTemplateNodes(xmlDoc, "/settings/templates/template");

            nodeList = xmlDoc.SelectNodes("/settings/objects/object");
            foreach (XmlNode xmlNode in nodeList)
            {
                string nameOfObject = GetAttribute(xmlNode, "name");

                if (string.IsNullOrEmpty(nameOfObject))
                    continue;

                object gameObject = null;
                Visit(ref gameObject, (XmlElement)xmlNode);

                if (gameObject == null)
                    continue;

                objects.Add(nameOfObject, gameObject);
            }

            templateNodes.Clear();
        }

        protected void ReplaceIncludeNodes(XmlDocument xmlDoc, string includeTagName)
        {
            XmlNodeList includeNodesToReplace = xmlDoc.DocumentElement.GetElementsByTagName(includeTagName);
            for (int i = 0; i < includeNodesToReplace.Count; i++)
            {
                XmlNode includeNode = includeNodesToReplace[i];
                string includeFilename = GetAttribute(includeNode, "filename");
                if (includeFilename != "")
                {
                    XmlDocument xmlIncludeDocument = new XmlDocument();
                    xmlIncludeDocument.Load(includeFilename);

                    foreach (XmlNode childNode in xmlIncludeDocument)
                    {
                        if ((childNode is XmlElement) || (childNode is XmlText))
                        {
                            XmlNode tempNode = xmlDoc.ImportNode(childNode, true);
                            includeNode.ParentNode.InsertAfter(tempNode, includeNode);
                        }
                    }
                }
                includeNode.ParentNode.RemoveChild(includeNode);
            }
        }

        protected void CollectTemplateNodes(XmlDocument xmlDoc, string templateSelect)
        {
            XmlNodeList nodeList = xmlDoc.SelectNodes(templateSelect);
            foreach (XmlNode xmlNode in nodeList)
            {
                string templateName = GetAttribute(xmlNode, "name");
                if (!string.IsNullOrEmpty(templateName))
                {
                    templateNodes[templateName] = xmlNode;
                }
            }
        }

        protected void Visit(ref object targetObject, XmlElement xmlElement)
        {
            object customLoadedObject = null;

            string customLoader = GetAttribute(xmlElement, "customLoader");
            if ((customLoader != "") && (customLoaderDelegates.ContainsKey(customLoader)))
            {
                customLoaderDelegates[customLoader](ref customLoadedObject, xmlElement);
                if ((customLoadedObject != null) && (targetObject == null))
                {
                    targetObject = customLoadedObject;
                    return;
                }
            }

            if (targetObject == null)
            {
                string typeName = GetAttribute(xmlElement, "type");
                targetObject = CreateInstance(GetTypeByName(typeName));
                if (targetObject == null)
                    throw new Exception("could not create type");
            }

            string template = GetAttribute(xmlElement, "template");
            if ((!string.IsNullOrEmpty(template)) && (templateNodes.ContainsKey(template)))
            {
                VisitChildren(ref targetObject, templateNodes[template]);
            }

            if (TryToSetField(ref targetObject, xmlElement, customLoadedObject))
                return;

            if (TryToSetProperty(ref targetObject, xmlElement, customLoadedObject))
                return;

            if (TryToInvokeMethod(ref targetObject, xmlElement))
                return;

            if (customLoadedObject == null)
                VisitChildren(ref targetObject, xmlElement);
            else
                targetObject = customLoadedObject;
        }

        protected void Visit(ref object targetObject, XmlText xmlText, string objectValue)
        {
            if (targetObject == null)
                return;

            if (xmlText != null)
                objectValue = xmlText.InnerText;

            if (string.IsNullOrEmpty(objectValue))
                return;

            TypeConverter typeConverter = TypeDescriptor.GetConverter(targetObject);
            if (typeConverter.CanConvertFrom(typeof(string)))
            {
                targetObject = typeConverter.ConvertFromString(objectValue.Trim());
            }
            else
            {
#if DEBUG
                Debug.WriteLine(String.Format("no TypeConverter found for '{0}'", targetObject.GetType().FullName));
#endif
            }
        }

        protected void VisitChildren(ref object[] obj, XmlNode xmlNode)
        {
            int childCount = 0;

            foreach (XmlNode xmlChild in xmlNode.ChildNodes)
            {
                if (xmlChild is XmlElement)
                {
                    Visit(ref obj[childCount++], (XmlElement)xmlChild);
                    continue;
                }
                if (xmlChild is XmlText)
                {
                    Visit(ref obj[childCount++], (XmlText)xmlChild, null);
                    continue;
                }
                if (xmlChild is XmlComment)
                {
                    continue;
                }
            }
        }

        protected void VisitChildren(ref object obj, XmlNode xmlNode)
        {
            foreach (XmlNode xmlChild in xmlNode.ChildNodes)
            {
                if (xmlChild is XmlElement)
                {
                    if (obj.GetType().IsArray)
                    {
                        Array arrayObj = (Array)obj;
                        Array tempArrayObj = Array.CreateInstance(arrayObj.GetType().GetElementType(), arrayObj.Length + 1);
                        arrayObj.CopyTo(tempArrayObj, 0);

                        object arrayElement = CreateInstance(arrayObj.GetType().GetElementType());
                        Visit(ref arrayElement, (XmlElement)xmlChild);
                        tempArrayObj.SetValue(arrayElement, tempArrayObj.Length - 1);

                        obj = tempArrayObj;
                    }
                    else
                    {
                        Visit(ref obj, (XmlElement)xmlChild);
                    }
                    continue;
                }
                if (xmlChild is XmlText)
                {
                    Visit(ref obj, (XmlText)xmlChild, null);
                    continue;
                }
                if (xmlChild is XmlComment)
                {
                    continue;
                }
            }
        }

        #region Reflection
        protected bool TryToSetField(ref object targetObject, XmlElement xmlElement, object fieldValue)
        {
            FieldInfo fieldInfo = targetObject.GetType().GetField(xmlElement.Name);
            if (fieldInfo == null)
                return false;

            if (fieldValue == null)
            {
                fieldValue = CreateInstance(fieldInfo.FieldType);
                VisitChildren(ref fieldValue, xmlElement);
            }
            try
            {
                fieldInfo.SetValue(targetObject, fieldValue);
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(String.Format("could not set field '{0}': {1}", xmlElement.Name, e));
#endif
            }

            return true;
        }
        protected bool TryToSetProperty(ref object targetObject, XmlElement xmlElement, object propertyValue)
        {
            PropertyInfo propertyInfo = targetObject.GetType().GetProperty(xmlElement.Name);
            if (propertyInfo == null)
                return false;

            if (propertyValue == null)
            {
                propertyValue = CreateInstance(propertyInfo.PropertyType);
                VisitChildren(ref propertyValue, xmlElement);
            }

            ParameterInfo[] indexParamInfos = propertyInfo.GetIndexParameters();
            object[] indexParamValues = null;

            if ((indexParamInfos != null) && (indexParamInfos.Length > 0))
            {
                indexParamValues = new Object[indexParamInfos.Length];
                string[] indices = GetAttribute(xmlElement, "index").Split(',');

                for (int i = 0; i < indexParamInfos.Length; i++)
                {
                    object indexParamValue = CreateInstance(indexParamInfos[i].ParameterType);
                    if (i < indices.Length)
                        Visit(ref indexParamValue, null, indices[i]);

                    indexParamValues.SetValue(indexParamValue, i);
                }
            }

            try
            {
                propertyInfo.SetValue(targetObject, propertyValue, indexParamValues);
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(String.Format("could not set property '{0}': {1}", xmlElement.Name, e));
#endif
            }
            return true;
        }
        protected bool TryToInvokeMethod(ref object targetObject, XmlElement xmlElement) 
        {
            object refObject = targetObject;
            string refObjectName = GetAttribute(xmlElement, "refObjectName");
            if ((!string.IsNullOrEmpty(refObjectName)) && (globalVariables.ContainsKey(refObjectName)))
            {
                refObject = globalVariables[refObjectName];
            }

            MethodInfo methodInfo = refObject.GetType().GetMethod(xmlElement.Name);
            if (methodInfo == null)
                return false;

            ParameterInfo[] paramInfos = methodInfo.GetParameters();
            object[] methodParams = null;

            if ((paramInfos != null) && (paramInfos.Length > 0))
            {
                methodParams = new Object[paramInfos.Length];

                for (int i = 0; i < paramInfos.Length; i++)
                {
                    object paramValue = CreateInstance(paramInfos[i].ParameterType);
                    methodParams.SetValue(paramValue, i);
                }

                VisitChildren(ref methodParams, xmlElement);
            }

            try
            {
                object returnObject = methodInfo.Invoke(refObject, methodParams);
                if (returnObject != null)
                    targetObject = returnObject;
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(String.Format("could not invoke method '{0}': {1}", xmlElement.Name, e));
#endif
            }

            return true;
        }
        #endregion

        protected object CreateInstance(Type targetType)
        {
            if (targetType == null)
                return null;

            if (customInstanceCreator != null)
            {
                object customCreatedInstance = null;
                if (customInstanceCreator(ref customCreatedInstance, targetType))
                    return customCreatedInstance;
            }

            if (targetType.IsArray)
            {
                try
                {
                    return Array.CreateInstance(targetType.GetElementType(), 0);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            try
            {
                return Activator.CreateInstance(targetType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected Type GetTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
#if DEBUG
                Debug.WriteLine("typeName is empty.");
#endif

                return null;
            }

            Type namedType = Type.GetType(typeName);
            if (namedType != null)
            {
                return namedType;
            }

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    namedType = assembly.GetType(typeName);
                    if (namedType != null)
                        return namedType;
                }
                catch (Exception)
                {
                }
            }

#if DEBUG
            Debug.WriteLine(String.Format("type: '{0}' not found.", typeName));
#endif

            return null;
        }

        private string GetAttribute(XmlNode node, string attributeName)
        {
            if (node.Attributes.Count == 0)
                return "";

            try
            {
                return (node.Attributes[attributeName] != null) ? node.Attributes[attributeName].InnerText : "";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
