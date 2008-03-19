using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WorldMakerDemo
{
	interface Drawable
	{

		//VertexPositionColor[] getTriangleVertexes();

		VertexBuffer getVertexBuffer();

		void setGraphicsDevice(GraphicsDevice device);

		int getVertexStride();

		void DrawMe();

	}
}
