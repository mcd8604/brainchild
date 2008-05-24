using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Project_blob
{
    public enum BlendModes
    {
        None,
        Alpha,
        Additive
    }

	public interface Drawable
	{

        BlendModes BlendMode
        {
            get;
            set;
        }

		//VertexPositionColor[] getTriangleVertexes();
		VertexBuffer getVertexBuffer();

		int getVertexStride();

		void DrawMe();

		//TextureInfo GetTextureKey();

		int GetTextureID();

		void SetTextureID(int id);

		BoundingBox GetBoundingBox();

        //BoundingSphere GetBoundingSphere();

        bool Drawn
        {
            get;
            set;
        }

	}
}
