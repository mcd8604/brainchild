using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Project_blob
{
    public class ModelManager
    {
        // Objects to ensure singleton design
        private static volatile ModelManager _instance;
        private static object _syncRoot = new Object();

        // Dictionary of models
        private static Dictionary<String, Model> _models;

        /// <summary>
        /// Constructor
        /// </summary>
        private ModelManager()
        {
            _models = new Dictionary<String, Model>();
        }

        //! Instance
        public static ModelManager getSingleton
        {
            get {
                if (_instance == null) {
                    lock (_syncRoot) {
                        if (_instance == null)
                            _instance = new ModelManager();
                    }
                }

                return _instance;
            }
        }

        public Model GetModel(String modelName)
        {
            if (_models.ContainsKey(modelName))
            {
                return _models[modelName];
            }
            return null;
        }

        public void AddModel(String modelName, Model model)
        {
            if (!_models.ContainsKey(modelName)){
                _models.Add(modelName, model);
            }
        }

        public void RemoveModel(String modelName)
        {
            if (_models.ContainsKey(modelName))
            {
                _models.Remove(modelName);
            }
        }

        public static string[] GetModelNames()
        {
            string[] modelNames = new string[_models.Count];
            _models.Keys.CopyTo(modelNames, 0);
            return modelNames;
        }
    }
}
