using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OctreeCulling
{
    class SceneObjectNode : Node
    {
        private SceneObject _sceneObject;
        public SceneObject SceneObject
        {
            get { return _sceneObject; }
            set { _sceneObject = value; }
        }

        public SceneObjectNode(SceneObject sceneObject)
        {
            _sceneObject = sceneObject;
        }

        public override void Draw(GameTime gameTime)
        {
            _sceneObject.Draw(gameTime);

            //Do Culling
        }
    }
}
