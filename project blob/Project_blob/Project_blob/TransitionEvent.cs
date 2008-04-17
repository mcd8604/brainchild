using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Project_blob.GameState;

namespace Project_blob
{
    public class TransitionEvent : EventTrigger {
        
        private String _area;
        private Vector3 _position;
    
        public TransitionEvent(String area, float xPos, float yPos, float zPos)
        {
            _area = area;
            _position = new Vector3(xPos, yPos, zPos);
        }

        public void PerformEvent(Physics.Point trigger)
        {
            GameplayScreen.currentArea = Level.Areas[_area];
			/*
            Vector3[] offsets = new Vector3[gameRef.Player.points.Count];
            for (int i = 0; i < offsets.Length; i++)
            {
                offsets[i] = gameRef.Player.points[i].CurrentPosition - gameRef.Player.getCenter();
            }
            for (int j = 0; j < offsets.Length; j++)
            {
                gameRef.Player.points[j].CurrentPosition = offsets[j] + _position;
            }
			*/
			Vector3 diff = _position - trigger.ParentBody.getCenter();

			foreach (Physics.Point p in trigger.ParentBody.getPoints())
			{
				// Physics is going to be changing soon, this will not be neccessary once events as 'handled' post collision.
				p.PotientialPosition = p.CurrentPosition + diff;
				p.NextPosition = p.CurrentPosition + diff;
			}
        }
    }
}
