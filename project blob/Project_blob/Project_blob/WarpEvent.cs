using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Project_blob.GameState;

namespace Project_blob
{
	[Serializable]
	public class WarpEvent : EventTrigger
	{
		private Vector3 _moveToPos;
		public Vector3 MoveToPos
		{
			get
			{
				return _moveToPos;
			}
			set
			{
				_moveToPos = value;
			}
		}
		private Vector3 _moveToVel;
		public Vector3 MoveToVel {
			get{
				return _moveToVel;
			}
			set {
				_moveToVel = value;
			}
		}

		public WarpEvent(float xPos, float yPos, float zPos, float xVel, float yVel, float zVel)
		{
			_moveToPos = new Vector3(xPos, yPos, zPos);
			_moveToVel = new Vector3(xVel, yVel, zVel);
		}

		public void PerformEvent(GameplayScreen gameRef)
		{
			Vector3[] offsets = new Vector3[gameRef.Player.points.Count];
			for (int i = 0; i < offsets.Length; i++)
			{
				offsets[i] = gameRef.Player.points[i].CurrentPosition - gameRef.Player.getCenter();
			}
			for (int j = 0; j < offsets.Length; j++)
			{
				gameRef.Player.points[j].NextPosition = offsets[j] + _moveToPos;
				gameRef.Player.points[j].NextVelocity = _moveToVel;
			}

			//Vector3 diff = _moveToPos - trigger.ParentBody.getCenter();

			//foreach (Physics.Point p in trigger.ParentBody.getPoints())
			//{
			//    // Physics is going to be changing soon, this will not be neccessary once events as 'handled' post collision.
			//    p.potentialPosition = p.CurrentPosition + diff;
			//    p.NextPosition = p.CurrentPosition + diff;

			//    p.potentialVelocity = _moveToVel;
			//    p.NextVelocity = _moveToVel;
			//}

		}
	}
}
