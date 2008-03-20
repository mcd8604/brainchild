using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo6
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
