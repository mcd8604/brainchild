using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Project_blob
{
    public abstract class T : Physics.Collidable, Drawable
    {

        public T()
            :base(null)
		{
		}

        public abstract VertexBuffer getVertexBuffer();

        public abstract int getVertexStride();

        public abstract void DrawMe();

        public abstract TextureInfo GetTextureKey();

        public abstract BoundingBox GetBoundingBox();

        public abstract BoundingSphere GetBoundingSphere();
    }
}
