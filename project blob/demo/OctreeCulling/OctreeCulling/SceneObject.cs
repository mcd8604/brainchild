using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OctreeCulling
{
    class SceneObject
    {
        /// <summary>
        /// Position of the object
        /// </summary>
        private Vector3 _position = Vector3.Zero;
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Scale of the object.
        /// </summary>
        private Vector3 _scale = Vector3.One;
        public Vector3 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// Yaw, pitch and roll of the object.
        /// </summary>
        private Quaternion _rotation = Quaternion.Identity;
        public Quaternion Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public virtual Matrix World
        {
            get
            {
                return Matrix.CreateScale(this.Scale) *
                       Matrix.CreateFromQuaternion(this.Rotation) *
                       Matrix.CreateTranslation(this.Position);
            }
        }

        /// <summary>
        /// Bounding box of the object
        /// </summary>
        private BoundingBox _boundingBox;
        public BoundingBox BoundingBox
        {
            get { return _boundingBox; }
            set { _boundingBox = value; }
        }

        /// <summary>
        /// Bounding sphere of the object
        /// </summary>
        private BoundingSphere _boundingSphere;
        public BoundingSphere BoundingSphere
        {
            get { return _boundingSphere; }
            set { _boundingSphere = value; }
        }

        private BasicEffect _effect;
        public BasicEffect Effect
        {
            get { return _effect; }
            set { _effect = value; }
        }

        private GraphicsDeviceManager _graphics;
        public GraphicsDeviceManager Graphics
        {
            get { return _graphics; }
            set { _graphics = value; }
        }

        protected virtual void CreateBoundingSphere()
        {

        }

        protected virtual void CreateBoundingBox()
        {

        }

        public virtual void Draw(GameTime gameTime)
        {

        }
    }
}
