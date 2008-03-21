using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WorldMakerDemo
{
	public interface Drawable
	{

		//VertexPositionColor[] getTriangleVertexes();

		VertexBuffer getVertexBuffer();

		void setGraphicsDevice(GraphicsDevice device);

		int getVertexStride();

		void DrawMe();

	}
}
