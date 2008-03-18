using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo6
{
	interface Drawable
	{

		VertexPositionColor[] getTriangleVertexes();

		void DrawMe(GraphicsDevice device);

	}
}
