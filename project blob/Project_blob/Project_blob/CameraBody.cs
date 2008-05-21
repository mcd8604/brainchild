using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Physics2;

namespace Project_blob {

	internal class CameraBody : Body {

		PhysicsPoint point;
		TaskCamera task;

		public CameraBody(Body Target) {
			point = new PhysicsPoint(Vector3.Zero);
			points.Add(point);
			task = new TaskCamera(Target);
			tasks.Add(task);

			initialize();
		}

		public Vector3 getCameraPosition() {
			return point.ExternalPosition;
		}

		public override void update(float TotalElapsedSeconds) {
			task.update(this, TotalElapsedSeconds);
			boundingBox.clear();
			boundingBox.expandToInclude(point.CurrentPosition);
			boundingBox.expandToInclude(point.PotentialPosition);
		}

		public override void updatePosition() {
			point.updatePosition();
		}

		public void setCameraOffset(Vector3 Offset) {
			task.OffsetVector = Offset;
		}

		public void setCameraTarget(Body Target) {
			task.Target = Target;
		}

	}
}
