using System;
using System.Collections.Generic;
using System.Text;
using Physics2;
using Microsoft.Xna.Framework;

namespace Project_blob {
	[Serializable]
	class DoorModel : DynamicModel {

		public DoorModel() { }

		public DoorModel(StaticModel p_Model)
			: base(p_Model) {
			Tasks = new List<Physics2.Task>();
			List<Vector3> patrolPoints = new List<Vector3>();
			patrolPoints.Add(Vector3.Zero);
			patrolPoints.Add(Vector3.One);
			Tasks.Add(new TaskTranslate(patrolPoints, 1f));
		}

		public void DoorOpen() {
		}
	}
}
