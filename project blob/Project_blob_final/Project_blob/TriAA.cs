using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Physics2;

namespace Project_blob
{
	class TriAA : CollidableTri, Drawable
	{

		public static bool DEBUG_DrawNormal = false;
		const int Num_Vertex = 5; // max 5 for drawnormal

		internal PhysicsPoint[] points;

		VertexPositionColor[] vertices = new VertexPositionColor[Num_Vertex];

		Color color;

		private GraphicsDevice theDevice;
		private VertexBuffer myVertexBuffer;

		public TriAA(PhysicsPoint point1, PhysicsPoint point2, PhysicsPoint point3, Color p_color)
			:base(point1, point2, point3)
		{
			points = new PhysicsPoint[3];

			points[0] = point1;
			points[1] = point2;
			points[2] = point3;

			color = p_color;


			vertices[0] = new VertexPositionColor(points[0].ExternalPosition, color);
			vertices[1] = new VertexPositionColor(points[1].ExternalPosition, color);
			vertices[2] = new VertexPositionColor(points[2].ExternalPosition, color);

			vertices[3] = new VertexPositionColor(Vector3.Negate(getOrigin()), Color.Pink);
			vertices[4] = new VertexPositionColor(Vector3.Negate(getOrigin()) + Normal, Color.Red);

		}

		private Vector3 getOrigin()
		{
			return Vector3.Negate((points[0].NextPosition + points[1].NextPosition + points[2].NextPosition) / 3f);
		}

		public override void ApplyForce(Vector3 at, Vector3 f)
		{

			float dist0 = Vector3.Distance(points[0].NextPosition, at);
			float dist1 = Vector3.Distance(points[1].NextPosition, at);
			float dist2 = Vector3.Distance(points[2].NextPosition, at);
			float total = dist0 + dist1 + dist2;

			points[0].ForceNextFrame += f * (dist0 / total);
			points[1].ForceNextFrame += f * (dist1 / total);
			points[2].ForceNextFrame += f * (dist2 / total);

		}

		public override void ImpartVelocity(Vector3 at, Vector3 v)
		{


			float dist0 = Vector3.Distance(points[0].NextPosition, at);
			float dist1 = Vector3.Distance(points[1].NextPosition, at);
			float dist2 = Vector3.Distance(points[2].NextPosition, at);
			float total = dist0 + dist1 + dist2;

			// next next?

			//points[0].NextVelocity += v * (dist0 / total);
			//points[1].NextVelocity += v * (dist1 / total);
			//points[2].NextVelocity += v * (dist2 / total);

		}

		//public override Physics.Material getMaterial()
		//{
		//    return new Physics.MaterialBasic();
		//}

		// Drawable:

		public VertexPositionColor[] getTriangleVertexes()
		{
			vertices[0].Position = points[0].NextPosition;
			vertices[1].Position = points[1].NextPosition;
			vertices[2].Position = points[2].NextPosition;

			if (DEBUG_DrawNormal)
			{
				vertices[3].Position = Vector3.Negate(getOrigin());
				vertices[4].Position = Vector3.Negate(getOrigin()) + Normal;
			}

			return vertices;
		}

		public VertexBuffer getVertexBuffer()
		{
			myVertexBuffer.SetData<VertexPositionColor>(getTriangleVertexes());
			return myVertexBuffer;
		}

		public void setGraphicsDevice(GraphicsDevice device)
		{
			theDevice = device;
			myVertexBuffer = new VertexBuffer(device, VertexPositionColor.SizeInBytes * Num_Vertex, BufferUsage.None);
		}

		public int getVertexStride()
		{
			return VertexPositionColor.SizeInBytes;
		}

		public void DrawMe()
		{
			theDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
			if (DEBUG_DrawNormal)
			{
				theDevice.DrawPrimitives(PrimitiveType.LineList, 3, 1);
			}
		}


		#region Drawable Members


		public BoundingBox GetBoundingBox()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public BoundingSphere GetBoundingSphere()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

		#region Drawable Members


		public int GetTextureID()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void SetTextureID(int id)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
