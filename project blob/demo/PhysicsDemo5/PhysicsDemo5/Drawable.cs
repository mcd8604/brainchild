using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo5
{
	interface Drawable
	{

		VertexPositionColor[] getTriangleVertexes();

		void DrawMe(GraphicsDevice device);

	}
}
