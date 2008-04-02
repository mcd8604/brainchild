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

        private bool _culled = false;
        public bool Culled
        {
            get { return _culled; }
            set { _culled = value; }
        }

        public SceneObjectNode(SceneObject sceneObject)
        {
            _sceneObject = sceneObject;
        }

        public override void Draw(GameTime gameTime)
        {
            SceneManager.getSingleton.Drawn += 1;
            _sceneObject.Draw(gameTime);
        }

        public override void CullDraw(GameTime gameTime)
        {
            //Replace true with Culling
            if (!Cull())
            {
                SceneManager.getSingleton.Drawn += 1;
                _sceneObject.Draw(gameTime);
            }
            else
            {
                SceneManager.getSingleton.Culled += 1;
            }
        }

        private bool Cull()
        {
            //ContainmentType containment;
            //containment = CameraManager.getSingleton.ActiveCamera.Frustum.Contains(_sceneObject.BoundingBox);

            //if (CameraManager.getSingleton.GetCamera("test").Frustum.Contains(_sceneObject.GetBoundingBoxTransformed()) == ContainmentType.Disjoint)
            if (CameraManager.getSingleton.ActiveCamera.Frustum.Contains(_sceneObject.GetBoundingBoxTransformed()) == ContainmentType.Disjoint)
            //if (CameraManager.getSingleton.ActiveCamera.Frustum.Contains(_sceneObject.BoundingBox) == ContainmentType.Disjoint)
            {
                _culled = true;

                //containment = CameraManager.getSingleton.ActiveCamera.Frustum.Contains(_sceneObject.BoundingBox);
                //if (containment == ContainmentType.Disjoint)
                //{
                //    _culled = true;
                //}
                //else
                //{
                //    _culled = false;
                //}
            }
            else
            {
                _culled = false;
            }

            return _culled;
        }
    }
}
