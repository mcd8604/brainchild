using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WorldMakerDemo
{
	public interface Drawable
	{
		//VertexPositionColor[] getTriangleVertexes();
		VertexBuffer getVertexBuffer();

		int getVertexStride();

		void DrawMe();

	}
}
