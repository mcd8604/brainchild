using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine
{
	public class BasicCamera : Camera
	{
		public BasicCamera() { }

		public override void Update(GameTime gameTime)
		{
			UpdateMatrices();
			base.Update(gameTime);
		}

		public override void UpdateMatrices()
		{
			View = Matrix.CreateLookAt(Position, Target, Up);

			Projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearPlane, FarPlane);

			Frustum = new BoundingFrustum(Matrix.Multiply(View, Projection));

			BoundingSphere = BoundingSphere.CreateFromFrustum(Frustum);
		}
	}
}
