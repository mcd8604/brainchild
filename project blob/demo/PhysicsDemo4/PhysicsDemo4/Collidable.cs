using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhysicsDemo4
{
	public interface Collidable
	{
		bool couldIntersect();

		float DotNormal(Vector3 pos);

		Vector3 Normal();

		float didIntersect(Vector3 start, Vector3 end);

		Plane getPlane();

		VertexPositionColor[] getTriangleVertexes();

		void DrawMe(GraphicsDevice device);
	}
}
