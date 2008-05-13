using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Project_blob
{
	public interface Drawable
	{
		//VertexPositionColor[] getTriangleVertexes();
		VertexBuffer getVertexBuffer();

		int getVertexStride();

		void DrawMe();

		//TextureInfo GetTextureKey();

		int GetTextureID();

		void SetTextureID(int id);

		BoundingBox GetBoundingBox();

		//BoundingSphere GetBoundingSphere();

	}
}
